using UnityEngine;
using Supabase;
using System.Threading.Tasks;
using TMPro;

public class SupabaseFunctionality : MonoBehaviour
{
    public TextMeshProUGUI testTextBox;

    public const string SUPABASE_URL = "https://xozwpzdevpxfduhyyjpt.supabase.co";

    public const string SUPABASE_KEY = "sb_publishable_0k-mq65TqOs-drrYps0zUg_QAtfZ4wW";

    private static Client _supabase;

    public static Client supabase => _supabase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        Debug.Log("Starting");
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private async Task Test()
    {
        Debug.Log("Testing");
        if(_supabase == null)
        {
            Debug.LogWarning("Supabase not initialized yet");
            return;
        }
        var result = await _supabase
            .From<Rooms>()
            .Get();

        var room1 = result.Models[0];
        testTextBox.text = "" + room1.name + "\n" + room1.description + "\n Active: " + room1.active;
    }
}
