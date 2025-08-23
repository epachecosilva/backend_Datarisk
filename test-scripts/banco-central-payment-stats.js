// Script de pré-processamento para estatísticas de meios de pagamento do Banco Central
// Filtra apenas cartões empresariais e agrupa por trimestre e bandeira
function process(data) {
  // Filtrar apenas cartões empresariais
  const corporativeData = data.filter(item => item.produto === "Empresarial");
  
  // Agrupar por trimestre e nome da bandeira
  const byQuarterAndIssuer = corporativeData.reduce((acc, item) => {
    const key = `${item.trimestre}-${item.nomeBandeira}`;
    
    if (!acc[key]) {
      acc[key] = {
        trimestre: item.trimestre,
        nomeBandeira: item.nomeBandeira,
        qtdCartoesEmitidos: 0,
        qtdCartoesAtivos: 0,
        qtdTransacoesNacionais: 0,
        valorTransacoesNacionais: 0,
        qtdTransacoesInternacionais: 0,
        valorTransacoesInternacionais: 0
      };
    }
    
    // Somar os valores
    acc[key].qtdCartoesEmitidos += item.qtdCartoesEmitidos;
    acc[key].qtdCartoesAtivos += item.qtdCartoesAtivos;
    acc[key].qtdTransacoesNacionais += item.qtdTransacoesNacionais;
    acc[key].valorTransacoesNacionais += item.valorTransacoesNacionais;
    acc[key].qtdTransacoesInternacionais += item.qtdTransacoesInternacionais;
    acc[key].valorTransacoesInternacionais += item.valorTransacoesInternacionais;
    
    return acc;
  }, {});
  
  // Retornar apenas os valores (sem as chaves)
  return Object.values(byQuarterAndIssuer);
}
