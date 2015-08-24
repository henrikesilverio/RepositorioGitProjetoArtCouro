$.extend(Portal, {
    Search: function (settings) {
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
                            "dom": "T<\"clear\">lfrtip",
                            "sPaginationType": "bootstrap",
                            "sDom": "<'dt-top-row'Tlf>r<'dt-wrapper't><'dt-row dt-bottom-row'<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-sm-6 col-xs-12'p>>",
                            "bDestroy": true,
                            //"oTableTools": {
                            //    "aButtons": ["copy", "print", {
                            //        "sExtends": "collection",
                            //        "sButtonText": "Save <span class=\"caret\" />",
                            //        "aButtons": ["csv", "xls", "pdf"]
                            //    }],
                            //    "sSwfPath": "js/plugin/datatables/media/swf/copy_csv_xls_pdf.swf"
                            //},
                            //"fnInitComplete": function (oSettings, json) {
                            //    $(this).closest("#dt_table_tools_wrapper").find(".DTTT.btn-group").addClass("table_tools_group").children("a.btn").each(function () {
                            //        $(this).addClass("btn-sm btn-default");
                            //    });
                            //},
                            "iDisplayLength": 15,
                            "aaData": ret.ObjetoRetorno,
                            "aoColumns": settings.OrdenacaoDoCabecalho,
                            "aaSorting": [],
                            "oLanguage": {
                                "sInfo": "_START a END em TOTAL_ " + settings.TituloRodape,
                                "sInfoEmpty": "Nenhum " + settings.InformacaoTabela + " Encontrado",
                                "sEmptyTable": settings.InformacaoTabela + " não encontrados",
                                "oPaginate": {
                                    "sFirst": "<<",
                                    "sLast": ">>",
                                    "sPrevious": "<",
                                    "sNext": ">"
                                }
                            }
                        });
                    }
                }).error(function (ex) {
                    Portal.PreencherAlertaErros(ex.responseText, settings.AlertaMensagensSeletor);
                });
            }
        });
    }
});
