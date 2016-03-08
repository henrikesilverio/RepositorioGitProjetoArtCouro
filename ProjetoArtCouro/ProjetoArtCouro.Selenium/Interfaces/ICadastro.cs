using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoArtCouro.Selenium
{
    public interface ICadastro
    {
        void Novo();
        void Cadastrar();
        bool ExisteNaListagem();
    }
}
