var Default = Default || {};
$.extend(Default, {
    Mask: function () {
        $(".cpf").mask("999.999.999-99");
        $(".cep").mask("99999-999");
        $(".phone").mask("(99)9999-9999");
        $(".mobilePhone").mask("(99)9?99999999");
    },
    Form: function (obj) {
        $(obj.Selector).click(function () {
            $(obj.Form).submit();
        })

    }
});