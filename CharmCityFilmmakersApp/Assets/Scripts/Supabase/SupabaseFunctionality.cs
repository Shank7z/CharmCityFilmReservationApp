using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using TMPro;
using Supabase.Postgrest.Models;
using System;
using System.Linq;
using System.Collections.Generic;

public class SupabaseFunctionality : MonoBehaviour
{
    public static SupabaseFunctionality Instance { get; private set; }

    public static event Action OnSupabaseConnected;

    public enum TableType
    {
        Profile,
        Room,
        Reservation
    }
    public TextMeshProUGUI testTextBox;

    public const string SUPABASE_URL = "https://xozwpzdevpxfduhyyjpt.supabase.co";

    public const string SUPABASE_KEY = "sb_publishable_0k-mq65TqOs-drrYps0zUg_QAtfZ4wW";

    public static Client _supabase { get; private set; }

    public static Client supabase => _supabase;

    public int currentUserID;

    public bool isAdmin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        await Initialize();
    }

    private static async Task Initialize()
    {
        Debug.Log("Initializing");
        if (_supabase != null) return;
        _supabase = new Client(SUPABASE_URL, SUPABASE_KEY);
        await _supabase.InitializeAsync();
        OnSupabaseConnected?.Invoke();
    }

    public async Task<T> RetrieveByID<T>(int id) where T : BaseModel, IHasID, new()
    {
        var result = await _supabase
            .From<T>()
            .Where(x => x.id == id)
            .Single();

        return result;
    }

    public async Task<Profiles> RetrieveProfileByUsername(string username)
    {
        var result = await _supabase.From<Profiles>().Where(x => x.username == username).Single();
        return result;
    }

    public async Task<List<Reservations>> RetrieveReservations(DateTime start, DateTime end)
    {
        var result = await _supabase.From<Reservations>().Where(x => (x.endTime > start && x.startTime < end)).Get();
        return result.Models.ToList<Reservations>();
    }

    public async Task<bool> CreateReservations( List<int> roomIDs, DateTime start, DateTime end, int groupID, string title, string notes)
    {
        var parameters = new Dictionary<string, object>
    {
        { "p_room_ids", roomIDs.ToArray() },
        { "p_user_id", currentUserID },
        { "p_group_id", groupID},
        { "p_start_time", start },
        { "p_end_time", end },
        { "p_notes", notes },
        { "p_title", title }
    };
        Debug.Log("Calling sql querry");
        var result = await supabase.Rpc<bool>(
            "create_reservations",
            parameters
        );

        Debug.Log(result);

        return result;
    }

    public async Task<bool> EditReservationGroup(int groupID, List<int> roomIDs, DateTime start, DateTime end)
    {
        var parameters = new Dictionary<string, object>
    {
        { "p_group_id", groupID },
        { "p_user_id", currentUserID },
        { "p_room_ids", roomIDs.ToArray() },
        { "p_start_time", start },
        { "p_end_time", end }
    };

        Debug.Log("Editing reservation group...");

        var result = await supabase.Rpc<bool>(
            "edit_reservation_group",
            parameters
        );

        Debug.Log("Edit result: " + result);

        return result;
    }

    public async Task<bool> DeleteReservationGroup(int groupID)
    {
        var parameters = new Dictionary<string, object>
    {
        { "p_group_id", groupID },
        { "p_user_id", currentUserID }
    };

        var result = await supabase.Rpc<bool>(
            "delete_reservation_group",
            parameters
        );

        return result;
    }

    public async Task<List<Reservations>> GetReservationsForUser(int userID)
    {
        var result = await supabase
            .From<Reservations>()
            .Where(r => r.userId == userID)
            .Where(r => r.endTime >= DateTime.Today)
            .Order(r => r.startTime, Supabase.Postgrest.Constants.Ordering.Ascending)
            .Get();

        return result.Models;
    }



    private async Task<Profiles> RetrieveProfile(int id)
    {
        var result = await _supabase.From<Profiles>().Where(x => x.id == id).Single();
        return result;
    }


    private async Task<Rooms> RetrieveRoom(int id)
    {
        var result = await _supabase.From<Rooms>().Where(x => x.id == id).Single();
        return result;
    }

    private async Task<Reservations> RetrieveReservation(int id)
    {
        var result = await _supabase.From<Reservations>().Where(x => x.id == id).Single();
        return result;
    }
}
