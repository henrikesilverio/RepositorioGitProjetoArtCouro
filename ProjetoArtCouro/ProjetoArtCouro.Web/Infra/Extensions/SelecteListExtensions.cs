using System.Collections.Generic;
using System.Web.Mvc;

namespace ProjetoArtCouro.Web.Infra.Extensions
{
    public static class SelecteListExtensions
    {
        public static IEnumerable<SelectListItem> EmptyListWithDefaultOption(string dataTextField, string selectedValue = "")
        {
            var items = new List<SelectListItem> {new SelectListItem() {Text = dataTextField, Value = selectedValue}};
            return items;
        }

        public static IEnumerable<SelectListItem> AddDefaultOption(this IEnumerable<SelectListItem> list, string dataTextField, string selectedValue = "")
        {
            var items = new List<SelectListItem> {new SelectListItem() {Text = dataTextField, Value = selectedValue}};
            items.AddRange(list);
            return items;
        }

        public static IEnumerable<SelectListItem> AddDefaultLastOption(this IEnumerable<SelectListItem> list, string dataTextField, string selectedValue = "")
        {
            var items = new List<SelectListItem>();
            items.AddRange(list);
            items.Add(new SelectListItem() { Text = dataTextField, Value = selectedValue });
            return items;
        }
    }
}