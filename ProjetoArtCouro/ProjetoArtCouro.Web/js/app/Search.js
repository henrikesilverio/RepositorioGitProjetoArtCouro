$.extend(Portal, {
    Search: function(settings) {
        $(settings.WidgetSeletor).hide();
        $(settings.TabelaSeletor).dataTable({
            "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-6'f><'col-sm-6 col-xs-6 hidden-xs'T>r>" +
                "t<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-sm-6 col-xs-12'p>>",
            "oTableTools": {
                "aButtons": [
                    {
                        "sExtends": "xls",
                        "sButtonText": "XLS",
                        "sToolTip": "Exportar XLS"
                    },
                    {
                        "sExtends": "csv",
                        "sButtonText": "CSV",
                        "sToolTip": "Exportar CSV"
                    },
                    {
                        "sExtends": "pdf",
                        "sButtonText": "PDF",
                        "sToolTip": "Exportar PDF",
                        "sPdfMessage": "Exportar PDF",
                        "sPdfSize": "letter"
                    },
                    {
                        "sExtends": "print",
                        "sButtonText": "Imprimir",
                        "sToolTip": "Visualizar para impressão",
                        "sMessage": "<i>(Preecione Esc para sair)</i>"
                    }
                ],
                "sSwfPath": "../js/plugin/datatables/swf/copy_csv_xls_pdf.swf"
            },
            "autoWidth": true,
            "iDisplayLength": 15,
            "aaData": [],
            "aoColumns": settings.OrdenacaoDoCabecalho,
            "aaSorting": [],
            "oLanguage": {
                "sInfo": "_START_ a _END_ em _TOTAL_ " + settings.TituloRodape,
                "sInfoEmpty": settings.TituloRodapeContador,
                "sEmptyTable": settings.InformacaoTabela,
                "sInfoFiltered": "",
                "sZeroRecords": "Não há registros para o filtro informado",
                "oPaginate": {
                    "sFirst": "<<",
                    "sLast": ">>",
                    "sPrevious": "<",
                    "sNext": ">"
                }
            }
        });
        $(settings.WidgetSeletor).show();
        var tableSettings = $(settings.TabelaSeletor).dataTable();

        $(settings.BotaoPesquisarSeletor).on("click", function() {
            var listaCampos = $(settings.FormularioSeletor).find("input:not(:hidden), select");
            var contador = 0;
            $(listaCampos).each(function() {
                if (this.value === "" && this.type !== "checkbox") {
                    contador++;
                } else if ((this.type === "checkbox" || this.type === "radio") && !this.checked) {
                    contador++;
                }
            });

            if (listaCampos.length === contador) {
                Portal.PreencherAlertaErros("Preencha pelo menos um campo", "#AlertaMensagens", true);
                return;
            }

            var formData = $(settings.FormularioSeletor).serializeArray();
            if ($(settings.FormularioSeletor).valid()) {
                $.ajax({
                    url: settings.UrlDados,
                    data: formData,
                    type: "POST",
                    traditional: true
                }).success(function(ret) {
                    tableSettings.fnClearTable();
                    if (ret.TemErros) {
                        Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens", true);
                    } else if (ret.ObjetoRetorno.length === 0) {
                        Portal.PreencherAlertaAtencao(ret.Mensagem, "#AlertaMensagens", true);
                    } else if (ret.ObjetoRetorno.length > 0) {
                        Portal.LimparAlertar("#AlertaMensagens");
                        tableSettings.fnAddData(ret.ObjetoRetorno);
                    }
                }).error(function(ex) {
                    Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
                });
            }
        });
    },
    RequisicaoDeletarLinha: function(url, dados, tabelaSeletor, tr) {
        $.ajax({
            url: url,
            data: dados,
            type: "POST",
            traditional: true
        }).success(function(ret) {
            if (ret.TemErros) {
                Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens", true);
            } else {
                Portal.LimparAlertar("#AlertaMensagens");
                Portal.PreencherAlertaSucesso(ret.Mensagem, "#AlertaMensagens", true);
                var tabela = $(tabelaSeletor).dataTable();
                tabela.fnDeleteRow(tr);
            }
        }).error(function(ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
        });
    },
    CriarBotoes: function(urlEditar, funcaoExcluir) {
        var div = $("<div>").attr({
            "class": "editable-buttons"
        });

        var botaoEditar = $("<a>").attr({
            "href": urlEditar,
            "class": "btn btn-success btn-sm",
            "title": "Editar"
        }).append($("<i>").attr({
            "class": "fa fa-lg fa-pencil-square-o"
        }));

        var botaoExcluir = $("<a>").attr({
            "class": "btn btn-danger btn-sm BotaoExcluir",
            "title": "Excluir",
            "onclick": funcaoExcluir
        }).append($("<i>").attr({
            "class": "fa fa-lg fa-trash-o"
        }));

        div.append(botaoEditar);
        div.append(botaoExcluir);
        return div;
    },
    CriarBotoesTabelaDinamica: function(funcaoEditar, funcaoExcluir, seletorModal) {
        var div = $("<div>").attr({
            "class": "editable-buttons"
        });
        var botaoEditar = $("<a>").attr({
            "class": "btn btn-success btn-sm",
            "title": "Editar",
            "onclick": funcaoEditar,
            "data-toggle": "modal",
            "data-target": seletorModal
        }).append($("<i>").attr({
            "class": "fa fa-lg fa-pencil-square-o"
        }));

        var botaoExcluir = $("<a>").attr({
            "class": "btn btn-danger btn-sm BotaoExcluir",
            "title": "Excluir",
            "onclick": funcaoExcluir
        }).append($("<i>").attr({
            "class": "fa fa-lg fa-trash-o"
        }));

        div.append(botaoEditar);
        div.append(botaoExcluir);
        return div;
    },
    EstruturaTabelaDinamica: function (settings) {
        var tabela = $(settings.TabelaSeletor).dataTable({
            "autoWidth": true,
            "iDisplayLength": 10,
            "aaData": [],
            "aoColumns": settings.OrdenacaoDoCabecalho,
            "aaSorting": [],
            "oLanguage": {
                "sInfo": "_START_ a _END_ em _TOTAL_ " + settings.TituloRodape,
                "sInfoEmpty": settings.TituloRodapeContador,
                "sEmptyTable": settings.InformacaoTabela,
                "sInfoFiltered": "",
                "sZeroRecords": "Não há registros para o filtro informado",
                "oPaginate": {
                    "sFirst": "<<",
                    "sLast": ">>",
                    "sPrevious": "<",
                    "sNext": ">"
                }
            }
        });
        return tabela;
    },
    TabelaItemProduto: function (settings) {
        var tabela = Portal.EstruturaTabelaDinamica(settings);
        $(settings.AdicionaLinhaSeletor).on("click", function () {
            Portal.AdicionarItemProdutoTabela(settings, tabela);
        });
    },
    AdicionarItemProdutoTabela: function (settings, tabela) {
        var temEsseProduto = Portal.VerificaSeProdutoEstaNaTabela(tabela, $("#ProdutoId").val());
        if (settings.$Formulario.valid() && !temEsseProduto) {
            tabela.fnAddData(settings.ObterCamposPrincipais());
            Portal.LimparCampos();
            Portal.CalcularTotal(tabela);
        } else if (temEsseProduto) {
            Portal.PreencherAlertaAtencao("O produto selecionado já existe na lista", "#AlertaMensagens", true);
        }
    },
    VerificaSeProdutoEstaNaTabela: function (tabela, produtoId) {
        var produtos = tabela.fnGetData();
        var temEsseProduto = _.some(produtos, function(produto) {
            return produto.Codigo === produtoId;
        });
        return temEsseProduto;
    },
    EditarItemProdutoTabela: function (tdCorrete, settings) {
        var tabela = $(settings.seletorTabela).dataTable();
        var $tr = $(tdCorrete).closest("tr");
        var obj = tabela.fnGetData($tr);
        Portal.PreencherCamposModal(obj);
        $(settings.seletorBotaoModalAtualizar).off("click").on("click", function () {
            $.extend(obj, Portal.ObterCamposModal(obj));
            tabela.fnUpdate(obj, $tr[0]._DT_RowIndex);
            Portal.CalcularTotal(tabela);
            $(settings.seletorModal).modal("hide");
        });
    },
    ExcluirItemProdutoTabela: function (tdCorrete, settings) {
        var tabela = $(settings.seletorTabela).dataTable();
        tabela.fnDeleteRow($(tdCorrete).closest("tr"));
        Portal.CalcularTotal(tabela);
    },
    TabelaDinamica: function(settings) {
        var tabela = Portal.EstruturaTabelaDinamica(settings);
        Portal.PreencherTabelaDinamica(settings, tabela);
        $(settings.AdicionaLinhaSeletor).on("click", function() {
            Portal.AdicionarItemTabelaDinamica(settings, tabela);
        });
    },
    PreencherTabelaDinamica: function (settings, tabela) {
        if (settings.UrlLista !== undefined) {
            $.ajax({
                url: settings.UrlLista,
                type: "GET",
                traditional: true
            }).success(function (data) {
                if (data.ObjetoRetorno.length) {
                    tabela.fnAddData(data.ObjetoRetorno);
                }
            }).error(function (ex) {
                Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
            });
        }
    },
    AdicionarItemTabelaDinamica: function(settings, tabela) {
        var formularioDados = settings.$Formulario.serializeArray();
        if (settings.$Formulario.valid()) {
            $.ajax({
                url: settings.UrlCriar,
                data: formularioDados,
                type: "POST",
                traditional: true
            }).success(function (data) {
                if (data.TemErros) {
                    Portal.PreencherAlertaErros(data.Mensagem, "#AlertaMensagens", true);
                } else if (data.ObjetoRetorno != null && data.ObjetoRetorno !== undefined) {
                    Portal.LimparCampos.call(this, data);
                    Portal.LimparAlertar("#AlertaMensagens");
                    Portal.PreencherAlertaSucesso(data.Mensagem, "#AlertaMensagens", true);
                    tabela.fnAddData(data.ObjetoRetorno);
                }
            }).error(function(ex) {
                Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
            });
        }
    },
    EditarItemTabelaDinamica: function (tdCorrete, settings) {
        var tabela = $(settings.seletorTabela).dataTable();
        var $tr = $(tdCorrete).closest("tr");
        var obj = tabela.fnGetData($tr);
        Portal.PreencherCamposModal.call(this, obj);
        $(settings.seletorBotaoModalAtualizar).off("click").on("click", function() {
            var $formulario = $(settings.seletorFormulario);
            var formularioDados = $formulario.serializeArray();
            if ($formulario.valid()) {
                formularioDados.push(settings.objetoAdicional);
                $.ajax({
                    url: settings.urlEditar,
                    data: formularioDados,
                    type: "POST",
                    traditional: true
                }).success(function (data) {
                    if (data.TemErros) {
                        Portal.PreencherAlertaErros(data.Mensagem, "#AlertaMensagens", true);
                    } else if (data.ObjetoRetorno != null && data.ObjetoRetorno !== undefined) {
                        Portal.LimparAlertar("#AlertaMensagens");
                        Portal.PreencherAlertaSucesso(data.Mensagem, "#AlertaMensagens", true);
                        tabela.fnUpdate(data.ObjetoRetorno, $tr[0]._DT_RowIndex);
                        $(settings.seletorModal).modal("hide");
                    }
                }).error(function(ex) {
                    Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
                });
            }
        });
    },
    ExcluirItemTabelaDinamica: function (tdCorrete, settings) {
        $.ajax({
            url: settings.urlExcluir,
            data: settings.objetoExcluir,
            type: "POST"
        }).success(function (ret) {
            if (ret.TemErros) {
                Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens", true);
            } else {
                Portal.LimparAlertar("#AlertaMensagens");
                Portal.PreencherAlertaSucesso(ret.Mensagem, "#AlertaMensagens", true);
                var tabela = $(settings.seletorTabela).dataTable();
                tabela.fnDeleteRow($(tdCorrete).closest("tr"));
            }
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
        });
    },
    LimparCampos: function() {},
    PreencherCamposModal: function () {},
    ObterCamposModal: function () {},
    CalcularTotal: function() {}
});
