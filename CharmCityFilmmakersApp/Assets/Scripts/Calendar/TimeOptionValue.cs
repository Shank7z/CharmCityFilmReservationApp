using TMPro;
using UnityEngine;

public enum SelectorButtonType
{
    Hour,
    Minute,
    AMPM,
    Day,
    Month,
    Year
}
public class TimeOptionValue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public SelectorButtonType type;
    public int value;
    public ScrollRectSelector parentSelector;

    private void Start()
    {
        text.text = value.ToString();
        if (text.text == "0") text.text = "00";
    }
    public void SendValue()
    {

        switch (type)
        {
            case SelectorButtonType.Hour:
                parentSelector.time.x = value;
                break;
            case SelectorButtonType.Minute:
                parentSelector.time.y = value;
                break;
            case SelectorButtonType.AMPM:
                parentSelector.ampm = value == 0 ? "am": "pm";
                break;
            case SelectorButtonType.Day:
                parentSelector.date.y = value;
                break;
            case SelectorButtonType.Month:
                parentSelector.date.x = value;
                break;
            case SelectorButtonType.Year:
                parentSelector.date.z = value;
                break;
            default:
                Debug.Log("Not found");
                break;
        }
        parentSelector.UpdateButtonText();
    }
}
