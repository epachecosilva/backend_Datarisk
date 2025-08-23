using System.ComponentModel.DataAnnotations;

namespace Datarisk.Core.Entities;

public class ScriptExecution
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public string ScriptCode { get; set; } = string.Empty;
    
    [Required]
    public string TestData { get; set; } = string.Empty;
    
    public string? ExpectedResult { get; set; }
    
    public string? ActualResult { get; set; }
    
    public bool IsSuccessful { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public double? ExecutionTimeMs { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ExecutedAt { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty; // "Banco Central", "E-commerce", "Customer Segmentation"
    
    public int Version { get; set; } = 1;
    
    public bool IsActive { get; set; } = true;
    
    // Relacionamento com Processing (opcional)
    public int? ProcessingId { get; set; }
    public Processing? Processing { get; set; }
}
