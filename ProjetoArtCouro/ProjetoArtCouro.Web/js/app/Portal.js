var Portal = Portal || {};
$.extend(Portal, {
    Mask: function () {
        $(".cpf").mask("999.999.999-99");
        $(".cnpj").mask("99.999.999/9999-99");
        $(".cep").mask("99999-999");
        $(".phone").mask("(99)9999-9999");
        $(".mobilePhone").mask("(99)9?99999999");
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

    }
});