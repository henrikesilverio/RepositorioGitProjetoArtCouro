using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Usuario;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Usuarios
{
    public class UsuarioController : Controller
    {
        public ActionResult Index()
        {
            CriarViewBags(Mensagens.SearchUser);
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaUsuario(PesquisaUsuarioModel model)
        {
            var response = ServiceRequest.Post<List<UsuarioModel>>(model, "api/Usuario/PesquisarUsuario");
            if (!response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoUsersForTheGivenFilter;
            }
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovoUsuario()
        {
            CriarViewBags(Mensagens.NewUser);
            return View();
        }

        [HttpPost]
        public JsonResult NovoUsuario(UsuarioModel model)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarUsuario(int codigoUsuario)
        {
            CriarViewBags(Mensagens.EditUser);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditarUsuario(UsuarioModel model)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirUsuario(int codigoUsuario)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void CriarViewBags(string subTitulo)
        {
            var response = ServiceRequest.Get<List<PermissaoModel>>(null, "api/Usuario/ObterListaPermissao");
            ViewBag.Permissoes = response.Data.ObjetoRetorno;
            ViewBag.Title = Mensagens.User;
            ViewBag.SubTitle = subTitulo;
        }
    }
}