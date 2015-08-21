using System.Collections.Generic;
using System.Web.Mvc;
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
            var response = ServiceRequest.Post(model, "api/Cliente/PesquisarCliente");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovoCliente()
        {
            ViewBag.Title = Mensagens.NewClient;
            ViewBag.SubTitle = Mensagens.SearchCliente;
            ViewBag.EstadosCivis = new List<LookupModel>();
            ViewBag.Estados = new List<LookupModel>();
            var listaBase = new List<LookupModel>
            {
                new LookupModel {Nome = Mensagens.Select},
                new LookupModel {Codigo = -1, Nome = Mensagens.New}
            };

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
            var response = ServiceRequest.Post(model, "api/Cliente/CriarCliente");
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

            var response = ServiceRequest.Post(model, "api/Cliente/PesquisarCliente");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Cliente");
            }

            ModelState.AddModelError("Erro", response.Data.Mensagem);

            return View("NovoCliente", model);
        }
    }
}