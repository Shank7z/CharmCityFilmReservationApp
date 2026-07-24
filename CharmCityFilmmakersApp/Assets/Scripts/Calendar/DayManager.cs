using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DayManager : MonoBehaviour
{
    public TextMeshProUGUI dateText;
    public Transform barHolder;
    public BarManager[] bars;

    public bool hideSurroundingDays;

    private DayArgs info;

    private Image background;
    private void Start()
    {
        if (dateText == null) dateText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        background = GetComponentInParent<Image>();
    }
    public void Setup(DayArgs info)
    {
        ToggleDay(true);
        Debug.Log("Running day setup");
        this.info = info;
        dateText.text = info.date.Day.ToString();
        if(info.reservationList?.Count > 0) barHolder.gameObject.SetActive(true);
        else barHolder.gameObject.SetActive(false);
        SetBars();
    }

    public void Clear()
    {
        dateText.text = null;
        barHolder.gameObject.SetActive(false);
        if (hideSurroundingDays)ToggleDay(false);
    }

    public void ToggleDay(bool on)
    {
        background.enabled = on;
    }

    public void SetBars()
    {
        Debug.Log("Toggling Bar Visibility");
        for(int x=0; x < bars.Length; x++)
        {
            List<Reservations> roomReserved = info.reservationList.Where(y => y.roomID == bars[x].roomID).ToList();
            if (roomReserved.Count <= 0) bars[x].GetComponent<CanvasGroup>().alpha = 0;
            else bars[x].GetComponent<CanvasGroup>().alpha = 1;
        }
    }
}
