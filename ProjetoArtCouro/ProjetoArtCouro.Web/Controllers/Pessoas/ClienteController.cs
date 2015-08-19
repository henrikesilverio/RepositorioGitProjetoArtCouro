using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Cliente;
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
            return View();
        }

        [HttpPost]
        public ActionResult NovoCliente(ClienteModel model)
        {
            ViewBag.Title = Mensagens.NewClient;
            ViewBag.SubTitle = Mensagens.SearchCliente;
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = ServiceRequest.Post(model, "api/Cliente/CriarCliente");
            if (!response.Data.TemErros)
            {
                return RedirectToAction("Index", "Cliente");
            }

            ModelState.AddModelError("Erro", response.Data.Mensagem);
            
            return View();
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