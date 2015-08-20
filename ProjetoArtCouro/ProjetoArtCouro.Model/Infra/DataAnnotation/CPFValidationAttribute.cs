using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ProjetoArtCouro.Model.Infra.DataAnnotation
{
    /// <summary>
    /// Data anotation Valida CPF
    /// </summary>
    public class CPFValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            value = RemoveFormat(value.ToString());
            if (string.IsNullOrEmpty(value.ToString()) && value.ToString().Length == 11)
            {
                return ValidaCPF(value.ToString());
            }
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.FormatErrorMessage(null),
                ValidationType = "customvalidationcpf"
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
            {
                return false;
            }

            while (cpf.Length != 11)
            {
                cpf = '0' + cpf;
            }

            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
            {
                if (cpf[i] != cpf[0])
                    igual = false;
            }

            if (igual || cpf == "12345678909")
            {
                return false;
            }

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0 && numeros[9] != 0)
            {
                return false;
            }
            if (numeros[9] != 11 - resultado)
            {
                return false;
            }

            soma = 0;
            for (var i = 0; i < 10; i++)
            {
                soma += (11 - i) * numeros[i];
            }

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0 && numeros[10] != 0)
            {
                return false;
            }

            return numeros[10] == 11 - resultado;
        }
    }
}
