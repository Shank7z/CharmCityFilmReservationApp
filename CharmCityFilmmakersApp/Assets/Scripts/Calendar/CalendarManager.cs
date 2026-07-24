using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance;
    public TextMeshProUGUI monthNameText;
    public TextMeshProUGUI yearNameText;
    public Transform grid;
    public Transform dayPrefab;
    public int daysToDisplay;
    public List<DayManager> days;

    private DateTime monthStart;
    private DateTime monthEnd;
    private int daysInMonth;

    [SerializeField] private List<Reservations> monthReservations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        
        monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        daysInMonth = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);
        monthEnd = monthStart.AddDays(daysInMonth - 1);

        LayoutSetup();
        MonthSetup();

    }



    private async void MonthSetup()
    {
        monthNameText.text = monthStart.ToString("MMMM");
        yearNameText.text = monthStart.ToString("yyyy");
        DayArgs args = new DayArgs { };
        Debug.Log("Running month setup");
        int dayOfWeek = (int)monthStart.DayOfWeek;
        DateTime currentDay = monthStart.AddDays(-dayOfWeek);
        monthReservations = await SupabaseFunctionality.Instance.RetrieveReservations(currentDay, currentDay.AddDays(daysToDisplay));
        Debug.Log("Retrieved reservations");

        for (int x = 0; x < daysToDisplay; x++)
        {
            Debug.Log("Entered loop");
            List<Reservations> dayReservations = monthReservations.Where(x => x.startTime.Date <= currentDay.Date && x.endTime.Date >= currentDay.Date).ToList();
            args.reservationList = dayReservations;
            args.date = currentDay;
            days[x].Setup(args);
            currentDay = currentDay.AddDays(1);
        }
    }

    public void LayoutSetup()
    {
        for (int x = 0; x < daysToDisplay; x++)
        {
            Transform day = Instantiate(dayPrefab, grid);
            days.Add(day.GetComponent<DayManager>());
        }
    }

    public void ChangeMonth(bool next)
    {
        int change = next ? 1 : -1;
        monthStart = monthStart.AddMonths(change);

        daysInMonth = DateTime.DaysInMonth(monthStart.Year, monthStart.Month);
        monthEnd = new DateTime(monthStart.Year, monthStart.Month, daysInMonth);
        MonthSetup();

    }
}
