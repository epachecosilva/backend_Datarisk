using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Datarisk.Core.Entities;

public class Processing
{
    public int Id { get; set; }
    
    public int ScriptId { get; set; }
    
    [Required]
    public string InputData { get; set; } = string.Empty;
    
    public string? OutputData { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public ProcessingStatus Status { get; set; } = ProcessingStatus.Pending;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? StartedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    // Navigation property
    public Script Script { get; set; } = null!;
    
    // Helper methods
    public JsonDocument? GetInputDataAsJson()
    {
        try
        {
            return JsonDocument.Parse(InputData);
        }
        catch
        {
            return null;
        }
    }
    
    public JsonDocument? GetOutputDataAsJson()
    {
        if (string.IsNullOrEmpty(OutputData))
            return null;
            
        try
        {
            return JsonDocument.Parse(OutputData);
        }
        catch
        {
            return null;
        }
    }
}

public enum ProcessingStatus
{
    Pending,
    Running,
    Completed,
    Failed
}
