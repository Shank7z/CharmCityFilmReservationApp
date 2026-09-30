using TMPro;
using UnityEngine;

public class RoomID : MonoBehaviour
{
    public int roomID;

    public Color defaultColor;
    public Color selectedColor;

    public void ChangeTextColor(bool b)
    {
        transform.GetComponentInChildren<TextMeshProUGUI>().color = b ? selectedColor : defaultColor;
    }
}
