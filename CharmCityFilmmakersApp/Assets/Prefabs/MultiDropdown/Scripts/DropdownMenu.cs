using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DropdownType
{
    String,
    Float,
    Vector3,
    Date,
    Time,
}

public class DropdownMenu : MonoBehaviour
{
    [Header("References")]
    public List<Dropdown> dropdowns;
    public Transform dropdownFolder;
    public Button openDropdownButton;
    public TextMeshProUGUI displayText;
    public Transform dropdownPrefab;
    public Transform closeArea;

    [Header("Dropdown Options")]
    public DropdownType preset;
    public List<DropdownBuilder> dropdownBuilders;
    public Vector2 dropdownDimensions;
    public bool dropdownGoesDown;
    public string displayFormat;
    public float dropdownFadeSpeed;

    private Coroutine fadeCoroutine;
    [SerializeField] private string finalDisplayText;

    public int TotalDropdowns => dropdowns.Count;

    private void Start()
    {
        SetText(displayFormat);
        CreateDropdowns();
        dropdownFolder.GetComponent<RectTransform>().sizeDelta = new Vector2(0, dropdownDimensions.y);
        GetComponent<RectTransform>().sizeDelta = dropdownDimensions;
        UpdateText();
    }

    public string GetString()
    {
        string finalValue = string.Empty;
        for(int x=0; x<dropdowns.Count; x++)
        {
            finalValue = finalValue + dropdowns[x].Value;
        }
        return finalValue;
    }

    public float GetFloat(bool combined)
    {
        float finalValue = 0;

        for(int x=0;x<dropdowns.Count; x++)
        {
            if (combined) finalValue += int.Parse(dropdowns[x].Value);
            else
            {
                int multiplier = (int)Mathf.Pow(10, dropdowns.Count-x);
                finalValue += int.Parse(dropdowns[x].Value) * multiplier;
            }
        }
        return finalValue;
    }

    public Vector3 GetVector3()
    {
        if (dropdowns.Count > 2) return new Vector3(float.Parse(dropdowns[0].Value), float.Parse(dropdowns[1].Value), float.Parse(dropdowns[2].Value));
        else if (dropdowns.Count > 1) return new Vector2(float.Parse(dropdowns[0].Value), float.Parse(dropdowns[1].Value));
        else return Vector3.zero;
    }

    public DateTime GetDate()
    {
        return new DateTime(int.Parse(dropdowns[2].Value), int.Parse(dropdowns[0].Value), int.Parse(dropdowns[1].Value));
    }

    public DateTime GetTime()
    {
        int ampm = dropdowns[2].Value == "am" ? 0:12;
        bool twelveException = int.Parse(dropdowns[0].Value) == 12 && ampm == 0;
        int hourValue;
        if (twelveException) hourValue = 0;
        else hourValue = int.Parse(dropdowns[0].Value);
        return new DateTime(1, 1, 1, hourValue + ampm, int.Parse(dropdowns[1].Value), 0);
    }



    public void UpdateText()
    {
        finalDisplayText = displayFormat;
        for (int x = 0; x < dropdowns.Count; x++)
        {
            if (x > dropdowns.Count) continue;
            int index = finalDisplayText.IndexOf($"{x}*");
            finalDisplayText = finalDisplayText.Remove(index, 2).Insert(index, dropdowns[x].Value);
            Debug.Log("Updated button text with value: " + dropdowns[x].Value + " in position: " + x
                + "\n" + "String index was " + index);
        }
        displayText.text = finalDisplayText;
    }   

    public void SetText(string s)
    {
        displayText.text = s;
    }

    public void ToggleDropdown()
    {
        StartFade(!dropdownFolder.gameObject.activeSelf);
        closeArea.gameObject.SetActive(!closeArea.gameObject.activeSelf);
    }

    private void CreateDropdowns()
    {
        for (int x = 0; x< dropdownBuilders.Count; x++)
        {
            dropdowns.Add(Instantiate(dropdownPrefab, dropdownFolder).GetComponent<Dropdown>());
            dropdowns[x].Setup(this, dropdownBuilders[x]);
        }
    }

    public void StartFade(bool alphaIncrease)
    {

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        fadeCoroutine = StartCoroutine(Fade(dropdownFolder.GetComponent<CanvasGroup>().alpha <= 0));
    }



    public IEnumerator Fade(bool alphaIncrease)
    {
        if(!dropdownFolder.gameObject.activeSelf) dropdownFolder.gameObject.SetActive(true);
        CanvasGroup dropdownCanvasGroup = dropdownFolder.GetComponent<CanvasGroup>();
        float change = alphaIncrease ? 1f : -1f;
        float target = alphaIncrease ? 1f : 0f;

        while (dropdownCanvasGroup.alpha != target)
        {
            dropdownCanvasGroup.alpha += dropdownFadeSpeed * change * Time.deltaTime;
            yield return null;
        }
        if (!alphaIncrease) dropdownFolder.gameObject.SetActive(false);
    }

}
