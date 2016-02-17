using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using ProjetoArtCouro.Selenium.Paginas;
using ProjetoArtCouro.Selenium.TesteSeleniumBrowsers;

namespace ProjetoArtCouro.Selenium
{
    [TestClass]
    public class TestSeleniumCompra
    {
        [TestMethod]
        public void DeveCriarCompraChrome()
        {
            IWebDriver driver = TestSeleniumBrowsers.RealizarLoginChrome();
            Compra compraPage = new Compra(driver);
            PesquisaCompra pesquisaCompraPage = new PesquisaCompra(driver);
            compraPage.Novo();
            compraPage.Cadastrar();
            pesquisaCompraPage.Novo();
            pesquisaCompraPage.Cadastrar();
            Assert.IsTrue(pesquisaCompraPage.ExisteNaListagem());
            driver.Close();
        }
    }
}
