$.extend(Portal, {
    NovoGrupo: function (settings) {
        Portal.SalvarDados(settings);
        Portal.ConfigurarSelect2();
    },
    EditarGrupo: function(settings) {
        Portal.SalvarDados(settings);
        Portal.DesbilitarCampo("#GrupoNome");
        Portal.ConfigurarSelect2();
        $("#PermissaoId").select2("val", _.map(settings.ListaPermissao, function(obj) { return obj.Codigo }));
    },
    AntesDeSerializar: function () {
        Portal.HabilitarCampo("#GrupoNome");
    },
    DepoisDeValidar: function(formularioDados) {
        var permissoes = $("#PermissaoId").select2("data");
        Portal.IncluirListaPermissaoNoEnvio(formularioDados, permissoes);
    },
    FurmularioInvalido: function() {
        Portal.DesbilitarCampo("#GrupoNome");
    },
    AdicionaErro: function() {
        var $select2Container = $(".select2-container");
        var $spanPermissaoId = $("span[data-valmsg-for=\"PermissaoId\"]");
        $select2Container.addClass("input-validation-error");
        $select2Container.closest("label").removeClass("state-success");
        $select2Container.closest("label").addClass("state-error");
        $spanPermissaoId.addClass("field-validation-error");
        $spanPermissaoId.append("<span for=\"PermissaoId\">Campo Obrigatório</span>");
    },
    RemoveErro: function() {
        var $select2Container = $(".select2-container");
        var $spanPermissaoId = $("span[data-valmsg-for=\"PermissaoId\"]");
        $select2Container.removeClass("input-validation-error");
        $select2Container.closest("label").removeClass("state-error");
        $select2Container.closest("label").addClass("state-success");
        $spanPermissaoId.removeClass("field-validation-error");
        $spanPermissaoId.addClass("field-validation-valid");
        $spanPermissaoId.html("");
    },
    ConfigurarSelect2: function() {
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
    }
});