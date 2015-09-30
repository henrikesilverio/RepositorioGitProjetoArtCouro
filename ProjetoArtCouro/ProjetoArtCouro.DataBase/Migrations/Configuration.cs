using ProjetoArtCouro.Domain.Models.Pessoas;
using System.Data.Entity.Migrations;
using ProjetoArtCouro.Domain.Models.Usuarios;

namespace ProjetoArtCouro.DataBase.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataBase.DataBaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataBase.DataBaseContext context)
        {
            //Atualização inicial estados
            context.Estados.AddOrUpdate(
                e => e.EstadoNome,
                new Estado {EstadoNome = "Acre"},
                new Estado {EstadoNome = "Alagoas"},
                new Estado {EstadoNome = "Amapá"},
                new Estado {EstadoNome = "Amazonas"},
                new Estado {EstadoNome = "Bahia"},
                new Estado {EstadoNome = "Ceará"},
                new Estado {EstadoNome = "Distrito Federal"},
                new Estado {EstadoNome = "Espírito Santo"},
                new Estado {EstadoNome = "Goiás"},
                new Estado {EstadoNome = "Maranhão"},
                new Estado {EstadoNome = "Mato Grosso"},
                new Estado {EstadoNome = "Mato Grosso do Sul"},
                new Estado {EstadoNome = "Minas Gerais"},
                new Estado {EstadoNome = "Pará"},
                new Estado {EstadoNome = "Paraíba"},
                new Estado {EstadoNome = "Paraná"},
                new Estado {EstadoNome = "Pernambuco"},
                new Estado {EstadoNome = "Piauí"},
                new Estado {EstadoNome = "Rio de Janeiro"},
                new Estado {EstadoNome = "Rio Grande do Norte"},
                new Estado {EstadoNome = "Rio Grande do Sul"},
                new Estado {EstadoNome = "Rondônia"},
                new Estado {EstadoNome = "Roraima"},
                new Estado {EstadoNome = "Santa Catarina"},
                new Estado {EstadoNome = "São Paulo"},
                new Estado {EstadoNome = "Sergipe"},
                new Estado {EstadoNome = "Tocantins"}
                );

            //Atualização inicial estado civil
            context.EstadosCivis.AddOrUpdate(
                ec => ec.EstadoCivilNome,
                new EstadoCivil { EstadoCivilNome = "Solteiro(a)" },
                new EstadoCivil { EstadoCivilNome = "Casado(a)" },
                new EstadoCivil { EstadoCivilNome = "Divorciado(a)" },
                new EstadoCivil { EstadoCivilNome = "Viúvo(a)" },
                new EstadoCivil { EstadoCivilNome = "Separado(a)" }
                );

            //Atualização inicial papel
            context.Papeis.AddOrUpdate(
                p => p.PapelNome,
                new Papel { PapelNome = "Pessoa Fisica" },
                new Papel { PapelNome = "Pessoa Juridica" },
                new Papel { PapelNome = "Empregado" },
                new Papel { PapelNome = "Cliente" },
                new Papel { PapelNome = "Fornecedor" }
                );

            //Atualização inicial Permissão
            context.Permissoes.AddOrUpdate(
                p => p.PermissaoNome,
                new Permissao
                {
                    PermissaoNome = "Pesquisa Compra",
                    AcaoNome = "PesquisaCompra"
                },
                new Permissao
                {
                    PermissaoNome = "Nova Compra",
                    AcaoNome = "NovaCompra"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Compra",
                    AcaoNome = "EditarCompra"
                },
                new Permissao
                {
                    PermissaoNome = "Excluir Compra",
                    AcaoNome = "ExcluirCompra"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Condicao Pagamento",
                    AcaoNome = "PesquisaCondicaoPagamento"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Conta Receber",
                    AcaoNome = "PesquisaContaReceber"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Forma Pagamento",
                    AcaoNome = "PesquisaFormaPagamento"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Cliente",
                    AcaoNome = "PesquisaCliente"
                },
                new Permissao
                {
                    PermissaoNome = "Novo Cliente",
                    AcaoNome = "NovoCliente"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Cliente",
                    AcaoNome = "EditarCliente"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Fornecedor",
                    AcaoNome = "PesquisaFornecedor"
                },
                new Permissao
                {
                    PermissaoNome = "Novo Fornecedor",
                    AcaoNome = "NovoFornecedor"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Fornecedor",
                    AcaoNome = "EditarFornecedor"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Funcionario",
                    AcaoNome = "PesquisaFuncionario"
                },
                new Permissao
                {
                    PermissaoNome = "Novo Funcionario",
                    AcaoNome = "NovoFuncionario"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Funcionario",
                    AcaoNome = "EditarFuncionario"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Produto",
                    AcaoNome = "PesquisaProduto"
                },
                new Permissao
                {
                    PermissaoNome = "Novo Produto",
                    AcaoNome = "NovoProduto"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Produto",
                    AcaoNome = "EditarProduto"
                },
                new Permissao
                {
                    PermissaoNome = "Excluir Produto",
                    AcaoNome = "ExcluirProduto"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Grupo",
                    AcaoNome = "PesquisaGrupo"
                },
                new Permissao
                {
                    PermissaoNome = "Novo Grupo",
                    AcaoNome = "NovoGrupo"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Grupo",
                    AcaoNome = "EditarGrupo"
                },
                new Permissao
                {
                    PermissaoNome = "Excluir Grupo",
                    AcaoNome = "ExcluirGrupo"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Usuario",
                    AcaoNome = "PesquisaUsuario"
                },
                new Permissao
                {
                    PermissaoNome = "Novo Usuario",
                    AcaoNome = "NovoUsuario"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Usuario",
                    AcaoNome = "EditarUsuario"
                },
                new Permissao
                {
                    PermissaoNome = "Excluir Usuario",
                    AcaoNome = "ExcluirUsuario"
                },
                new Permissao
                {
                    PermissaoNome = "Pesquisa Venda",
                    AcaoNome = "PesquisaVenda"
                },
                new Permissao
                {
                    PermissaoNome = "Nova Venda",
                    AcaoNome = "NovaVenda"
                },
                new Permissao
                {
                    PermissaoNome = "Editar Venda",
                    AcaoNome = "EditarVenda"
                },
                new Permissao
                {
                    PermissaoNome = "Excluir Venda",
                    AcaoNome = "ExcluirVenda"
                }
                );
        }
    }
}