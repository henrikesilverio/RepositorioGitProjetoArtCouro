using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Fornecedor;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Pessoas
{
    public class FornecedorController : Controller
    {
        // GET: Fornecedor
        public ActionResult Index()
        {
            ViewBag.Title = Mensagens.Provider;
            ViewBag.SubTitle = Mensagens.NewProvider;
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaFornecedor(PesquisaFornecedorModel model)
        {
            //var response = ServiceRequest.Post<List<FornecedorModel>>(model, "api/Cliente/PesquisarFornecedore");
            //return Json(response.Data, JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovoFornecedor()
        {
            ViewBag.Title = Mensagens.Provider;
            ViewBag.SubTitle = Mensagens.NewProvider;
            var listaBase = new List<LookupModel>();
            //Obtem do banco os estados e estados civis
            var response = ServiceRequest.Get<List<LookupModel>>(null, "api/Pessoa/ObterListaEstado");
            if (response.Data.TemErros)
            {
                ModelState.AddModelError("Erro", response.Data.Mensagem);
                ViewBag.Estados = listaBase;
            }
            ViewBag.Estados = response.Data.ObjetoRetorno;
            response = ServiceRequest.Get<List<LookupModel>>(null, "api/Pessoa/ObterListaEstadoCivil");
            if (response.Data.TemErros)
            {
                ModelState.AddModelError("Erro", response.Data.Mensagem);
                ViewBag.EstadosCivis = listaBase;
            }
            ViewBag.EstadosCivis = response.Data.ObjetoRetorno;
            //Inicia as lista dos dropdowns
            ViewBag.Enderecos = listaBase;
            ViewBag.Telefones = listaBase;
            ViewBag.Celulares = listaBase;
            ViewBag.Emails = listaBase; 
            return View();
        }

        [HttpPost]
        public JsonResult NovoFornecedor(FornecedorModel model)
        {
            //model.PapelPessoa = (int)TipoPapelPessoaEnum.Cliente;
            //var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Cliente/CriarFornecedor");
            //return Json(response.Data, JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarFornecedor()
        {
            ViewBag.Title = Mensagens.Provider;
            ViewBag.SubTitle = Mensagens.EditProvider;
            var model = new FornecedorModel();
            return View("NovoFornecedor", model);
        }

        [HttpPost]
        public ActionResult EditarFornecedor(FornecedorModel model)
        {
            ViewBag.Title = Mensagens.Provider;
            ViewBag.SubTitle = Mensagens.EditProvider;
            if (!ModelState.IsValid)
            {
                return View("NovoFornecedor", model);
            }

            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Cliente/PesquisarFornecedor");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Fornecedor");
            }
            ModelState.AddModelError("Erro", response.Data.Mensagem);

            return View("NovoFornecedor", model);
        }
    }
}