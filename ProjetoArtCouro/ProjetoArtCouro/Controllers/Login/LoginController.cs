using System.Web.Mvc;
using System.Web.Security;
using ProjetoArtCouro.Web.Models.Login;

namespace ProjetoArtCouro.Web.Controllers.Login
{
    public class LoginController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    FormsAuthentication.RedirectFromLoginPage(model.UserName, true);
                    return RedirectToAction("Index", "Home");

                //ModelState.AddModelError("Erro", "Nome ou senhas estão incorretos");
            }
            //ModelState.AddModelError("Erro", "Informações invalidas");
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}
