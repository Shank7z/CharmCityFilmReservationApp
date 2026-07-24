using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CalendarSwipe : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform pages;

    private RectTransform viewport;
    [SerializeField] private float snapSpeed;

    [SerializeField] private float swipeThreshold = 100f;
    [SerializeField] private float spacing;
    [SerializeField] private AnimationCurve snapCurve;

    private Vector2 dragStartPosition;
    private float pageWidth;
    private Coroutine slideCoroutine;
    private bool snapping;
    private bool hasReordered;

    private void Start()
    {
        viewport = scrollRect.viewport;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        pageWidth = viewport.rect.width + spacing;

        float currentX = -pages.anchoredPosition.x;
        int currentPage = Mathf.RoundToInt(currentX / pageWidth);

        float dragDistance = pages.anchoredPosition.x - dragStartPosition.x;

        int targetPage = currentPage;

        // Swiped left (next page)
        if (dragDistance < -swipeThreshold)
        {
            targetPage++;
        }
        // Swiped right (previous page)
        else if (dragDistance > swipeThreshold)
        {
            targetPage--;
        }

        targetPage = Mathf.Clamp(targetPage, 0, pages.childCount - 1);
        scrollRect.velocity = Vector2.zero;
        slideCoroutine = StartCoroutine(Snap(targetPage));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
            slideCoroutine = null;
        }

        float x = pages.anchoredPosition.x;

        if (x <= -pageWidth * 1.5f)
        {
            MoveFirstPageToEnd();
        }
        else if (x >= -pageWidth * 0.5f)
        {
            MoveLastPageToBeginning();
        }
        else
        {
            // Return to center if we interrupted too early
            /*pages.anchoredPosition = new Vector2(
                -pageWidth,
                pages.anchoredPosition.y
            );*/
        }

        dragStartPosition = pages.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    private IEnumerator Snap(int page)
    {
        snapping = true;
        hasReordered = false;

        float targetX = page * pageWidth;

        Vector2 start = pages.anchoredPosition;
        Vector2 end = new Vector2(-targetX, pages.anchoredPosition.y);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * snapSpeed;

            float curveT = snapCurve.Evaluate(t);

            pages.anchoredPosition = Vector2.Lerp(start, end, curveT);


            // Reached the next/previous page
            if (!hasReordered)
            {
                if (page == 2 && pages.anchoredPosition.x <= -pageWidth * 2)
                {
                    MoveFirstPageToEnd();
                    snapping = false;
                    yield break;
                }
                if (page == 0 && pages.anchoredPosition.x >= 0)
                {
                    MoveLastPageToBeginning();
                    snapping = false;
                    yield break;
                }
            }

            yield return null;
        }

        pages.anchoredPosition = new Vector2(-pageWidth, pages.anchoredPosition.y);

        snapping = false;

        if (!hasReordered)
        {
            if (page == 2)
                MoveFirstPageToEnd();
            else if (page == 0)
                MoveLastPageToBeginning();
        }
    }

    private void MoveFirstPageToEnd()
    {
        Transform first = pages.GetChild(0);

        first.SetSiblingIndex(pages.childCount - 1);

        LayoutRebuilder.ForceRebuildLayoutImmediate(pages);

        pages.anchoredPosition = new Vector2(
            -pageWidth,
            pages.anchoredPosition.y
        );

        UpdateInfo(first.GetComponent<SingleMonthPage>(), true);
    }

    private void MoveLastPageToBeginning()
    {
        Transform last = pages.GetChild(pages.childCount - 1);

        last.SetSiblingIndex(0);

        LayoutRebuilder.ForceRebuildLayoutImmediate(pages);

        pages.anchoredPosition = new Vector2(
            -pageWidth,
            pages.anchoredPosition.y
        );

        UpdateInfo(last.GetComponent<SingleMonthPage>(), false);
    }

    private async void UpdateInfo(SingleMonthPage pageToUpdate, bool next)
    {
        await CalendarManager.Instance.ChangeMonth(pageToUpdate, next);
    }
}