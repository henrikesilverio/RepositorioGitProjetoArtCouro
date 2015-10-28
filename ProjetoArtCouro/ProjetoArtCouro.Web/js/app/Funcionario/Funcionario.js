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
    AntesDeEnviar: function(formularioDados) {
        formularioDados.push({ "name": "EPessoaFisica", "value": $(".TipoPessoa:checked").val() });
    }
});