$.extend(Portal, {
    NovoCliente: function (settings) {
        //Função para mostrar ou esconder dados pessoa fisica ou juridica
        $(".PessoaFisica").on("change", function () {
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
        $(".PessoaFisica[value=\"True\"]")[0].checked = true;
        $(".PessoaFisica:checked").trigger("change");

        //Função para mostras ou esconder endereço
        $("#Endereco_EnderecoId").on("change", function() {
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

        Portal.SalvarDados(settings);
    },
    EditarCliente: function () {
        
    },
    SalvarDados: function (settings) {
        $("#SalvarCliente").on("click", function () {
            var formularioDados = $("#formularioCliente").serializeArray();
            if ($("#formularioCliente").valid()) {
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
                }).error(function(ex) {
                    Portal.PreencherAlertaErros(ex.responseText, settings.AlertaMensagensSeletor);
                });
            }
        });
    }
});