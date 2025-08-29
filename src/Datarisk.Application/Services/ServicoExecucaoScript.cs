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
                var engine = new Engine(cfg =>
                {
                    cfg.LimitMemory(4_000_000);
                    cfg.LimitRecursion(100);
                    cfg.TimeoutInterval(TimeSpan.FromSeconds(30));
                    cfg.CatchClrExceptions();
                });

                engine.Execute(codigoScript);

                var inputJson = JsonDocument.Parse(dadosEntrada);
                var inputArray = inputJson.RootElement;
                
                engine.SetValue("inputData", dadosEntrada);
                engine.Execute("var data = JSON.parse(inputData);");
                
                var result = engine.Invoke("process", engine.GetValue("data"));
              
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
                var engine = new Engine(cfg =>
                {
                    cfg.LimitMemory(1_000_000); 
                    cfg.LimitRecursion(50);
                    cfg.TimeoutInterval(TimeSpan.FromSeconds(10));
                    cfg.CatchClrExceptions();
                });

                engine.Execute(codigoScript);

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
