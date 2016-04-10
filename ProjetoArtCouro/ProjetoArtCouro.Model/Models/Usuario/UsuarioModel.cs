using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Model.Models.Usuario
{
    public class UsuarioModel
    {
        public Guid UsuarioId { get; set; }

        public int? UsuarioCodigo { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Erros))]
        [Display(Name = "Name", ResourceType = typeof(Mensagens))]
        public string UsuarioNome { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Erros))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Mensagens))]
        public string Senha { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Erros))]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessageResourceName = "ConfirmPasswordAndPasswordNotMatch", ErrorMessageResourceType = typeof(Erros))]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Mensagens))]
        public string ConfirmarSenha { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Erros))]
        [Display(Name = "Active", ResourceType = typeof(Mensagens))]
        public bool Ativo { get; set; }

        [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Erros))]
        [Display(Name = "PermissionsGroups", ResourceType = typeof(Mensagens))]
        public int? GrupoId { get; set; }

        public List<PermissaoModel> Permissoes { get; set; }
    }
}
