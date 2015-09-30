using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Venda;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Vendas
{
    public class VendaController : Controller
    {
        // GET: Venda
        public ActionResult PesquisaVenda()
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.SearchSale;
            ViewBag.StatusVenda = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Aberto"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Confirmado"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Cancelado"
                }
            };
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaVenda(PesquisaVendaModel model)
        {
            //var response = ServiceRequest.Post<List<VendaModel>>(model, "api/Venda/PesquisaVenda");
            //return Json(response.Data, JsonRequestBehavior.AllowGet);
            var resultado = new List<VendaModel>
            {
                new VendaModel()
                {
                   CodigoVenda = 1,
                   ClienteId = 2,
                   DataCadastro = DateTime.Now,
                   Status = "Aberto",
                   NomeCliente = "Vitor",
                   CPFCNPJ = "123.456.789-09"
                },
                new VendaModel()
                {
                   CodigoVenda = 3,
                   ClienteId = 5,
                   DataCadastro = DateTime.Now,
                   Status = "Aberto",
                   NomeCliente = "Felipe",
                   CPFCNPJ = "222.333.666-38" 
                }
            };
            return Json(new {ObjetoRetorno = resultado}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovaVenda()
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.NewSale;
            ViewBag.Clientes = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Vitor"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Henrique"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Andressa"
                },
                 new LookupModel
                {
                    Codigo = 4,
                    Nome = "Felipe"
                }
            };

            ViewBag.FormasPagamento = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Cartão"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Dinheiro"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Cheque"
                }
            };

            ViewBag.CondicoesPagamento = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "A vista"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "1 + 1"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "1 + 2"
                },
                 new LookupModel
                {
                    Codigo = 4,
                    Nome = "1 + 3"
                }
            };

            ViewBag.Produtos = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Cinto de couro"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Bolsa de couro"
                }
            };

            var model = new VendaModel
            {
                Status = "Aberto",
                DataCadastro = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult NovaVenda(VendaModel model)
        {
            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Venda/CriarVenda");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarVenda(int codigoVenda)
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.EditSale;
            ViewBag.Clientes = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Vitor"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Henrique"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Andressa"
                },
                 new LookupModel
                {
                    Codigo = 4,
                    Nome = "Felipe"
                }
            };

            ViewBag.FormasPagamento = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Cartão"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Dinheiro"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "Cheque"
                }
            };

            ViewBag.CondicoesPagamento = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "A vista"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "1 + 1"
                },
                new LookupModel
                {
                    Codigo = 3,
                    Nome = "1 + 2"
                },
                 new LookupModel
                {
                    Codigo = 4,
                    Nome = "1 + 3"
                }
            };

            ViewBag.Produtos = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "Cinto de couro"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Bolsa de couro"
                }
            };

            var model = new VendaModel
            {
                Status = "Aberto",
                DataCadastro = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditarVenda(VendaModel model)
        {
            ViewBag.Title = Mensagens.NewSale;
            ViewBag.SubTitle = Mensagens.EditSale;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Venda/PesquisarVenda");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Venda");
            }
            ModelState.AddModelError("Erro", response.Data.Mensagem);

            return View(model);
        }

        [HttpPost]
        public JsonResult ExcluirVenda(int codigoVenda)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}