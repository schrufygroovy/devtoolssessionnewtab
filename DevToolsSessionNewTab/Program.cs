using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V93.Network;

using var chromeDriver = new ChromeDriver();
chromeDriver.Url = "https://www.selenium.dev";
string originalWindow = chromeDriver.CurrentWindowHandle;
await DevToolsSessionNewTab.Helper.GetAllCookies(chromeDriver);

chromeDriver.SwitchTo().NewWindow(WindowType.Tab);
string newWindowHandle = chromeDriver.CurrentWindowHandle;
await DevToolsSessionNewTab.Helper.GetAllCookies(chromeDriver);

// this .Close() kills the dev tools session, no chance to ever retrieve a new one for the other tab
chromeDriver.SwitchTo().Window(originalWindow).Close();
chromeDriver.SwitchTo().Window(newWindowHandle);
chromeDriver.Url = "https://www.selenium.dev/documentation/webdriver/browser_manipulation/";
await DevToolsSessionNewTab.Helper.GetAllCookies(chromeDriver);


namespace DevToolsSessionNewTab
{
    public static class Helper
    {
        public static Task GetAllCookies(ChromeDriver driver)
        {
            return driver.GetDevToolsSession()
                .GetVersionSpecificDomains<OpenQA.Selenium.DevTools.V93.DevToolsSessionDomains>()
                .Network
                .GetAllCookies(new GetAllCookiesCommandSettings { });
        }
    }
}
