using System;
using UnityEngine;
using System.Collections;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ReservationPopup : MonoBehaviour
{
    public static ReservationPopup Instance;
    [SerializeField] private DayArgs info;
    public TextMeshProUGUI dateText;
    public RoomSelector selector;
    public ScrollRectSelector[] dateTimeSelectors;

    private Coroutine fadeCoroutine;
    public float fadeSpeed;

    public DateTime start;
    public DateTime end;
    public void Setup(DayArgs da)
    {
        this.info = da;
        dateText.text = info.date.ToString("D");
        StartFade(true);
        foreach(ScrollRectSelector s in dateTimeSelectors)
        {
            s.Setup(da);
        }
    }
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateTimes()
    {
        this.start = new DateTime(info.date.Year, info.date.Month, info.date.Day, (int)dateTimeSelectors[0].time.x, (int)dateTimeSelectors[0].time.y,0);
        this.end = new DateTime((int)dateTimeSelectors[2].date.z, (int)dateTimeSelectors[2].date.x, (int)(dateTimeSelectors[2].date.y), (int)dateTimeSelectors[1].time.x, (int)dateTimeSelectors[1].time.y, 0);
    }

    public void Close()
    {
        Debug.Log("Attempting to close");
        StartFade(false);
    }

    public void StartFade(bool alphaIncrease)
    {

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        fadeCoroutine = StartCoroutine(Fade(GetComponent<CanvasGroup>().alpha <= 0));
    }



    public IEnumerator Fade(bool alphaIncrease)
    {
        float change = alphaIncrease ? 1f : -1f;
        float target = alphaIncrease ? 1f : 0f;

        while (GetComponent<CanvasGroup>().alpha != target)
        {
            GetComponent<CanvasGroup>().alpha += fadeSpeed * change * Time.deltaTime;
            yield return null;
        }
        if (!alphaIncrease) gameObject.SetActive(false);
    }

    public void SubmitReservations()
    {

        Debug.Log("Button clicked");
        _ = AttemptCreateReservations();
    }
    public async Task AttemptCreateReservations()
    {
        Debug.Log("Starting reservation attempt");
        List<int> selectedRoomIDs = new();

        for (int x = 0; x < selector.roomButtons.Length; x++)
        {
            if (selector.roomButtons[x].isOn)
            {
                selectedRoomIDs.Add(selector.roomButtons[x].GetComponent<RoomID>().roomID);
            }
        }

        Debug.Log("Acquired room ids");

        if (selectedRoomIDs.Count == 0)
        {
            Debug.Log("Select at least one room.");
            return;
        }

        Debug.Log("Sending reservation request");

        bool success = await SupabaseFunctionality.Instance.CreateReservations(
            selectedRoomIDs,
            start,
            end
        );

        Debug.Log("Reservation request returned");

        if (success)
        {
            Debug.Log("Reservation created!");

            // Close popup
            // Refresh calendar
            // Clear selections
        }
        else
        {
            Debug.Log("One or more rooms are unavailable.");

            // Show error message to user
        }
    }
}
