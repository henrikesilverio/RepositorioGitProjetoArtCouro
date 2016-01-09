using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetoArtCouro.Api.Controllers.ContasReceber;
using ProjetoArtCouro.Business.Services.VendaService;
using ProjetoArtCouro.Domain.Contracts.IRepository.IVenda;
using ProjetoArtCouro.Domain.Contracts.IService.IVenda;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.ContaReceber;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Test.TesteApiContaReceber
{
    [TestClass]
    public class TestContaReceberController
    {
        private readonly Mock<IContaReceberRepository> _mockContaReceberRepository = new Mock<IContaReceberRepository>();

        private static ContaReceberController CreateContaReceberController(IMock<IContaReceberRepository> mockContaReceberRepository)
        {
            var contaReceberService = new ContaReceberService(mockContaReceberRepository.Object);
            var controller = new ContaReceberController(contaReceberService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            return controller;
        }

        [TestInitialize]
        public void AutoMapper()
        {
            Mapper.CreateMap<ContaReceberModel, ContaReceber>()
                .ForMember(d => d.ContaReceberCodigo, m => m.MapFrom(s => s.CodigoContaReceber))
                .ForMember(d => d.DataVencimento, m => m.MapFrom(s => s.DataVencimento))
                .ForMember(d => d.Recebido, m => m.MapFrom(s => s.Recebido))
                .ForMember(d => d.StatusContaReceber,
                    m => m.MapFrom(s => Enum.Parse(typeof(StatusContaReceberEnum), s.Status)))
                .ForMember(d => d.ValorDocumento, m => m.MapFrom(s => s.ValorDocumento));

            Mapper.CreateMap<ContaReceber, ContaReceberModel>()
                .ForMember(d => d.CodigoVenda, m => m.MapFrom(s => s.Venda.VendaCodigo))
                .ForMember(d => d.CodigoCliente, m => m.MapFrom(s => s.Venda.Cliente.PessoaCodigo))
                .ForMember(d => d.DataEmissao, m => m.MapFrom(s => s.Venda.DataCadastro))
                .ForMember(d => d.DataVencimento, m => m.MapFrom(s => s.DataVencimento))
                .ForMember(d => d.ValorDocumento, m => m.MapFrom(s => s.ValorDocumento))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.StatusContaReceber.ToString()))
                .ForMember(d => d.StatusId, m => m.MapFrom(s => (int)s.StatusContaReceber))
                .ForMember(d => d.Recebido, m => m.MapFrom(s => s.Recebido))
                .ForMember(d => d.NomeCliente, m => m.MapFrom(s => s.Venda.Cliente.Nome))
                .AfterMap((s, d) =>
                {
                    d.CPFCNPJ = s.Venda.Cliente.PessoaFisica != null
                        ? s.Venda.Cliente.PessoaFisica.CPF
                        : s.Venda.Cliente.PessoaJuridica.CNPJ;
                });
        }

        [TestMethod]
        public void TestePesquisarContaReceberMockContaReceberService()
        {
            var mockService = new Mock<IContaReceberService>();
            mockService.Setup(x => x.PesquisarContaReceber(1, 0, new DateTime(), new DateTime(), 1, string.Empty, string.Empty, 1));
            var controller = new ContaReceberController(mockService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var response = controller.PesquisaContaReceber(new PesquisaContaReceberModel());
            var data = response.Result.Content.ReadAsAsync<RetornoBase<object>>();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.AreEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(false, data.Result.TemErros);
            Assert.AreEqual(Mensagens.ReturnSuccess, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteReceberContaMockContaReceberService()
        {
            var mockService = new Mock<IContaReceberService>();
            mockService.Setup(x => x.ReceberContas(new List<ContaReceber>()));
            var controller = new ContaReceberController(mockService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var response = controller.ReceberConta(new List<ContaReceberModel>());
            var data = response.Result.Content.ReadAsAsync<RetornoBase<object>>();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.AreEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(false, data.Result.TemErros);
            Assert.AreEqual(Mensagens.ReturnSuccess, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteReceberContasSemCodigoContaReceberCodigo()
        {
            _mockContaReceberRepository.Setup(x => x.ObterPorCodigoComVenda(1)).Returns(new ContaReceber{Venda = new Venda()});
            _mockContaReceberRepository.Setup(x => x.Atualizar(new ContaReceber()));
            var controller = CreateContaReceberController(_mockContaReceberRepository);
            var response = controller.ReceberConta(new List<ContaReceberModel>
            {
                new ContaReceberModel
                {
                    Recebido = true,
                    Status = "Aberto"
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.ThereReceivableWithZeroCode, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteReceberContasComCodigoContaReceberCodigo()
        {
            _mockContaReceberRepository.Setup(x => x.ObterPorCodigoComVenda(1)).Returns(new ContaReceber { Venda = new Venda() });
            _mockContaReceberRepository.Setup(x => x.Atualizar(new ContaReceber()));
            var controller = CreateContaReceberController(_mockContaReceberRepository);
            var response = controller.ReceberConta(new List<ContaReceberModel>
            {
                new ContaReceberModel
                {
                    CodigoContaReceber = 1,
                    Recebido = true,
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
