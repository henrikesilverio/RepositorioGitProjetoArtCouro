$.extend(Portal, {
    NovaVenda: function (settings) {
        Portal.ListaProduto(settings, $("#ProdutoId"));
        Portal.AplicaFuncoesTabelaItemProduto();
        $("#EfetuarVenda").remove();
        $("#CancelarVenda").remove();
        $("#GerarOrcamento").off("click").on("click", function () {
            var tabela = settings.$TabelaSeletor.dataTable();
            if (tabela.fnGetData().length === 0) {
                Portal.PreencherAlertaAtencao("Para gerar o orçamento é necessário adicionar produto(s)", "#AlertaMensagens", true);
            } else {
                //Monta o model e manda pro controller
                Portal.CriarOrcamento(settings.UrlDados, tabela);
            }
        });
    },
    EditarVenda: function (settings) {
        Portal.ListaProduto(settings, $("#ProdutoId"));
        Portal.FormasPagamento(settings, $("#FormaPagamentoId"));
        Portal.CondicoesPagamento(settings, $("#CondicaoPagamentoId"));
        Portal.ListaCliente(settings, $("#ClienteId"));
        Portal.AplicaFuncoesTabelaItemProduto();
        $("#GerarOrcamento").remove();
        var tabela = settings.$TabelaSeletor.dataTable();
        tabela.fnAddData(settings.ItensVenda);
        if (settings.StatusVenda === "Aberto") {
            $("#CancelarVenda").remove();
            $("#EfetuarVenda").off("click").on("click", function () {
                if (tabela.fnGetData().length === 0) {
                    Portal.PreencherAlertaAtencao("Para efetuar a venda é necessário adicionar produto(s)", "#AlertaMensagens", true);
                } else {
                    Portal.ValidarAntesDeMostrarModal();
                    $("#Vender").off("click").on("click", function () {
                        Portal.EfetuarVenda(settings, tabela);
                    });
                }
                Portal.RemoveErro("#Quantidade");
                Portal.RemoveErro("#ValorDesconto");
                Portal.RemoveErroSelect2("ProdutoId");
            });
        } else if (settings.StatusVenda === "Confirmado") {
            Portal.BloqueiaEfetuarVenda(tabela);
            $("#CancelarVenda").off("click").on("click", function () {
                Portal.CancelarVenda(settings, tabela);
            });
        } else {
            $("#CancelarVenda").remove();
            Portal.BloqueiaEfetuarVenda(tabela);
        }
    },
    ExcluirVenda: function (url, dados, tabelaSeletor, tr) {
        $("#modalConfirmacao .modal-title").html("Excluir Venda");
        if (dados.StatusVenda === "Confirmado") {
            $("#ConfirmarAcao").hide();
            Portal.PreencherAlertaAtencao("Não e possivel excluir uma venda confirmada, " +
                "caso deseje excluir altere o status para <b>Cancelado</b> e clique novamente em <b>Excluir</b>",
                "#ModalAlertaMensagens", true);
        } else {
            $("#ConfirmarAcao").show();
            Portal.PreencherAlertaAtencao("Esta ação ira excluir venda, clique em <b>Confirmar</b> caso deseje excluir.",
                "#ModalAlertaMensagens", true);
            $("#ConfirmarAcao").off("click").on("click", function() {
                Portal.RequisicaoDeletarLinha(url, { "CodigoVenda": dados.CodigoVenda }, tabelaSeletor, tr);
            });
        }
        $("#ModalAlertaMensagens button").remove();
        $("#modalConfirmacao").modal("show");
    },
    BloqueiaEfetuarVenda: function (tabela) {
        tabela.fnSetColumnVis(7, false);
        $("#AdicionarProduto").remove();
        $("#EfetuarVenda").remove();
        Portal.DesbilitarCampo("#ProdutoId");
        Portal.DesbilitarCampo("#Quantidade");
        Portal.DesbilitarCampo("#ValorDesconto");
    },
    CancelarVenda: function (settings, tabela) {
        $("#modalConfirmacao").modal("show");
        $("#modalConfirmacao .modal-title").html("Cancelar Venda");
        Portal.PreencherAlertaAtencao("Esta ação ira remover a conta a receber referente a venda, o(s) item(s) da venda retornaram para o estoque.", 
            "#ModalAlertaMensagens", true);
        $("#ModalAlertaMensagens button").remove();
        $("#ConfirmarAcao").off("click").on("click", function () {
            var formularioDados = [];
            Portal.AdicionarCamposNoFormularioDados(formularioDados, [{ "name": "CodigoVenda", "value": settings.CodigoVenda }]);
            Portal.EnviarFormulario(settings.UrlDados, tabela, formularioDados, function () {
                $("#CancelarVenda").attr("disabled", "disabled");
                $("#CancelarVenda").off("click");
                $("#CancelarVenda").remove();
                $("#Status").val("Cancelado");
            });
        });
    },
    EfetuarVenda: function (settings, tabela) {
        var formularioDados = [];
        if ($("#formModalEfetivarVenda").valid()) {
            Portal.AdicionarCamposNoFormularioDados(formularioDados, [{ "name": "CodigoVenda", "value": settings.CodigoVenda }]);
            Portal.AdicionarCamposNoFormularioDados(formularioDados, $("#formModalEfetivarVenda").serializeArray());
            Portal.EnviarFormulario(settings.UrlDados, tabela, formularioDados, function () {
                $("#EfetuarVenda").attr("disabled", "disabled");
                $("#EfetuarVenda").off("click");
                $("#modalEfetivarVenda").modal("hide");
                $("#Status").val("Confirmado");
            }, function () {
                $("#modalEfetivarVenda").modal("hide");
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
            Portal.AdicionarItensVendaFormularioDados(formularioDados, tabela.fnGetData());
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
    AdicionarItensVendaFormularioDados: function (formularioDados, itensVenda) {
        _.each(itensVenda, function (obj, index) {
            formularioDados.push({ "name": "ItemVendaModel[" + index + "].Codigo", "value": obj.Codigo });
            formularioDados.push({ "name": "ItemVendaModel[" + index + "].Descricao", "value": obj.Descricao });
            formularioDados.push({ "name": "ItemVendaModel[" + index + "].Quantidade", "value": obj.Quantidade });
            formularioDados.push({ "name": "ItemVendaModel[" + index + "].PrecoVenda", "value": obj.PrecoVenda });
            formularioDados.push({ "name": "ItemVendaModel[" + index + "].ValorBruto", "value": obj.ValorBruto });
            formularioDados.push({ "name": "ItemVendaModel[" + index + "].ValorDesconto", "value": obj.ValorDesconto });
            formularioDados.push({ "name": "ItemVendaModel[" + index + "].ValorLiquido", "value": obj.ValorLiquido });
        });
    },
    AdicionarCamposNoFormularioDados: function (formularioDados, arrayCampo) {
        _.each(arrayCampo, function (obj) {
            formularioDados.push(obj);
        });
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
        Portal.LimparCampos = function () {
            $("#ProdutoId").select2("val", "");
            $("#Quantidade").val("");
            $("#ValorDesconto").val("");
        };
        Portal.PreencherCamposModal = function (obj) {
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
    },
    HabilitarAjudaPesquisaVenda: function () {
        $("#Ajuda").off("click").on("click", function () {
            introJs().setOptions({
                "nextLabel": "Próximo",
                "prevLabel": "Anterior",
                "skipLabel": "Sair",
                "doneLabel": "Fim"
            }).start();
        });
        $("#formPesquisaVenda fieldset").attr({
            "data-step": "1",
            "data-intro": "Campos utilizados para pesquisa, informe os filtros desejados e clique em <b>Pesquisar</b>.",
            "data-position": "botton"
        });
        $("#NovaVenda").attr({
            "data-step": "2",
            "data-intro": "Ao clicar você será redirecionado(a) para tela <b>Nova Venda</b>.",
            "data-position": "left"
        });
        $(".WidgetVenda").attr({
            "data-step": "3",
            "data-intro": "Tabala que informara o(s) resultado(s) da pesquisa.",
            "data-position": "top"
        });
        $("#datatableVenda_filter label").attr({
            "data-step": "4",
            "data-intro": "Campo utilizado para filtrar o(s) resultado(s) da tabela.",
            "data-position": "right"
        });
        $(".DTTT.btn-group").attr({
            "data-step": "4",
            "data-intro": "Opções de exportação do(s) resultado(s) da tabela. </br>" +
                "<b>XLS</b> - Exporta para o formato .XLS Excel </br>" +
                "<b>CSV</b> - Exporta para o formato .CSV Excel </br>" +
                "<b>PDF</b> - Exporta para o formato .PDF </br>" +
                "<b>Imprimir</b> - Expande a tabela em tela cheia para impressão",
            "data-position": "top"
        });
    },
    HabilitarAjudaNovaVenda: function () {
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
            "data-intro": "Campos utilizados para apresentar os valores totais da venda.",
            "data-position": "top"
        });
    },
    HabilitarAjudaEditarVenda: function (settings) {
        $("#Ajuda").off("click").on("click", function () {
            introJs().setOptions({
                "nextLabel": "Próximo",
                "prevLabel": "Anterior",
                "skipLabel": "Sair",
                "doneLabel": "Fim"
            }).start();
        });
        if (settings.StatusVenda === "Aberto") {
            $("#StatusVenda").closest("label").attr({
                "data-step": "1",
                "data-intro": "Enquanto o status da venda for aberto você pode continuar adicionado produtos.",
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
            $("#EfetuarVenda").attr({
                "data-step": "4",
                "data-intro": "Ao clicar você ira efetuar a venda do(s) produto(s) que estão na tabela.",
                "data-position": "left"
            });
            $("#formularioValoresTotais fieldset").attr({
                "data-step": "5",
                "data-intro": "Campos utilizados para apresentar os valores totais da venda.",
                "data-position": "top"
            });
        } else if (settings.StatusVenda === "Confirmado") {
            $("#CancelarVenda").attr({
                "data-step": "1",
                "data-intro": "Ao clicar você ira cancelar a venda e o(s) produto(s) que estão na tabela ira(m) voltar para o estoque.",
                "data-position": "left"
            });
        } else if (settings.StatusVenda === "Cancelado") {
            $(".widget-toolbar").remove();
        }
    }
});