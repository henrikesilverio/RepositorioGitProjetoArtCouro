$.extend(Portal, {
    NovaVenda: function (settings) {
        Portal.ListaProduto(settings, $("#ProdutoId"));
        Portal.FormasPagamento(settings, $("#FormaPagamentoId"));
        Portal.CondicoesPagamento(settings, $("#CondicaoPagamentoId"));
        Portal.ListaCliente(settings, $("#ClienteId"));
        $("#EfetuarVenda").off("click").on("click", function () {
            var tabela = settings.$TabelaSeletor.dataTable();
            if (tabela.fnGetData().length === 0) {
                Portal.PreencherAlertaAtencao("Para efetuar a venda é necessário adicionar produto(s)", "#AlertaMensagens", true);
            } else {
                Portal.ValidarAntesDeMostrarModal();
            }
            Portal.RemoveErro("#Quantidade");
            Portal.RemoveErro("#ValorDesconto");
            Portal.RemoveErroSelect2("ProdutoId");
        });
        Portal.AplicaFuncoesTabelaItemProduto();
    },
    ValidarAntesDeMostrarModal: function () {
        var temErro = false;
        if ($("#ClienteId option").length === 1) {
            Portal.PreencherAlertaAtencao("Nenhum cliente cadastrado", "#AlertaMensagens", false);
            temErro = true;
        }
        if ($("#FormaPagamentoId option").length === 1) {
            Portal.PreencherAlertaAtencao("Nenhuma forma de pagamento cadastrada", "#AlertaMensagens", false);
            temErro = true;
        }
        if ($("#CondicaoPagamentoId option").length === 1) {
            Portal.PreencherAlertaAtencao("Nenhuma condição de pagamento cadastrada", "#AlertaMensagens", false);
            temErro = true;
        }
        if (!temErro) {
            $("#modalEfetivarVenda").modal("show");
        }
    },
    ListaProduto: function (settings, $ProdutoId) {
        $.ajax({
            url: settings.UrlProduto,
            type: "GET",
            traditional: true
        }).success(function (retorno) {
            _.each(retorno.ObjetoRetorno, function (produto) {
                $ProdutoId.append($("<option/>", {
                    value: produto.ProdutoCodigo,
                    text: produto.Descricao
                }).data("precoVenda", produto.PrecoVenda));
            });
            $ProdutoId.select2({
                formatNoMatches: "Produto não encontrada"
            }).change(function() {
                if ($ProdutoId.val() === "") {
                    Portal.AdicionaErroSelect2("ProdutoId");
                } else {
                    Portal.RemoveErroSelect2("ProdutoId");
                }
            });
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, settings.AlertaMensagensSeletor, true);
        });
    },
    CondicoesPagamento: function (settings, $CondicaoPagamentoId) {
        $.ajax({
            url: settings.UrlCondicoesPagamento,
            type: "GET",
            traditional: true
        }).success(function (retorno) {
            _.each(retorno.ObjetoRetorno, function (condicoesPagamento) {
                $CondicaoPagamentoId.append($("<option/>", {
                    value: condicoesPagamento.CondicaoPagamentoCodigo,
                    text: condicoesPagamento.Descricao
                }));
            });
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, settings.AlertaMensagensSeletor, true);
        });
    },
    FormasPagamento: function (settings, $FormaPagamentoId) {
        $.ajax({
            url: settings.UrlFormasPagamento,
            type: "GET",
            traditional: true
        }).success(function (retorno) {
            _.each(retorno.ObjetoRetorno, function (formasPagamento) {
                $FormaPagamentoId.append($("<option/>", {
                    value: formasPagamento.FormaPagamentoCodigo,
                    text: formasPagamento.Descricao
                }));
            });
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, settings.AlertaMensagensSeletor, true);
        });
    },
    ListaCliente: function (settings, $ClienteId) {
        $.ajax({
            url: settings.UrlCliente,
            type: "GET",
            traditional: true
        }).success(function (retorno) {
            _.each(retorno.ObjetoRetorno, function (cliente) {
                $ClienteId.append($("<option/>", {
                    value: cliente.Codigo,
                    text: cliente.Nome == null || cliente.Nome === ""
                        ? cliente.RazaoSocial
                        : cliente.Nome
                }));
            });
            $ClienteId.select2({
                formatNoMatches: "Cliente não encontrado"
            }).change(function () {
                if ($ClienteId.val() === "") {
                    Portal.AdicionaErroSelect2("ClienteId");
                } else {
                    Portal.RemoveErroSelect2("ClienteId");
                }
            });
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, settings.AlertaMensagensSeletor, true);
        });
    },
    AplicaFuncoesTabelaItemProduto: function () {
        Portal.LimparCampos = function() {
            $("#ProdutoId").select2("val", "");
            $("#Quantidade").val("");
            $("#ValorDesconto").val("");
        };
        Portal.PreencherCamposModal = function(obj) {
            $("#ModalItemProdutoQuantidade").val(obj["Quantidade"]);
            $("#ModalItemProdutoValorDesconto").val(obj["ValorDesconto"].replace(/[a-zA-Z][$]/g, "").trim());
        };
        Portal.ObterCamposModal = function (obj) {
            var valorDescontoFormatado = Portal.FormataValor($("#ModalItemProdutoValorDesconto").val());
            var valorBrutoFormatado = "R$ "
                + Portal.CalculaValorBruto(obj["PrecoVenda"], $("#ModalItemProdutoQuantidade").val());
            var valorLiquidoFormatado = "R$ "
                + Portal.CalculaValorLiquido(valorBrutoFormatado, valorDescontoFormatado);
            return {
                "Quantidade": $("#ModalItemProdutoQuantidade").val(),
                "ValorDesconto": valorDescontoFormatado,
                "ValorBruto": valorBrutoFormatado,
                "ValorLiquido": valorLiquidoFormatado
            };
        };
        Portal.CalcularTotal = function (tabela) {
            var produtos = tabela.fnGetData();
            var resultado = {
                "ValorTotalBruto": 0,
                "ValorTotalDesconto": 0,
                "ValorTotalLiquido": 0
            };
            _.each(produtos, function (produto) {
                resultado.ValorTotalBruto += Portal.CoverteStringEmFloat(produto.ValorBruto);
                resultado.ValorTotalDesconto += Portal.CoverteStringEmFloat(produto.ValorDesconto);
                resultado.ValorTotalLiquido += Portal.CoverteStringEmFloat(produto.ValorLiquido);
            });
            $("#ValorTotalBruto").val(Portal.FormataFloatParaDinheiro(resultado.ValorTotalBruto));
            $("#ValorTotalDesconto").val(Portal.FormataFloatParaDinheiro(resultado.ValorTotalDesconto));
            $("#ValorTotalLiquido").val(Portal.FormataFloatParaDinheiro(resultado.ValorTotalLiquido));
        }
    }
});