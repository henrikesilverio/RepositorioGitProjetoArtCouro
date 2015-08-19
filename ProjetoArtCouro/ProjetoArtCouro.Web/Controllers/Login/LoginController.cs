using System.Web.Mvc;
using System.Web.Security;
using ProjetoArtCouro.Web.Infra.Service;
using ProjetoArtCouro.Model.Models.Login;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Web.Controllers.Login
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (ServiceRequest.SetAuthenticationToken(model.UsuarioNome, model.Senha))
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Erro", Erros.InvalidUserOrPassword);
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}