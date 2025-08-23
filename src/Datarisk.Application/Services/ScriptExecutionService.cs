using Datarisk.Core.Interfaces;
using Jint;
using Jint.Runtime;
using System.Text.Json;

namespace Datarisk.Application.Services;

public class ScriptExecutionService : IScriptExecutionService
{
    public async Task<string> ExecuteScriptAsync(string scriptCode, string inputData)
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
                engine.Execute(scriptCode);

                // Parse input data as JSON and convert to JavaScript array
                var inputJson = JsonDocument.Parse(inputData);
                var inputArray = inputJson.RootElement;
                
                // Convert JSON to JavaScript object and pass to process function
                engine.SetValue("inputData", inputData);
                engine.Execute("var data = JSON.parse(inputData);");
                
                // Call the process function with the parsed data
                var result = engine.Invoke("process", engine.GetValue("data"));

                // Convert result back to JSON string
                return JsonSerializer.Serialize(result.ToObject());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Script execution failed: {ex.Message}", ex);
            }
        });
    }

    public async Task<bool> ValidateScriptAsync(string scriptCode)
    {
        return await Task.Run(() =>
        {
            try
            {
                // Create engine for validation
                var engine = new Engine(cfg =>
                {
                    cfg.LimitMemory(1_000_000); // 1MB for validation
                    cfg.LimitRecursion(10);
                    cfg.TimeoutInterval(TimeSpan.FromSeconds(5));
                });

                // Try to parse and execute the script
                engine.Execute(scriptCode);

                // Check if the process function exists
                var processFunction = engine.GetValue("process");
                if (processFunction.IsUndefined())
                {
                    throw new InvalidOperationException("Script must contain a 'process' function");
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        });
    }
}
