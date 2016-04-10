using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Model.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "ERR_Name", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string UsuarioNome { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Password", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Password", ResourceType = typeof(Mensagens))]
        public string Senha { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}