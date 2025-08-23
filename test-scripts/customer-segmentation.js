// Script de pré-processamento para segmentação de clientes
// Analisa comportamento de compra e cria segmentos
function process(data) {
  // Calcular métricas por cliente
  const customerMetrics = data.reduce((acc, item) => {
    const customerId = item.clienteId;
    
    if (!acc[customerId]) {
      acc[customerId] = {
        clienteId: customerId,
        nome: item.nome,
        email: item.email,
        totalCompras: 0,
        valorTotal: 0,
        primeiraCompra: new Date(item.dataCompra),
        ultimaCompra: new Date(item.dataCompra),
        categoriasCompradas: new Set(),
        frequenciaCompras: 0
      };
    }
    
    const customer = acc[customerId];
    customer.totalCompras += item.quantidade;
    customer.valorTotal += item.valorCompra;
    customer.categoriasCompradas.add(item.categoria);
    
    const compraDate = new Date(item.dataCompra);
    if (compraDate < customer.primeiraCompra) {
      customer.primeiraCompra = compraDate;
    }
    if (compraDate > customer.ultimaCompra) {
      customer.ultimaCompra = compraDate;
    }
    
    return acc;
  }, {});
  
  // Calcular frequência de compras e criar segmentos
  const segmentedCustomers = Object.values(customerMetrics).map(customer => {
    // Calcular frequência (compras por mês)
    const mesesAtivo = Math.max(1, 
      (customer.ultimaCompra - customer.primeiraCompra) / (1000 * 60 * 60 * 24 * 30)
    );
    customer.frequenciaCompras = customer.totalCompras / mesesAtivo;
    
    // Determinar segmento baseado em valor e frequência
    let segmento = "Bronze";
    if (customer.valorTotal >= 10000 && customer.frequenciaCompras >= 2) {
      segmento = "Diamante";
    } else if (customer.valorTotal >= 5000 && customer.frequenciaCompras >= 1) {
      segmento = "Ouro";
    } else if (customer.valorTotal >= 1000 && customer.frequenciaCompras >= 0.5) {
      segmento = "Prata";
    }
    
    // Calcular recência (dias desde última compra)
    const hoje = new Date();
    const diasRecencia = Math.floor((hoje - customer.ultimaCompra) / (1000 * 60 * 60 * 24));
    
    return {
      clienteId: customer.clienteId,
      nome: customer.nome,
      email: customer.email,
      segmento: segmento,
      valorTotal: customer.valorTotal,
      totalCompras: customer.totalCompras,
      ticketMedio: customer.valorTotal / customer.totalCompras,
      frequenciaCompras: customer.frequenciaCompras,
      recenciaDias: diasRecencia,
      categoriasCompradas: Array.from(customer.categoriasCompradas),
      primeiraCompra: customer.primeiraCompra.toISOString().split('T')[0],
      ultimaCompra: customer.ultimaCompra.toISOString().split('T')[0]
    };
  });
  
  // Ordenar por valor total (decrescente)
  return segmentedCustomers.sort((a, b) => b.valorTotal - a.valorTotal);
}
