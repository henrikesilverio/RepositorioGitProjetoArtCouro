using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoArtCouro.Selenium.Paginas
{
    public class ContasAReceber : ICadastro
    {
        private IWebDriver driver;
        private WebDriverWait driverWait;
        public ContasAReceber(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(this.driver, System.TimeSpan.FromSeconds(10));
        }

        public void Novo()
        {
            driver.Navigate().GoToUrl("http://portalartcouro.apphb.com/ContaReceber/PesquisaContaReceber");
        }

        public void Cadastrar()
        {
            new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(1);
            driver.FindElement(By.Id("PesquisarContaReceber")).Click();
            driverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name*='checkbox']")));
            driver.FindElement(By.CssSelector("input[name*='checkbox']")).SendKeys(Keys.Space);
            driver.FindElement(By.Id("SalvarContaReceber")).Click();
            driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//a[@id='ConfirmarAcao']")));
            driver.FindElement(By.XPath(".//a[@id='ConfirmarAcao']")).Click();
        }

        public bool ExisteNaListagem()
        {
            try
            {
                new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(2);
                driver.FindElement(By.Id("PesquisarContaReceber")).Click();
                driverWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//input[@name='checkbox']")));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerificaCriacaoContaReceber()
        {
            try
            {
                Novo();
                new SelectElement(driver.FindElement(By.Id("StatusId"))).SelectByIndex(1);
                driver.FindElement(By.Id("PesquisarContaReceber")).Click();
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
