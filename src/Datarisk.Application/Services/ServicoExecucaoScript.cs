using Datarisk.Core.Interfaces;
using Jint;
using Jint.Runtime;
using System.Text.Json;

namespace Datarisk.Application.Services;

public class ServicoExecucaoScript : IServicoExecucaoScript
{
    public async Task<string> ExecutarScriptAsync(string codigoScript, string dadosEntrada)
    {
        return await Task.Run(() =>
        {
            try
            {
                // Create a new JavaScript engine with restricted capabilities
                var engine = new Engine(cfg =>
                {
                    cfg.LimitMemory(4_000_000); // 4MB memory limit
                    cfg.LimitRecursion(100); // 100 recursion limit
                    cfg.TimeoutInterval(TimeSpan.FromSeconds(30)); // 30 second timeout
                    cfg.CatchClrExceptions(); // Catch CLR exceptions
                });

                // Add the script code
                engine.Execute(codigoScript);

                // Parse input data as JSON and convert to JavaScript array
                var inputJson = JsonDocument.Parse(dadosEntrada);
                var inputArray = inputJson.RootElement;
                
                // Convert JSON to JavaScript object and pass to process function
                engine.SetValue("inputData", dadosEntrada);
                engine.Execute("var data = JSON.parse(inputData);");
                
                // Call the process function with the parsed data
                var result = engine.Invoke("process", engine.GetValue("data"));

                // Convert result back to JSON string
                return JsonSerializer.Serialize(result.ToObject());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Falha na execução do script: {ex.Message}", ex);
            }
        });
    }

    public async Task<bool> ValidarScriptAsync(string codigoScript)
    {
        return await Task.Run(() =>
        {
            try
            {
                // Create a new JavaScript engine with restricted capabilities
                var engine = new Engine(cfg =>
                {
                    cfg.LimitMemory(1_000_000); // 1MB memory limit for validation
                    cfg.LimitRecursion(50); // 50 recursion limit for validation
                    cfg.TimeoutInterval(TimeSpan.FromSeconds(10)); // 10 second timeout for validation
                    cfg.CatchClrExceptions(); // Catch CLR exceptions
                });

                // Try to execute the script code
                engine.Execute(codigoScript);

                // Check if the process function exists
                var processFunction = engine.GetValue("process");
                return !processFunction.IsUndefined() && processFunction.Type == Jint.Runtime.Types.Object;
            }
            catch
            {
                return false;
            }
        });
    }
}
