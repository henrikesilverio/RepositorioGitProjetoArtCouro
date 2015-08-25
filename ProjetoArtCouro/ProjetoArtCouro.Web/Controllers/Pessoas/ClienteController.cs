using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Pessoas
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            ViewBag.Title = Mensagens.Client;
            ViewBag.SubTitle = Mensagens.SearchCliente;
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaCliente(PesquisaClienteModel model)
        {
            var response = ServiceRequest.Post<List<ClienteModel>>(model, "api/Cliente/PesquisarCliente");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovoCliente()
        {
            ViewBag.Title = Mensagens.Client;
            ViewBag.SubTitle = Mensagens.NewClient;
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
        public JsonResult NovoCliente(ClienteModel model)
        {
            model.PapelPessoa = (int)TipoPapelPessoaEnum.Cliente;
            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Cliente/CriarCliente");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarCliente()
        {
            ViewBag.Title = Mensagens.NewClient;
            ViewBag.SubTitle = Mensagens.SearchCliente;
            var model = new ClienteModel();
            return View("NovoCliente", model);
        }

        [HttpPost]
        public ActionResult EditarCliente(ClienteModel model)
        {
            ViewBag.Title = Mensagens.NewClient;
            ViewBag.SubTitle = Mensagens.SearchCliente;
            if (!ModelState.IsValid)
            {
                return View("NovoCliente", model);
            }

            var response = ServiceRequest.Post<RetornoBase<string>>(model, "api/Cliente/PesquisarCliente");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Cliente");
            }
            ModelState.AddModelError("Erro", response.Data.Mensagem);

            return View("NovoCliente", model);
        }
    }
}