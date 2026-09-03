using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RoomSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CanvasGroup roomButtonBucket;
    public Toggle[] roomButtons;
    public TextMeshProUGUI roomCountText;

    public float fadeSpeed;
    private Coroutine fadeCoroutine;
    private int roomCount;

    private void Start()
    {
        roomCount = 0;
        foreach(Toggle t in roomButtons)
        {
            roomCount += t.isOn ? 1 : 0;
        }
        roomCountText.text = roomCount.ToString();
    }

    public void ToggleShowButtons()
    {

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        fadeCoroutine = StartCoroutine(Fade(!roomButtonBucket.gameObject.activeSelf));
    }



    public IEnumerator Fade(bool alphaIncrease)
    {
        if(alphaIncrease) roomButtonBucket.gameObject.SetActive(true);
        float change = alphaIncrease ? 1f : -1f;
        float target = alphaIncrease ? 1f : 0f;

        while (roomButtonBucket.alpha != target)
        {
            roomButtonBucket.alpha += fadeSpeed * change * Time.deltaTime;
            yield return null;
        }
        if (!alphaIncrease) roomButtonBucket.gameObject.SetActive(false);
    }

    public void ToggleChange(bool b)
    {
        roomCount += b ? 1 : -1;
        roomCountText.text = roomCount.ToString();
    }
}
