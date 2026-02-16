using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IncidentTracker.Api.Models;

public class Incident
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Service { get; set; } = string.Empty;
    
    [Required]
    public Severity Severity { get; set; }
    
    [Required]
    public Status Status { get; set; }
    
    [StringLength(100)]
    public string? Owner { get; set; }
    
    [StringLength(2000)]
    public string? Summary { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Severity
{
    SEV1,
    SEV2,
    SEV3,
    SEV4
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    OPEN,
    MITIGATED,
    RESOLVED
}