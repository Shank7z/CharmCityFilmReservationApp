using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Screens
{
    Splash,
    Login,
    Calendar
}
public class AppController : MonoBehaviour
{
    public static AppController Instance { get; private set; }

    public Transform splashScreen;
    public Transform loginScreen;
    public Transform loadingScreen;
    public Transform calendarScreen;

    public Transform currentScreen;

    private void OnEnable()
    {
        SupabaseFunctionality.OnSupabaseConnected += LoadApp;
    }

    private void OnDisable()
    {
        SupabaseFunctionality.OnSupabaseConnected -= LoadApp;
    }

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentScreen = splashScreen;
    }

    public void LoadApp()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(1f);
        ChangeScreen(Screens.Login);
    }

    public void ChangeScreen(Screens screen)
    {
        StartCoroutine(ScreenTransition(screen));
    }

    private IEnumerator ScreenTransition(Screens desiredScreen)
    {
        Debug.Log("Reached coroutine");
        Transform newScreen;
        switch(desiredScreen)
        {
            case Screens.Splash:
                newScreen = splashScreen;
                break;
            case Screens.Login:
                newScreen = loginScreen;
                break;
            case Screens.Calendar:
                newScreen = calendarScreen;
                break;
            default:
                newScreen = loginScreen;
                break;

        }
        yield return StartCoroutine(FadeScreen(loadingScreen, true, 4f));
        newScreen.gameObject.SetActive(true);

        if (currentScreen != null) currentScreen.gameObject.SetActive(false);

        currentScreen = newScreen;
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeScreen(loadingScreen, false, 4f));

        yield return null;
    }

    private IEnumerator FadeScreen(Transform canvas, bool alphaIncrease, float speed)
    {
        if (alphaIncrease) loadingScreen.gameObject.SetActive(true);
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        Debug.Log("Loading screen coroutine");
        float finalAlpha = alphaIncrease ? 1.0f : 0.0f;

        while(cg.alpha != finalAlpha)
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, finalAlpha, speed * Time.deltaTime);
            yield return null;
        }
        if(!alphaIncrease) loadingScreen.gameObject.SetActive(false);
 
    }

}
