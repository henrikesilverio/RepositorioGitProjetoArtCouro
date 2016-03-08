using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using ProjetoArtCouro.Selenium.TesteSeleniumBrowsers;
using ProjetoArtCouro.Selenium.Paginas;

namespace ProjetoArtCouro.Selenium.Testes
{
    [TestClass]
    public class TestContaAReceber
    {
        [TestMethod]
        public void DeveReceberContaReceberChrome()
        {
            IWebDriver driver = TestSeleniumBrowsers.RealizarLoginChrome();
            ContasAReceber contaAReceberPage = new ContasAReceber(driver);
            contaAReceberPage.Novo();
            contaAReceberPage.Cadastrar();
            Assert.IsTrue(contaAReceberPage.ExisteNaListagem());
            driver.Close();
        }
    }
}
