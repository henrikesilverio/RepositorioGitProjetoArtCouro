using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao;
using ProjetoArtCouro.Resources.Resources;
using ProjetoArtCouro.Startup.DependencyResolver;

namespace ProjetoArtCouro.Api.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IAutenticacao _autenticacaoService;

        public AuthorizationServerProvider(IAutenticacao autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                var container = new UnityContainer();
                DependencyResolver.Resolve(container);
                _autenticacaoService = container.Resolve<IAutenticacao>();
                var user = _autenticacaoService.AutenticarUsuario(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", Erros.InvalidCredentials);
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.Senha));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.UsuarioCodigo.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.PrimarySid, user.UsuarioId.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.UsuarioNome));

                //Setando as permissao do usuario
                foreach (var permissao in user.Permissoes)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, permissao.AcaoNome));
                }

                //Inclui as o nome e as permissões do usuario no retorno da autenticação
                var properties = CreateProperties(user.UsuarioNome, JsonConvert.SerializeObject(user.Permissoes.Select(x => x.AcaoNome).ToArray()));
                var ticket = new AuthenticationTicket(identity, properties);
                
                var principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;
                
                context.Validated(ticket);
            }
            catch (Exception)
            {
                context.SetError("invalid_grant", Erros.InvalidCredentials);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        private static AuthenticationProperties CreateProperties(string userName, string roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"user_name", userName},
                {"roles", roles}
            };
            return new AuthenticationProperties(data);
        }
    }
}