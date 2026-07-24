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
