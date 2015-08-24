using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Business.Services.PessoaService
{
    public class PessoaService : IPessoaService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IEstadoCivilRepository _estadoCivilRepository;
        private readonly IEstadoRepository _estadoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IPapelRepository _papelRepository;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;

        public PessoaService(IEnderecoRepository enderecoRepository, IEstadoCivilRepository estadoCivilRepository,
            IEstadoRepository estadoRepository, IPessoaRepository pessoaRepository,
            IPessoaFisicaRepository pessoaFisicaRepository, IPessoaJuridicaRepository pessoaJuridicaRepository,
            IPapelRepository papelRepository)
        {
            _enderecoRepository = enderecoRepository;
            _estadoCivilRepository = estadoCivilRepository;
            _estadoRepository = estadoRepository;
            _pessoaRepository = pessoaRepository;
            _papelRepository = papelRepository;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
        }

        public void CriarPessoaFisica(Pessoa pessoa)
        {
            pessoa.Validar();
            pessoa.PessoaFisica.Validar();
            //Recupera informações do banco
            var codigoPapel = pessoa.Papeis.First().PapelCodigo;
            pessoa.Papeis = new List<Papel>() { _papelRepository.ObterPorCodigo(codigoPapel) };
            pessoa.Enderecos.First().Estado = _estadoRepository.ObterPorCodigo(pessoa.Enderecos.First().Estado.EstadoCodigo);
            pessoa.PessoaFisica.EstadoCivil =
                _estadoCivilRepository.ObterPorCodigo(pessoa.PessoaFisica.EstadoCivil.EstadoCivilCodigo);
            _pessoaRepository.Criar(pessoa);
        }

        public List<PessoaFisica> PesquisarPessoaFisica(int codigo, string nome, string cpf, string email)
        {
            if (codigo.Equals(0) && 
                string.IsNullOrEmpty(nome) && 
                string.IsNullOrEmpty(cpf) && 
                string.IsNullOrEmpty(email))
            {
                throw new Exception(Erros.EmptyParameters);
            };

            return _pessoaFisicaRepository.ObterLista(codigo, nome, cpf, email);
        }

        public List<Estado> ObterEstados()
        {
            return _estadoRepository.ObterLista();
        }

        public List<EstadoCivil> ObterEstadosCivis()
        {
            return _estadoCivilRepository.ObterLista();
        }

        public void Dispose()
        {
            _enderecoRepository.Dispose();
            _estadoCivilRepository.Dispose();
            _estadoRepository.Dispose();
            _pessoaRepository.Dispose();
            _pessoaFisicaRepository.Dispose();
            _pessoaJuridicaRepository.Dispose();
        }
    }
}
