// Script de pré-processamento para análise de vendas de e-commerce
// Agrupa vendas por categoria, calcula métricas e filtra por período
function process(data) {
  // Filtrar vendas dos últimos 6 meses
  const sixMonthsAgo = new Date();
  sixMonthsAgo.setMonth(sixMonthsAgo.getMonth() - 6);
  
  const recentSales = data.filter(item => {
    const saleDate = new Date(item.dataVenda);
    return saleDate >= sixMonthsAgo && item.status === "Concluída";
  });
  
  // Agrupar por categoria e calcular métricas
  const salesByCategory = recentSales.reduce((acc, item) => {
    const category = item.categoria;
    
    if (!acc[category]) {
      acc[category] = {
        categoria: category,
        totalVendas: 0,
        quantidadeItens: 0,
        ticketMedio: 0,
        vendasPorMes: {},
        produtosMaisVendidos: {}
      };
    }
    
    // Acumular valores
    acc[category].totalVendas += item.valorVenda;
    acc[category].quantidadeItens += item.quantidade;
    
    // Agrupar por mês
    const month = new Date(item.dataVenda).toISOString().substring(0, 7);
    if (!acc[category].vendasPorMes[month]) {
      acc[category].vendasPorMes[month] = 0;
    }
    acc[category].vendasPorMes[month] += item.valorVenda;
    
    // Produtos mais vendidos
    if (!acc[category].produtosMaisVendidos[item.produto]) {
      acc[category].produtosMaisVendidos[item.produto] = 0;
    }
    acc[category].produtosMaisVendidos[item.produto] += item.quantidade;
    
    return acc;
  }, {});
  
  // Calcular ticket médio e ordenar produtos mais vendidos
  Object.keys(salesByCategory).forEach(category => {
    const cat = salesByCategory[category];
    cat.ticketMedio = cat.totalVendas / cat.quantidadeItens;
    
    // Ordenar produtos mais vendidos
    cat.produtosMaisVendidos = Object.entries(cat.produtosMaisVendidos)
      .sort(([,a], [,b]) => b - a)
      .slice(0, 5)
      .reduce((acc, [produto, quantidade]) => {
        acc[produto] = quantidade;
        return acc;
      }, {});
  });
  
  return Object.values(salesByCategory);
}
