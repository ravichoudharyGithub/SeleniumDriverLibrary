using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Drawing;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using SeleniumDriverLibrary.Internal;

namespace SeleniumDriverLibrary
{
    public class SeleniumDriverFactoryClass : IDisposable
    {
        public SeleniumDriverFactoryClass()
        {

        }

        ChromeDriver _driver = new ChromeDriver();
        //PhantomJSDriver _driver = new PhantomJSDriver();

        public void OpenUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
            try
            {
                IAlert simpleAlert = _driver.SwitchTo().Alert();
                simpleAlert.Accept();
                //simpleAlert.Dismiss();
            }
            catch
            {

            }

            _driver.Manage().Window.Maximize();
            Thread.Sleep(8000);
        }

        public void Dispose()
        {
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }

        public string GetHtmlSourceVisibleTextOnly()
        {
            try
            {
                if (_driver != null)
                {

                    var bodyElement = _driver.FindElement(By.TagName("body"));

                    return bodyElement.Text;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
            return "";
        }

        public void Click(string xpath, string DropDownValue = "")
        {
            var nameStartsWithName = xpath.StartsWith("name=");
            var nameStartsWithId = xpath.StartsWith("id=");
            var nameStartsWithClassName = xpath.StartsWith("class=");
            var nameStartwithXpath = xpath.StartsWith("xpath=");
            var nameStartwithXpath2 = xpath.StartsWith("Xpath2=");
            try
            {

                IWebElement elem = null;

                if (nameStartsWithId)
                {
                    var locname = xpath.Substring("id=".Length);
                    elem = _driver.FindElement(By.Id(locname));
                }
                else if (nameStartsWithName)
                {
                    var locname = xpath.Substring("name=".Length);
                    elem = _driver.FindElement(By.Name(locname));
                }
                else if (nameStartsWithClassName)
                {
                    var locname = xpath.Substring("class=".Length);
                    elem = _driver.FindElement(By.ClassName(locname));
                }
                else if (nameStartwithXpath)
                {
                    var locname = xpath.Substring("xpath=".Length);
                    elem = _driver.FindElement(By.XPath(locname));
                }

                else if (nameStartwithXpath2)
                {
                    var locname = xpath.Substring("xpath2=".Length);
                    var elems = _driver.FindElements(By.XPath(locname));
                    foreach (var item in elems)
                    {
                        if (item.Text.Contains(DropDownValue))
                        {
                            elem = item;
                            break;
                        }
                    }
                }

                else
                {
                    elem = _driver.FindElement(By.Id(xpath));
                }

                elem?.Click();
            }
            catch (Exception e)
            {
                var s = "nothing";
            }

        }

        public void Type(string name, string value)
        {
            var nameStartsWithName = name.StartsWith("name=");
            var nameStartsWithId = name.StartsWith("id=");
            var nameStartsWithClassName = name.StartsWith("class=");
            var nameStartsWithXpath = name.StartsWith("xpath=");
            try
            {
                IWebElement elem = null;
                var backSpaces = new string('\b', 20);

                if (nameStartsWithId)
                {
                    var locname = name.Substring("id=".Length);
                    elem = _driver.FindElement(By.Id(locname));
                }
                else if (nameStartsWithName)
                {
                    var locname = name.Substring("name=".Length);
                    elem = _driver.FindElement(By.Name(locname));
                }
                else if (nameStartsWithClassName)
                {
                    var locname = name.Substring("class=".Length);
                    elem = _driver.FindElement(By.ClassName(locname));
                }
                else if (nameStartsWithXpath)
                {
                    var locname = name.Substring("xpath=".Length);
                    elem = _driver.FindElement(By.XPath(locname));
                }
                else
                {
                    elem = _driver.FindElement(By.Id(name));
                }

                if (elem != null)
                {
                    elem.Clear();
                    elem.SendKeys(value);
                }
            }
            catch
            {
                var s = "nothing";
            }
        }

        public void SelectExtended(string locator, string Value, bool Search = false)
        {
            Value = Value.Replace("label=", "");
            try
            {

                var type = Helper.GetIdentifierBeforeEquals(locator);
                var noprefix = Helper.ExtractAfterEquals(locator);

                SelectElement select = null;

                if (type.Equals("xpath"))
                {
                    select = new SelectElement(_driver.FindElement(By.XPath(noprefix)));

                }
                else if (type.Equals("id"))
                {
                    select = new SelectElement(_driver.FindElement(By.Id(noprefix)));
                }
                else if (type.Equals("name"))
                {
                    select = new SelectElement(_driver.FindElement(By.Name(noprefix)));

                }
                else if (type.Equals("css"))
                {
                    select = new SelectElement(_driver.FindElement(By.CssSelector(noprefix)));
                }
                else
                {
                    select = new SelectElement(_driver.FindElement(By.Id(noprefix)));
                }

                if (Search)
                {
                    var options = select.Options;
                    var selected = options.FirstOrDefault(x => x.Text.Contains(Value));
                    if (selected == null)
                    {
                        foreach (var s in Value.Replace(" ", "|").Split('|'))
                        {
                            selected = options.FirstOrDefault(x => x.Text == s);
                            if (selected != null)
                            {
                                break;
                            }
                        }
                    }
                    if (selected == null) selected = options.FirstOrDefault();
                    if (selected != null)
                        Value = selected.Text;
                    else
                        Value = null;
                }
                //if (string.IsNullOrEmpty(select.SelectedOption.GetAttribute("value")))
                select.SelectByText(Value);
                //Return = 0;
            }
            catch
            {
                var ex = "nothing,...";
                //Return = 1;
            }
        }

        public IWebElement FindElement(string locator)
        {
            IWebElement element = null;
            try
            {
                var attribute = Helper.GetIdentifierBeforeEquals(locator);
                var noprefix = Helper.ExtractAfterEquals(locator);
                if (attribute.Equals("xpath"))
                    element = _driver.FindElement(By.XPath(noprefix));
                else if (attribute.Equals("id"))
                    element = _driver.FindElement(By.Id(noprefix));
                else if (attribute.Equals("name"))
                    element = _driver.FindElement(By.Name(noprefix));
                else if (attribute.Equals("css"))
                    element = _driver.FindElement(By.CssSelector(noprefix));
                else if (attribute.Equals("linkText"))
                    element = _driver.FindElement(By.LinkText(noprefix));
                else if (attribute.Equals("class"))
                    element = _driver.FindElement(By.ClassName(noprefix));
            }
            catch (Exception e)
            {
                var ex = "nothing,...";
            }
            return element;
        }

        public ICollection<IWebElement> FindElementCollection(string locator)
        {

            ICollection<IWebElement> element = null;
            try
            {
                var attribute = Helper.GetIdentifierBeforeEquals(locator);
                var noprefix = Helper.ExtractAfterEquals(locator);
                if (attribute.Equals("xpath"))
                    element = _driver.FindElements(By.XPath(noprefix));
                else if (attribute.Equals("id"))
                    element = _driver.FindElements(By.Id(noprefix));
                else if (attribute.Equals("name"))
                    element = _driver.FindElements(By.Name(noprefix));
                else if (attribute.Equals("css"))
                    element = _driver.FindElements(By.CssSelector(noprefix));
                else if (attribute.Equals("linkText"))
                    element = _driver.FindElements(By.LinkText(noprefix));
                else if (attribute.Equals("class"))
                    element = _driver.FindElements(By.ClassName(noprefix));
            }
            catch (Exception e)
            {
                var ex = "nothing,...";
            }
            return element;
        }

        public IList<IWebElement> FindElementCollectionInList(string locator)
        {

            IList<IWebElement> element = null;
            try
            {
                var attribute = Helper.GetIdentifierBeforeEquals(locator);
                var noprefix = Helper.ExtractAfterEquals(locator);
                if (attribute.Equals("xpath"))
                    element = _driver.FindElements(By.XPath(noprefix));
                else if (attribute.Equals("id"))
                    element = _driver.FindElements(By.Id(noprefix));
                else if (attribute.Equals("name"))
                    element = _driver.FindElements(By.Name(noprefix));
                else if (attribute.Equals("css"))
                    element = _driver.FindElements(By.CssSelector(noprefix));
                else if (attribute.Equals("linkText"))
                    element = _driver.FindElements(By.LinkText(noprefix));
                else if (attribute.Equals("class"))
                    element = _driver.FindElements(By.ClassName(noprefix));
            }
            catch (Exception e)
            {
                var ex = "nothing,...";
            }
            return element;
        }

        public IReadOnlyCollection<IWebElement> FindElementCollection(string lookupType, string locator)
        {
            IReadOnlyCollection<IWebElement> elements = null;
            try
            {
                switch (lookupType)
                {
                    case "xpath":
                        elements = _driver.FindElements(By.XPath(locator));
                        break;
                    case "id":
                        elements = _driver.FindElements(By.Id(locator));
                        break;
                    case "name":
                        elements = _driver.FindElements(By.Name(locator));
                        break;
                    case "css":
                        elements = _driver.FindElements(By.CssSelector(locator));
                        break;
                    case "class":
                        elements = _driver.FindElements(By.ClassName(locator));
                        break;
                    default:
                        break;
                }

                return elements;
            }
            catch
            {
                //string ex = "nothing,...";
            }
            return elements;
        }

        public string GetHtmlSource()
        {
            var a = _driver.PageSource;
            return a;
        }

        public void MoveElement(string className)
        {
            var element = _driver.FindElement(By.ClassName(className));
            var actions = new Actions(_driver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        public void MoveScreenToElement(int X, int Y)
        {
            try
            {
                var js = $"window.scrollTo({X}, {Y})";
                ((IJavaScriptExecutor)_driver).ExecuteScript(js);
            }
            catch (Exception e)
            {
                // nothing
            }
        }

        public Screenshot GetScreenShot()
        {
            return _driver.GetScreenshot();
        }
        public string GetScreenShot(string name)
        {
            try
            {
                var ss = ((ITakesScreenshot)_driver).GetScreenshot();
                ss.SaveAsFile($"{name}.jpeg", ScreenshotImageFormat.Jpeg);
            }
            catch
            {
                //
            }
            return name;
        }

        public byte[] CropScreenShot(string filename)
        {
            try
            {
                //var eleLocation = FindElement("xpath=//input[@name='captchaResponse']").Location;
                var eleLocation = FindElement("xpath=//img[@id='imgcap2']").Location;
                var part = new RectangleF(Math.Max(eleLocation.X - 300, 0), eleLocation.Y - 100, 500, 200);
                var bmpobj = (Bitmap)Image.FromFile(filename + ".jpeg");
                var converter = new ImageConverter();
                return (byte[])converter.ConvertTo(bmpobj, typeof(byte[]));

                //File.Delete(filename + ".jpeg");
                var bn = bmpobj.Clone(part, bmpobj.PixelFormat);
                bn.Save(filename);
                return (byte[])converter.ConvertTo(bn, typeof(byte[]));
            }
            catch (Exception e)
            {
                //
            }
            return null;
        }

        public void MoveScreenToElement(IWebElement element)
        {
            try
            {
                var actions = new Actions(_driver);
                actions.MoveToElement(element);
                actions.Perform();
            }
            catch (Exception e)
            {
                //Nothing
            }
        }

        DeathByCaptcha.Client _client;
        DeathByCaptcha.Captcha _captcha;

        public string BreakCaptcha(byte[] captchaImage)
        {
            if (captchaImage.Length > 0)
            {
                // now evaluate captcha

                var captchaGuess = "";
                const bool useDecaptcher = false;

                //const string apiDecaptcherHost = "api.de-captcher.info";
                //const string apiDecaptcherHost = "api.decaptcher-reloaded.com ";
                uint majorId;
                uint minorId;

                //DeathByCaptcha.Client client = null;
                //DeathByCaptcha.Captcha captcha = null;

                // get balance
                if (useDecaptcher)
                {
                    // 2012-5-19 - disable DecaptcherLib lib for now since they have separate .dlls for 32 and 64 bit with same name.
                    // was causing extra hassle to deploy 32 ot 64 bit version of software since had to copy over correct .dll
                    //Get your balance
                    //int ret = DecaptcherLib.Decaptcher.Balance(apiDecaptcherHost, apiDecaptcherPort, apiDecaptcherUsername, apiDecaptcherPassword, out balance);
                }
                //_client = new DeathByCaptcha.SocketClient("yourUserName", "yourPassword");
                _client = new DeathByCaptcha.SocketClient("dbcstb3", "H@pislv");
                var balance = _client.Balance;
                if (balance < 1)
                {
                }

                if (useDecaptcher)
                {
                    ////Get load system Decaptcher.
                    //uint load;
                    //int ret = DecaptcherLib.Decaptcher.SystemDecaptcherLoad(apiDecaptcherHost, apiDecaptcherPort, apiDecaptcherUsername, apiDecaptcherPassword, out load);

                    ////Send captcha to Decaptcher
                    //uint pPictTo;
                    //uint pPictType;
                    //ret = DecaptcherLib.Decaptcher.RecognizePicture(
                    //    apiDecaptcherHost, apiDecaptcherPort, apiDecaptcherUsername, apiDecaptcherPassword, captchaImage, out pPictTo, out pPictType, out captchaGuess,
                    //    out majorId, out minorId);

                    ////Recognize Asirra captcha.
                    ////ret = DecaptcherLib.Decaptcher.RecognizeAsirra(host, port, name, passw, assira, out p_pict_to, out p_pict_type, out answer_captcha, out major_id, out minor_id);
                }
                // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA
                // image file name, file object, stream, or a vector of bytes,
                // and desired solving timeout (in seconds) here.  If solved,
                // you'll receive a DeathByCaptcha.Captcha object.
                _captcha = _client.Decode(captchaImage, DeathByCaptcha.Client.DefaultTimeout);
                if (null != _captcha)
                {
                    //captchaGuess = _captcha.Text.ToUpper();
                    captchaGuess = _captcha.Text;
                    //Console.WriteLine("CAPTCHA {0:D} solved: {1}", _captcha.Id, _captcha.Text);
                }
                //  Console.WriteLine("Your balance is {0:F2} US cents", _client.Balance);
                return captchaGuess;
            }

            return null;
        }

        public bool ReportBadCaptcha()
        {
            var report = false;
            try
            {
                report = _client.Report(_captcha);
            }
            catch { }
            return report;
        }
    }
}
