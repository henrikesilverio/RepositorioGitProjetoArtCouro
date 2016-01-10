$.extend(Portal, {
    PagarConta: function () {
        $("#SalvarContaPagar").off("click").on("click", function () {
            $("#modalConfirmacao .modal-title").html("Pagar Conta(s)");
            var tabela = $("#datatableContaPagar").dataTable();
            var listaContaPagar = tabela.fnGetData();
            if (listaContaPagar.length === 0) {
                Portal.PreencherAlertaAtencao("Não há conta(s) a pagar", "#AlertaMensagens", true);
            } else {
                $("#modalConfirmacao").modal("show");
                Portal.PreencherAlertaAtencao("Esta ação ira alterar os status da(s) linha(s) checada(s) para pago, clique em <b>Confirmar</b> caso deseje pagar.",
                    "#ModalAlertaMensagens", true);
                $("#ConfirmarAcao").off("click").on("click", function () {
                    var model = [];
                    _.each(listaContaPagar, function (obj, index) {
                        model.push({ "name": "model[" + index + "].CodigoContaPagar", "value": obj.CodigoContaPagar });
                        model.push({ "name": "model[" + index + "].CodigoCompra", "value": obj.CodigoCompra });
                        model.push({ "name": "model[" + index + "].CodigoFornecedor", "value": obj.CodigoFornecedor });
                        model.push({ "name": "model[" + index + "].DataEmissao", "value": obj.DataEmissao });
                        model.push({ "name": "model[" + index + "].DataVencimento", "value": obj.DataVencimento });
                        model.push({ "name": "model[" + index + "].NomeFornecedor", "value": obj.NomeFornecedor });
                        model.push({ "name": "model[" + index + "].CPFCNPJ", "value": obj.CPFCNPJ });
                        model.push({ "name": "model[" + index + "].StatusId", "value": obj.StatusId });
                        model.push({ "name": "model[" + index + "].Status", "value": obj.Status });
                        model.push({ "name": "model[" + index + "].ValorDocumento", "value": obj.ValorDocumento });
                        model.push({ "name": "model[" + index + "].Pago", "value": $(tabela.fnGetNodes(index)).find("input").is(":checked") });
                    });
                    Portal.AjaxPagarConta(model);
                });
            }
        });
    },
    AjaxPagarConta: function (model) {
        $.ajax({
            url: "/ContaPagar/PagarConta",
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