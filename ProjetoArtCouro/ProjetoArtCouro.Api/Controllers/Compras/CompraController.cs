using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json.Linq;
using ProjetoArtCouro.Api.Helpers;
using ProjetoArtCouro.Domain.Contracts.IService.ICompra;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Usuarios;
using ProjetoArtCouro.Model.Models.Compra;

namespace ProjetoArtCouro.Api.Controllers.Compras
{
    [RoutePrefix("api/Compra")]
    public class CompraController : BaseApiController
    {
        private readonly ICompraService _compraService;

        public CompraController(ICompraService compraService)
        {
            _compraService = compraService;
        }

        [Route("CriarCompra")]
        [Authorize(Roles = "NovaCompra")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarCompra(CompraModel model)
        {
            HttpResponseMessage response;
            try
            {
                var compra = Mapper.Map<Compra>(model);
                var usuario = new Usuario
                {
                    UsuarioCodigo = ObterCodigoUsuarioLogado()
                };
                compra.Usuario = usuario;
                _compraService.CriarCompra(compra);
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

        [Route("PesquisarCompra")]
        [Authorize(Roles = "PesquisaCompra")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarCompra(PesquisaCompraModel model)
        {
            HttpResponseMessage response;
            try
            {
                var usuarioCodigo = ObterCodigoUsuarioLogado();
                var compras = _compraService.PesquisarCompra(model.CodigoCompra ?? 0, model.CodigoFornecedor ?? 0,
                    model.DataCadastro.ToDateTimeWithoutHour(), model.StatusId ?? 0, model.NomeFornecedor, model.CPFCNPJ,
                    usuarioCodigo);
                response = ReturnSuccess(Mapper.Map<List<CompraModel>>(compras));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("PesquisarCompraPorCodigo")]
        [Authorize(Roles = "EditarCompra")]
        [HttpPost]
        public Task<HttpResponseMessage> PesquisarCompraPorCodigo([FromBody]JObject jObject)
        {
            var codigoCompra = jObject["codigoCompra"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var compra = _compraService.ObterCompraPorCodigo(codigoCompra);
                response = ReturnSuccess(Mapper.Map<CompraModel>(compra));
            }
            catch (Exception ex)
            {
                response = ReturnError(ex);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarCompra")]
        [Authorize(Roles = "EditarCompra")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarCompra(CompraModel model)
        {
            HttpResponseMessage response;

            try
            {
                var compra = Mapper.Map<Compra>(model);
                var usuario = new Usuario
                {
                    UsuarioCodigo = ObterCodigoUsuarioLogado()
                };
                compra.Usuario = usuario;
                _compraService.AtualizarCompra(compra);
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

        [Route("ExcluirCompra")]
        [Authorize(Roles = "ExcluirCompra")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirCompra([FromBody]JObject jObject)
        {
            var codigoCompra = jObject["codigoCompra"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                _compraService.ExcluirCompra(codigoCompra);
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
            _compraService.Dispose();
        }
    }
}
