using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour
{
    public TextMeshProUGUI debugTextBox;
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

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
}
