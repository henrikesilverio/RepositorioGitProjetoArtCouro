using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetoArtCouro.Api.Controllers.Compras;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Business.Services.CompraService;
using ProjetoArtCouro.Domain.Contracts.IRepository.ICompra;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IService.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Compra;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Test.TesteApiCompra
{
    [TestClass]
    public class TestCompraController
    {
        private readonly Mock<ICompraRepository> _mockCompraRepository = new Mock<ICompraRepository>();
        private readonly Mock<IItemCompraRepository> _mockItemCompraRepository = new Mock<IItemCompraRepository>();
        private readonly Mock<IPessoaRepository> _mockPessoaRepository = new Mock<IPessoaRepository>();
        private readonly Mock<IFormaPagamentoRepository> _mockFormaPagamentoRepository =
            new Mock<IFormaPagamentoRepository>();
        private readonly Mock<ICondicaoPagamentoRepository> _mockCondicaoPagamentoRepository =
            new Mock<ICondicaoPagamentoRepository>();
        private readonly Mock<IUsuarioRepository> _usuarioRepository = new Mock<IUsuarioRepository>();

        private CompraController CreateCompraController()
        {
            _mockCompraRepository.Setup(x => x.Criar(new Compra()));
            _mockPessoaRepository.Setup(x => x.ObterPorCodigo(1)).Returns(new Pessoa());
            _mockFormaPagamentoRepository.Setup(x => x.ObterPorCodigo(1)).Returns(new FormaPagamento());
            _mockCondicaoPagamentoRepository.Setup(x => x.ObterPorCodigo(1)).Returns(new CondicaoPagamento());

            var compraService = new CompraService(_mockCompraRepository.Object, _mockItemCompraRepository.Object,
                _mockPessoaRepository.Object, _mockFormaPagamentoRepository.Object,
                _mockCondicaoPagamentoRepository.Object, _usuarioRepository.Object);

            var controller = new CompraController(compraService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            return controller;
        }

        [TestInitialize]
        public void AutoMapper()
        {
            Mapper.CreateMap<CompraModel, Compra>()
               .ForMember(d => d.Fornecedor, m => m.MapFrom(s => new Pessoa { PessoaCodigo = s.FornecedorId ?? 0 }))
               .ForMember(d => d.CondicaoPagamento,
                   m => m.MapFrom(s => new CondicaoPagamento { CondicaoPagamentoCodigo = s.CondicaoPagamentoId ?? 0 }))
               .ForMember(d => d.FormaPagamento,
                   m => m.MapFrom(s => new FormaPagamento { FormaPagamentoCodigo = s.FormaPagamentoId ?? 0 }))
               .ForMember(d => d.DataCadastro, m => m.MapFrom(s => s.DataCadastro.ToDateTime()))
               .ForMember(d => d.ItensCompra, m => m.MapFrom(s => s.ItemCompraModel))
               .ForMember(d => d.StatusCompra, m => m.MapFrom(s => Enum.Parse(typeof(StatusCompraEnum), s.Status)))
               .ForMember(d => d.CompraCodigo, m => m.MapFrom(s => s.CodigoCompra ?? 0))
               .ForMember(d => d.ValorTotalBruto, m => m.MapFrom(s => s.ValorTotalBruto.ToDecimal()))
               .ForMember(d => d.ValorTotalFrete, m => m.MapFrom(s => s.ValorTotalFrete.ToDecimal()))
               .ForMember(d => d.ValorTotalLiquido, m => m.MapFrom(s => s.ValorTotalLiquido.ToDecimal()));

            Mapper.CreateMap<ItemCompraModel, ItemCompra>()
                .ForMember(d => d.ProdutoCodigo, m => m.MapFrom(s => s.Codigo))
                .ForMember(d => d.ProdutoNome, m => m.MapFrom(s => s.Descricao))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => s.PrecoVenda.ToDecimal()))
                .ForMember(d => d.ValorBruto, m => m.MapFrom(s => s.ValorBruto.ToDecimal()))
                .ForMember(d => d.ValorFrete, m => m.MapFrom(s => s.ValorFrete.ToDecimal()))
                .ForMember(d => d.ValorLiquido, m => m.MapFrom(s => s.ValorLiquido.ToDecimal()));
        }

        [TestMethod]
        public void TesteCriarCompraMockCompraService()
        {
            var mockService = new Mock<ICompraService>();
            mockService.Setup(x => x.CriarCompra(new Compra()));
            var controller = new CompraController(mockService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var response = controller.CriarCompra(new CompraModel() { Status = "Aberto" });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<object>>();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.AreEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(false, data.Result.TemErros);
            Assert.AreEqual(Mensagens.ReturnSuccess, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraSemDataCadastro()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel() { Status = "Aberto" });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.InvalidParameter, "DataCadastro"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraSemStatusCompra()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "None",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now)
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.InvalidParameter, "StatusCompra"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraSemValorTotalBruto()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now)
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.NotZeroParameter, "ValorTotalBruto"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraSemValorTotalLiquido()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05"
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.NotZeroParameter, "ValorTotalLiquido"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraSemItemCompraModel()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.SaleItemsNotSet, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraItemCompraModelSemCodigo()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 1",
                ValorTotalLiquido = "R$ 1",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        ValorBruto = "R$ 1",
                        ValorLiquido = "R$ 1"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.NotZeroParameter, "ProdutoCodigo"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraItemCompraModelSemDescricao()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        ValorBruto = "R$ 2,05",
                        ValorLiquido = "R$ 2,05"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.EmptyParameter, "ProdutoNome"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraItemCompraModelSemQuantidade()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        ValorBruto = "R$ 2,05",
                        ValorLiquido = "R$ 2,05"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.NotZeroParameter, "Quantidade"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraItemCompraModelSemPrecoVenda()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        ValorBruto = "R$ 2,05",
                        ValorLiquido = "R$ 2,05"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.NotZeroParameter, "PrecoVenda"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraItemCompraModelSemValorBruto()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ValorFrete = "R$ 0",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.NotZeroParameter, "ValorBruto"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraItemCompraModelSemValorLiquido()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.NotZeroParameter, "ValorLiquido"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraComStatusDiferenteDeAberto()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Confirmado",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorLiquido = "R$ 30,00"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.StatusOfDifferentPurchasingOpen, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraComSomaValorBrutoDiferenteValorTotalBruto()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorFrete = "R$ 0",
                ValorTotalLiquido = "R$ 60,00",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorLiquido = "R$ 30,00"
                    },
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorLiquido = "R$ 30,00"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.SumDoNotMatchTotalCrudeValue, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraComSomaValorLiquidoDiferenteValorTotalLiquido()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 60,00",
                ValorFrete = "R$ 0",
                ValorTotalLiquido = "R$ 2,05",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorLiquido = "R$ 30,00"
                    },
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorLiquido = "R$ 30,00"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.SumDoNotMatchTotalValueLiquid, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarCompraComSomaValorFreteDiferenteValorTotalFrete()
        {
            var controller = CreateCompraController();
            var response = controller.CriarCompra(new CompraModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 60,00",
                ValorTotalFrete = "R$ 2,05",
                ValorTotalLiquido = "R$ 60,00",
                FornecedorId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemCompraModel = new List<ItemCompraModel>
                {
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorFrete = "R$ 1,00",
                        ValorLiquido = "R$ 30,00"
                    },
                    new ItemCompraModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorFrete = "R$ 1,00",
                        ValorLiquido = "R$ 30,00"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.SumDoNotMatchTotalValueShipping, data.Result.Mensagem);
        }
    }
}
