using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ProjetoArtCouro.Model.Models.ContaPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoArtCouro.Selenium.Paginas
{
    public class ContasAPagar : ICadastro
    {
        private IWebDriver driver;
        private ContaPagarModel contaPagar;
        private WebDriverWait driverWait;
        public ContasAPagar(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(this.driver, System.TimeSpan.FromSeconds(10));
        }

        public void Novo()
        {
            driver.Navigate().GoToUrl("http://portalartcouro.apphb.com/ContaPagar/PesquisaContaPagar");
        }

        public void Cadastrar()
        {
            new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(1);
            driver.FindElement(By.Id("PesquisarContaPagar")).Click();
            driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name*='checkbox']")));
            driver.FindElement(By.XPath(".//input[@name='checkbox']")).SendKeys(Keys.Space);
            driver.FindElement(By.Id("SalvarContaPagar")).Click();
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.Id("ConfirmarAcao")));
            driver.FindElement(By.Id("ConfirmarAcao")).Click();
        }

        public bool ExisteNaListagem()
        {
            try
            {
                new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(2);
                driver.FindElement(By.Id("PesquisarContaPagar")).Click();
                driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//input[@name='checkbox']")));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerificaCriacaoContaPagar()
        {
            try
            {
                Novo();
                new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(1);
                driver.FindElement(By.Id("PesquisarContaPagar")).Click();
                driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//input[@name='checkbox']")));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
