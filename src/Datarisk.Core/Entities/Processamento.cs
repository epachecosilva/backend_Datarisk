using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Datarisk.Core.Entities;

public class Processamento
{
    public int Id { get; set; }
    
    public int ScriptId { get; set; }
    
    [Required]
    public string DadosEntrada { get; set; } = string.Empty;
    
    public string? DadosSaida { get; set; }
    
    public string? MensagemErro { get; set; }
    
    public StatusProcessamento Status { get; set; } = StatusProcessamento.Pendente;
    
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    
    public DateTime? IniciadoEm { get; set; }
    
    public DateTime? ConcluidoEm { get; set; }
    
    // Navigation property
    [System.Text.Json.Serialization.JsonIgnore]
    public Script Script { get; set; } = null!;
    
    // Helper methods
    public JsonDocument? ObterDadosEntradaComoJson()
    {
        try
        {
            return JsonDocument.Parse(DadosEntrada);
        }
        catch
        {
            return null;
        }
    }
    
    public JsonDocument? ObterDadosSaidaComoJson()
    {
        if (string.IsNullOrEmpty(DadosSaida))
            return null;
            
        try
        {
            return JsonDocument.Parse(DadosSaida);
        }
        catch
        {
            return null;
        }
    }
}

public enum StatusProcessamento
{
    Pendente,
    Executando,
    Concluido,
    Falhou
}
