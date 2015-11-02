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
                } else if (this.type === "checkbox" && !this.checked) {
                    contador++;
                }
            });

            if (listaCampos.length === contador) {
                Portal.PreencherAlertaErros("Preencha pelo menos um campo", "#AlertaMensagens");
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
                        Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens");
                    } else if (ret.ObjetoRetorno.length === 0) {
                        Portal.PreencherAlertaAtencao(ret.Mensagem, "#AlertaMensagens");
                    } else if (ret.ObjetoRetorno.length > 0) {
                        Portal.LimparAlertar("#AlertaMensagens");
                        tableSettings.fnAddData(ret.ObjetoRetorno);
                    }
                }).error(function(ex) {
                    Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens");
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
                Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens");
            } else {
                Portal.LimparAlertar("#AlertaMensagens");
                Portal.PreencherAlertaSucesso(ret.Mensagem, "#AlertaMensagens");
                var tabela = $(tabelaSeletor).dataTable();
                tabela.fnDeleteRow(tr);
            }
        }).error(function(ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens");
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
    TabelaDinamica: function(settings) {
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
                "oPaginate": {
                    "sFirst": "<<",
                    "sLast": ">>",
                    "sPrevious": "<",
                    "sNext": ">"
                }
            }
        });
        Portal.PreencherTabelaDinamica(settings, tabela);

        $(settings.AdicionaLinhaSeletor).on("click", function() {
            Portal.AdicionarItemTabelaDinamica(settings, tabela);
            //if ($(settings.LinhaCorrenteSeletor).val() === "") {
            //    settings.Codigo++;
            //    tabela.fnAddData(settings.ObterCampos(settings.Codigo));
            //} else {
            //    var dadosLinhaCorrente = tabela.fnGetData($(settings.LinhaCorrenteSeletor).val());
            //    var novosDados = settings.ObterCampos(dadosLinhaCorrente.Codigo)[0];
            //    tabela.fnUpdate(novosDados, $(settings.LinhaCorrenteSeletor).val());
            //    $(settings.LinhaCorrenteSeletor).val("");
            //}

            //$(settings.BotaoEditarSeletor).off("click").on("click", function () {
            //    var obj = tabela.fnGetData($(this).closest("tr"));
            //    settings.PreencherCampos(obj);
            //    $(settings.LinhaCorrenteSeletor).val($(this).closest("tr")[0]._DT_RowIndex);
            //});
            //$(settings.BotaoExcluirSeletor).off("click").on("click", function () {
            //    tabela.fnDeleteRow($(this).closest("tr"));
            //});
        });
    },
    PreencherTabelaDinamica: function(settings, tabela) {
        $.ajax({
            url: settings.UrlLista,
            type: "GET",
            traditional: true
        }).success(function(data) {
            if (data.ObjetoRetorno.length) {
                tabela.fnAddData(data.ObjetoRetorno);
            }
        }).error(function(ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens");
        });
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
                    Portal.PreencherAlertaErros(data.Mensagem, "#AlertaMensagens");
                } else if (data.ObjetoRetorno != null && data.ObjetoRetorno !== undefined) {
                    Portal.LimparAlertar("#AlertaMensagens");
                    Portal.PreencherAlertaSucesso(data.Mensagem, "#AlertaMensagens");
                    tabela.fnAddData(data.ObjetoRetorno);
                }
            }).error(function(ex) {
                Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens");
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
                        Portal.PreencherAlertaErros(data.Mensagem, "#AlertaMensagens");
                    } else if (data.ObjetoRetorno != null && data.ObjetoRetorno !== undefined) {
                        Portal.LimparAlertar("#AlertaMensagens");
                        Portal.PreencherAlertaSucesso(data.Mensagem, "#AlertaMensagens");
                        tabela.fnUpdate(data.ObjetoRetorno, $tr[0]._DT_RowIndex);
                        $(settings.seletorModal).modal("hide");
                    }
                }).error(function(ex) {
                    Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens");
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
                Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens");
            } else {
                Portal.LimparAlertar("#AlertaMensagens");
                Portal.PreencherAlertaSucesso(ret.Mensagem, "#AlertaMensagens");
                var tabela = $(settings.seletorTabela).dataTable();
                tabela.fnDeleteRow($(tdCorrete).closest("tr"));
            }
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens");
        });
    },
    PreencherCamposModal: function() {}
});
