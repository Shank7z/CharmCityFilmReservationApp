using System;
using UnityEngine;
using System.Collections;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;
// need title and notes
public class ReservationPopup : MonoBehaviour
{
    public bool editing = false;
    public int editingGroupID;
    public static ReservationPopup Instance;
    [SerializeField] private DayArgs info;
    [SerializeField] private Transform reservationBarHolder;
    [SerializeField] private Transform reservationBarPrefab;
    [SerializeField] private Transform ErrorPopup;
    public TextMeshProUGUI dateText;
    public RoomSelector selector;
    public DropdownMenu startDate;
    public DropdownMenu endDate;
    public DropdownMenu startTime;
    public DropdownMenu endTime;
    public DropdownMenu repetition;

    public TMP_InputField titleBox;
    public TMP_InputField notesBox;

    public List<Color> roomColors;

    private Coroutine fadeCoroutine;
    public float fadeSpeed;

    public DateTime start;
    public DateTime end;
    public void Setup(DayArgs da)
    {
        this.info = da;
        dateText.text = info.date.ToString("D");
        StartCoroutine(SetDropdownValuesDelay());
        SetupDisplay();
        StartFade(true);
    }

    public void ToggleErrorPopup(bool b)
    {
        ErrorPopup.gameObject.SetActive(b);
    }

    private void SetupDisplay()
    {

        DateTime dayStart = info.date;
        DateTime dayEnd = dayStart.AddDays(1);

        foreach (Reservations r in info.reservationList)
        {
            if (r.endTime <= dayStart || r.startTime >= dayEnd)
                continue;
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                reservationBarHolder.GetComponent<RectTransform>()
            );
            int totalWidth = (int)reservationBarHolder
                .GetComponent<RectTransform>().rect.width;

            int anchoredX = (totalWidth / 5) * ((int)r.roomID - 1);

            DateTime displayStart = r.startTime > dayStart
                ? r.startTime
                : dayStart;

            DateTime displayEnd = r.endTime < dayEnd
                ? r.endTime
                : dayEnd;

            float startHour = (float)(displayStart - dayStart).TotalHours;
            float duration = (float)(displayEnd - displayStart).TotalHours;

            Transform temp = Instantiate(
                reservationBarPrefab,
                reservationBarHolder
            );

            temp.GetComponent<Image>().color =
                roomColors[(int)r.roomID - 1];

            RectTransform tempRect =
                temp.GetComponent<RectTransform>();

            tempRect.anchoredPosition = new Vector2(
                anchoredX,
                (-startHour * 100) - 1
            );

            tempRect.sizeDelta = new Vector2(
                tempRect.rect.width,
                (duration * 100) - 2
            );
        }
    }
    private void SetStartingValues(DropdownMenu dropdown, bool isDate, bool start)
    {
        if (isDate)
        {
            dropdown.dropdowns[0].SetDropDownValue(info.date.Month.ToString());
            dropdown.dropdowns[1].SetDropDownValue(info.date.Day.ToString());
            dropdown.dropdowns[2].SetDropDownValue(info.date.Year.ToString());
        }

        else
        {
            int hourIncrease = 1;
            if (start) hourIncrease = 0;
            int ampm = DateTime.Now.Hour + hourIncrease > 12 ? 12 : 0;
            string ampmText = ampm == 0 ? "am" : "pm";

            dropdown.dropdowns[0].SetDropDownValue((DateTime.Now.AddHours(hourIncrease).Hour - ampm).ToString());
            dropdown.dropdowns[1].SetDropDownValue("00");
            dropdown.dropdowns[2].SetDropDownValue(ampmText);
        }
    }
    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateTimes()
    {
        SetStartingValues(startDate, true, true);
        SetStartingValues(startTime, false, true);
        SetStartingValues(endDate, true, false);
        SetStartingValues(endTime, false, false);
        repetition.dropdowns[0].SetDropDownValue("Does not repeat");
    }

    public void Close()
    {
        editing = false;
        Refresh();
        Debug.Log("Attempting to close");
        StartFade(false);
    }

    private async Task Refresh()
    {
        Debug.Log("Refreshing");
        await CalendarManager.Instance.RefreshCalendar();
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
        if (!alphaIncrease)
        {
            titleBox.text = string.Empty;
            notesBox.text = string.Empty;
            Debug.Log("VALUE: " + titleBox.text);
            Debug.Log("VALUE: " + notesBox.text);
            gameObject.SetActive(false);
            foreach (Transform t in reservationBarHolder)
            {
                Destroy(t.gameObject);
            }
            ToggleErrorPopup(false);
        }
    }

    public void SubmitReservations()
    {
        Debug.Log("Button clicked");
        _ = AttemptCreateReservations();

    }
    public async Task AttemptCreateReservations()
    {
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
            ErrorPopupManager.Instance.CreatePopup("Select at least 1 room to reserve", "ok", "Cancel", false);
            return;
        }

        if(titleBox.text == string.Empty)
        {
            ErrorPopupManager.Instance.CreatePopup("Title required", "ok", "Cancel", false);
            return;
        }

        int recurrenceAmount = 1;
        int interval = 0;
        if (repetition.GetString() == "Repeats weekly")
        {
            recurrenceAmount = 104;
            interval = 1;
        }
        else if (repetition.GetString() == "Repeats monthly")
        {
            recurrenceAmount = 24;
            interval = 2;
        }

        Debug.Log("Acquiring dateTimes");
        DateTime tempTime = startTime.GetTime();
        DateTime tempDate = startDate.GetDate();

        Debug.Log("Starting reservation attempt");
        start = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, tempTime.Hour, tempTime.Minute, 0);
        tempTime = endTime.GetTime();
        tempDate = endDate.GetDate();
        end = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, tempTime.Hour, tempTime.Minute, 0);

        if (end < start)
        {
            Debug.Log("Invalid end date/time");
            return;
        }

        Debug.Log("Start: " + start.ToString("yyyy-MM-dd HH:mm:ss"));
        Debug.Log("End:   " + end.ToString("yyyy-MM-dd HH:mm:ss"));
        Debug.Log("Duration: " + (end - start).TotalHours + " hours");
        Debug.Log("Start: " + start.ToString("f") + " - " + end.ToString("f"));
        bool success = false;
        int groupID = editing ? editingGroupID : SupabaseFunctionality.Instance.currentUserID * 10000000 + UnityEngine.Random.Range(0, 9999999);
        
        //Debug.Log("Sending reservation request: " + x);
        if (!editing)
        {
            for (int x = 0; x < recurrenceAmount; x++)
            {
                success = await SupabaseFunctionality.Instance.CreateReservations(
                selectedRoomIDs,
                start,
                end,
                groupID,
                titleBox.text == null ? string.Empty: titleBox.text,
                notesBox.text == null ? string.Empty: notesBox.text);
                if (interval == 1)
                {
                    start = start.AddDays(7);
                    end = end.AddDays(7);
                }
                else if (interval == 2)
                {
                    start = start.AddMonths(1);
                    end = end.AddMonths(1);
                }
            }
        }
        else
        {
            success = await SupabaseFunctionality.Instance.EditReservationGroup(
                groupID,
                selectedRoomIDs,
                start,
                end);
        
        }
        //Debug.Log("Reservation request returned: " + x);

    



        if (success)
        {
            Debug.Log("Reservation created!");

            Close();
        }
        else
        {
            Debug.Log("One or more rooms are unavailable.");

            ToggleErrorPopup(true);
        }
    }

    private IEnumerator SetDropdownValuesDelay()
    {
        yield return new WaitForSeconds(.1f);
        UpdateTimes();
        if (!SupabaseFunctionality.Instance.isAdmin)
        {
            repetition.gameObject.SetActive(false);
        }
    }
}
