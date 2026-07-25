using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum SelectorType
{
    Date,
    Time
}
public class ScrollRectSelector : MonoBehaviour
{
    public SelectorType type;
    public bool start;
    public List<ScrollRect> scrollRects;
    public TextMeshProUGUI buttonText;
    public Vector2 time;
    public string ampm;
    public Vector3 date;

    private void Start()
    {
        
    }

    public void Setup(DayArgs args)
    {
        date.x = args.date.Month;
        date.y = args.date.Day;
        date.z = args.date.Year;

        time.x = args.date.Hour;
        time.y = args.date.Minute;
    }
    public void ShowRects()
    {
        scrollRects[0].transform.parent.gameObject.SetActive(true);
    }

    public void UpdateButtonText()
    {
        if(type == SelectorType.Date)
        {
            buttonText.text = "" + date.x + "/" + date.y + "/" + date.z;
        }
        else if(type == SelectorType.Time)
        {
            buttonText.text = "" + time.x + ":" + time.y + ampm;
        }
    }
}
