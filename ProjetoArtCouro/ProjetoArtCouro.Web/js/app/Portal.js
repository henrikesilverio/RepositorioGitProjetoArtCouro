var Portal = Portal || {};
$.extend(Portal, {
    Mask: function () {
        $(".CpfMask").mask("000.000.000-00");
        $(".CnpjMask").mask("00.000.000/0000-00");
        $(".CepMask").mask("00000-000");
        $(".TelefoneMask").mask("(00) 0000-0000");
        var spMaskBehavior = function(val) {
                return val.replace(/\D/g, "").length === 11 ? "(00) 00000-0000" : "(00) 0000-00009";
            },
            spOptions = {
                onKeyPress: function(val, e, field, options) {
                    field.mask(spMaskBehavior.apply({}, arguments), options);
                }
            };

        $(".CelularMask").mask(spMaskBehavior, spOptions);
    },
    Form: function (obj) {
        $(obj.Button).click(function() {
            $(obj.Form).submit();
        });
    },
    PreencherAlertaErros: function (mensagem, selotorLocal) {
        var div = $("<div>").attr({
            "class": "alert alert-danger fade in"
        });

        var button = $("<button>").attr({
            "class": "close",
            "data-dismiss":"alert"
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
    },
    PreencherAlertaAtencao: function() {

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
    },
    LimparCampos: function(seletor) {
        $(seletor).find("input, select, input[type=\"radio\"]").each(function() {
            if (this.type === "radio") {
                this.checked = false;
            } else {
                this.value = "";
            }
        });
    }
});