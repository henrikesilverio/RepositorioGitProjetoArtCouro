using System.Web.Mvc;
using System.Web.Security;
using ProjetoArtCouro.Web.Infra.Service;
using ProjetoArtCouro.Model.Models.Login;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Extensions;

namespace ProjetoArtCouro.Web.Controllers.Login
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var tokenModel = ServiceRequest.GetAuthenticationToken(model.UsuarioNome, model.Senha);
            if (tokenModel != null)
            {
                Response.SetAuthCookie(model.UsuarioNome, tokenModel.access_token, tokenModel.roles, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Erro", Erros.InvalidUserOrPassword);
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
    }
}