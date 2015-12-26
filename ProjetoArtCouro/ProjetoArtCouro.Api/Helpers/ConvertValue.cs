using System;
using System.Text.RegularExpressions;

namespace ProjetoArtCouro.Api.Helpers
{
    public static class ConvertValue
    {
        public static decimal ToDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0.0M;
            }
            decimal newValue;
            var regex = new Regex(@"[a-zA-Z][$]");
            var unscaledValue = regex.Replace(value, "").Trim().Replace(".", "");
            decimal.TryParse(unscaledValue, out newValue);
            return newValue;
        }

        public static int ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            int newValue;
            int.TryParse(value, out newValue);
            return newValue;
        }

        public static DateTime ToDateTime(this string date)
        {
            return string.IsNullOrEmpty(date) ? new DateTime() : DateTime.ParseExact(date, "dd/MM/yyyy H:mm", null);
        }

        public static DateTime ToDateTimeWithoutHour(this string date)
        {
            return string.IsNullOrEmpty(date) ? new DateTime() : DateTime.ParseExact(date, "dd/MM/yyyy", null);
        }

        public static string ToFormatMoney(this decimal value)
        {
            return value.ToString("C");
        }
    }
}