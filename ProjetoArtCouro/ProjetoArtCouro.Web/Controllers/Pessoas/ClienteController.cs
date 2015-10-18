using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Model.Models.Cliente;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Pessoas
{
    public class ClienteController : Controller
    {
        [CustomAuthorize(Roles = "PesquisaCliente")]
        public ActionResult PesquisaCliente()
        {
            ViewBag.Title = Mensagens.Client;
            ViewBag.SubTitle = Mensagens.SearchClient;
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "PesquisaCliente")]
        public JsonResult PesquisaCliente(PesquisaClienteModel model)
        {
            var response = ServiceRequest.Post<List<ClienteModel>>(model, "api/Cliente/PesquisarCliente");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoClientForTheGivenFilter;
            }
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "NovoCliente")]
        public ActionResult NovoCliente()
        {
            CriarViewBags(Mensagens.NewClient);
            var listaBase = new List<LookupModel>();
            ViewBag.Enderecos = listaBase;
            ViewBag.Telefones = listaBase;
            ViewBag.Celulares = listaBase;
            ViewBag.Emails = listaBase;
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovoCliente")]
        public JsonResult NovoCliente(ClienteModel model)
        {
            model.PapelPessoa = (int)TipoPapelPessoaEnum.Cliente;
            var response = ServiceRequest.Post<RetornoBase<object>>(model, "api/Cliente/CriarCliente");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = "EditarCliente")]
        public ActionResult EditarCliente(int codigoCliente)
        {
            CriarViewBags(Mensagens.EditClient);
            var response = ServiceRequest.Post<ClienteModel>(new { codigoCliente = codigoCliente }, "api/Cliente/PesquisarClientePorCodigo");
            var model = response.Data.ObjetoRetorno;
            //TODO Criar tratamentoo de exceção
            if (model == null)
            {
                return View(response.Data.ObjetoRetorno);
            }

            var listaBase = new List<LookupModel>();
            ViewBag.Enderecos = model.Enderecos.Select(x => new LookupModel
            {
                Codigo = x.EnderecoId ?? 0,
                Nome = string.Format("Logradouro: {0}, N: {1}, Cidade: {2}, CEP: {3}", x.Logradouro, x.Numero, x.Cidade, x.Cep)
            }).ToList();
            ViewBag.Telefones = model.MeioComunicacao.Telefones ?? listaBase;
            ViewBag.Celulares = model.MeioComunicacao.Celulares ?? listaBase;
            ViewBag.Emails = model.MeioComunicacao.Emalis ?? listaBase;

            return View(response.Data.ObjetoRetorno);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "EditarCliente")]
        public JsonResult EditarCliente(ClienteModel model)
        {
            var response = ServiceRequest.Put<RetornoBase<object>>(model, "api/Cliente/EditarCliente");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirCliente")]
        public JsonResult ExcluirCliente(int codigoCliente)
        {
            var response = ServiceRequest.Delete<RetornoBase<object>>(new { codigoCliente = codigoCliente }, "api/Cliente/ExcluirCliente");
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        private void CriarViewBags(string subTitle)
        {
            ViewBag.Title = Mensagens.Client;
            ViewBag.SubTitle = subTitle;

            var listaBase = new List<LookupModel>();
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
        }
    }
}