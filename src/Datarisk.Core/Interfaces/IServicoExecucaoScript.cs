namespace Datarisk.Core.Interfaces;

public interface IServicoExecucaoScript
{
    Task<string> ExecutarScriptAsync(string codigoScript, string dadosEntrada);
    Task<bool> ValidarScriptAsync(string codigoScript);
}
