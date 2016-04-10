using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetoArtCouro.Api.Controllers.ContasPagar;
using ProjetoArtCouro.Business.Services.CompraService;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Contracts.IService.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.ContaPagar;
using ProjetoArtCouro.Resources.Resources;

namespace ProjetoArtCouro.Test.TesteApiContaPagar
{
    [TestClass]
    public class TestContaPagarController
    {
        private readonly Mock<IContaPagarRepository> _mockContaPagarRepository = new Mock<IContaPagarRepository>();

        private static ContaPagarController CreateContaPagarController(IMock<IContaPagarRepository> mockContaPagarRepository)
        {
            var contaPagarService = new ContaPagarService(mockContaPagarRepository.Object);
            var controller = new ContaPagarController(contaPagarService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            return controller;
        }

        [TestInitialize]
        public void AutoMapper()
        {
            Mapper.CreateMap<ContaPagarModel, ContaPagar>()
                .ForMember(d => d.ContaPagarCodigo, m => m.MapFrom(s => s.CodigoContaPagar))
                .ForMember(d => d.DataVencimento, m => m.MapFrom(s => s.DataVencimento))
                .ForMember(d => d.Pago, m => m.MapFrom(s => s.Pago))
                .ForMember(d => d.StatusContaPagar,
                    m => m.MapFrom(s => Enum.Parse(typeof(StatusContaPagarEnum), s.Status)))
                .ForMember(d => d.ValorDocumento, m => m.MapFrom(s => s.ValorDocumento));

            Mapper.CreateMap<ContaPagar, ContaPagarModel>()
                .ForMember(d => d.CodigoContaPagar, m => m.MapFrom(s => s.ContaPagarCodigo))
                .ForMember(d => d.CodigoCompra, m => m.MapFrom(s => s.Compra.CompraCodigo))
                .ForMember(d => d.CodigoFornecedor, m => m.MapFrom(s => s.Compra.Fornecedor.PessoaCodigo))
                .ForMember(d => d.DataEmissao, m => m.MapFrom(s => s.Compra.DataCadastro))
                .ForMember(d => d.DataVencimento, m => m.MapFrom(s => s.DataVencimento))
                .ForMember(d => d.ValorDocumento, m => m.MapFrom(s => s.ValorDocumento))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.StatusContaPagar.ToString()))
                .ForMember(d => d.StatusId, m => m.MapFrom(s => (int)s.StatusContaPagar))
                .ForMember(d => d.Pago, m => m.MapFrom(s => s.Pago))
                .ForMember(d => d.NomeFornecedor, m => m.MapFrom(s => s.Compra.Fornecedor.Nome))
                .AfterMap((s, d) =>
                {
                    d.CPFCNPJ = s.Compra.Fornecedor.PessoaFisica != null
                        ? s.Compra.Fornecedor.PessoaFisica.CPF
                        : s.Compra.Fornecedor.PessoaJuridica.CNPJ;
                });
        }

        [TestMethod]
        public void TestePesquisarContaPagarMockContaPagarService()
        {
            var mockService = new Mock<IContaPagarService>();
            mockService.Setup(x => x.PesquisarContaPagar(1, 0, new DateTime(), new DateTime(), 1, string.Empty, string.Empty, 1));
            var controller = new ContaPagarController(mockService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var response = controller.PesquisaContaPagar(new PesquisaContaPagarModel());
            var data = response.Result.Content.ReadAsAsync<RetornoBase<object>>();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.AreEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(false, data.Result.TemErros);
            Assert.AreEqual(Mensagens.ReturnSuccess, data.Result.Mensagem);
        }

        [TestMethod]
        public void TestePagarContaMockContaPagarService()
        {
            var mockService = new Mock<IContaPagarService>();
            mockService.Setup(x => x.PagarContas(new List<ContaPagar>()));
            var controller = new ContaPagarController(mockService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var response = controller.PagarConta(new List<ContaPagarModel>());
            var data = response.Result.Content.ReadAsAsync<RetornoBase<object>>();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.AreEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(false, data.Result.TemErros);
            Assert.AreEqual(Mensagens.ReturnSuccess, data.Result.Mensagem);
        }

        [TestMethod]
        public void TestePagarContasSemCodigoContaPagar()
        {
            _mockContaPagarRepository.Setup(x => x.ObterPorCodigoComCompra(1)).Returns(new ContaPagar{Compra = new Compra()});
            _mockContaPagarRepository.Setup(x => x.Atualizar(new ContaPagar()));
            var controller = CreateContaPagarController(_mockContaPagarRepository);
            var response = controller.PagarConta(new List<ContaPagarModel>
            {
                new ContaPagarModel
                {
                    Pago = true,
                    Status = "Aberto"
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.ThereAccountPayableWithCodeZero, data.Result.Mensagem);
        }

        [TestMethod]
        public void TestePagarContasComCodigoContaPagar()
        {
            _mockContaPagarRepository.Setup(x => x.ObterPorCodigoComCompra(1)).Returns(new ContaPagar { Compra = new Compra() });
            _mockContaPagarRepository.Setup(x => x.Atualizar(new ContaPagar()));
            var controller = CreateContaPagarController(_mockContaPagarRepository);
            var response = controller.PagarConta(new List<ContaPagarModel>
            {
                new ContaPagarModel
                {
                    CodigoContaPagar = 1,
                    Pago = true,
                    Status = "Aberto"
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<object>>();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.AreEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(false, data.Result.TemErros);
            Assert.AreEqual(Mensagens.ReturnSuccess, data.Result.Mensagem);
        }
    }
}
