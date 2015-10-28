using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Fornecedor;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Pessoas
{
    public class FornecedorController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaFornecedor")]
        public ActionResult PesquisaFornecedor()
        {
            ViewBag.Title = Mensagens.Provider;
            ViewBag.SubTitle = Mensagens.NewProvider;
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "PesquisaFornecedor")]
        public JsonResult PesquisaFornecedor(PesquisaFornecedorModel model)
        {
            var response = ServiceRequest.Post<List<FornecedorModel>>(model, "api/Fornecedor/PesquisarFornecedor");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoClientForTheGivenFilter;
            }
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovoFornecedor")]
        public ActionResult NovoFornecedor()
        {
            CriarViewBags(Mensagens.NewProvider);
            var listaBase = new List<LookupModel>();
            ViewBag.Enderecos = listaBase;
            ViewBag.Telefones = listaBase;
            ViewBag.Celulares = listaBase;
            ViewBag.Emails = listaBase; 
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovoFornecedor")]
        public JsonResult NovoFornecedor(FornecedorModel model)
        {
            model.PapelPessoa = (int)TipoPapelPessoaEnum.Fornecedor;
            var response = ServiceRequest.Post<RetornoBase<object>>(model, "api/Fornecedor/CriarFornecedor");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "EditarFornecedor")]
        public ActionResult EditarFornecedor(int codigoFornecedor)
        {
            CriarViewBags(Mensagens.EditProvider);
            var response = ServiceRequest.Post<FornecedorModel>(new { codigoFornecedor = codigoFornecedor }, "api/Fornecedor/PesquisarFornecedorPorCodigo");
            var model = response.Data.ObjetoRetorno;
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
        [CustomAuthorize(Roles = "EditarFornecedor")]
        public ActionResult EditarFornecedor(FornecedorModel model)
        {
            var response = ServiceRequest.Put<RetornoBase<object>>(model, "api/Fornecedor/EditarFornecedor");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirFornecedor")]
        public JsonResult ExcluirFornecedor(int codigoFornecedor)
        {
            var response = ServiceRequest.Delete<RetornoBase<object>>(new { codigoFornecedor = codigoFornecedor }, "api/Fornecedor/ExcluirFornecedor");
            return ReturnResponse(response);
        }

        private void CriarViewBags(string subTitle)
        {
            ViewBag.Title = Mensagens.Provider;
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