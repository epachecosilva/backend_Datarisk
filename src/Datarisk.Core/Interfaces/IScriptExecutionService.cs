namespace Datarisk.Core.Interfaces;

public interface IScriptExecutionService
{
    Task<string> ExecuteScriptAsync(string scriptCode, string inputData);
    Task<bool> ValidateScriptAsync(string scriptCode);
}
