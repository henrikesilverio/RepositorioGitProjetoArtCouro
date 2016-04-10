using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Estoque;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Estoques
{
    public class EstoqueController : BaseController
    {
        public ActionResult PesquisaEstoque()
        {
            ViewBag.Title = Mensagens.Stock;
            ViewBag.SubTitle = Mensagens.StockSearch;
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaEstoque(PesquisaEstoqueModel model)
        {
            var response = ServiceRequest.Post<List<EstoqueModel>>(model, "api/Estoque/PesquisarEstoque");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoAccountReceivableForTheGivenFilter;
            }
            return ReturnResponse(response);
        }
    }
}