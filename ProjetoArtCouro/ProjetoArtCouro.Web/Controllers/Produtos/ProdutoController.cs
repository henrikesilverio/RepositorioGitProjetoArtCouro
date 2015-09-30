using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Web.Controllers.Produtos
{
    public class ProdutoController : Controller
    {
        // GET: Produto
        public ActionResult PesquisaProduto()
        {
            ViewBag.Title = Mensagens.Product;
            ViewBag.Unidades = new List<LookupModel>
            {
                new LookupModel
                {
                    Codigo = 1,
                    Nome = "KG"
                },
                new LookupModel
                {
                    Codigo = 2,
                    Nome = "Unidade"
                }
            };
            return View();
        }

        [HttpPost]
        public JsonResult NovoProduto()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditarProduto()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirProduto()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}