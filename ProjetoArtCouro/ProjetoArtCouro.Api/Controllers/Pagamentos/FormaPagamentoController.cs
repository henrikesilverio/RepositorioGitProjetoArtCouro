using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json.Linq;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IPagamento;
using ProjetoArtCouro.Domain.Models.Pagamentos;
using ProjetoArtCouro.Model.Models.FormaPagamento;

namespace ProjetoArtCouro.Api.Controllers.Pagamentos
{
    [RoutePrefix("api/FormaPagamento")]
    public class FormaPagamentoController : BaseApiController
    {
        private readonly IFormaPagamentoService _formaPagamentoService;
        public FormaPagamentoController(IFormaPagamentoService formaPagamentoService)
        {
            _formaPagamentoService = formaPagamentoService;
        }

        [Route("ObterListaFormaPagamento")]
        [Authorize(Roles = "PesquisaFormaPagamento")]
        [HttpGet]
        public Task<HttpResponseMessage> ObterListaFormaPagamento()
        {
            HttpResponseMessage response;
            try
            {
                var listaformaPagamento = _formaPagamentoService.ObterListaFormaPagamento();
                response = ReturnSuccess(Mapper.Map<List<FormaPagamentoModel>>(listaformaPagamento));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("CriarFormaPagamento")]
        [Authorize(Roles = "NovaFormaPagamento")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarFormaPagamento(FormaPagamentoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var formaPagamento = Mapper.Map<FormaPagamento>(model);
                var formaPagamentoRetorno = _formaPagamentoService.CriarFormaPagamento(formaPagamento);
                response = ReturnSuccess(Mapper.Map<FormaPagamentoModel>(formaPagamentoRetorno));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarFormaPagamento")]
        [Authorize(Roles = "EditarFormaPagamento")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarFormaPagamento(FormaPagamentoModel model)
        {
            HttpResponseMessage response;
            try
            {
                var formaPagamento = Mapper.Map<FormaPagamento>(model);
                var formaPagamentoRetorno = _formaPagamentoService.AtualizarFormaPagamento(formaPagamento);
                response = ReturnSuccess(Mapper.Map<FormaPagamentoModel>(formaPagamentoRetorno));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ExcluirFormaPagamento")]
        [Authorize(Roles = "ExcluirFormaPagamento")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirFormaPagamento([FromBody]JObject jObject)
        {
            var codigoformaPagamento = jObject["codigoFormaPagamento"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _formaPagamentoService.ExcluirFormaPagamento(codigoformaPagamento);
                response = ReturnSuccess();
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        protected override void Dispose(bool disposing)
        {
            _formaPagamentoService.Dispose();
        }
    }
}
