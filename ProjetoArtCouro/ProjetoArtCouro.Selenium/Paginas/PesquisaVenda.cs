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
    public class PesquisaVenda
    {
        private IWebDriver driver;
        private WebDriverWait driverWait;
        public PesquisaVenda(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
        }

        public void Novo()
        {
            driver.Navigate().GoToUrl("http://portalartcouro.apphb.com/Venda/PesquisaVenda");
        }

        public void Cadastrar()
        {
            new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(1);
            driver.FindElement(By.Id("PesquisarVenda")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href*='Editar']")));
            driver.FindElement(By.CssSelector("a[href*='Editar']")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.Id("EfetuarVenda")));
            driver.FindElement(By.Id("EfetuarVenda")).Click();
            driverWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//span[@id='select2-chosen-2']")));
            driver.FindElement(By.XPath(".//span[@id='select2-chosen-2']")).Click();
            driver.FindElement(By.Id("select2-results-2")).Click();
            new SelectElement(driver.FindElement(By.Id("FormaPagamentoId"))).SelectByIndex(1);
            new SelectElement(driver.FindElement(By.Id("CondicaoPagamentoId"))).SelectByIndex(1);
            driver.FindElement(By.CssSelector("a[id*='Vender']")).Click();
        }

        public bool ExisteNaListagem()
        {
            ContasAReceber contaReceber = new ContasAReceber(driver);
            return contaReceber.VerificaCriacaoContaReceber();
        }
    }
}
