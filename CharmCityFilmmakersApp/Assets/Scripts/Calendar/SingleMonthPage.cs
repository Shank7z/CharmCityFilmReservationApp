using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

[Serializable]
public class SingleMonthPage : MonoBehaviour
{
    public Transform header;
    public Transform grid;
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI yearText;

    public DateTime monthStart;
    public DateTime monthEnd;

    public int daysInMonth;
    public int dayOfWeek;

    public float offDayOpacity;

    public List<DayManager> days;

    public void SetDateTime(DateTime dt)
    {
        monthStart = dt;
        daysInMonth = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);
        monthEnd = monthStart.AddDays(daysInMonth - 1);
    }

    public void SetHeaderText()
    {
        monthText.text = monthStart.ToString("MMMM");
        yearText.text = monthStart.ToString("yyyy");
    }

    public async Task SetGrid(int dayCount)
    {
        dayOfWeek = (int)monthStart.DayOfWeek;
        DateTime workingDay = monthStart.AddDays(-dayOfWeek);
        List<Reservations> monthReservations = await SupabaseFunctionality.Instance.RetrieveReservations(workingDay, workingDay.AddDays(dayCount - 1));
        for (int x = 0; x < dayCount; x++)
        {
            DayArgs args = new DayArgs();
            Debug.Log("Entered loop");
            List<Reservations> dayReservations = monthReservations.Where(y => y.startTime.Date <= workingDay.Date && y.endTime.Date >= workingDay.Date).ToList();
            args.reservationList = dayReservations;
            args.date = workingDay;
            days[x].Setup(args);
            if (args.date.Month != monthStart.Month) days[x].GetComponent<CanvasGroup>().alpha = offDayOpacity;
            else days[x].GetComponent<CanvasGroup>().alpha = 1;
            workingDay = workingDay.AddDays(1);
        }
    }
}