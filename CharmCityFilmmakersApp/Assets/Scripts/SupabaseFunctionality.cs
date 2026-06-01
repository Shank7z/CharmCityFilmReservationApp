using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using TMPro;
using Supabase.Postgrest.Models;

public class SupabaseFunctionality : MonoBehaviour
{

    public enum TableType
    {
        Profile,
        Room,
        Reservation
    }
    public TextMeshProUGUI testTextBox;

    public const string SUPABASE_URL = "https://xozwpzdevpxfduhyyjpt.supabase.co";

    public const string SUPABASE_KEY = "sb_publishable_0k-mq65TqOs-drrYps0zUg_QAtfZ4wW";

    private static Client _supabase;

    public static Client supabase => _supabase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        await Initialize();
        await Test();
    }

    private static async Task Initialize()
    {
        Debug.Log("Initializing");
        if (_supabase != null) return;
        _supabase = new Client(SUPABASE_URL, SUPABASE_KEY);
        await _supabase.InitializeAsync();
    }

    private async Task Test()
    {
        Debug.Log("Testing");
        if (_supabase == null)
        {
            Debug.LogWarning("Supabase not initialized yet");
            return;
        }
        var result = await Retrieve<Rooms>(123123);


        testTextBox.text = "" + result.name + "\n" + result.description + "\n Active: " + result.active;
    }

    public async Task<T> Retrieve<T>(int id) where T : BaseModel, IHasID, new()
    {
        var result = await _supabase
            .From<T>()
            .Where(x => x.id == id)
            .Single();

        return result;
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
