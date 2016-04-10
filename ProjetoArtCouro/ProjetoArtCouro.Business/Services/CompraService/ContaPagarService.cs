using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Contracts.IService.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Business.Services.CompraService
{
    public class ContaPagarService : IContaPagarService
    {
        private readonly IContaPagarRepository _contaPagarRepository;
        public ContaPagarService(IContaPagarRepository contaPagarRepository)
        {
            _contaPagarRepository = contaPagarRepository;
        }

        public List<ContaPagar> PesquisarContaPagar(int codigoCompra, int codigoFornecedor, DateTime dataEmissao, DateTime dataVencimento,
            int statusContaPagar, string nomeFornecedor, string documento, int codigoUsuario)
        {
            if (codigoCompra.Equals(0) && codigoFornecedor.Equals(0) &&
                dataEmissao.Equals(new DateTime()) && dataVencimento.Equals(new DateTime()) &&
                statusContaPagar.Equals(0) && string.IsNullOrEmpty(nomeFornecedor) &&
                string.IsNullOrEmpty(documento) && codigoUsuario.Equals(0))
            {
                throw new Exception(Erros.EmptyParameters);
            };

            return _contaPagarRepository.ObterLista(codigoCompra, codigoFornecedor, dataEmissao, dataVencimento,
                statusContaPagar, nomeFornecedor, documento, codigoUsuario);
        }

        public void PagarContas(List<ContaPagar> contasPagar)
        {
            AssertionConcern.AssertArgumentFalse(contasPagar.Any(x => x.ContaPagarCodigo.Equals(0)), Erros.ThereAccountPayableWithCodeZero);
            contasPagar.ForEach(x =>
            {
                var contaPagarAtual = _contaPagarRepository.ObterPorCodigoComCompra(x.ContaPagarCodigo);
                contaPagarAtual.Pago = x.Pago;
                contaPagarAtual.StatusContaPagar = x.Pago
                    ? StatusContaPagarEnum.Pago
                    : StatusContaPagarEnum.Aberto;
                _contaPagarRepository.Atualizar(contaPagarAtual);
            });
        }

        public void Dispose()
        {
            _contaPagarRepository.Dispose();
        }
    }
}
