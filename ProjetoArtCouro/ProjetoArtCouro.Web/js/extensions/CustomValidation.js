//CustomValidationCPF
jQuery.validator.addMethod("customvalidationcpf", function(value) {
    value = value.replace(/[^0-9]/gi, "");
    return ValidationCPF(value);
}, "");

jQuery.validator.unobtrusive.adapters.add("customvalidationcpf", {}, function (options) {
    options.rules["customvalidationcpf"] = true;
    options.messages["customvalidationcpf"] = options.message;
});

jQuery.validator.addMethod("customvalidationcnpj", function(value) {
    value = value.replace(/[^0-9]/gi, "");
    return ValidationCNPJ(value);
}, "");

jQuery.validator.unobtrusive.adapters.add("customvalidationcnpj", {}, function (options) {
    options.rules["customvalidationcnpj"] = true;
    options.messages["customvalidationcnpj"] = options.message;
});

function ValidationCPF(value) {
    value = value.replace(/[^0-9]/gi, "");

    //vazio
    if (value.length === 0) {
        return true;
    }

    if (value.length !== 11 ||
        value === "00000000000" ||
        value === "11111111111" ||
        value === "22222222222" ||
        value === "33333333333" ||
        value === "44444444444" ||
        value === "55555555555" ||
        value === "66666666666" ||
        value === "77777777777" ||
        value === "88888888888" ||
        value === "99999999999")
        return false;

    var add = 0;
    for (var i = 0; i < 9; i++) {
        add += parseInt(value.charAt(i)) * (10 - i);
    }

    var rev = 11 - (add % 11);
    if (rev === 10 || rev === 11) {
        rev = 0;
    }

    if (rev !== parseInt(value.charAt(9))) {
        return false;
    }

    add = 0;
    for (i = 0; i < 10; i++) {
        add += parseInt(value.charAt(i)) * (11 - i);
    }

    rev = 11 - (add % 11);
    if (rev === 10 || rev === 11) {
        rev = 0;
    }

    if (rev !== parseInt(value.charAt(10))) {
        return false;
    }

    //cpf válido
    return true; 
}

function ValidationCNPJ(value) {

    value = value.replace(/[^\d]+/g, "");
    if (value === "") {
        return false;
    }
    if (value.length !== 14) {
        return false;
    }
    // Elimina CNPJs invalidos conhecidos
    if (value === "00000000000000" ||
        value === "11111111111111" ||
        value === "22222222222222" ||
        value === "33333333333333" ||
        value === "44444444444444" ||
        value === "55555555555555" ||
        value === "66666666666666" ||
        value === "77777777777777" ||
        value === "88888888888888" ||
        value === "99999999999999") {
        return false;
    }
    // Valida DVs
    var tamanho = value.length - 2;
    var numeros = value.substring(0, tamanho);
    var digitos = value.substring(tamanho);
    var soma = 0;
    var pos = tamanho - 7;
    for (var i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) {
            pos = 9;
        }
    }
    var resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado.toString() !== digitos.charAt(0)) {
        return false;
    }

    tamanho = tamanho + 1;
    numeros = value.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) {
            pos = 9;
        }
    }

    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado.toString() !== digitos.charAt(1)) {
        return false;
    }

    return true;
}