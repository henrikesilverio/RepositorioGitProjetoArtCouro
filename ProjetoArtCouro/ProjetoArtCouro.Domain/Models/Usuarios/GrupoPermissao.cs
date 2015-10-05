using System;
using System.Collections.Generic;

namespace ProjetoArtCouro.Domain.Models.Usuarios
{
    public class GrupoPermissao
    {
        public Guid GrupoPermissaoId { get; set; }
        public int GrupoPermissaoCodigo { get; set; }
        public string GrupoPermissaoNome { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Permissao> Permissoes { get; set; }
    }
}
