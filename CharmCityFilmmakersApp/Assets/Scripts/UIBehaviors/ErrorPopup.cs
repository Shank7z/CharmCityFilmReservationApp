using TMPro;
using UnityEngine;
using System.Collections;

public class ErrorPopup : MonoBehaviour
{
    public RectTransform rTransform;

    public CanvasGroup cg;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;

    public RectTransform button1;

    public float fadeSpeed = 3;

    public void Setup(Vector2 location, string message, string b1text, string b2text, bool extraButton)
    {
        cg.alpha = 0;
        rTransform.anchoredPosition = location;
        messageText.text = message;
        button2Text.text = b1text;
        if(extraButton)
        {
            button1Text.text = b2text;
        }
        button1.gameObject.SetActive(extraButton);
            StartCoroutine(Fade(true));
    }
    public void ClosePopup()
    {
        StartCoroutine(Fade(false));
    }

    public IEnumerator Fade(bool alphaIncrease)
    {
        float change = alphaIncrease ? 1f : -1f;
        float target = alphaIncrease ? 1f : 0f;

        while (GetComponent<CanvasGroup>().alpha != target)
        {
            GetComponent<CanvasGroup>().alpha += fadeSpeed * change * Time.deltaTime;
            yield return null;
        }
        if (!alphaIncrease)
        {
            Destroy(gameObject);
        }
    }
}
