using UnityEngine;
using UnityEngine.EventSystems;

public class CalendarSwipe : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool swipeByScreenPercentage = false;
    public float validSwipePercentage;
    [SerializeField] private float minimumSwipeDistance;
    private Vector2 startPosition;

    private void Start()
    {
        if(swipeByScreenPercentage) minimumSwipeDistance = Screen.currentResolution.width * validSwipePercentage;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = eventData.position;
        Debug.Log("SwipeStarted");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 difference = eventData.position - startPosition;
        if (difference.x > minimumSwipeDistance)
        {
            Debug.Log("Previous month");
            CalendarManager.Instance.ChangeMonth(false);
        }
        else if (difference.x < -minimumSwipeDistance)
        {
            Debug.Log("Next month");
            CalendarManager.Instance.ChangeMonth(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}