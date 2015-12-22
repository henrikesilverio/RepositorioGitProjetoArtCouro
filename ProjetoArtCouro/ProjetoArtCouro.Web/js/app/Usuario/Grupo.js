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
    ConfigurarSelect2: function() {
        $("#PermissaoId").select2({
            formatNoMatches: "Permissão nao encontrada",
            placeholder: "Clique para selecionar uma permissão"
        }).change(function () {
            if ($("#PermissaoId").val() == null) {
                Portal.AdicionaErroSelect2("PermissaoId");
            } else {
                Portal.RemoveErroSelect2("PermissaoId");
            }
        });
    }
});