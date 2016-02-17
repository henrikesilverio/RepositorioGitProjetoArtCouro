using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjetoArtCouro.Model.Models.Venda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoArtCouro.Selenium.Paginas
{
    public class Venda : ICadastro
    {
        private IWebDriver driver;
        private VendaModel venda;
        public Venda(IWebDriver driver)
        {
            this.driver = driver;
            venda = new VendaModel();
        }

        public void Novo()
        {
            driver.Navigate().GoToUrl("http://portalartcouro.apphb.com/Venda/NovaVenda");
        }

        public void Cadastrar()
        {
            driver.FindElement(By.XPath(".//span[@id='select2-chosen-1']")).Click();
            driver.FindElement(By.Id("select2-results-1")).Click();
            driver.FindElement(By.Id("ValorDesconto")).SendKeys(venda.ValorDesconto);
            driver.FindElement(By.Id("Quantidade")).SendKeys(venda.Quantidade);
            driver.FindElement(By.Id("AdicionarProduto")).Click();
            driver.FindElement(By.Id("GerarOrcamento")).Click();
        }

        public bool ExisteNaListagem()
        {
            throw new NotImplementedException();
        }

        public void PreencherVenda()
        {
            venda.ProdutoId = 1;
            venda.Quantidade = "1";
            venda.ValorDesconto = "10,00";
        }

    }
}
