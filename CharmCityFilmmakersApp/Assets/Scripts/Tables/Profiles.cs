using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
#nullable enable

[Table("Profiles")]
public class Profiles : BaseModel, IHasID
{
    [PrimaryKey("id")]
    public int id { get; set; }

    [Column("name")]
    public string? name { get; set; }

    [Column("email")]
    public string? email { get; set; }

    [Column("role")]
    public string? role { get; set; }

    [Column("created_at")]
    public string? creationTime { get; set; }
}