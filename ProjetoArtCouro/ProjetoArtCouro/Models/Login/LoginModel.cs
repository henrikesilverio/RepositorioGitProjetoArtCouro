using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Web.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "ERR_Name", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Password", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Password", ResourceType = typeof(Mensagens))]
        public string Password { get; set; }
    }
}