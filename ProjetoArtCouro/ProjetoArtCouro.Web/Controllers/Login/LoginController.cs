using System.Web.Mvc;
using ProjetoArtCouro.Web.Infra.Service;
using ProjetoArtCouro.Model.Models.Login;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Extensions;

namespace ProjetoArtCouro.Web.Controllers.Login
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
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
                Request.SignIn(model.UsuarioNome, tokenModel.access_token, tokenModel.roles, false);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            ModelState.AddModelError("Erro", Erros.InvalidUserOrPassword);
            return View();
        }

        public ActionResult SignOut()
        {
            Request.SignOut();
            return RedirectToAction("Login", "Login");
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }
    }
}