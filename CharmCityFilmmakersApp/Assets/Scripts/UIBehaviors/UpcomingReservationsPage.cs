using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpcomingReservationsPage : MonoBehaviour
{
    public ReservationPopup reservationPopup;
    List<Reservations> reservations;
    public Transform singleReservationPrefab;
    public Transform contentTransform;
    private void OnEnable()
    {
        Setup();
    }
    private void OnDisable()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public async Task GetReservations()
    {
        List<Reservations> reservations = await SupabaseFunctionality.Instance.GetReservationsForUser(SupabaseFunctionality.Instance.currentUserID);

        var reservationGroups = reservations.GroupBy(r => r.groupID);

        foreach (var group in reservationGroups)
        {
            Transform prefab = Instantiate(singleReservationPrefab,contentTransform);

            prefab.GetComponent<SingleReservation>().Setup(group.ToList());
        }
        RectTransform scrollBox = transform.GetChild(2).GetChild(0).GetComponent<RectTransform>();
        scrollBox.sizeDelta = new Vector2(scrollBox.sizeDelta.x, reservationGroups.Count() * singleReservationPrefab.GetComponent<RectTransform>().rect.height);
        Debug.LogWarning("New height is " + reservationGroups.Count() * singleReservationPrefab.GetComponent<RectTransform>().rect.height);
    }


    public async Task Setup()
    {
        await GetReservations();
    }


}
