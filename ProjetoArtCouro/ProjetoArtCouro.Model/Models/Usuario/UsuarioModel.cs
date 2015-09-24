using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Model.Models.Usuario
{
    public class UsuarioModel
    {
        public Guid UsuarioId { get; set; }

        public int? UsuarioCodigo { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Name", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string UsuarioNome { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Password", ErrorMessageResourceType = typeof(Mensagens))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Mensagens))]
        public string Senha { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Password", ErrorMessageResourceType = typeof(Mensagens))]
        [DataType(DataType.Password)]
        [Compare("ComfirmaSenha")]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Mensagens))]
        public string ConfirmarSenha { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Name", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Active", ResourceType = typeof(Mensagens))]
        public bool Ativo { get; set; }

        [Required(ErrorMessageResourceName = "ERR_Name", ErrorMessageResourceType = typeof(Mensagens))]
        [Display(Name = "Permissions", ResourceType = typeof(Mensagens))]
        public int? PermissaoId { get; set; }

        public List<PermissaoModel> Permissoes { get; set; }
    }
}
