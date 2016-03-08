using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using ProjetoArtCouro.Selenium.TesteSeleniumBrowsers;
using ProjetoArtCouro.Selenium.Paginas;

namespace ProjetoArtCouro.Selenium.Testes
{
    [TestClass]
    public class TestContaAPagar
    {
        [TestMethod]
        public void DevePagarContaAPagarChrome()
        {
            IWebDriver driver = TestSeleniumBrowsers.RealizarLoginChrome();
            ContasAPagar contaAPagarPage = new ContasAPagar(driver);
            contaAPagarPage.Novo();
            contaAPagarPage.Cadastrar();
            Assert.IsTrue(contaAPagarPage.ExisteNaListagem());
            driver.Close();
        }
    }
}
