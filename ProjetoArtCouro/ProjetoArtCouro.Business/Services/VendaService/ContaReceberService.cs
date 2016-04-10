using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Contracts.IRepository.IVenda;
using ProjetoArtCouro.Domain.Contracts.IService.IVenda;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.VendaService
{
    public class ContaReceberService : IContaReceberService
    {
        private readonly IContaReceberRepository _contaReceberRepository;

        public ContaReceberService(IContaReceberRepository contaReceberRepository)
        {
            _contaReceberRepository = contaReceberRepository;
        }

        public List<ContaReceber> PesquisarContaReceber(int codigoVenda, int codigoCliente, DateTime dataEmissao, DateTime dataVencimento,
            int statusContaReceber, string nomeCliente, string documento, int codigoUsuario)
        {
            if (codigoVenda.Equals(0) && codigoCliente.Equals(0) &&
                dataEmissao.Equals(new DateTime()) && dataVencimento.Equals(new DateTime()) &&
                statusContaReceber.Equals(0) && string.IsNullOrEmpty(nomeCliente) && 
                string.IsNullOrEmpty(documento) && codigoUsuario.Equals(0))
            {
                throw new Exception(Erros.EmptyParameters);
            };

            return _contaReceberRepository.ObterLista(codigoVenda, codigoCliente, dataEmissao, dataVencimento,
                statusContaReceber, nomeCliente, documento, codigoUsuario);
        }

        public void ReceberContas(List<ContaReceber> contasReceber)
        {
            AssertionConcern.AssertArgumentFalse(contasReceber.Any(x => x.ContaReceberCodigo.Equals(0)), Erros.ThereReceivableWithZeroCode);
            contasReceber.ForEach(x =>
            {
                var contaReceberAtual = _contaReceberRepository.ObterPorCodigoComVenda(x.ContaReceberCodigo);
                contaReceberAtual.Recebido = x.Recebido;
                contaReceberAtual.StatusContaReceber = x.Recebido
                    ? StatusContaReceberEnum.Recebido
                    : StatusContaReceberEnum.Aberto;
                _contaReceberRepository.Atualizar(contaReceberAtual);
            });
        }

        public void Dispose()
        {
            _contaReceberRepository.Dispose();
        }
    }
}
