$.extend(Portal, {
    NovoUsuario: function (settings) {
        Portal.SalvarDados(settings);
    },
    EditarUsuario: function (settings) {
        Portal.SalvarDados(settings);
        $("#Senha").removeAttr("data-val-required");
        $("#ConfirmarSenha").removeAttr("data-val-required");
    },
    AlterarSenha: function (settings) {
        Portal.SalvarDados(settings);
    }
});