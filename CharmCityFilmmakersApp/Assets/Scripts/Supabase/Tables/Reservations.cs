using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;
#nullable enable
[Serializable]
[Table("Reservations")]
public class Reservations : BaseModel, IHasID
{
    [PrimaryKey("id")]
    public int id { get; set; }

    [Column("room_id")]
    public int? roomID { get; set; }

    [Column("user_id")]
    public int? userId { get; set; }

    [Column("start_time")]
    public DateTime startTime { get; set; }

    [Column("end_time")]
    public DateTime endTime { get; set; }

    [Column("notes")]
    public string? notes { get; set; }

    [Column("createdAt")]
    public string? creationTime { get; set; }

    [Column("status")]
    public string? status { get; set; }
}