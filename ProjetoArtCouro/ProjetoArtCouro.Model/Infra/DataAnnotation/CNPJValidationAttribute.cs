using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ProjetoArtCouro.Model.Infra.DataAnnotation
{
    /// <summary>
    /// Data anotation Valida CNPJ
    /// </summary>
    public class CNPJValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            value = RemoveFormat(value.ToString());
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
                ErrorMessage = FormatErrorMessage(null),
                ValidationType = "customvalidationcnpj"
            };
        }

        public static string RemoveFormat(string text)
        {
            var reg = new Regex(@"[^0-9]");
            var ret = reg.Replace(text, string.Empty);
            return ret;
        }

        public static bool ValidaCNPJ(string cnpj)
        {
            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
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
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            }

            var resto = (soma % 11);

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
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
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