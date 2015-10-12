$.extend(Portal, {
    NovoFuncionario: function (settings) {
        $(".TipoPessoa[value=\"True\"]")[0].checked = true;
        Portal.ConfiguracaoNovaPessoa();
        Portal.SalvarDados(settings);
    },
    EditarFuncionario: function () {
        Portal.ConfiguracaoNovaPessoa();
        Portal.SalvarDados(settings, function (formularioDados) {
            formularioDados.push({ "name": "EPessoaFisica", "value": $(".TipoPessoa:checked").val() });
        });
        Portal.DesbilitarCampo("#CPF");
        Portal.DesbilitarCampo("#CNPJ");
        Portal.DesbilitarCampo(".TipoPessoa");
    },
    SalvarDados: function (settings, antesDeEnviar) {
        $("#SalvarFuncionario").on("click", function () {
            var formularioDados = $("#formularioFuncionario").serializeArray();
            if ($.isFunction(antesDeEnviar)) {
                antesDeEnviar.call(this, formularioDados);
            }
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