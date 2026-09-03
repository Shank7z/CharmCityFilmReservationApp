using TMPro;
using UnityEngine;

public enum SelectorButtonType
{
    Int,
    String
}
public class OptionValue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public SelectorButtonType type;
    public string value;

    public Dropdown parentDropdown;

    public void Setup(Dropdown parent, SelectorButtonType type, string value)
    {
        parentDropdown = parent;
        this.value = value;
        this.type = type;
        text.text = value;
    }

    public void UpdateParentValue()
    {
        parentDropdown.SetDropDownValue(value);
    }
}
