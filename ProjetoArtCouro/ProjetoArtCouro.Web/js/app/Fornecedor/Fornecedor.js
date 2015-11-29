﻿$.extend(Portal, {
    NovoFornecedor: function(settings) {
        $(".TipoPessoa[value=\"True\"]")[0].checked = true;
        Portal.ConfiguracaoNovaPessoa();
        Portal.SalvarDados(settings);
    },
    EditarFornecedor: function (settings) {
        Portal.ConfiguracaoNovaPessoa();
        Portal.SalvarDados(settings);
        Portal.DesbilitarCampo("#CPF");
        Portal.DesbilitarCampo("#CNPJ");
        Portal.DesbilitarCampo(".TipoPessoa");
    },
    AntesDeEnviar: function(formularioDados) {
        formularioDados.push({ "name": "EPessoaFisica", "value": $(".TipoPessoa:checked").val() });
    }
});