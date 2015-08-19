var Default = Default || {};
$.extend(Default, {
    Mask: function () {
        $(".cpf").mask("999.999.999-99");
        $(".cnpj").mask("99.999.999/9999-99");
        $(".cep").mask("99999-999");
        $(".phone").mask("(99)9999-9999");
        $(".mobilePhone").mask("(99)9?99999999");

        $("#testeUser").unbind("blur").blur(function () {
            var $this = $(this);
            var value = $this.val().replace(/\D/g, "");
            $this.val(value ? value + "%" : value);
        });

        $("#testeUser").unbind("keypress").keypress(function() {
            $(this).val($(this).val().replace(/\D/g, ""));
        });

        $("#testeUser").unbind("keyup").keyup(function (e) {
            var $this = $(this);
            if (e.keyCode === 8) {
                $this.val($this.val().substring($this.val().length - 1, 0));
            }
            $this.val($this.val() ? $this.val().replace(/\D/g, "") + "%" : $this.val());
        });

    },
    Form: function (obj) {
        $(obj.Button).click(function() {
            $(obj.Form).submit();
        });
    }
});