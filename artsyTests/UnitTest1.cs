using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Support.UI;

namespace artsyTests;

[TestFixture]
public class LoginTests
{
    [SetUp]
    public void Setup()
    {
        Console.WriteLine("--- Ustawienia testu (Setup) ---");

        var appiumOptions = new AppiumOptions();
        appiumOptions.PlatformName = "Android";
        appiumOptions.DeviceName = "Artsy";
        appiumOptions.AutomationName = "UiAutomator2";

        // Podstawowe ustawienia aplikacji
        var appPackage = "dev.bykowski.artsy.mobile";
        var appActivity = "crc644c25d90d1d995a67.MainActivity";

        appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, appPackage);
        appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, appActivity);

        // Opcje uruchomienia - wyłączamy autoLaunch, będziemy uruchamiać ręcznie
        appiumOptions.AddAdditionalAppiumOption("autoLaunch", false);
        appiumOptions.AddAdditionalAppiumOption("noReset", true);
        appiumOptions.AddAdditionalAppiumOption("fullReset", false);

        // Timeouty
        appiumOptions.AddAdditionalAppiumOption("newCommandTimeout", 300);
        appiumOptions.AddAdditionalAppiumOption("androidInstallTimeout", 90000);
        appiumOptions.AddAdditionalAppiumOption("uiautomator2ServerInstallTimeout", 60000);

        // Automatyczne uprawnienia
        appiumOptions.AddAdditionalAppiumOption("autoGrantPermissions", true);
        appiumOptions.AddAdditionalAppiumOption("autoAcceptAlerts", true);

        Console.WriteLine($"Łączę się z aplikacją: {appPackage}");
        Console.WriteLine($"Aktywność: {appActivity}");

        try
        {
            driver = new AndroidDriver(new Uri("http://127.0.0.1:4723"), appiumOptions);
            Console.WriteLine("Sterownik Appium uruchomiony.");

            // Krótka pauza na ustabilizowanie się połączenia
            Thread.Sleep(3000);

            var currentActivity = driver.CurrentActivity;
            Console.WriteLine($"Aktualna aktywność: {currentActivity}");

            // Uruchom aplikację poprzez kliknięcie ikony
            if (!LaunchArtsyApp()) Assert.Fail("Nie udało się uruchomić aplikacji Artsy");

            Console.WriteLine("✅ Aplikacja Artsy uruchomiona pomyślnie!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BŁĄD KRYTYCZNY: {ex.GetType().Name}");
            Console.WriteLine($"Wiadomość: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            Assert.Fail($"Nie udało się uruchomić aplikacji: {ex.Message}");
        }

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        Console.WriteLine("Setup zakończony.");
    }


    [TearDown]
    public void TearDown()
    {
        Console.WriteLine("--- Zakończenie testu (TearDown) ---");
        if (driver != null)
            try
            {
                driver.Quit();
                Console.WriteLine("Sterownik Appium został zamknięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas zamykania sterownika: {ex.Message}");
            }
            finally
            {
                driver.Dispose();
                Console.WriteLine("Sterownik Appium został dispose'owany.");
            }
        else
            Console.WriteLine("Sterownik Appium nie został zainicjalizowany, pominięto zamykanie/dispose.");
    }

    private AndroidDriver driver;
    private WebDriverWait wait;

    [Test]
    public void DiagnosticTest()
    {
        Console.WriteLine("=== TEST DIAGNOSTYCZNY ===");

        try
        {
            // Sprawdź czy driver działa
            Console.WriteLine($"Driver session ID: {driver.SessionId}");

            // Sprawdź aktualną aktywność
            var currentActivity = driver.CurrentActivity;
            Console.WriteLine($"Aktualna aktywność: {currentActivity}");

            // Sprawdź package aplikacji
            var currentPackage = driver.CurrentPackage;
            Console.WriteLine($"Aktualny package: {currentPackage}");

            // Sprawdź wszystkie widoczne elementy
            var allElements = driver.FindElements(By.XPath("//*"));
            Console.WriteLine($"Liczba znalezionych elementów: {allElements.Count}");

            // Sprawdź źródło strony (ograniczone do pierwszych 2000 znaków)
            var pageSource = driver.PageSource;
            Console.WriteLine("=== ŹRÓDŁO STRONY (pierwsze 2000 znaków) ===");
            Console.WriteLine(pageSource.Length > 2000 ? pageSource.Substring(0, 2000) + "..." : pageSource);

            // Zrób screenshot
            var screenshot = driver.GetScreenshot();
            var screenshotPath = Path.Combine(Environment.CurrentDirectory, "test_screenshot.png");
            screenshot.SaveAsFile(screenshotPath);
            Console.WriteLine($"Screenshot zapisany jako: {screenshotPath}");

            // Sprawdź czy jesteśmy w aplikacji Artsy
            if (currentPackage == "dev.bykowski.artsy.mobile")
            {
                Console.WriteLine("✅ Jesteśmy w aplikacji Artsy!");

                // Sprawdź klkalne elementy w aplikacji
                var clickableElements = driver.FindElements(By.XPath("//*[@clickable='true']"));
                Console.WriteLine($"Znaleziono {clickableElements.Count} klikalnych elementów w aplikacji");

                // Wypisz pierwsze 5 klikalnych elementów
                for (var i = 0; i < Math.Min(5, clickableElements.Count); i++)
                {
                    var element = clickableElements[i];
                    var text = element.Text;
                    var contentDesc = element.GetAttribute("content-desc");
                    var resourceId = element.GetAttribute("resource-id");

                    Console.WriteLine(
                        $"Element {i + 1}: Text='{text}', ContentDesc='{contentDesc}', ResourceId='{resourceId}'");
                }
            }
            else
            {
                Console.WriteLine($"❌ Nie jesteśmy w aplikacji Artsy. Aktualny package: {currentPackage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd w teście diagnostycznym: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    [Test]
    public void BasicLoginTest()
    {
        Console.WriteLine("=== TEST LOGOWANIA ===");

        try
        {
            // Upewnij się, że jesteśmy w aplikacji
            var currentPackage = driver.CurrentPackage;
            if (currentPackage != "dev.bykowski.artsy.mobile")
            {
                Console.WriteLine("Nie jesteśmy w aplikacji Artsy, próbuję uruchomić...");
                if (!LaunchArtsyApp()) Assert.Fail("Nie udało się uruchomić aplikacji Artsy");
            }

            // Poczekaj na załadowanie aplikacji
            Thread.Sleep(5000);

            // Sprawdź aktualne elementy
            var allElements = driver.FindElements(By.XPath("//*[@clickable='true']"));
            Console.WriteLine($"Znaleziono {allElements.Count} klikalnych elementów");

            // Spróbuj znaleźć elementy logowania
            IWebElement loginButton = null;
            IWebElement emailField = null;
            IWebElement passwordField = null;

            // Różne sposoby znalezienia elementów logowania
            try
            {
                // Szukaj po tekście
                loginButton = driver.FindElement(By.XPath(
                    "//*[contains(@text, 'Login') or contains(@text, 'Sign In') or contains(@text, 'Zaloguj')]"));
                Console.WriteLine("✅ Znaleziono przycisk logowania po tekście");
            }
            catch
            {
                try
                {
                    // Szukaj po content-desc
                    loginButton =
                        driver.FindElement(
                            By.XPath("//*[contains(@content-desc, 'Login') or contains(@content-desc, 'Sign In')]"));
                    Console.WriteLine("✅ Znaleziono przycisk logowania po content-desc");
                }
                catch
                {
                    Console.WriteLine("❌ Nie znaleziono przycisku logowania");
                }
            }

            // Szukaj pól tekstowych
            var textFields = driver.FindElements(By.XPath("//android.widget.EditText"));
            Console.WriteLine($"Znaleziono {textFields.Count} pól tekstowych");

            if (textFields.Count >= 2)
            {
                emailField = textFields[0];
                passwordField = textFields[1];
                Console.WriteLine("✅ Znaleziono pola email i hasło");
            }

            // Jeśli znaleziono elementy, wykonaj test logowania
            if (emailField != null && passwordField != null && loginButton != null)
            {
                Console.WriteLine("Wykonuję test logowania...");

                emailField.Clear();
                emailField.SendKeys("test@example.com");
                Console.WriteLine("✅ Wpisano email");

                passwordField.Clear();
                passwordField.SendKeys("testpassword");
                Console.WriteLine("✅ Wpisano hasło");

                loginButton.Click();
                Console.WriteLine("✅ Kliknięto przycisk logowania");

                // Poczekaj na rezultat
                Thread.Sleep(3000);

                var newActivity = driver.CurrentActivity;
                Console.WriteLine($"Aktywność po logowaniu: {newActivity}");
            }
            else
            {
                Console.WriteLine("❌ Nie znaleziono wszystkich elementów potrzebnych do logowania");
                Console.WriteLine("Sprawdź strukturę aplikacji w source'ie strony");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd w teście logowania: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    // Metoda pomocnicza do uruchomienia aplikacji Artsy
    private bool LaunchArtsyApp()
    {
        try
        {
            Console.WriteLine("Próbuję uruchomić aplikację Artsy poprzez kliknięcie ikony...");

            // Poczekaj na załadowanie launchera
            Thread.Sleep(2000);

            // Metoda 1: Kliknij ikonę Artsy z hotseat (dolny pasek)
            try
            {
                var artsyIcon = wait.Until(d => d.FindElement(By.XPath("//android.widget.TextView[@text='Artsy']")));
                if (artsyIcon != null)
                {
                    Console.WriteLine("Znaleziono ikonę Artsy w hotseat, klikam...");
                    artsyIcon.Click();
                    Thread.Sleep(5000); // Daj więcej czasu na uruchomienie

                    var currentActivity = driver.CurrentActivity;
                    var currentPackage = driver.CurrentPackage;
                    Console.WriteLine($"Po kliknięciu - Aktywność: {currentActivity}, Package: {currentPackage}");

                    if (currentPackage == "dev.bykowski.artsy.mobile")
                    {
                        Console.WriteLine("✅ Aplikacja Artsy uruchomiona pomyślnie!");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nie udało się kliknąć ikony z hotseat: {ex.Message}");
            }

            // Metoda 2: Otwórz app drawer i szukaj aplikacji
            try
            {
                Console.WriteLine("Próbuję otworzyć app drawer...");

                // Swipe up aby otworzyć app drawer
                var size = driver.Manage().Window.Size;
                var startX = size.Width / 2;
                var startY = size.Height - 100;
                var endY = size.Height / 2;

                driver.ExecuteScript("mobile: swipe", new Dictionary<string, object>
                {
                    { "startX", startX },
                    { "startY", startY },
                    { "endX", startX },
                    { "endY", endY },
                    { "duration", 1000 }
                });

                Thread.Sleep(2000);

                // Szukaj aplikacji Artsy w app drawer
                var artsyInDrawer = driver.FindElement(By.XPath("//android.widget.TextView[@text='Artsy']"));
                if (artsyInDrawer != null)
                {
                    Console.WriteLine("Znaleziono Artsy w app drawer, klikam...");
                    artsyInDrawer.Click();
                    Thread.Sleep(5000);

                    var currentPackage = driver.CurrentPackage;
                    if (currentPackage == "dev.bykowski.artsy.mobile")
                    {
                        Console.WriteLine("✅ Aplikacja Artsy uruchomiona przez app drawer!");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nie udało się uruchomić przez app drawer: {ex.Message}");
            }

            // Metoda 3: Użyj startActivity (bezpośrednie uruchomienie)
            try
            {
                Console.WriteLine("Próbuję uruchomić przez startActivity...");
                driver.ActivateApp("dev.bykowski.artsy.mobile");
                Thread.Sleep(5000);

                var currentPackage = driver.CurrentPackage;
                if (currentPackage == "dev.bykowski.artsy.mobile")
                {
                    Console.WriteLine("✅ Aplikacja Artsy uruchomiona przez startActivity!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StartActivity nie powiodło się: {ex.Message}");
            }

            Console.WriteLine("❌ Wszystkie metody uruchomienia aplikacji zawiodły");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas uruchamiania aplikacji: {ex.Message}");
            return false;
        }
    }
}