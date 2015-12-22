using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IService.IPessoa;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.PessoaService
{
    public class PessoaService : IPessoaService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IEstadoCivilRepository _estadoCivilRepository;
        private readonly IEstadoRepository _estadoRepository;
        private readonly IMeioComunicacaoRepository _meioComunicacaoRepository;
        private readonly IPapelRepository _papelRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;

        public PessoaService(IEnderecoRepository enderecoRepository, IEstadoCivilRepository estadoCivilRepository,
            IEstadoRepository estadoRepository, IMeioComunicacaoRepository meioComunicacaoRepository, IPessoaRepository pessoaRepository,
            IPessoaFisicaRepository pessoaFisicaRepository, IPessoaJuridicaRepository pessoaJuridicaRepository,
            IPapelRepository papelRepository)
        {
            _enderecoRepository = enderecoRepository;
            _estadoCivilRepository = estadoCivilRepository;
            _estadoRepository = estadoRepository;
            _meioComunicacaoRepository = meioComunicacaoRepository;
            _papelRepository = papelRepository;
            _pessoaRepository = pessoaRepository;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
        }

        public void CriarPessoaFisica(Pessoa pessoa)
        {
            pessoa.Validar();
            pessoa.PessoaFisica.Validar();
            //Verifica se a pessoa existe, caso exista atualiza o papel da pessoa
            var existePessoaFisica = _pessoaFisicaRepository.ObterPorCPF(pessoa.PessoaFisica.CPF);
            //Recupera informações do banco
            var firstOrDefault = pessoa.Papeis.FirstOrDefault();
            AssertionConcern.AssertArgumentNotNull(firstOrDefault, string.Format(Erros.NullParameter, "Papeis"));
            if (firstOrDefault != null)
            {
                pessoa.Papeis = new List<Papel>() { _papelRepository.ObterPorCodigo(firstOrDefault.PapelCodigo) };
            }

            if (existePessoaFisica != null)
            {
                existePessoaFisica.Pessoa.Papeis.Add(pessoa.Papeis.First());
                _pessoaRepository.Atualizar(existePessoaFisica.Pessoa);
            }
            else
            {
                pessoa.Enderecos.First().Estado = _estadoRepository.ObterPorCodigo(pessoa.Enderecos.First().Estado.EstadoCodigo);
                pessoa.PessoaFisica.EstadoCivil =
                    _estadoCivilRepository.ObterPorCodigo(pessoa.PessoaFisica.EstadoCivil.EstadoCivilCodigo);
                _pessoaRepository.Criar(pessoa);
            }
        }

        public void CriarPessoaJuridica(Pessoa pessoa)
        {
            pessoa.Validar();
            pessoa.PessoaJuridica.Validar();
            //Verifica se a pessoa existe, caso exista atualiza o papel da pessoa
            var existePessoaJuridica = _pessoaJuridicaRepository.ObterPorCNPJ(pessoa.PessoaJuridica.CNPJ);
            //Recupera informações do banco
            var firstOrDefault = pessoa.Papeis.FirstOrDefault();
            AssertionConcern.AssertArgumentNotNull(firstOrDefault, string.Format(Erros.NullParameter, "Papeis"));
            if (firstOrDefault != null)
            {
                pessoa.Papeis = new List<Papel>() { _papelRepository.ObterPorCodigo(firstOrDefault.PapelCodigo) };
            }

            if (existePessoaJuridica != null)
            {
                existePessoaJuridica.Pessoa.Papeis.Add(pessoa.Papeis.First());
                _pessoaRepository.Atualizar(existePessoaJuridica.Pessoa);
            }
            else
            {
                pessoa.Enderecos.First().Estado = _estadoRepository.ObterPorCodigo(pessoa.Enderecos.First().Estado.EstadoCodigo);
                _pessoaRepository.Criar(pessoa);
            }
        }

        public void AtualizarPessoa(Pessoa pessoa)
        {
            var pessoaAtual = _pessoaRepository.ObterPorCodigoComPessoaCompleta(pessoa.PessoaCodigo);
            AssertionConcern.AssertArgumentNotNull(pessoaAtual, Erros.PersonDoesNotExist);

            pessoaAtual.Nome = pessoa.Nome;
            if (pessoaAtual.PessoaFisica != null)
            {
                pessoaAtual.PessoaFisica.RG = pessoa.PessoaFisica.RG;
                pessoaAtual.PessoaFisica.Sexo = pessoa.PessoaFisica.Sexo;
                pessoaAtual.PessoaFisica.EstadoCivil = _estadoCivilRepository.ObterPorCodigo(pessoa.PessoaFisica.EstadoCivil.EstadoCivilCodigo);
            }
            else
            {
                pessoaAtual.PessoaJuridica.Contato = pessoa.PessoaJuridica.Contato;
            }

            //Adiciona um novo endereço ou modifica o exitente para principal
            AtualizarEnderecoPessoa(pessoa, pessoaAtual);
            //Adiciona um meio de cominicação ou modifica o exitente para principal
            AtualizarMeioCominicacaoPessoa(pessoa, pessoaAtual, TipoComunicacaoEnum.Telefone);
            AtualizarMeioCominicacaoPessoa(pessoa, pessoaAtual, TipoComunicacaoEnum.Celular);
            AtualizarMeioCominicacaoPessoa(pessoa, pessoaAtual, TipoComunicacaoEnum.Email);
            _pessoaRepository.Atualizar(pessoaAtual);
        }

        public void ExcluirPessoa(int pessoaCodigo)
        {
            AssertionConcern.AssertArgumentNotEquals(pessoaCodigo, 0, Erros.InvalidCode);
            var pessoa = _pessoaRepository.ObterPorCodigoComPessoaCompleta(pessoaCodigo);
            AssertionConcern.AssertArgumentNotNull(pessoa, Erros.PersonDoesNotExist);
            _pessoaRepository.Deletar(pessoa);
        }

        public List<PessoaFisica> PesquisarPessoaFisica(int codigo, string nome, string cpf, string email, TipoPapelPessoaEnum papelCodigo)
        {
            if (codigo.Equals(0) &&
                string.IsNullOrEmpty(nome) &&
                string.IsNullOrEmpty(cpf) &&
                string.IsNullOrEmpty(email) &&
                papelCodigo.Equals(TipoPapelPessoaEnum.Nenhum))
            {
                throw new Exception(Erros.EmptyParameters);
            };

            return _pessoaFisicaRepository.ObterLista(codigo, nome, cpf, email, papelCodigo);
        }

        public List<PessoaJuridica> PesquisarPessoaJuridica(int codigo, string nome, string cnpj, string email, TipoPapelPessoaEnum papelCodigo)
        {
            if (codigo.Equals(0) &&
                string.IsNullOrEmpty(nome) &&
                string.IsNullOrEmpty(cnpj) &&
                string.IsNullOrEmpty(email) &&
                papelCodigo.Equals(TipoPapelPessoaEnum.Nenhum))
            {
                throw new Exception(Erros.EmptyParameters);
            }

            return _pessoaJuridicaRepository.ObterLista(codigo, nome, cnpj, email, papelCodigo);
        }

        public Pessoa ObterPessoaPorCodigo(int codigo)
        {
            return _pessoaRepository.ObterPorCodigoComPessoaCompleta(codigo);
        }

        public List<Estado> ObterEstados()
        {
            return _estadoRepository.ObterLista();
        }

        public List<EstadoCivil> ObterEstadosCivis()
        {
            return _estadoCivilRepository.ObterLista();
        }

        public List<PessoaFisica> ObterListaPessoaFisicaPorPapel(TipoPapelPessoaEnum papelCodigo)
        {
            return _pessoaFisicaRepository.ObterLista(0, null, null, null, papelCodigo);
        }

        public List<PessoaJuridica> ObterListaPessoaJuridicaPorPapel(TipoPapelPessoaEnum papelCodigo)
        {
            return _pessoaJuridicaRepository.ObterLista(0, null, null, null, papelCodigo);
        }

        private void AtualizarEnderecoPessoa(Pessoa pessoa, Pessoa pessoaAtual)
        {
            //Adiciona um novo endereço ou modifica o exitente para principal
            var enderecoAtualizar = pessoa.Enderecos.FirstOrDefault();
            AssertionConcern.AssertArgumentNotEquals(enderecoAtualizar, null, Erros.EmptyAddress);
            if (enderecoAtualizar != null && enderecoAtualizar.EnderecoCodigo.Equals(0) &&
                !pessoaAtual.Enderecos.Any(x =>
                    x.Bairro.Equals(enderecoAtualizar.Bairro) &&
                    x.CEP.Equals(enderecoAtualizar.CEP) &&
                    x.Cidade.Equals(enderecoAtualizar.Cidade) &&
                    x.Logradouro.Equals(enderecoAtualizar.Logradouro) &&
                    x.Numero.Equals(enderecoAtualizar.Numero)))
            {
                enderecoAtualizar.Pessoa = pessoaAtual;
                enderecoAtualizar.Estado = _estadoRepository.ObterPorCodigo(enderecoAtualizar.Estado.EstadoCodigo);
                var novoEndereco = _enderecoRepository.Criar(enderecoAtualizar);
                pessoaAtual.Enderecos.Add(novoEndereco);
            }
            else if (enderecoAtualizar != null && !enderecoAtualizar.EnderecoCodigo.Equals(0))
            {
                //Encotra o endereço que sera o principal
                foreach (var item in pessoaAtual.Enderecos)
                {
                    item.Principal = item.EnderecoCodigo.Equals(enderecoAtualizar.EnderecoCodigo);
                }
            }
        }

        private void AtualizarMeioCominicacaoPessoa(Pessoa pessoa, Pessoa pessoaAtual, TipoComunicacaoEnum tipoComunicacao)
        {
            //Adiciona um meio de cominicação ou modifica o exitente para principal
            var meioComunicacaoAtualizar = pessoa.MeiosComunicacao.FirstOrDefault(x => x.TipoComunicacao.Equals(tipoComunicacao));
            if (meioComunicacaoAtualizar == null && (tipoComunicacao == TipoComunicacaoEnum.Email || tipoComunicacao == TipoComunicacaoEnum.Celular))
            {
                return;
            }
            AssertionConcern.AssertArgumentNotEquals(meioComunicacaoAtualizar, null, Erros.EmptyPhone);
            if (meioComunicacaoAtualizar != null && meioComunicacaoAtualizar.MeioComunicacaoCodigo.Equals(0) && 
                !pessoaAtual.MeiosComunicacao.Any(x => x.MeioComunicacaoNome.Equals(meioComunicacaoAtualizar.MeioComunicacaoNome)))
            {
                meioComunicacaoAtualizar.Pessoa = pessoaAtual;
                var novoMeioComunicacao = _meioComunicacaoRepository.Criar(meioComunicacaoAtualizar);
                pessoaAtual.MeiosComunicacao.Add(novoMeioComunicacao);
            }
            else if (meioComunicacaoAtualizar != null && !meioComunicacaoAtualizar.MeioComunicacaoCodigo.Equals(0))
            {
                //Encotra o endereço que sera o principal
                foreach (var item in pessoaAtual.MeiosComunicacao)
                {
                    item.Principal = item.MeioComunicacaoCodigo.Equals(meioComunicacaoAtualizar.MeioComunicacaoCodigo);
                }
            }
        }

        public void Dispose()
        {
            _enderecoRepository.Dispose();
            _estadoCivilRepository.Dispose();
            _estadoRepository.Dispose();
            _meioComunicacaoRepository.Dispose();
            _papelRepository.Dispose();
            _pessoaRepository.Dispose();
            _pessoaFisicaRepository.Dispose();
            _pessoaJuridicaRepository.Dispose();
        }
    }
}
