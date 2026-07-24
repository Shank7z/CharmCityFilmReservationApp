using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
#nullable enable

[Table("Rooms")]
public class Rooms : BaseModel, IHasID
{
    [PrimaryKey("id")]
    public int id { get; set; }

    [Column("name")]
    public string? name { get; set; }

    [Column("description")]
    public string? description { get; set; }

    [Column("active")]
    public bool? active { get; set; }

    [Column("is_full_studio")]
    public bool? isFullStudio { get; set; }
}