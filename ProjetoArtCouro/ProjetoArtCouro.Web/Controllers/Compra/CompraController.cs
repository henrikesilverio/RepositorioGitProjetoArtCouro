using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Compra;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Compra
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Index()
        {
            ViewBag.Title = Mensagens.Buy;
            ViewBag.SubTitle = Mensagens.SearchBuy;
            ViewBag.StatusCompra = new List<LookupModel>
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
        public JsonResult PesquisaCompra(PesquisaCompraModel model)
        {
            //var response = ServiceRequest.Post<List<CompraModel>>(model, "api/Compra/PesquisaCompra");
            //return Json(response.Data, JsonRequestBehavior.AllowGet);
            var resultado = new List<CompraModel>
            {
                new CompraModel()
                {
                   CodigoCompra = 1,
                   FornecedorId = 2,
                   DataCadastro = DateTime.Now,
                   Status = "Aberto",
                   NomeFornecedor = "Vitor",
                   CPFCNPJ = "123.456.789-09"
                },
                new CompraModel()
                {
                   CodigoCompra = 3,
                   FornecedorId = 5,
                   DataCadastro = DateTime.Now,
                   Status = "Aberto",
                   NomeFornecedor = "Felipe",
                   CPFCNPJ = "222.333.666-38" 
                }
            };
            return Json(new { ObjetoRetorno = resultado }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovaCompra()
        {
            ViewBag.Title = Mensagens.Sale;
            ViewBag.SubTitle = Mensagens.NewSale;
            ViewBag.Fornecedores = new List<LookupModel>
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
                    Nome = "Couro de búfalo"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Repeti de aluminio"
                }
            };

            var model = new CompraModel
            {
                Status = "Aberto",
                DataCadastro = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult NovaCompra(CompraModel model)
        {
            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Compra/CriarCompra");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarCompra(int codigoCompra)
        {
            ViewBag.Title = Mensagens.Buy;
            ViewBag.SubTitle = Mensagens.EditBuy;
            ViewBag.Fornecedores = new List<LookupModel>
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
                    Nome = "Couro de búfalo"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Repeti de aluminio"
                }
            };

            var model = new CompraModel
            {
                Status = "Aberto",
                DataCadastro = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditarCompra(CompraModel model)
        {
            ViewBag.Title = Mensagens.Buy;
            ViewBag.SubTitle = Mensagens.EditBuy;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Compra/PesquisarCompra");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Compra");
            }
            ModelState.AddModelError("Erro", response.Data.Mensagem);

            return View(model);
        }

        [HttpPost]
        public JsonResult ExcluirCompra(int codigoCompra)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}