using System;
using System.Collections.Generic;

namespace ProjetoArtCouro.Domain.Models.Usuarios
{
    public class Permissao
    {
        public Guid PermissaoId { get; set; }
        public int PermissaoCodigo { get; set; }
        public string PermissaoNome { get; set; }
        public string AcaoNome { get; set; }
        public virtual ICollection<GrupoPermissao> GrupoPermissao { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
