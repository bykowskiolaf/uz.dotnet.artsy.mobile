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
        var appiumOptions = new AppiumOptions();
        appiumOptions.PlatformName = "Android";
        appiumOptions.DeviceName = "Artsy";
        appiumOptions.AutomationName = "UiAutomator2";

        var appPackage = "dev.bykowski.artsy.mobile";
        var appActivity = "crc644c25d90d1d995a67.MainActivity";

        appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, appPackage);
        appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, appActivity);
        appiumOptions.AddAdditionalAppiumOption("autoLaunch", false);
        appiumOptions.AddAdditionalAppiumOption("noReset", true);
        appiumOptions.AddAdditionalAppiumOption("fullReset", false);
        appiumOptions.AddAdditionalAppiumOption("newCommandTimeout", 300);
        appiumOptions.AddAdditionalAppiumOption("androidInstallTimeout", 90000);
        appiumOptions.AddAdditionalAppiumOption("uiautomator2ServerInstallTimeout", 60000);
        appiumOptions.AddAdditionalAppiumOption("autoGrantPermissions", true);
        appiumOptions.AddAdditionalAppiumOption("autoAcceptAlerts", true);

        Console.WriteLine("--- Ustawienia testu (Setup) ---");
        Console.WriteLine($"Łączę się z aplikacją: {appPackage}");
        Console.WriteLine($"Aktywność: {appActivity}");

        try
        {
            driver = new AndroidDriver(new Uri("http://127.0.0.1:4723"), appiumOptions);
            Console.WriteLine("Sterownik Appium uruchomiony.");
            Thread.Sleep(3000);
            Console.WriteLine($"Aktualna aktywność: {driver.CurrentActivity}");

            Console.WriteLine("Próbuję uruchomić aplikację Artsy przez startActivity...");
            driver.ActivateApp("dev.bykowski.artsy.mobile");
            Thread.Sleep(5000); // Dłuższa pauza na uruchomienie aplikacji
            Console.WriteLine($"Aktualna aktywność po uruchomieniu: {driver.CurrentActivity}");

            if (driver.CurrentPackage != "dev.bykowski.artsy.mobile")
                Assert.Fail("Nie udało się uruchomić aplikacji Artsy");
            Console.WriteLine("✅ Aplikacja Artsy uruchomiona pomyślnie!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BŁĄD KRYTYCZNY: {ex.GetType().Name}");
            Console.WriteLine($"Wiadomość: {ex.Message}");
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
    }

    private AndroidDriver driver;
    private WebDriverWait wait;

    [Test]
    public void BasicLoginAndNavigateToArtistsTest()
    {
        Console.WriteLine("=== TEST LOGOWANIA I NAWIGACJI DO ARTYSTÓW ===");

        try
        {
            Thread.Sleep(3000); // Pauza na załadowanie ekranu logowania

            // Logowanie (przenieś do osobnej metody, jeśli testujesz wiele scenariuszy)
            var emailField = wait.Until(d => d.FindElement(By.Id("EmailEntry")));
            emailField.Clear();
            emailField.SendKeys("test@example.com");
            Console.WriteLine("✅ Wpisano email.");

            var passwordField = wait.Until(d => d.FindElement(By.Id("PasswordEntry")));
            passwordField.Clear();
            passwordField.SendKeys("password");
            Console.WriteLine("✅ Wpisano hasło.");

            var loginButton = wait.Until(d => d.FindElement(By.Id("LoginButton")));
            loginButton.Click();
            Console.WriteLine("✅ Kliknięto przycisk logowania.");

            Thread.Sleep(5000); // Sztywna pauza na nawigację po logowaniu

            // Weryfikacja po zalogowaniu (usunięto sprawdzanie zniknięcia LoginButton,
            // bo to już się dzieje, jeśli przechodzi do ArtworksPage)

            // Poczekaj na załadowanie ArtworksPage (np. po tytule "Artworks")
            wait.Until(d => d.FindElement(By.XPath("//*[@text='Artworks']")));
            Console.WriteLine("✅ Strona 'Artworks' załadowana pomyślnie.");

            // --- NOWA SEKCJA: OTWARCIE MENU I NAWIGACJA DO ARTYSTÓW ---

            Console.WriteLine("Próbuję otworzyć menu Flyout (Hamburger icon)...");
            // Appium często rozpoznaje ikonę hamburgera jako element z content-desc "Open navigation drawer"
            var hamburgerMenuButton = wait.Until(d =>
            {
                try
                {
                    return d.FindElement(By.ClassName("android.widget.ImageButton"));
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });

            if (hamburgerMenuButton == null)
                throw new NoSuchElementException("Hamburger menu button (Open navigation drawer) not found.");

            hamburgerMenuButton.Click();
            Console.WriteLine("✅ Kliknięto przycisk menu (Hamburger icon).");
            Thread.Sleep(2000);

            Console.WriteLine("Próbuję znaleźć i kliknąć element 'Artists' w menu Flyout.");
            var artistsMenuItem = wait.Until(d =>
            {
                try
                {
                    return d.FindElement(By.XPath("//*[@text='Artists']"));
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });

            if (artistsMenuItem == null)
                throw new NoSuchElementException("Menu item 'Artists' not found in Flyout.");

            artistsMenuItem.Click();
            Console.WriteLine("✅ Kliknięto w element 'Artists' w menu.");
            Thread.Sleep(5000); // Dłuższa pauza na załadowanie strony ArtistsPage

            var artistsPageTitle = wait.Until(d =>
            {
                try
                {
                    return d.FindElement(By.XPath("//*[@text='Artists']"));
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });

            if (artistsPageTitle == null)
                Assert.Fail("Nie udało się nawigować do strony 'Artists'. Tytuł 'Artists' nie znaleziony.");
            Console.WriteLine("✅ Strona 'Artists' załadowana pomyślnie!");

            Console.WriteLine("🎉 TEST NAWIGACJI DO ARTYSTÓW ZAKOŃCZONY SUKCESEM!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ BŁĄD W TEŚCIE: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Console.WriteLine("--- Aktualne źródło strony (dla debugowania): ---");
            Console.WriteLine(driver.PageSource);
            Assert.Fail($"Test zakończył się błędem: {ex.Message}");
        }
    }
}