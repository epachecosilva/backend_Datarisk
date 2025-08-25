using System.ComponentModel.DataAnnotations;

namespace Datarisk.Core.Entities;

public class Script
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Descricao { get; set; }
    
    [Required]
    public string Codigo { get; set; } = string.Empty;
    
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    
    public DateTime? AtualizadoEm { get; set; }
    
    // Navigation property
    public ICollection<Processamento> Processamentos { get; set; } = new List<Processamento>();
}
