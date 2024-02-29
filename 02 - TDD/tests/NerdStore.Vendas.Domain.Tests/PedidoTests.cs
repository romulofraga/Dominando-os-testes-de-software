﻿using System.Linq;

namespace NerdStore.Vendas.Domain.Tests;

public class PedidoTests
{
    //create a test with fact and trait
    [Fact(DisplayName = "Adicionar Item Pedido Vazio")]
    [Trait("Categoria", "PedidoTest")]
    public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

        var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

        // Act
        pedido.AdicionarItem(pedidoItem);

        // Assert
        Assert.Equal(200, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Adicionar Item Pedido Existente")]
    [Trait("Categoria", "PedidoTest")]
    public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadeSomarValores()
    {
        // Arrange
        var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        var produtoId = Guid.NewGuid();
        var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
        pedido.AdicionarItem(pedidoItem);

        var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 100);
        // Act
        pedido.AdicionarItem(pedidoItem2);
        // Assert
        Assert.Equal(300, pedido.ValorTotal);
        Assert.Equal(1, pedido.PedidoItems.Count);
        Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
    }
}