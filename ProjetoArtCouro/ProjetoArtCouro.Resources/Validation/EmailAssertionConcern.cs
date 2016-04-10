using System;
using System.Text.RegularExpressions;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Resources.Validation
{
    public class EmailAssertionConcern
    {
        public static void AssertIsValid(string email)
        {
            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                throw new Exception(Erros.InvalidEmail);
        }
    }
}
