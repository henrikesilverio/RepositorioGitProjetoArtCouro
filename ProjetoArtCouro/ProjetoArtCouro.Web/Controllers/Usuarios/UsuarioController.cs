using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Usuario;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;
namespace ProjetoArtCouro.Web.Controllers.Usuarios
{
    public class UsuarioController : Controller
    {
        public ActionResult PesquisaUsuario()
        {
            CriarViewBags(Mensagens.SearchUser);
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaUsuario(PesquisaUsuarioModel model)
        {
            var response = ServiceRequest.Post<List<UsuarioModel>>(model, "api/Usuario/PesquisarUsuario");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
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
            var response = ServiceRequest.Post<RetornoBase<object>>(model, "api/Usuario/CriarUsuario");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarUsuario(int codigoUsuario)
        {
            CriarViewBags(Mensagens.EditUser);
            var response = ServiceRequest.Post<UsuarioModel>(new { codigoUsuario = codigoUsuario }, "api/Usuario/PesquisarUsuarioPorCodigo");
            return View(response.Data.ObjetoRetorno);
        }

        [HttpPost]
        public JsonResult EditarUsuario(UsuarioModel model)
        {
            var response = ServiceRequest.Put<UsuarioModel>(model, "api/Usuario/EditarUsuario");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirUsuario(int codigoUsuario)
        {
            var response = ServiceRequest.Delete<RetornoBase<object>>(new { codigoUsuario = codigoUsuario }, "api/Usuario/ExcluirUsuario");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        private void CriarViewBags(string subTitulo)
        {
            var response = ServiceRequest.Get<List<GrupoModel>>(null, "api/Usuario/ObterListaGrupo");
            ViewBag.Grupos = response.Data.ObjetoRetorno;
            ViewBag.Title = Mensagens.User;
            ViewBag.SubTitle = subTitulo;
        }
    }
}