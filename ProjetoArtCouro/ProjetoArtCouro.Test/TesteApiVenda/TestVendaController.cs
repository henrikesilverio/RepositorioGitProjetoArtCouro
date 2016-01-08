using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjetoArtCouro.Api.Controllers.Vendas;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Business.Services.VendaService;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPagamento;
using ProjetoArtCouro.Domain.Contracts.IRepository.IPessoa;
using ProjetoArtCouro.Domain.Contracts.IRepository.IUsuario;
using ProjetoArtCouro.Domain.Contracts.IRepository.IVenda;
using ProjetoArtCouro.Domain.Contracts.IService.IVenda;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Venda;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Test.TesteApiVenda
{
    [TestClass]
    public class TestVendaController
    {
        private readonly Mock<IVendaRepository> _mockVendaRepository = new Mock<IVendaRepository>();
        private readonly Mock<IItemVendaRepository> _mockItemVendaRepository = new Mock<IItemVendaRepository>();
        private readonly Mock<IContaReceberRepository> _mockContaReceberRepository = new Mock<IContaReceberRepository>();
        private readonly Mock<IPessoaRepository> _mockPessoaRepository = new Mock<IPessoaRepository>();
        private readonly Mock<IFormaPagamentoRepository> _mockFormaPagamentoRepository =
            new Mock<IFormaPagamentoRepository>();
        private readonly Mock<ICondicaoPagamentoRepository> _mockCondicaoPagamentoRepository =
            new Mock<ICondicaoPagamentoRepository>();
        private readonly Mock<IUsuarioRepository> _usuarioRepository = new Mock<IUsuarioRepository>();

        private VendaController CreateVendaController()
        {
            _mockVendaRepository.Setup(x => x.Criar(new Venda()));
            _mockPessoaRepository.Setup(x => x.ObterPorCodigo(1)).Returns(new Pessoa());
            _mockFormaPagamentoRepository.Setup(x => x.ObterPorCodigo(1)).Returns(new FormaPagamento());
            _mockCondicaoPagamentoRepository.Setup(x => x.ObterPorCodigo(1)).Returns(new CondicaoPagamento());

            var vendaService = new VendaService(_mockVendaRepository.Object, _mockItemVendaRepository.Object,
                _mockPessoaRepository.Object, _mockFormaPagamentoRepository.Object,
                _mockCondicaoPagamentoRepository.Object, _usuarioRepository.Object, _mockContaReceberRepository.Object);

            var controller = new VendaController(vendaService)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            return controller;
        }

        [TestInitialize]
        public void AutoMapper()
        {
            Mapper.CreateMap<VendaModel, Venda>()
               .ForMember(d => d.Cliente, m => m.MapFrom(s => new Pessoa { PessoaCodigo = s.ClienteId ?? 0 }))
               .ForMember(d => d.CondicaoPagamento,
                   m => m.MapFrom(s => new CondicaoPagamento { CondicaoPagamentoCodigo = s.CondicaoPagamentoId ?? 0 }))
               .ForMember(d => d.FormaPagamento,
                   m => m.MapFrom(s => new FormaPagamento { FormaPagamentoCodigo = s.FormaPagamentoId ?? 0 }))
               .ForMember(d => d.DataCadastro, m => m.MapFrom(s => s.DataCadastro.ToDateTime()))
               .ForMember(d => d.ItensVenda, m => m.MapFrom(s => s.ItemVendaModel))
               .ForMember(d => d.StatusVenda, m => m.MapFrom(s => Enum.Parse(typeof(StatusVendaEnum), s.Status)))
               .ForMember(d => d.VendaCodigo, m => m.MapFrom(s => s.CodigoVenda ?? 0))
               .ForMember(d => d.ValorTotalBruto, m => m.MapFrom(s => s.ValorTotalBruto.ToDecimal()))
               .ForMember(d => d.ValorTotalDesconto, m => m.MapFrom(s => s.ValorTotalDesconto.ToDecimal()))
               .ForMember(d => d.ValorTotalLiquido, m => m.MapFrom(s => s.ValorTotalLiquido.ToDecimal()));

            Mapper.CreateMap<ItemVendaModel, ItemVenda>()
                .ForMember(d => d.ProdutoCodigo, m => m.MapFrom(s => s.Codigo))
                .ForMember(d => d.ProdutoNome, m => m.MapFrom(s => s.Descricao))
                .ForMember(d => d.PrecoVenda, m => m.MapFrom(s => s.PrecoVenda.ToDecimal()))
                .ForMember(d => d.ValorBruto, m => m.MapFrom(s => s.ValorBruto.ToDecimal()))
                .ForMember(d => d.ValorDesconto, m => m.MapFrom(s => s.ValorDesconto.ToDecimal()))
                .ForMember(d => d.ValorLiquido, m => m.MapFrom(s => s.ValorLiquido.ToDecimal()));
        }

        [TestMethod]
        public void TesteCriarVendaMockVendaService()
        {
            var mockService = new Mock<IVendaService>();
            mockService.Setup(x => x.CriarVenda(new Venda()));
            var controller = new VendaController(mockService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var response = controller.CriarVenda(new VendaModel() { Status = "Aberto" });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<object>>();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.AreEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(false, data.Result.TemErros);
            Assert.AreEqual(Mensagens.ReturnSuccess, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarVendaSemDataCadastro()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel() { Status = "Aberto" });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.InvalidParameter, "DataCadastro"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarVendaSemStatusVenda()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "None",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now)
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(string.Format(Erros.InvalidParameter, "StatusVenda"), data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarVendaSemValorTotalBruto()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
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
        public void TesteCriarVendaSemValorTotalLiquido()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
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

        //[TestMethod]
        //public void TesteCriarVendaSemClienteId()
        //{
        //    var controller = CreateVendaController();
        //    var response = controller.CriarVenda(new VendaModel()
        //    {
        //        Status = "Aberto",
        //        DataCadastro = DateTime.Now,
        //        ValorTotalBruto = "R$ 2,05",
        //        ValorTotalLiquido = "R$ 2,05"
        //    });
        //    var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
        //    Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
        //    Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
        //    Assert.AreEqual(true, data.Result.TemErros);
        //    Assert.AreEqual(Erros.ClientNotSet, data.Result.Mensagem);
        //}

        //[TestMethod]
        //public void TesteCriarVendaSemFormaPagamentoId()
        //{
        //    var controller = CreateVendaController();
        //    var response = controller.CriarVenda(new VendaModel()
        //    {
        //        Status = "Aberto",
        //        DataCadastro = DateTime.Now,
        //        ValorTotalBruto = "R$ 2,05",
        //        ValorTotalLiquido = "R$ 2,05",
        //        ClienteId = 1
        //    });
        //    var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
        //    Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
        //    Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
        //    Assert.AreEqual(true, data.Result.TemErros);
        //    Assert.AreEqual(Erros.NotSetPayment, data.Result.Mensagem);
        //}

        //[TestMethod]
        //public void TesteCriarVendaSemCondicaoPagamentoId()
        //{
        //    var controller = CreateVendaController();
        //    var response = controller.CriarVenda(new VendaModel()
        //    {
        //        Status = "Aberto",
        //        DataCadastro = DateTime.Now,
        //        ValorTotalBruto = "R$ 2,05",
        //        ValorTotalLiquido = "R$ 2,05",
        //        ClienteId = 1,
        //        FormaPagamentoId = 1
        //    });
        //    var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
        //    Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
        //    Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
        //    Assert.AreEqual(true, data.Result.TemErros);
        //    Assert.AreEqual(Erros.PaymentConditionNotSet, data.Result.Mensagem);
        //}

        [TestMethod]
        public void TesteCriarVendaSemItemVendaModel()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ClienteId = 1,
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
        public void TesteCriarVendaItemVendaModelSemCodigo()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 1",
                ValorTotalLiquido = "R$ 1",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
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
        public void TesteCriarVendaItemVendaModelSemDescricao()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
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
        public void TesteCriarVendaItemVendaModelSemQuantidade()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
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
        public void TesteCriarVendaItemVendaModelSemPrecoVenda()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
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
        public void TesteCriarVendaItemVendaModelSemValorBruto()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ValorDesconto = "R$ 0",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
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
        public void TesteCriarVendaItemVendaModelSemValorLiquido()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
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
        public void TesteCriarVendaComStatusDiferenteDeAberto()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Confirmado",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorTotalLiquido = "R$ 2,05",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
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
            Assert.AreEqual(Erros.StatusOfDifferentSalesOpen, data.Result.Mensagem);
        }

        [TestMethod]
        public void TesteCriarVendaComSomaValorBrutoDiferenteValorTotalBruto()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 2,05",
                ValorDesconto = "R$ 0",
                ValorTotalLiquido = "R$ 60,00",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorLiquido = "R$ 30,00"
                    },
                    new ItemVendaModel
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
        public void TesteCriarVendaComSomaValorLiquidoDiferenteValorTotalLiquido()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 60,00",
                ValorDesconto = "R$ 0",
                ValorTotalLiquido = "R$ 2,05",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorLiquido = "R$ 30,00"
                    },
                    new ItemVendaModel
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
        public void TesteCriarVendaComSomaValorDescontoDiferenteValorTotalDesconto()
        {
            var controller = CreateVendaController();
            var response = controller.CriarVenda(new VendaModel()
            {
                Status = "Aberto",
                DataCadastro = string.Format("{0:dd/MM/yyyy H:mm}", DateTime.Now),
                ValorTotalBruto = "R$ 60,00",
                ValorTotalDesconto = "R$ 2,05",
                ValorTotalLiquido = "R$ 60,00",
                ClienteId = 1,
                FormaPagamentoId = 1,
                CondicaoPagamentoId = 1,
                ItemVendaModel = new List<ItemVendaModel>
                {
                    new ItemVendaModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorDesconto = "R$ 1,00",
                        ValorLiquido = "R$ 30,00"
                    },
                    new ItemVendaModel
                    {
                        Codigo = 1,
                        Descricao = "sdasd",
                        Quantidade = 1,
                        PrecoVenda = "R$ 22,00",
                        ValorBruto = "R$ 30,00",
                        ValorDesconto = "R$ 1,00",
                        ValorLiquido = "R$ 30,00"
                    }
                }
            });
            var data = response.Result.Content.ReadAsAsync<RetornoBase<Exception>>();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Result.StatusCode);
            Assert.AreNotEqual(null, data.Result.ObjetoRetorno);
            Assert.AreEqual(true, data.Result.TemErros);
            Assert.AreEqual(Erros.SumDoNotMatchTotalValueDiscount, data.Result.Mensagem);
        }
    }
}
