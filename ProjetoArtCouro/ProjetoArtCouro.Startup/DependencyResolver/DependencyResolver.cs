using Microsoft.Practices.Unity;
using ProjetoArtCouro.Business.Services.AutenticacaoService;
using ProjetoArtCouro.Business.Services.PessoaService;
using ProjetoArtCouro.Business.Services.UsuarioService;
using ProjetoArtCouro.DataBase.DataBase;
using ProjetoArtCouro.DataBase.Repositorios.PessoaRepository;
using ProjetoArtCouro.DataBase.Repositorios.UsuarioRepository;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IService.IUsuario;

namespace ProjetoArtCouro.Startup.DependencyResolver
{
    public static class DependencyResolver
    {
        public static void Resolve(UnityContainer container)
        {
            container.RegisterType<DataBaseContext, DataBaseContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioService, UsuarioService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioRepository, UsuarioRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAutenticacao, AutenticacaoService>(new HierarchicalLifetimeManager());

            container.RegisterType<IPessoaService, PessoaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPessoaRepository, PessoaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPessoaFisicaRepository, PessoaFisicaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPessoaJuridicaRepository, PessoaJuridicaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEnderecoRepository, EnderecoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEstadoCivilRepository, EstadoCivilRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEstadoRepository, EstadoRepository>(new HierarchicalLifetimeManager());


            //container.RegisterType<User, User>(new HierarchicalLifetimeManager());
        }
    }
}