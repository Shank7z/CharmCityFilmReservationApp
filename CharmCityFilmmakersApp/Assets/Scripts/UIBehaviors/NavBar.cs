using UnityEngine;

public class NavBar : MonoBehaviour
{

    public Transform myReservationsScreen;

    public void ToggleMyReservationsScreen(bool b)
    {
        myReservationsScreen.gameObject.SetActive(b);
    }
}
