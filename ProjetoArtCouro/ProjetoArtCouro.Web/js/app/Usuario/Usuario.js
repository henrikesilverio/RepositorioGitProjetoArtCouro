$.extend(Portal, {
    NovoUsuario: function (settings) {
        Portal.SalvarDados(settings);
    },
    EditarUsuario: function (settings) {
        Portal.SalvarDados(settings);
        Portal.DesbilitarCampo("#ConfirmarSenha");
        Portal.DesbilitarCampo("#Senha");
    },
    SalvarDados: function (settings) {
        $("#SalvarUsuario").on("click", function () {
            var formularioDados = $("#formularioUsuario").serializeArray();
            if ($("#formularioUsuario").valid()) {
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