$.extend(Portal, {
    NovoCliente: function () {
        //Função para mostrar os esconder dados pessoa fisica ou juridica
        $("#PessoaFisica").on("change", function() {
            if (this.value === "true") {
                $("#DadosPessoaFisica").show("slow");
                $("#DadosPessoaJuridica").hide("slow");
            } else {
                $("#DadosPessoaFisica").hide("slow");
                $("#DadosPessoaJuridica").show("slow");
            }
        });
        $("#PessoaFisica[value=\"true\"]")[0].checked = true;
        $("#PessoaFisica").trigger("change");
    },
    EditarCliente: function () {
        
    }
});