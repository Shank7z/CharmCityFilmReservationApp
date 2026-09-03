using System.Collections.Generic;
using UnityEngine;
public enum ValueType
{
    Int,
    Float,
    String,
    Vector2,
    Vector3
}

public class Dropdown : MonoBehaviour
{
    [SerializeField] private string dropdownValue;

    [Header("References")]
    public DropdownMenu menu;
    public List<OptionValue> buttons = new();
    public string[] desiredValues;
    public Transform optionValuePrefab;
    public RectTransform contentTransform;
    [SerializeField] private RectTransform viewport;

    [Header("Options")]
    public ValueType valueType;
    public float desiredWidth;
    public Vector2 buttonDimensions;
    public int sequentialStartValue;

    public string Value => dropdownValue;
    public void Create(DropdownMenu menu)
    {
        this.menu = menu;
    }

    public void Setup(DropdownMenu menu, DropdownBuilder builder)
    {
        this.menu = menu;
        this.valueType = builder.valueType;
        this.dropdownValue = builder.defaultValue;
        this.desiredValues = builder.buttonValues;
        this.buttonDimensions = builder.buttonDimensions;
        this.sequentialStartValue = builder.sequentialStartingValue;
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, menu.dropdownDimensions.y);

        contentTransform.sizeDelta = new Vector2(contentTransform.rect.width, desiredValues.Length * buttonDimensions.y);
        for (int x = 0; x < desiredValues.Length; x++)
        {
            buttons.Add(Instantiate(optionValuePrefab, contentTransform).GetComponent<OptionValue>());
            string chosenValue = desiredValues[x] == null || desiredValues[x] == string.Empty ? (sequentialStartValue + x+1).ToString() : desiredValues[x];
            buttons[x].Setup(this, ChildValueType(), chosenValue);
            buttons[x].GetComponent<RectTransform>().sizeDelta = buttonDimensions;
        }
    }

    private SelectorButtonType ChildValueType()
    {
        switch(valueType)
        {
            case ValueType.String:
                return SelectorButtonType.String;
            default:
                return SelectorButtonType.Int;
        }
    }

    public void SetDropDownValue(string value)
    {
        dropdownValue = value;
        menu.UpdateText();
    }

}
