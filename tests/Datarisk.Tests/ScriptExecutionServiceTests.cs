using Datarisk.Application.Services;
using Datarisk.Core.Interfaces;
using FluentAssertions;
using Xunit;

namespace Datarisk.Tests;

public class ScriptExecutionServiceTests
{
    private readonly IServicoExecucaoScript _service;

    public ScriptExecutionServiceTests()
    {
        _service = new ServicoExecucaoScript();
    }

    [Fact]
    public async Task ValidateScript_WithValidScript_ShouldReturnTrue()
    {
        var validScript = @"
            function process(data) {
                return data.filter(item => item.produto === 'Empresarial');
            }
        ";

        var result = await _service.ValidarScriptAsync(validScript);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateScript_WithInvalidScript_ShouldReturnFalse()
    {
        var invalidScript = @"
            function invalidFunction(data) {
                return data;
            }
        ";

        var result = await _service.ValidarScriptAsync(invalidScript);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateScript_WithSyntaxError_ShouldReturnFalse()
    {
        var invalidScript = @"
            function process(data) {
                return data.filter(item => item.produto === 'Empresarial';
            }
        ";

        var result = await _service.ValidarScriptAsync(invalidScript);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteScript_WithValidScriptAndData_ShouldReturnExpectedResult()
    {
        // Arrange
        var script = @"
            function process(data) {
                return data.filter(item => item.produto === 'Empresarial');
            }
        ";

        var inputData = @"[
            {
                ""trimestre"": ""20231"",
                ""nomeBandeira"": ""VISA"",
                ""produto"": ""Empresarial"",
                ""qtdCartoesEmitidos"": 1000
            },
            {
                ""trimestre"": ""20232"",
                ""nomeBandeira"": ""Mastercard"",
                ""produto"": ""Intermediário"",
                ""qtdCartoesEmitidos"": 2000
            }
        ]";

        // Act
        var result = await _service.ExecutarScriptAsync(script, inputData);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("VISA");
        result.Should().NotContain("Mastercard");
    }

    [Fact]
    public async Task ExecuteScript_WithComplexScript_ShouldProcessDataCorrectly()
    {
        var script = @"
            function process(data) {
                const corporativeData = data.filter(item => item.produto === 'Empresarial');
                
                const byQuarterAndIssuer = corporativeData.reduce((acc, item) => {
                    const key = `${item.trimestre}-${item.nomeBandeira}`;
                    if (!acc[key]) {
                        acc[key] = {
                            trimestre: item.trimestre,
                            nomeBandeira: item.nomeBandeira,
                            qtdCartoesEmitidos: 0,
                            qtdCartoesAtivos: 0
                        };
                    }
                    acc[key].qtdCartoesEmitidos += item.qtdCartoesEmitidos;
                    acc[key].qtdCartoesAtivos += item.qtdCartoesAtivos;
                    return acc;
                }, {});
                
                return Object.values(byQuarterAndIssuer);
            }
        ";

        var inputData = @"[
            {
                ""trimestre"": ""20231"",
                ""nomeBandeira"": ""VISA"",
                ""produto"": ""Empresarial"",
                ""qtdCartoesEmitidos"": 1000,
                ""qtdCartoesAtivos"": 800
            },
            {
                ""trimestre"": ""20231"",
                ""nomeBandeira"": ""VISA"",
                ""produto"": ""Empresarial"",
                ""qtdCartoesEmitidos"": 500,
                ""qtdCartoesAtivos"": 400
            }
        ]";

        var result = await _service.ExecutarScriptAsync(script, inputData);

        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("1500"); // Total qtdCartoesEmitidos
        result.Should().Contain("1200"); // Total qtdCartoesAtivos
    }

    [Fact]
    public async Task ExecuteScript_WithInvalidData_ShouldThrowException()
    {
        var script = @"
            function process(data) {
                return data.filter(item => item.produto === 'Empresarial');
            }
        ";

        var invalidData = "invalid json";

        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.ExecutarScriptAsync(script, invalidData));
    }
}
