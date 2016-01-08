$.extend(Portal, {
    ReceberConta: function () {
        $("#SalvarContaReceber").off("click").on("click", function () {
            $("#modalConfirmacao .modal-title").html("Receber Conta(s)");
            var tabela = $("#datatableContaReceber").dataTable();
            var listaContaReceber = tabela.fnGetData();
            if (listaContaReceber.length === 0) {
                Portal.PreencherAlertaAtencao("Não há conta(s) a receber", "#AlertaMensagens", true);
            } else {
                $("#modalConfirmacao").modal("show");
                Portal.PreencherAlertaAtencao("Esta ação ira alterar os status da(s) linha(s) checada(s) para recebido, clique em <b>Confirmar</b> caso deseje rebecer.",
                    "#ModalAlertaMensagens", true);
                $("#ConfirmarAcao").off("click").on("click", function () {
                    var model = [];
                    _.each(listaContaReceber, function (obj, index) {
                        model.push({ "name": "model[" + index + "].CodigoContaReceber", "value": obj.CodigoContaReceber });
                        model.push({ "name": "model[" + index + "].CodigoVenda", "value": obj.CodigoVenda });
                        model.push({ "name": "model[" + index + "].CodigoCliente", "value": obj.CodigoCliente });
                        model.push({ "name": "model[" + index + "].DataEmissao", "value": obj.DataEmissao });
                        model.push({ "name": "model[" + index + "].DataVencimento", "value": obj.DataVencimento });
                        model.push({ "name": "model[" + index + "].NomeCliente", "value": obj.NomeCliente });
                        model.push({ "name": "model[" + index + "].CPFCNPJ", "value": obj.CPFCNPJ });
                        model.push({ "name": "model[" + index + "].StatusId", "value": obj.StatusId });
                        model.push({ "name": "model[" + index + "].Status", "value": obj.Status });
                        model.push({ "name": "model[" + index + "].ValorDocumento", "value": obj.ValorDocumento });
                        model.push({ "name": "model[" + index + "].Recebido", "value": $(tabela.fnGetNodes(index)).find("input").is(":checked") });
                    });
                    Portal.AjaxReceberConta(model);
                });
            }
        });
    },
    AjaxReceberConta: function (model) {
        $.ajax({
            url: "/ContaReceber/ReceberConta",
            data: model,
            type: "POST",
            traditional: true
        }).success(function (ret) {
            if (ret.TemErros) {
                Portal.PreencherAlertaErros(ret.Mensagem, "#AlertaMensagens", true);
            } else {
                Portal.PreencherAlertaSucesso(ret.Mensagem, "#AlertaMensagens", true);
            }
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseJSON.message, "#AlertaMensagens", true);
        });
    }
});