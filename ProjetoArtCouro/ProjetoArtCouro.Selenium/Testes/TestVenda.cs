using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using ProjetoArtCouro.Selenium.TesteSeleniumBrowsers;
using ProjetoArtCouro.Selenium.Paginas;

namespace ProjetoArtCouro.Selenium.Testes
{
    [TestClass]
    public class TestVenda
    {
        [TestMethod]
        public void DeveCriarVendaChrome()
        {
            IWebDriver driver = TestSeleniumBrowsers.RealizarLoginChrome();
            Venda vendaPage = new Venda(driver);
            PesquisaVenda pesquisaVendaPage = new PesquisaVenda(driver);
            vendaPage.Novo();
            vendaPage.PreencherVenda();
            vendaPage.Cadastrar();
            pesquisaVendaPage.Novo();
            pesquisaVendaPage.Cadastrar();
            Assert.IsTrue(pesquisaVendaPage.ExisteNaListagem());
            driver.Close();
        }
    }
}
