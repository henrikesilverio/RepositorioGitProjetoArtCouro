using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoArtCouro.Selenium.Paginas
{
    public class PesquisaCompra
    {
        private IWebDriver driver;
        private WebDriverWait driverWait;
        public PesquisaCompra(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
        }

        public void Novo()
        {
            driver.Navigate().GoToUrl("http://portalartcouro.apphb.com/Compra/PesquisaCompra");
        }

        public void Cadastrar()
        {
            new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(1);
            driver.FindElement(By.Id("PesquisarCompra")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href*='/Compra/EditarCompra']")));
            driver.FindElement(By.CssSelector("a[href*='/Compra/EditarCompra']")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("EfetuarCompra")));
            driver.FindElement(By.Id("EfetuarCompra")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//span[@id='select2-chosen-2']")));
            driver.FindElement(By.XPath(".//span[@id='select2-chosen-2']")).Click();
            driver.FindElement(By.Id("select2-results-2")).Click();
            new SelectElement(driver.FindElement(By.Id("FormaPagamentoId"))).SelectByIndex(1);
            new SelectElement(driver.FindElement(By.Id("CondicaoPagamentoId"))).SelectByIndex(1);
            driver.FindElement(By.CssSelector("a[id*='Comprar']")).Click();
        }

        public bool ExisteNaListagem()
        {
            ContasAPagar contaPagar = new ContasAPagar(driver);
            contaPagar.VerificaCriacaoContaPagar();
            return true;
        }

        public static PesquisaCompra ProcessoCriacaoCompra(IWebDriver driver, PesquisaCompra pesquisaCompraPage)
        {
            Compra compraPage = new Compra(driver);
            compraPage.Novo();
            compraPage.Cadastrar();
            pesquisaCompraPage.Novo();
            pesquisaCompraPage.Cadastrar();
            return pesquisaCompraPage;
        }
    }
}
