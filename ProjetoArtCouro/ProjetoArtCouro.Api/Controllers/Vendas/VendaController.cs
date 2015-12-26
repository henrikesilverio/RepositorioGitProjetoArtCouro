using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json.Linq;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.IVenda;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Model.Models.Venda;

namespace ProjetoArtCouro.Api.Controllers.Vendas
{
    [RoutePrefix("api/Venda")]
    public class VendaController : BaseApiController
    {
        private readonly IVendaService _vendaService;

        public VendaController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [Route("CriarVenda")]
        [Authorize(Roles = "NovaVenda")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarVenda(VendaModel model)
        {
            HttpResponseMessage response;
            try
            {
                var venda = Mapper.Map<Venda>(model);
                var usuario = new Usuario
                {
                    UsuarioCodigo = ObterCodigoUsuarioLogado()
                };
                venda.Usuario = usuario;
                _vendaService.CriarVenda(venda);
                response = ReturnSuccess();
            }
            catch (DbEntityValidationException e)
            {
                response = ReturnError(e);
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarVenda")]
        [Authorize(Roles = "PesquisaVenda")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarVenda(PesquisaVendaModel model)
        {
            HttpResponseMessage response;
            try
            {
                var usuarioCodigo = ObterCodigoUsuarioLogado();
                var vendas = _vendaService.PesquisarVenda(model.CodigoVenda ?? 0, model.CodigoCliente ?? 0,
                    model.DataCadastro.ToDateTimeWithoutHour(), model.StatusId ?? 0, model.NomeCliente, model.CPFCNPJ,
                    usuarioCodigo);
                response = ReturnSuccess(Mapper.Map<List<VendaModel>>(vendas));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarVendaPorCodigo")]
        [Authorize(Roles = "EditarVenda")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarVendaPorCodigo([FromBody]JObject jObject)
        {
            var codigoVenda = jObject["codigoVenda"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var venda = _vendaService.ObterVendaPorCodigo(codigoVenda);
                response = ReturnSuccess(Mapper.Map<VendaModel>(venda));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarVenda")]
        [Authorize(Roles = "EditarVenda")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarVenda(VendaModel model)
        {
            HttpResponseMessage response;

            try
            {
                var venda = Mapper.Map<Venda>(model);
                var usuario = new Usuario
                {
                    UsuarioCodigo = ObterCodigoUsuarioLogado()
                };
                venda.Usuario = usuario;
                _vendaService.AtualizarVenda(venda);
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

        [Route("ExcluirVenda")]
        [Authorize(Roles = "ExcluirVenda")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirVenda([FromBody]JObject jObject)
        {
            var codigoVenda = jObject["codigoVenda"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _vendaService.ExcluirVenda(codigoVenda);
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

        private static int ObterCodigoUsuarioLogado()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var usuarioCodigo = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value).SingleOrDefault();
            return usuarioCodigo.ToInt();
        }

        protected override void Dispose(bool disposing)
        {
            _vendaService.Dispose();
        }
    }
}
