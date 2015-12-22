$.extend(Portal, {
    ConfiguracaoUsuario: function (settings) {
        settings.ListaUsuarios = [];
        $.ajax({
            url: "/Configuracao/ObterListaUsuario",
            type: "GET",
            traditional: true
        }).success(function (ret) {
            if (ret.TemErros) {
                Portal.PreencherAlertaErros(ret.Mensagem, settings.AlertaMensagensSeletor, true);
            } else {
                settings.ListaUsuarios = ret.ObjetoRetorno;
                _.each(ret.ObjetoRetorno, function (usuario) {
                    $("#UsuarioId").append($("<option>", { value: usuario.UsuarioCodigo, text: usuario.UsuarioNome }));
                });
            }
        }).error(function (ex) {
            Portal.PreencherAlertaErros(ex.responseText, settings.AlertaMensagensSeletor, true);
        });

        $("#PermissaoId").select2({
            formatNoMatches: "Permissão nao encontrada",
            placeholder: "Clique para selecionar uma permissão"
        }).change(function() {
            $("#PermissaoId").trigger("blur");
        });
        Portal.DesbilitarCampo("#PermissaoId");

        $("#UsuarioId").select2({
            formatNoMatches: "Usuário nao encontrado"
        }).change(function (item) {
            if (item.val === "") {
                $("#PermissaoId").val(null).trigger("change");
                Portal.DesbilitarCampo("#PermissaoId");
            } else {
                Portal.HabilitarCampo("#PermissaoId");
                var usuarioCorrente = _.find(settings.ListaUsuarios, function (usuario) {
                    return usuario.UsuarioCodigo.toString() === item.val;
                });
                $("#PermissaoId").val(_.map(usuarioCorrente.Permissoes, function (permissao) {
                    return permissao.Codigo.toString();
                })).trigger("change");
            }
            $("#UsuarioId").trigger("blur");
        });

        $("#SalvarConfiguracaoUsuario").on("click", function () {
            var formularioDados = $("#formularioConfiguracaoUsuario").serializeArray();
            if ($("#formularioConfiguracaoUsuario").valid()) {
                var permissoes = $("#PermissaoId").select2("data");
                Portal.IncluirListaPermissaoNoEnvio(formularioDados, permissoes);
                $.ajax({
                    url: settings.UrlDados,
                    data: formularioDados,
                    type: "POST",
                    traditional: true
                }).success(function (ret) {
                    if (ret.TemErros) {
                        Portal.PreencherAlertaErros(ret.Mensagem, settings.AlertaMensagensSeletor, true);
                    } else {
                        Portal.PreencherAlertaSucesso(ret.Mensagem, settings.AlertaMensagensSeletor, true);
                    }
                }).error(function (ex) {
                    Portal.PreencherAlertaErros(ex.responseText, settings.AlertaMensagensSeletor, true);
                });
            }
        });
    }
});