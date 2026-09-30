using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleReservation : MonoBehaviour
{
    private int groupID;
    private ReservationPopup popup;
    public Button editButton;
    public Button deleteButton;

    public Transform[] roomIcons;

    public TextMeshProUGUI dateText;
    public TextMeshProUGUI titleText;
    public void Setup(List<Reservations> reservations)
    {
        popup = GetComponentInParent<UpcomingReservationsPage>(true).reservationPopup;
        groupID = (int)reservations[0].groupID;
        DateTime start = reservations[0].startTime;
        DateTime end = reservations[0].endTime;
        deleteButton.onClick.AddListener(DeleteReservation);
        editButton.onClick.AddListener(OpenEditPopup);
        titleText.text = reservations[0].title;
        dateText.text = "Start: " + start.ToString("d") +" " + start.ToString("t") + 
            "\n" +
            "End:  " + end.ToString("d") + " " + end.ToString("t");

        foreach (Reservations r in reservations)
        {
            roomIcons[(int)r.roomID-1].gameObject.SetActive(true);
        }
    }

    private async void DeleteReservation()
    {
        bool success = await SupabaseFunctionality.Instance.DeleteReservationGroup(groupID);

        if (success)
        {
            Destroy(gameObject);
        }
        await CalendarManager.Instance.RefreshCalendar();
    }

    public void OpenEditPopup()
    {
        //CalendarManager.Instance.ShowReservationPopup(new DayArgs { date, });
        popup.editing = true;
    }
}
