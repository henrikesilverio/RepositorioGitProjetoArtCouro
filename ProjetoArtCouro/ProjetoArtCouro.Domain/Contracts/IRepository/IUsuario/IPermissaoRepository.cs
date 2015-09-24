using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario
{
    public interface IPermissaoRepository : IDisposable
    {
        List<Permissao> ObterLista();
    }
}
