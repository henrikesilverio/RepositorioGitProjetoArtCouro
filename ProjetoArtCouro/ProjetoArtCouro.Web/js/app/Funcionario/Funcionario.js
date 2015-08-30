$.extend(Portal, {
    NovoFuncionario: function (settings) {
        Portal.ConfiguracaoNovaPessoa();
        Portal.SalvarDados(settings);
    },
    EditarFuncionario: function () {

    },
    SalvarDados: function (settings) {
        $("#SalvarFuncionario").on("click", function () {
            var formularioDados = $("#formularioFuncionario").serializeArray();
            if ($("#formularioFuncionario").valid()) {
                $.ajax({
                    url: settings.UrlDados,
                    data: formularioDados,
                    type: "POST",
                    traditional: true
                }).success(function (ret) {
                    if (ret.TemErros) {
                        Portal.PreencherAlertaErros(ret.Mensagem, settings.AlertaMensagensSeletor);
                    } else {
                        Portal.PreencherAlertaSucesso(ret.Mensagem, settings.AlertaMensagensSeletor);
                    }
                }).error(function (ex) {
                    Portal.PreencherAlertaErros(ex.responseText, settings.AlertaMensagensSeletor);
                });
            }
        });
    }
});