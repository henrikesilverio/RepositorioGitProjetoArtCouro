$.extend(Portal, {
    NovoGrupo: function (settings) {
        Portal.SalvarDados(settings);
        $("#PermissaoId").select2({
            formatNoMatches: "Permissão nao encontrada",
            placeholder: "Clique para selecionar uma permissão"
        }).change(function () {
            if ($("#PermissaoId").val() == null) {
                Portal.AdicionaErro();
            } else {
                Portal.RemoveErro();
            }
        });
    },
    EditarGrupo: function () {
        $("#PermissaoId").select2({
            formatNoMatches: "Permissão nao encontrada",
            placeholder: "Clique para selecionar uma permissão"
        }).change(function () {
            if ($("#PermissaoId").val() == null) {
                Portal.AdicionaErro();
            } else {
                Portal.RemoveErro();
            }
        });
    },
    SalvarDados: function (settings) {
        $("#SalvarGrupo").on("click", function () {
            var formularioDados = $("#formularioGrupo").serializeArray();
            if ($("#formularioGrupo").valid()) {
                var permissoes = $("#PermissaoId").select2("data");
                Portal.IncluirListaPermissaoNoEnvio(formularioDados, permissoes);
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
    },
    IncluirListaPermissaoNoEnvio: function(formularioDados, permissoes) {
        _.each(permissoes, function(obj, index) {
            formularioDados.push({ "name": "Permissoes[" + index + "].Codigo", "value": obj.id });
            formularioDados.push({ "name": "Permissoes[" + index + "].Nome", "value": obj.text });
        });
    },
    AdicionaErro: function () {
        var $select2Container = $(".select2-container");
        var $spanPermissaoId = $("span[data-valmsg-for=\"PermissaoId\"]");
        $select2Container.addClass("input-validation-error");
        $select2Container.closest("label").removeClass("state-success");
        $select2Container.closest("label").addClass("state-error");
        $spanPermissaoId.addClass("field-validation-error");
        $spanPermissaoId.append("<span for=\"PermissaoId\">Campo Obrigatório</span>");
    },
    RemoveErro: function () {
        var $select2Container = $(".select2-container");
        var $spanPermissaoId = $("span[data-valmsg-for=\"PermissaoId\"]");
        $select2Container.removeClass("input-validation-error");
        $select2Container.closest("label").removeClass("state-error");
        $select2Container.closest("label").addClass("state-success");
        $spanPermissaoId.removeClass("field-validation-error");
        $spanPermissaoId.addClass("field-validation-valid");
        $spanPermissaoId.html("");
    }
});