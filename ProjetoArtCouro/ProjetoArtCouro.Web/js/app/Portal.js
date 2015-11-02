var Portal = Portal || {};
$.extend(Portal, {
    Mask: function () {
        $(".CpfMask").mask("000.000.000-00");
        $(".CnpjMask").mask("00.000.000/0000-00");
        function cpfCnpjMaskBehavior(val) {
            return val.replace(/\D/g, "").length > 11 ? "00.000.000/0000-00" : "000.000.000-00999";
        };
        var documentoOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(cpfCnpjMaskBehavior.apply({}, arguments), options);
            }
        };
        $(".CpfCnpjMask").mask(cpfCnpjMaskBehavior, documentoOptions);
        $(".CepMask").mask("00000-000");
        $(".TelefoneMask").mask("(00) 0000-0000");
        $(".DataMask").mask("00/00/0000");
        $(".SomenteLetraMask").mask("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
        var spMaskBehavior = function (val) {
            return val.replace(/\D/g, "").length === 11 ? "(00) 00000-0000" : "(00) 0000-00009";
        },
            spOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(spMaskBehavior.apply({}, arguments), options);
                }
            };

        $(".CelularMask").mask(spMaskBehavior, spOptions);
        $(".DinheiroMask").mask("0.000.000,00", { reverse: true });
    },
    Form: function (obj) {
        $(obj.Button).click(function () {
            $(obj.Form).submit();
        });
    },
    PreencherAlertaErros: function (mensagem, selotorLocal) {
        var div = $("<div>").attr({
            "class": "alert alert-danger fade in"
        });

        var button = $("<button>").attr({
            "class": "close",
            "data-dismiss": "alert"
        }).text("x");

        var i = $("<i>").attr({
            "class": "fa-fw fa fa-times"
        });

        var strong = $("<strong>").text("Erro!");

        div.append(button);
        div.append(i);
        div.append(strong);
        div.append((" " + mensagem));
        $(selotorLocal).html(div[0].outerHTML);
        $("body").stop().animate({ scrollTop: 0 }, "500", "swing");
    },
    PreencherAlertaAtencao: function (mensagem, selotorLocal) {
        var div = $("<div>").attr({
            "class": "alert alert-warning fade in"
        });

        var button = $("<button>").attr({
            "class": "close",
            "data-dismiss": "alert"
        }).text("x");

        var i = $("<i>").attr({
            "class": "fa fa-warning fa-fw fa-lg"
        });

        var strong = $("<strong>").text("Atenção!");

        div.append(button);
        div.append(i);
        div.append(strong);
        div.append((" " + mensagem));
        $(selotorLocal).html(div[0].outerHTML);
        $("body").stop().animate({ scrollTop: 0 }, "500", "swing");
    },
    PreencherAlertaSucesso: function (mensagem, selotorLocal) {
        var div = $("<div>").attr({
            "class": "alert alert-success fade in"
        });

        var button = $("<button>").attr({
            "class": "close",
            "data-dismiss": "alert"
        }).text("x");

        var i = $("<i>").attr({
            "class": "fa-fw fa fa-check"
        });

        var strong = $("<strong>").text("Sucesso");

        div.append(button);
        div.append(i);
        div.append(strong);
        div.append((" " + mensagem));
        $(selotorLocal).html(div[0].outerHTML);
        $("body").stop().animate({ scrollTop: 0 }, "500", "swing");
    },
    LimparAlertar: function (selotorLocal) {
        $(selotorLocal).empty();
    },
    LimparCampos: function (seletor) {
        $(seletor).find("input, select, input[type=\"radio\"]").each(function () {
            if (this.type === "radio") {
                this.checked = false;
            } else {
                this.value = "";
            }
        });
    },
    ConfiguracaoNovaPessoa: function () {
        //Função para mostrar ou esconder dados pessoa fisica ou juridica
        $(".TipoPessoa").on("change", function () {
            if (this.value === "True") {
                $("#DadosPessoaFisica").show("slow");
                $("#DadosPessoaJuridica").hide("slow");
                Portal.LimparCampos("#DadosPessoaJuridica");
                $("#DadosPessoaFisica").find("input, select, input[type='radio']").removeAttr("disabled");
                $("#DadosPessoaJuridica").find("input, select, input[type='radio']").attr("disabled", "disabled");
            } else {
                $("#DadosPessoaFisica").hide("slow");
                $("#DadosPessoaJuridica").show("slow");
                Portal.LimparCampos("#DadosPessoaFisica");
                $("#DadosPessoaJuridica").find("input, select, input[type='radio']").removeAttr("disabled");
                $("#DadosPessoaFisica").find("input, select, input[type='radio']").attr("disabled", "disabled");
            }
        });
        $(".TipoPessoa:checked").trigger("change");

        //Função para mostras ou esconder endereço
        $("#Endereco_EnderecoId").on("change", function () {
            if (this.value === "0") {
                $(".NovoEndereco").show("slow");
                $(".NovoEndereco").find("input, select").removeAttr("disabled");
            } else {
                $(".NovoEndereco").hide("slow");
                $(".NovoEndereco").find("input, select").attr("disabled", "disabled");
            }
        });

        //Função para mostras ou esconder meios de comunicação
        $("#MeioComunicacao_TelefoneId").on("change", function () {
            if (this.value === "0") {
                $(".NovoTelefone").css("visibility", "visible");
                $(".NovoTelefone").find("input").removeAttr("disabled");
            } else {
                $(".NovoTelefone").css("visibility", "hidden");
                $(".NovoTelefone").find("input").attr("disabled", "disabled");
            }
        });

        $("#MeioComunicacao_CelularId").on("change", function () {
            if (this.value === "0") {
                $(".NovoCelular").css("visibility", "visible");
                $(".NovoCelular").find("input").removeAttr("disabled");
            } else {
                $(".NovoCelular").css("visibility", "hidden");
                $(".NovoCelular").find("input").attr("disabled", "disabled");
            }
        });

        $("#MeioComunicacao_EmailId").on("change", function () {
            if (this.value === "0") {
                $(".NovoEmail").css("visibility", "visible");
                $(".NovoEmail").find("input").removeAttr("disabled");
            } else {
                $(".NovoEmail").css("visibility", "hidden");
                $(".NovoEmail").find("input").attr("disabled", "disabled");
            }
        });

        $("#Endereco_EnderecoId").trigger("change");
        $("#MeioComunicacao_TelefoneId").trigger("change");
        $("#MeioComunicacao_CelularId").trigger("change");
        $("#MeioComunicacao_EmailId").trigger("change");
    },
    ConverteData: function (data) {
        function pad(s) { return (s < 10) ? "0" + s : s; }

        var d = new Date(data);
        return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join("/");
    },
    DesbilitarCampo: function (seletorCampo) {
        $(seletorCampo).attr("disabled", "disabled");
        $(seletorCampo).closest("label").addClass("state-disabled");
    },
    HabilitarCampo: function (seletorCampo) {
        $(seletorCampo).removeAttr("disabled");
        $(seletorCampo).closest("label").removeClass("state-disabled");
    },
    IncluirListaPermissaoNoEnvio: function (formularioDados, permissoes) {
        _.each(permissoes, function (obj, index) {
            formularioDados.push({ "name": "Permissoes[" + index + "].Codigo", "value": obj.id });
            formularioDados.push({ "name": "Permissoes[" + index + "].Nome", "value": obj.text });
        });
    },
    AntesDeSerializar: function () { },
    AntesDeEnviar: function () { },
    DepoisDeValidar: function () { },
    FurmularioInvalido: function() {},
    SalvarDados: function (settings) {
        settings.$BotaoSalvar.on("click", function () {
            if ($.isFunction(Portal.AntesDeSerializar)) {
                Portal.AntesDeSerializar.call(this);
            }
            var formularioDados = settings.$Formulario.serializeArray();
            if ($.isFunction(Portal.AntesDeEnviar)) {
                Portal.AntesDeEnviar.call(this, formularioDados);
            }
            if (settings.$Formulario.valid()) {
                if ($.isFunction(Portal.DepoisDeValidar)) {
                    Portal.DepoisDeValidar.call(this, formularioDados);
                }
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
                    Portal.PreencherAlertaErros(ex.responseJSON.message, settings.AlertaMensagensSeletor);
                });
            } else if ($.isFunction(FurmularioInvalido)) {
                Portal.FurmularioInvalido.call(this, formularioDados);
            }
        });
    }
});

$(".div-loading").hide();
$(document).ajaxStart(function () {
    $(".div-loading").show();
});

$(document).ajaxStop(function () {
    $(".div-loading").hide();
});

$.validator.setDefaults({
    ignore: ""
});