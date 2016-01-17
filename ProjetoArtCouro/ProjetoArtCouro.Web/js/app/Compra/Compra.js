﻿$.extend(Portal, {
    NovaCompra: function (settings) {
        Portal.ListaProduto(settings, $("#ProdutoId"));
        Portal.AplicaFuncoesTabelaItemProduto();
        var tabela = settings.$TabelaSeletor.dataTable();
        Portal.CalularValorTotalLiquido(tabela);
        $("#EfetuarCompra").remove();
        $("#CancelarCompra").remove();
        $("#GerarOrcamento").off("click").on("click", function () {
            if (tabela.fnGetData().length === 0) {
                Portal.PreencherAlertaAtencao("Para gerar o orçamento é necessário adicionar produto(s)", "#AlertaMensagens", true);
            } else {
                //Monta o model e manda pro controller
                Portal.CriarOrcamento(settings.UrlDados, tabela);
            }
        });
    },
    EditarCompra: function (settings) {
        Portal.ListaProduto(settings, $("#ProdutoId"));
        Portal.FormasPagamento(settings, $("#FormaPagamentoId"));
        Portal.CondicoesPagamento(settings, $("#CondicaoPagamentoId"));
        Portal.ListaFornecedor(settings, $("#FornecedorId"));
        Portal.AplicaFuncoesTabelaItemProduto();
        $("#GerarOrcamento").remove();
        var tabela = settings.$TabelaSeletor.dataTable();
        tabela.fnAddData(settings.ItensCompra);
        if (settings.StatusCompra === "Aberto") {
            Portal.CalularValorTotalLiquido(tabela);
            $("#CancelarCompra").remove();
            $("#EfetuarCompra").off("click").on("click", function () {
                if (tabela.fnGetData().length === 0) {
                    Portal.PreencherAlertaAtencao("Para efetuar a compra é necessário adicionar produto(s)", "#AlertaMensagens", true);
                } else {
                    Portal.ValidarAntesDeMostrarModal();
                    $("#Comprar").off("click").on("click", function () {
                        Portal.EfetuarCompra(settings, tabela);
                    });
                }
                Portal.RemoveErro("#Quantidade");
                Portal.RemoveErroSelect2("ProdutoId");
            });
        } else if (settings.StatusCompra === "Confirmado") {
            Portal.DesabilitaCampoValorTotalFrete();
            Portal.BloqueiaEfetuarCompra(tabela);
            $("#CancelarCompra").off("click").on("click", function () {
                Portal.CancelarCompra(settings, tabela);
            });
        } else {
            Portal.DesabilitaCampoValorTotalFrete();
            $("#CancelarCompra").remove();
            Portal.BloqueiaEfetuarCompra(tabela);
        }
    },
    ExcluirCompra: function (url, dados, tabelaSeletor, tr) {
        $("#modalConfirmacao .modal-title").html("Excluir Compra");
        if (dados.StatusCompra === "Confirmado") {
            $("#ConfirmarAcao").hide();
            Portal.PreencherAlertaAtencao("Não e possivel excluir uma compra confirmada, " +
                "caso deseje excluir altere o status para <b>Cancelado</b> e clique novamente em <b>Excluir</b>",
                "#ModalAlertaMensagens", true);
        } else {
            $("#ConfirmarAcao").show();
            Portal.PreencherAlertaAtencao("Esta ação ira excluir a compra, clique em <b>Confirmar</b> caso deseje excluir.",
                "#ModalAlertaMensagens", true);
            $("#ConfirmarAcao").off("click").on("click", function () {
                Portal.RequisicaoDeletarLinha(url, { "CodigoCompra": dados.CodigoCompra }, tabelaSeletor, tr);
            });
        }
        $("#ModalAlertaMensagens button").remove();
        $("#modalConfirmacao").modal("show");
    },
    BloqueiaEfetuarCompra: function (tabela) {
        tabela.fnSetColumnVis(6, false);
        $("#AdicionarProduto").remove();
        $("#EfetuarCompra").remove();
        Portal.DesbilitarCampo("#ProdutoId");
        Portal.DesbilitarCampo("#Quantidade");
    },
    CancelarCompra: function (settings, tabela) {
        $("#modalConfirmacao").modal("show");
        $("#modalConfirmacao .modal-title").html("Cancelar Compra");
        Portal.PreencherAlertaAtencao("Esta ação ira remover a conta a pagar referente a compra, o(s) item(s) da compra sera(ão) subtraído(s) para o estoque.",
            "#ModalAlertaMensagens", true);
        $("#ModalAlertaMensagens button").remove();
        $("#ConfirmarAcao").off("click").on("click", function () {
            var formularioDados = [];
            Portal.AdicionarCamposNoFormularioDados(formularioDados, [{ "name": "CodigoCompra", "value": settings.CodigoCompra }]);
            Portal.EnviarFormulario(settings.UrlDados, tabela, formularioDados, function () {
                $("#CancelarCompra").attr("disabled", "disabled");
                $("#CancelarCompra").off("click");
                $("#CancelarCompra").remove();
                $("#StatusCompra").val("Cancelado");
                $(".widget-toolbar").remove();
            });
        });
    },
    EfetuarCompra: function (settings, tabela) {
        var formularioDados = [];
        if ($("#formModalEfetivarCompra").valid()) {
            Portal.AdicionarCamposNoFormularioDados(formularioDados, [{ "name": "CodigoCompra", "value": settings.CodigoCompra }]);
            Portal.AdicionarCamposNoFormularioDados(formularioDados, $("#formModalEfetivarCompra").serializeArray());
            Portal.EnviarFormulario(settings.UrlDados, tabela, formularioDados, function () {
                Portal.DesabilitaCampoValorTotalFrete();
                $("#EfetuarCompra").attr("disabled", "disabled");
                $("#EfetuarCompra").off("click");
                $("#modalEfetivarCompra").modal("hide");
                $("#StatusCompra").val("Confirmado");
            }, function () {
                $("#modalEfetivarCompra").modal("hide");
            });
        }
    },
    CriarOrcamento: function (urlDados, tabela) {
        var formularioDados = [];
        Portal.EnviarFormulario(urlDados, tabela, formularioDados, function () {
            $("#GerarOrcamento").attr("disabled", "disabled");
            $("#GerarOrcamento").off("click");
        });
    },
    EnviarFormulario: function (urlDados, tabela, formularioDados, funcaoSucesso, funcaoErro) {
        var $formularioStatus = $("<form>").append($("#formularioStatus").clone());
        if ($formularioStatus.valid() && $("#formularioValoresTotais").valid()) {
            Portal.AdicionarCamposNoFormularioDados(formularioDados, $formularioStatus.serializeArray());
            Portal.AdicionarCamposNoFormularioDados(formularioDados, $("#formularioValoresTotais").serializeArray());
            Portal.AdicionarItensCompraFormularioDados(formularioDados, tabela.fnGetData());
            $.ajax({
                url: urlDados,
                data: formularioDados,
                type: "POST",
                traditional: true
            }).success(function (ret) {
                if (ret.TemErros) {
                    Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens", true);
                } else {
                    Portal.PreencherAlertaSucesso(ret.Mensagem, "#AlertaMensagens", true);
                    if ($.isFunction(funcaoSucesso)) {
                        funcaoSucesso.call(this, ret);
                    }
                }
            }).error(function (ex) {
                if ($.isFunction(funcaoErro)) {
                    funcaoErro.call(this, ex);
                }
                Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
            });
        }
    },
    AdicionarItensCompraFormularioDados: function (formularioDados, itensCompra) {
        _.each(itensCompra, function (obj, index) {
            formularioDados.push({ "name": "ItemCompraModel[" + index + "].Codigo", "value": obj.Codigo });
            formularioDados.push({ "name": "ItemCompraModel[" + index + "].Descricao", "value": obj.Descricao });
            formularioDados.push({ "name": "ItemCompraModel[" + index + "].Quantidade", "value": obj.Quantidade });
            formularioDados.push({ "name": "ItemCompraModel[" + index + "].PrecoVenda", "value": obj.PrecoVenda });
            formularioDados.push({ "name": "ItemCompraModel[" + index + "].ValorBruto", "value": obj.ValorBruto });
            formularioDados.push({ "name": "ItemCompraModel[" + index + "].ValorLiquido", "value": obj.ValorLiquido });
        });
    },
    AdicionarCamposNoFormularioDados: function (formularioDados, arrayCampo) {
        _.each(arrayCampo, function (obj) {
            formularioDados.push(obj);
        });
    },
    ValidarAntesDeMostrarModal: function () {
        var temErro = false;
        if ($("#FornecedorId option").length === 1) {
            Portal.PreencherAlertaAtencao("Nenhum fornecedor cadastrado", "#AlertaMensagens", false);
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
            $("#modalEfetivarCompra").modal("show");
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
            }).change(function () {
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
    ListaFornecedor: function (settings, $FornecedorId) {
        $.ajax({
            url: settings.UrlFornecedor,
            type: "GET",
            traditional: true
        }).success(function (retorno) {
            _.each(retorno.ObjetoRetorno, function (fornecedor) {
                $FornecedorId.append($("<option/>", {
                    value: fornecedor.Codigo,
                    text: fornecedor.Nome == null || fornecedor.Nome === ""
                        ? fornecedor.RazaoSocial
                        : fornecedor.Nome
                }));
            });
            $FornecedorId.select2({
                formatNoMatches: "Fornecedor não encontrado"
            }).change(function () {
                if ($FornecedorId.val() === "") {
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
        Portal.LimparCampos = function () {
            $("#ProdutoId").select2("val", "");
            $("#Quantidade").val("");
        };
        Portal.PreencherCamposModal = function (obj) {
            $("#ModalItemProdutoQuantidade").val(obj["Quantidade"]);
        };
        Portal.ObterCamposModal = function (obj) {
            var valorBrutoFormatado = "R$ "
                + Portal.CalculaValorBruto(obj["PrecoVenda"], $("#ModalItemProdutoQuantidade").val());
            var valorLiquidoFormatado = "R$ "
                + Portal.CalculaValorLiquido(valorBrutoFormatado, "R$ 0,00");
            return {
                "Quantidade": $("#ModalItemProdutoQuantidade").val(),
                "ValorBruto": valorBrutoFormatado,
                "ValorLiquido": valorLiquidoFormatado
            };
        };
        Portal.CalcularTotal = function (tabela) {
            var produtos = tabela.fnGetData();
            var resultado = {
                "ValorTotalBruto": 0,
                "ValorTotalLiquido": 0
            };
            _.each(produtos, function (produto) {
                resultado.ValorTotalBruto += Portal.CoverteStringEmFloat(produto.ValorBruto);
                resultado.ValorTotalLiquido += Portal.CoverteStringEmFloat(produto.ValorLiquido);
            });
            $("#ValorTotalBruto").val(Portal.FormataFloatParaDinheiro(resultado.ValorTotalBruto));
            var valorTotalFrete = Portal.CoverteStringEmFloat($("#ValorTotalFrete").val());
            var valorTotalLiquidoComFrete = resultado.ValorTotalLiquido + valorTotalFrete;
            $("#ValorTotalLiquido").val(Portal.FormataFloatParaDinheiro(valorTotalLiquidoComFrete));
        }
    },
    CalularValorTotalLiquido: function (tabela) {
        $("#ValorTotalFrete").off("change").on("change", function () {
            if (this.value !== "") {
                Portal.CalcularTotal(tabela);
            }
        });
    },
    DesabilitaCampoValorTotalFrete: function () {
        $("#ValorTotalFrete").attr("readonly", "readonly");
        $("#ValorTotalFrete").closest("label").removeClass("state-success");
        $("#ValorTotalFrete").closest("label").addClass("state-disabled");
    },
    HabilitarAjudaPesquisaCompra: function () {
        $("#Ajuda").off("click").on("click", function () {
            introJs().setOptions({
                "nextLabel": "Próximo",
                "prevLabel": "Anterior",
                "skipLabel": "Sair",
                "doneLabel": "Fim"
            }).start();
        });
        $("#formPesquisaCompra fieldset").attr({
            "data-step": "1",
            "data-intro": "Campos utilizados para pesquisa, informe os filtros desejados e clique em <b>Pesquisar</b>.",
            "data-position": "botton"
        });
        $("#NovaCompra").attr({
            "data-step": "2",
            "data-intro": "Ao clicar você será redirecionado(a) para tela <b>Nova Compra</b>.",
            "data-position": "left"
        });
        $(".WidgetCompra").attr({
            "data-step": "3",
            "data-intro": "Tabala que informara o(s) resultado(s) da pesquisa.",
            "data-position": "top"
        });
        $("#datatableCompra_filter label").attr({
            "data-step": "4",
            "data-intro": "Campo utilizado para filtrar o(s) resultado(s) da tabela.",
            "data-position": "right"
        });
        $(".DTTT.btn-group").attr({
            "data-step": "5",
            "data-intro": "Opções de exportação do(s) resultado(s) da tabela. </br>" +
                "<b>XLS</b> - Exporta para o formato .XLS Excel </br>" +
                "<b>CSV</b> - Exporta para o formato .CSV Excel </br>" +
                "<b>PDF</b> - Exporta para o formato .PDF </br>" +
                "<b>Imprimir</b> - Expande a tabela em tela cheia para impressão",
            "data-position": "top"
        });
    },
    HabilitarAjudaNovaCompra: function () {
        $("#Ajuda").off("click").on("click", function () {
            introJs().setOptions({
                "nextLabel": "Próximo",
                "prevLabel": "Anterior",
                "skipLabel": "Sair",
                "doneLabel": "Fim"
            }).start();
        });
        $("#ProdutoId").closest("label").attr({
            "data-step": "1",
            "data-intro": "Campo utilizado para pesquisar o(s) produto(s).",
            "data-position": "top"
        });
        $("#AdicionarProduto").attr({
            "data-step": "2",
            "data-intro": "Ao clicar você ira adicionar o produto selecionado com a quantidade informada.",
            "data-position": "left"
        });
        $("#GerarOrcamento").attr({
            "data-step": "3",
            "data-intro": "Ao clicar você ira gerar um orçamento do(s) produto(s) que estão na tabela.",
            "data-position": "left"
        });
        $("#formularioValoresTotais fieldset").attr({
            "data-step": "4",
            "data-intro": "Campos utilizados para apresentar os valores totais da compra.",
            "data-position": "top"
        });
        $("#ValorTotalFrete").closest("label").attr({
            "data-step": "5",
            "data-intro": "Campo utilizado para adicionar o valor do frete, ao ser adicionado o valor e somado ao <b>Valor Total Liquido</b>",
            "data-position": "top"
        });
    },
    HabilitarAjudaEditarCompra: function(settings) {
        $("#Ajuda").off("click").on("click", function() {
            introJs().setOptions({
                "nextLabel": "Próximo",
                "prevLabel": "Anterior",
                "skipLabel": "Sair",
                "doneLabel": "Fim"
            }).start();
        });
        if (settings.StatusCompra === "Aberto") {
            $("#StatusCompra").closest("label").attr({
                "data-step": "1",
                "data-intro": "Enquanto o status da compra for aberto você pode continuar adicionado produtos.",
                "data-position": "botton"
            });
            $("#ProdutoId").closest("label").attr({
                "data-step": "2",
                "data-intro": "Campo utilizado para pesquisar o(s) produto(s).",
                "data-position": "top"
            });
            $("#AdicionarProduto").attr({
                "data-step": "3",
                "data-intro": "Ao clicar você ira adicionar o produto selecionado com a quantidade informada.",
                "data-position": "left"
            });
            $("#EfetuarCompra").attr({
                "data-step": "4",
                "data-intro": "Ao clicar você ira efetuar a compra do(s) produto(s) que estão na tabela.",
                "data-position": "left"
            });
            $("#formularioValoresTotais fieldset").attr({
                "data-step": "5",
                "data-intro": "Campos utilizados para apresentar os valores totais da compra.",
                "data-position": "top"
            });
            $("#ValorTotalFrete").closest("label").attr({
                "data-step": "6",
                "data-intro": "Campo utilizado para adicionar o valor do frete, ao ser adicionado o valor e somado ao <b>Valor Total Liquido</b>",
                "data-position": "top"
            });
        } else if (settings.StatusCompra === "Confirmado") {
            $("#CancelarCompra").attr({
                "data-step": "1",
                "data-intro": "Ao clicar você ira cancelar a compra e o(s) produto(s) que estão na tabela sera(m) baixado(s) do estoque.",
                "data-position": "left"
            });
        } else if (settings.StatusCompra === "Cancelado") {
            $(".widget-toolbar").remove();
        }
    }
});