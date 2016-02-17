using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjetoArtCouro.Model.Models.Compra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoArtCouro.Selenium.Paginas
{
    public class Compra : ICadastro
    {
        private IWebDriver driver;
        private CompraModel compra;
        private WebDriverWait driverWait;
        public Compra(IWebDriver driver)
        {
            this.driver = driver;
            compra = new CompraModel();
            compra.Quantidade = 1;
            driverWait = new WebDriverWait(this.driver, System.TimeSpan.FromSeconds(20));
        }

        public void Novo()
        {
            driver.Navigate().GoToUrl("http://portalartcouro.apphb.com/Compra/NovaCompra");
        }

        public void Cadastrar()
        {
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//span[@id='select2-chosen-1']")));
            driver.FindElement(By.XPath(".//span[@id='select2-chosen-1']")).Click();
            driver.FindElement(By.Id("select2-results-1")).Click();
            driver.FindElement(By.Id("Quantidade")).SendKeys(compra.Quantidade.ToString());
            driver.FindElement(By.Id("AdicionarProduto")).Click();
            driver.FindElement(By.Id("GerarOrcamento")).Click();
        }

        public bool ExisteNaListagem()
        {
            throw new NotImplementedException();
        }

        public bool NaoDeveCriarCompraSemProduto()
        {
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//span[@id='select2-chosen-1']")));
            driver.FindElement(By.Id("Quantidade")).SendKeys(compra.Quantidade.ToString());
            driver.FindElement(By.Id("AdicionarProduto")).Click();
            return driver.PageSource.Contains("Campo Obrigatório");
        }

    }
}
