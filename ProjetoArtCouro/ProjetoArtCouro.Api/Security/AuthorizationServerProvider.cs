using System;
using System.ComponentModel;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao;
using ProjetoArtCouro.Resource.Resources;
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
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new [] { "*" });

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
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.UsuarioId.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.UsuarioNome));

                //Setando as permissao do usuario
                foreach (var permissao in user.Permissoes)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, permissao.PermissaoNome));
                }

                var principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch (Exception)
            {
                context.SetError("invalid_grant", Erros.InvalidCredentials);
            }
        }
    }
}