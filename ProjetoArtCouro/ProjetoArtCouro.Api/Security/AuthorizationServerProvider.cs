using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using ProjetoArtCouro.Domain.Contracts.IService.IAutenticacao;
using ProjetoArtCouro.Resource.Resources;

namespace ProjetoArtCouro.Api.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAutenticacao _autenticacaoService;

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
                var user = _autenticacaoService.AutenticarUsuario(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", Erros.InvalidCredentials);
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.Senha));
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