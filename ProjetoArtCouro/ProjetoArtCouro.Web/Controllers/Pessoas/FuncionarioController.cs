using System.Collections.Generic;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Funcionario;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Pessoas
{
    public class FuncionarioController : Controller
    {
        // GET: Funcionario
        public ActionResult PesquisaFuncionario()
        {
            ViewBag.Title = Mensagens.Employee;
            ViewBag.SubTitle = Mensagens.SearchEmployee;
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaFuncionario(PesquisaFuncionarioModel model)
        {
            //var response = ServiceRequest.Post<List<FornecedorModel>>(model, "api/Cliente/PesquisarFornecedore");
            //return Json(response.Data, JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovoFuncionario()
        {
            ViewBag.Title = Mensagens.Employee;
            ViewBag.SubTitle = Mensagens.NewEmployee;
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
        public JsonResult NovoFuncionario(FuncionarioModel model)
        {
            //model.PapelPessoa = (int)TipoPapelPessoaEnum.Cliente;
            //var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Cliente/CriarFornecedor");
            //return Json(response.Data, JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarFuncionario()
        {
            ViewBag.Title = Mensagens.Employee;
            ViewBag.SubTitle = Mensagens.EditEmployee;
            var model = new FuncionarioModel();
            return View("NovoFuncionario", model);
        }

        [HttpPost]
        public ActionResult EditarFuncionario(FuncionarioModel model)
        {
            ViewBag.Title = Mensagens.Employee;
            ViewBag.SubTitle = Mensagens.EditEmployee;
            if (!ModelState.IsValid)
            {
                return View("NovoFuncionario", model);
            }

            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Cliente/PesquisarFuncionario");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Funcionario");
            }
            ModelState.AddModelError("Erro", response.Data.Mensagem);

            return View("NovoFuncionario", model);
        }
    }
}