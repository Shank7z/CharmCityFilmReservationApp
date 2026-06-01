using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

[Table("Rooms")]
public class Rooms : BaseModel
{
    [PrimaryKey("id")]
    public int id { get; set; }

    [Column("name")]
    public string name { get; set; }

    [Column("description")]
    public string description { get; set; }

    [Column("active")]
    public bool active { get; set; }

    [Column("parent_room_id")]
    public string parentRoom { get; set; }
}