namespace Datarisk.Api.Models;

public class ProcessamentoResponse
{
    public int Id { get; set; }
    public int ScriptId { get; set; }
    public object? DadosEntrada { get; set; }
    public object? DadosSaida { get; set; }
    public string? MensagemErro { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime? IniciadoEm { get; set; }
    public DateTime? ConcluidoEm { get; set; }
    public ScriptResponse? Script { get; set; }
}

public class ScriptResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
