using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace ProjetoArtCouro.Selenium.TesteSeleniumBrowsers
{
    public class TestSeleniumBrowsers
    {
        private static void AbrirUrl(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://portalartcouro.apphb.com/");
        }

        private static IWebDriver AbrirFirefox()
        {
            IWebDriver driver = new FirefoxDriver();
            AbrirUrl(driver);
            return driver;
        }

        private static IWebDriver AbrirChrome()
        {
            IWebDriver driver = new ChromeDriver(@"C:\Users\Felipe Silva\Documents\SubVersion");
            AbrirUrl(driver);
            return driver;
        }

        public static IWebDriver RealizarLoginFirefox()
        {
            IWebDriver driver = AbrirFirefox();
            Login(driver);
            return driver;
        }

        public static IWebDriver RealizarLoginChrome()
        {
            IWebDriver driver = AbrirChrome();
            Login(driver);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("add-event")));
            return driver;
        }

        private static void Login(IWebDriver driver)
        {
            IWebElement login = driver.FindElement(By.Name("UsuarioNome"));
            IWebElement senha = driver.FindElement(By.Name("Senha"));

            login.SendKeys("admin");
            senha.SendKeys("admin");
            senha.Submit();
        }
    }
}
