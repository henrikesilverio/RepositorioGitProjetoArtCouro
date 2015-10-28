using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Model.Models.Common;
using ProjetoArtCouro.Model.Models.Funcionario;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Web.Infra.Authorization;
using ProjetoArtCouro.Web.Infra.Service;

namespace ProjetoArtCouro.Web.Controllers.Pessoas
{
    public class FuncionarioController : BaseController
    {
        [CustomAuthorize(Roles = "PesquisaFuncionario")]
        public ActionResult PesquisaFuncionario()
        {
            ViewBag.Title = Mensagens.Employee;
            ViewBag.SubTitle = Mensagens.SearchEmployee;
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "PesquisaFuncionario")]
        public JsonResult PesquisaFuncionario(PesquisaFuncionarioModel model)
        {
            var response = ServiceRequest.Post<List<FuncionarioModel>>(model, "api/Funcionario/PesquisarFuncionario");
            if (response.Data.ObjetoRetorno != null && !response.Data.ObjetoRetorno.Any())
            {
                response.Data.Mensagem = Erros.NoClientForTheGivenFilter;
            }
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "NovoFuncionario")]
        public ActionResult NovoFuncionario()
        {
            CriarViewBags(Mensagens.NewEmployee);
            var listaBase = new List<LookupModel>();
            ViewBag.Enderecos = listaBase;
            ViewBag.Telefones = listaBase;
            ViewBag.Celulares = listaBase;
            ViewBag.Emails = listaBase;
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "NovoFuncionario")]
        public JsonResult NovoFuncionario(FuncionarioModel model)
        {
            model.PapelPessoa = (int)TipoPapelPessoaEnum.Funcionario;
            var response = ServiceRequest.Post<RetornoBase<object>>(model, "api/Funcionario/CriarFuncionario");
            return ReturnResponse(response);
        }

        [CustomAuthorize(Roles = "EditarFuncionario")]
        public ActionResult EditarFuncionario(int codigoFuncionario)
        {
            CriarViewBags(Mensagens.EditEmployee);
            var response = ServiceRequest.Post<FuncionarioModel>(new { codigoFuncionario = codigoFuncionario }, "api/Funcionario/PesquisarFuncionarioPorCodigo");
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
        [CustomAuthorize(Roles = "EditarFuncionario")]
        public JsonResult EditarFuncionario(FuncionarioModel model)
        {
            var response = ServiceRequest.Put<RetornoBase<object>>(model, "api/Funcionario/EditarFuncionario");
            return ReturnResponse(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "ExcluirFuncionario")]
        public JsonResult ExcluirFuncionario(int codigoFuncionario)
        {
            var response = ServiceRequest.Delete<RetornoBase<object>>(new { codigoFuncionario = codigoFuncionario }, "api/Funcionario/ExcluirFuncionario");
            return ReturnResponse(response);
        }

        private void CriarViewBags(string subTitle)
        {
            ViewBag.Title = Mensagens.Employee;
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