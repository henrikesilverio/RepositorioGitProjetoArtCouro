$.extend(Portal, {
    Search: function (settings) {
        $(settings.WidgetSeletor).hide();
        $(settings.BotaoPesquisarSeletor).on("click", function () {
            var listaCampos = $(settings.FormularioSeletor).find("input, select");
            var contador = 0;
            $(listaCampos).each(function () {
                if (this.value === "") {
                    contador++;
                }
            });

            if (listaCampos.length === contador) {
                Portal.PreencherAlertaErros("Preencha pelo menos um campo", settings.AlertaMensagensSeletor);
                return;
            }

            var formData = $(settings.FormularioSeletor).serializeArray();
            if ($(settings.FormularioSeletor).valid()) {
                $.ajax({
                    url: settings.UrlDados,
                    data: formData,
                    type: "POST",
                    traditional: true
                }).success(function (ret) {
                    if (ret.TemErros) {
                        Portal.PreencherAlertaErros(ret.Mensagem, settings.AlertaMensagensSeletor);
                    } else {
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
                                "sSwfPath": "js/plugin/datatables/swf/copy_csv_xls_pdf.swf"
                            },
                            "autoWidth": true,
                            "iDisplayLength": 15,
                            "aaData": ret.ObjetoRetorno,
                            "aoColumns": settings.OrdenacaoDoCabecalho,
                            "aaSorting": [],
                            "oLanguage": {
                                "sInfo": "_START_ a _END_ em _TOTAL_ " + settings.TituloRodape,
                                "oPaginate": {
                                    "sFirst": "<<",
                                    "sLast": ">>",
                                    "sPrevious": "<",
                                    "sNext": ">"
                                }
                            }
                        });
                        $(settings.WidgetSeletor).show();
                    }
                }).error(function (ex) {
                    Portal.PreencherAlertaErros(ex.responseText, settings.AlertaMensagensSeletor);
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
        }).success(function () {
            var tabela = $(tabelaSeletor).dataTable();
            tabela.fnDeleteRow(tr);
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseText, "#AlertaMensagens");
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
            "title": "Editar",
            "onclick": funcaoExcluir
        }).append($("<i>").attr({
            "class": "fa fa-lg fa-trash-o"
        }));

        div.append(botaoEditar);
        div.append(botaoExcluir);
        return div;
    },
    TabelaDinamica: function (settings) {
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

        $(settings.AdicionaLinhaSeletor).on("click", function () {
            if ($(settings.LinhaCorrenteSeletor).val() === "") {
                settings.Codigo++;
                tabela.fnAddData(settings.ObterCampos(settings.Codigo));
            } else {
                var dadosLinhaCorrente = tabela.fnGetData($(settings.LinhaCorrenteSeletor).val());
                var novosDados = settings.ObterCampos(dadosLinhaCorrente.Codigo)[0];
                tabela.fnUpdate(novosDados, $(settings.LinhaCorrenteSeletor).val());
                $(settings.LinhaCorrenteSeletor).val("");
            }
            
            $(settings.BotaoEditarSeletor).off("click").on("click", function () {
                var obj = tabela.fnGetData($(this).closest("tr"));
                settings.PreencherCampos(obj);
                $(settings.LinhaCorrenteSeletor).val($(this).closest("tr")[0]._DT_RowIndex);
            });
            $(settings.BotaoExcluirSeletor).off("click").on("click", function () {
                tabela.fnDeleteRow($(this).closest("tr"));
            });
        });
    }
});
