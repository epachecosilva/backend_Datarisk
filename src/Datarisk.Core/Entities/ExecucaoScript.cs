using System.ComponentModel.DataAnnotations;

namespace Datarisk.Core.Entities;

public class ExecucaoScript
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Descricao { get; set; }
    
    [Required]
    public string CodigoScript { get; set; } = string.Empty;
    
    [Required]
    public string DadosTeste { get; set; } = string.Empty;
    
    public string? ResultadoEsperado { get; set; }
    
    public string? ResultadoReal { get; set; }
    
    public bool Sucesso { get; set; }
    
    public string? MensagemErro { get; set; }
    
    public double? TempoExecucaoMs { get; set; }
    
    public DateTime CriadoEm { get; set; }
    
    public DateTime? ExecutadoEm { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Categoria { get; set; } = string.Empty; // "Banco Central", "E-commerce", "Customer Segmentation"
    
    public int Versao { get; set; } = 1;
    
    public bool Ativo { get; set; } = true;
    
    // Relacionamento com Processamento (opcional)
    public int? ProcessamentoId { get; set; }
    public Processamento? Processamento { get; set; }
}
