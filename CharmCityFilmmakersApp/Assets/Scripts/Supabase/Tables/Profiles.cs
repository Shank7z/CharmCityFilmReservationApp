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

    [Column("status")]
    public user_status? userStatus { get; set; }
    public enum user_status
    {
        admin,
        active,
        inactive
    }

    [Column("created_at")]
    public string? creationTime { get; set; }

    [Column("password")]
    public string? password { get; set; }

    [Column("username")]
    public string? username { get; set; }
}