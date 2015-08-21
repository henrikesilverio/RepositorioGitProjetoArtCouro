using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ProjetoArtCouro.Web.Extensions.DataAnnotation
{
    /// <summary>
    /// Data anotation Valida CPF ou CNPJ 
    /// </summary>
    public class CPFCNPJValidationAttribute : ValidationAttribute, IClientValidatable
    {

        public override bool IsValid(object value)
        {
            value = RemoveFormat(value.ToString());
            if (string.IsNullOrEmpty(value.ToString()) && value.ToString().Length == 11) 
            {
                return ValidaCPF(value.ToString());
            }
            if(string.IsNullOrEmpty(value.ToString()) && value.ToString().Length == 14)
            {
                return ValidaCNPJ(value.ToString());
            }
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.FormatErrorMessage(null),
                ValidationType = "customvalidationcpfcnpj"
            };
        }

        public static string RemoveFormat(string text)
        {
            var reg = new Regex(@"[^0-9]");
            var ret = reg.Replace(text, string.Empty);
            return ret;
        }

        public static bool ValidaCPF(string cpf)
        {
            //Remove formatação do número, ex: "123.456.789-01" vira: "12345678901"
            cpf = RemoveFormat(cpf);

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                    return false;

            return true;
        }

        public static bool ValidaCNPJ(string cnpj)
        {
            var multiplicador1 = new int[12] {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiplicador2 = new int[13] {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
            {
                return false;
            }

            var tempCnpj = cnpj.Substring(0, 12);

            var soma = 0;

            for (var i = 0; i < 12; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString())*multiplicador1[i];
            }

            var resto = (soma%11);

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }
            var digito = resto.ToString();

            tempCnpj = tempCnpj + digito;

            soma = 0;

            for (var i = 0; i < 13; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString())*multiplicador2[i];
            }

            resto = (soma % 11);

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }
                
            digito = digito + resto;
            return cnpj.EndsWith(digito);

        }
    }
}