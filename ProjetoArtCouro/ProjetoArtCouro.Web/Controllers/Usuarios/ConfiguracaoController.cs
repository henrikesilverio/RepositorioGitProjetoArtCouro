﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Model.Models.Usuario;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Usuarios
{
    public class ConfiguracaoController : Controller
    {
        // GET: Configuracao
        public ActionResult PesquisaGrupo()
        {
            CriarViewBags(Mensagens.GroupSearch);
            return View();
        }

        [HttpPost]
        public JsonResult PesquisaGrupo(PesquisaGrupoModel model)
        {
            var response = ServiceRequest.Post<List<GrupoModel>>(model, "api/Usuario/PesquisarGrupo");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoUsersForTheGivenFilter;
            }
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NovoGrupo()
        {
            CriarViewBags(Mensagens.NewGroup);
            CriarViewBagPermissoes();
            return View();
        }

        [HttpPost]
        public JsonResult NovoGrupo(GrupoModel model)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarGrupo(int codigoGrupo)
        {
            CriarViewBags(Mensagens.EditGroup);
            CriarViewBagPermissoes();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditarGrupo(GrupoModel model)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirGrupo(int codigoGrupo)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void CriarViewBags(string subTitulo)
        {
            ViewBag.Title = Mensagens.GroupSettings;
            ViewBag.SubTitle = subTitulo;
        }

        private void CriarViewBagPermissoes()
        {
            var response = ServiceRequest.Get<List<PermissaoModel>>(null, "api/Usuario/ObterListaPermissao");
            ViewBag.Permissoes = response.Data.ObjetoRetorno;
        }
    }
}