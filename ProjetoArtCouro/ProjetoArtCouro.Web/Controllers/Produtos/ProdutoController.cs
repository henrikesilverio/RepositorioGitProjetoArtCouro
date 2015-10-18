using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;

namespace ProjetoArtCouro.Web.Controllers.Produtos
{
    public class ProdutoController : Controller
    {
        [CustomAuthorize(Roles = "PesquisaProduto")]
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
        [CustomAuthorize(Roles = "NovoProduto")]
        public JsonResult NovoProduto()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarProduto")]
        public JsonResult EditarProduto()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirProduto")]
        public JsonResult ExcluirProduto()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}