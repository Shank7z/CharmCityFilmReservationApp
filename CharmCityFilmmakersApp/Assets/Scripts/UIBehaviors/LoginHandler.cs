using NUnit;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour
{
    public TextMeshProUGUI debugTextBox;
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

    public CanvasGroup loginBucket;
    public RectTransform logo;

    [SerializeField] private float fadeSpeed;
    [SerializeField] private AnimationCurve logoMoveCurve;
    [SerializeField] private float logoAnimSpeed;
    [SerializeField] private AnimationCurve logoResizeCurve;

    [SerializeField] private Vector2 logoStartPos;
    [SerializeField] private Vector2 logoEndPos;
    [SerializeField] private Vector2 logoStartSize;
    [SerializeField] private Vector2 logoEndSize;

    public async Task<Profiles> AttemptLogin(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(usernameField.text) || string.IsNullOrWhiteSpace(passwordField.text)) return null;
        var profile = await SupabaseFunctionality.Instance.RetrieveProfileByUsername(username);

        if (profile == null)
        {
            Debug.Log("No profile with that username");
            debugTextBox.text = "Invalid username";
            return null;
        }

        if(profile.userStatus == Profiles.user_status.inactive)
        {
            Debug.Log("Account not currently active");
            debugTextBox.text = "Account deactivated";
            return null;
        }

        if (password == profile.password)
        {
            Debug.Log("Login Worked");
            debugTextBox.text = "";
            SupabaseFunctionality.Instance.currentUserID = profile.id;
            SupabaseFunctionality.Instance.isAdmin = profile.userStatus == Profiles.user_status.admin;
            AppController.Instance.ChangeScreen(Screens.Calendar);
        }
        else
        {
            Debug.Log("Incorrect Password");
            debugTextBox.text = "Incorrect password";
        } 
        return null;

    }

    public async void BeginAttemptLogin()
    {
        await AttemptLogin(usernameField.text, passwordField.text);
    }

    public void Load()
    {
        StartCoroutine(InitializeScreen());
    }

    public IEnumerator InitializeScreen()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(MoveLogo());
        yield return StartCoroutine(FadeInLogin());
    }

    private IEnumerator MoveLogo()
    {
        float t = 0;
        while (t <1)
        {
            t += Time.deltaTime * logoAnimSpeed;

            float moveT = logoMoveCurve.Evaluate(t);
            float resizeT = logoResizeCurve.Evaluate(t);
            logo.anchoredPosition = Vector2.Lerp(logoStartPos, logoEndPos, moveT);
            logo.localScale = Vector2.Lerp(logoStartSize, logoEndSize, resizeT);
            yield return null;
        }
    }

    private IEnumerator FadeInLogin()
    {
        while(loginBucket.alpha < 1)
        {
            loginBucket.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

    }
}
