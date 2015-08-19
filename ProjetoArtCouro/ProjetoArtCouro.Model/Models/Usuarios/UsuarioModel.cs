using System;
using System.Collections.Generic;

namespace ProjetoArtCouro.Model.Models.Usuarios
{
    public class UsuarioModel
    {
        public Guid UsuarioId { get; set; }
        public string UsuarioNome { get; set; }
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
        public List<PermissaoModel> Permissoes { get; set; }
    }
}
