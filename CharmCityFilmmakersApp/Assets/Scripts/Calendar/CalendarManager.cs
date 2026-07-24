using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using System.Threading.Tasks;
public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance;
    public List<SingleMonthPage> pages;
    public Transform dayPrefab;
    public int daysToDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        LayoutSetup();
        PageSetup();
    }



    private async void PageSetup()
    {
        DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        for (int x=0; x< pages.Count; x++)
        {
            pages[x].SetDateTime(currentMonthStart.AddMonths(x - 1));
            pages[x].SetHeaderText();
            await pages[x].SetGrid(daysToDisplay);
        }
    }

    public void LayoutSetup()
    {
        for(int x=0; x < pages.Count; x++)
        {
            for (int y = 0; y < daysToDisplay; y++)
            {
                Transform day = Instantiate(dayPrefab, pages[x].grid);
                pages[x].days.Add(day.GetComponent<DayManager>());
            }
        }

    }

    public async Task ChangeMonth(SingleMonthPage pageToUpdate, bool next)
    {
        int amountToChange = pages.Count;
        int change = next ? amountToChange : -amountToChange;

        pageToUpdate.SetDateTime(pageToUpdate.monthStart.AddMonths(change));
        pageToUpdate.SetHeaderText();
        await pageToUpdate.SetGrid(daysToDisplay);
    }
}
