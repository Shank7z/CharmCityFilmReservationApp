using UnityEngine;

public class ErrorPopupManager : MonoBehaviour
{
    public static ErrorPopupManager Instance;
    public RectTransform ErrorPopupPrefab;

    public ErrorPopup currentPopup;
    public Vector2 popupLocation;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        //popupLocation = new Vector2(Screen.width/2, Screen.height/2);
    }

    public void CreatePopup(string message, string b1Text, string b2Text, bool extraButton)
    {
        if(currentPopup != null) Destroy(currentPopup.gameObject);
        currentPopup = Instantiate(ErrorPopupPrefab, FindFirstObjectByType<Canvas>().transform).GetComponent<ErrorPopup>();
        currentPopup.Setup(popupLocation, message, b1Text, b2Text, extraButton);
    }
}
