using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using ModelContextProtocol.Server;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace AppiumMcpServer.Tools
{
    /// <summary>
    /// Provides Appium automation tools for cross-platform mobile and desktop application testing.
    /// Supports Android, iOS, and Windows platforms through the Model Context Protocol (MCP) server framework.
    /// </summary>
    [McpServerToolType]
    public partial class AppiumTools
    {
        /// <summary>
        /// The current active Appium driver instance for interacting with applications.
        /// </summary>
        static AppiumDriver? _driver;

        /// <summary>
        /// The local Appium service instance for managing the Appium server lifecycle.
        /// </summary>
        static AppiumLocalService? _appiumService;

        /// <summary>
        /// Tracks the current platform (android, ios, windows) for platform-specific operations.
        /// </summary>
        static string _currentPlatform = "";

        /// <summary>
        /// Starts the Appium server on the specified host and port.
        /// </summary>
        /// <param name="port">The port number for the Appium server (default: 4723)</param>
        /// <param name="host">The host address for the Appium server (default: 127.0.0.1)</param>
        /// <returns>Status message indicating success or failure of server startup</returns>
        [McpServerTool(Name = "appium_start_server")]
        [Description("Start the Appium server.")]
        public string StartAppiumServer(
            int port = 4723,
            string host = "127.0.0.1")
        {
            try
            {
                if (_appiumService != null)
                {
                    return "Appium server is already running";
                }

                var builder = new AppiumServiceBuilder()
                    .WithIPAddress(host)
                    .UsingPort(port);

                _appiumService = builder.Build();
                _appiumService.Start();

                return $"Appium server started successfully on {host}:{port}";
            }
            catch (Exception ex)
            {
                return $"Failed to start Appium server: {ex.Message}";
            }
        }

        /// <summary>
        /// Stops the currently running Appium server and releases associated resources.
        /// </summary>
        /// <returns>Status message indicating success or failure of server shutdown</returns>
        [McpServerTool(Name = "appium_stop_server")]
        [Description("Stop the Appium server.")]
        public string StopAppiumServer()
        {
            try
            {
                _appiumService?.Dispose();
                _appiumService = null;
                return "Appium server stopped successfully";
            }
            catch (Exception ex)
            {
                return $"Failed to stop Appium server: {ex.Message}";
            }
        }

        /// <summary>
        /// Establishes a connection to an application on Android, iOS, or Windows platforms.
        /// </summary>
        /// <param name="platform">Target platform: "android", "ios", or "windows"</param>
        /// <param name="deviceName">Name or identifier of the target device</param>
        /// <param name="appPackage">Android app package name (Android only)</param>
        /// <param name="appActivity">Android app activity name (Android only)</param>
        /// <param name="bundleId">iOS app bundle identifier (iOS only)</param>
        /// <param name="appPath">Path to the application executable or package</param>
        /// <param name="udid">Unique device identifier for physical devices</param>
        /// <param name="appiumUrl">Appium server URL (default: http://127.0.0.1:4723)</param>
        /// <returns>Status message indicating connection success or failure details</returns>
        [McpServerTool(Name = "appium_connect_app")]
        [Description("Connect to an app on Android, iOS, or Windows.")]
        public string ConnectApp(
            string platform,
            string deviceName,
            string? appPackage = null,
            string? appActivity = null,
            string? bundleId = null,
            string? appPath = null,
            string? udid = null,
            string appiumUrl = "http://127.0.0.1:4723")
        {
            try
            {
                // Clean up existing connection
                if (_driver != null)
                {
                    _driver.Quit();
                    _driver = null;
                }

                var options = new AppiumOptions();
                var serverUri = new Uri(appiumUrl);
                _currentPlatform = platform.ToLower();

                switch (_currentPlatform)
                {
                    case "android":
                        return ConnectAndroid(serverUri, options, deviceName, appPackage, appActivity, udid);

                    case "ios":
                        return ConnectiOS(serverUri, options, deviceName, bundleId, appPath, udid);

                    case "windows":
                        return ConnectWindows(serverUri, options, deviceName, appPath);

                    default:
                        return $"Unsupported platform: {platform}. Use 'android', 'ios', or 'windows'";
                }
            }
            catch (Exception ex)
            {
                return $"Failed to connect to app: {ex.Message}";
            }
        }

        /// <summary>
        /// Configures and establishes connection to an Android application.
        /// </summary>
        /// <param name="serverUri">Appium server URI</param>
        /// <param name="options">Appium options to configure</param>
        /// <param name="deviceName">Android device name</param>
        /// <param name="appPackage">Android app package name</param>
        /// <param name="appActivity">Android app activity name</param>
        /// <param name="udid">Device UDID for physical devices</param>
        /// <returns>Connection status message</returns>
        string ConnectAndroid(Uri serverUri, AppiumOptions options, string deviceName,
            string? appPackage, string? appActivity, string? udid)
        {
            options.PlatformName = "Android";
            options.DeviceName = deviceName;
            options.AutomationName = "UIAutomator2";

            if (!string.IsNullOrEmpty(udid))
            {
                options.AddAdditionalAppiumOption("udid", udid);
            }

            if (!string.IsNullOrEmpty(appPackage))
            {
                options.AddAdditionalAppiumOption("appPackage", appPackage);
            }

            if (!string.IsNullOrEmpty(appActivity))
            {
                options.AddAdditionalAppiumOption("appActivity", appActivity);
            }

            // Preserve app state between sessions
            options.AddAdditionalAppiumOption("noReset", true);

            _driver = new AndroidDriver(serverUri, options);
            return $"Successfully connected to Android app: {appPackage}";
        }

        /// <summary>
        /// Configures and establishes connection to an iOS application.
        /// </summary>
        /// <param name="serverUri">Appium server URI</param>
        /// <param name="options">Appium options to configure</param>
        /// <param name="deviceName">iOS device name</param>
        /// <param name="bundleId">iOS app bundle identifier</param>
        /// <param name="appPath">Path to iOS app package</param>
        /// <param name="udid">Device UDID for physical devices</param>
        /// <returns>Connection status message</returns>
        string ConnectiOS(
            Uri serverUri,
            AppiumOptions
                options,
            string deviceName,
            string? bundleId,
            string? appPath,
            string? udid)
        {
            options.PlatformName = "iOS";
            options.DeviceName = deviceName;
            options.AutomationName = "XCUITest";

            if (!string.IsNullOrEmpty(udid))
            {
                options.AddAdditionalAppiumOption("udid", udid);
            }

            if (!string.IsNullOrEmpty(bundleId))
            {
                options.AddAdditionalAppiumOption("bundleId", bundleId);
            }
            else if (!string.IsNullOrEmpty(appPath))
            {
                options.App = appPath;
            }

            // Preserve app state between sessions
            options.AddAdditionalAppiumOption("noReset", true);

            _driver = new IOSDriver(serverUri, options);
            return $"Successfully connected to iOS app: {bundleId ?? appPath ?? "com.apple.Preferences"}";
        }

        /// <summary>
        /// Configures and establishes connection to a Windows application.
        /// </summary>
        /// <param name="serverUri">Appium server URI</param>
        /// <param name="options">Appium options to configure</param>
        /// <param name="deviceName">Windows device name</param>
        /// <param name="appPath">Path to Windows application executable</param>
        /// <returns>Connection status message</returns>
        string ConnectWindows(Uri serverUri, AppiumOptions options, string deviceName, string? appPath)
        {
            options.PlatformName = "Windows";
            options.DeviceName = deviceName;
            options.AutomationName = "Windows";

            if (!string.IsNullOrEmpty(appPath))
            {
                options.App = appPath;
            }

            _driver = new WindowsDriver(serverUri, options);
            return $"Successfully connected to Windows app: {appPath ?? "Calculator"}";
        }
        
        /// <summary>
        /// Captures a screenshot of the current application state.
        /// </summary>
        /// <param name="filePath">Optional file path for saving the screenshot. If null, generates timestamp-based filename</param>
        /// <returns>Path where screenshot was saved or error message</returns>
        [McpServerTool(Name = "appium_take_screenshot")]
        [Description("Take a screenshot.")]
        public string TakeScreenshot(
            string? filePath = null)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                }

                var screenshot = _driver.GetScreenshot();
                screenshot.SaveAsFile(filePath);

                return $"Screenshot saved: {Path.GetFullPath(filePath)}";
            }
            catch (Exception ex)
            {
                return $"Failed to take screenshot: {ex.Message}";
            }
        }

        /// <summary>
        /// Retrieves the complete XML source of the current application page/screen.
        /// Useful for debugging element hierarchy and structure.
        /// </summary>
        /// <returns>XML page source with character count or error message</returns>
        [McpServerTool(Name = "appium_get_page_source")]
        [Description("Get the current page source XML.")]
        public string GetPageSource()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var pageSource = _driver.PageSource;
                return $"Page source retrieved (length: {pageSource.Length} characters):\n{pageSource}";
            }
            catch (Exception ex)
            {
                return $"Failed to get page source: {ex.Message}";
            }
        }

        /// <summary>
        /// Disconnects from the current application and cleans up the driver session.
        /// </summary>
        /// <returns>Disconnection status message</returns>
        [McpServerTool(Name = "appium_disconnect")]
        [Description("Disconnect from the current app.")]
        public string Disconnect()
        {
            try
            {
                if (_driver != null)
                {
                    _driver.Quit();
                    _driver = null;
                    _currentPlatform = "";
                    return "Successfully disconnected from app";
                }

                return "No active connection to disconnect";
            }
            catch (Exception ex)
            {
                return $"Failed to disconnect: {ex.Message}";
            }
        }

        /// <summary>
        /// Attempts to find a UI element using the specified locator strategy with timeout and retry logic.
        /// </summary>
        /// <param name="locatorStrategy">The strategy to use for finding the element</param>
        /// <param name="locatorValue">The value associated with the locator strategy</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element</param>
        /// <returns>Found AppiumElement or null if not found within timeout</returns>
        AppiumElement? FindElementWithTimeout(string locatorStrategy, string locatorValue, int timeoutSeconds)
        {
            if (_driver == null) return null;

            var endTime = DateTime.Now.AddSeconds(timeoutSeconds);

            while (DateTime.Now < endTime)
            {
                try
                {
                    var by = GetByLocator(locatorStrategy, locatorValue);
                    var element = _driver.FindElement(by);
                    if (element != null) return element;
                }
                catch
                {
                    // Element not found, continue trying
                }

                // Wait before retrying
                Thread.Sleep(500);
            }

            return null;
        }

        /// <summary>
        /// Converts locator strategy and value into appropriate Selenium By locator.
        /// Supports platform-specific optimizations for element finding.
        /// </summary>
        /// <param name="strategy">Locator strategy name</param>
        /// <param name="value">Value for the locator</param>
        /// <returns>Selenium By locator instance</returns>
        /// <exception cref="ArgumentException">Thrown when unsupported locator strategy is provided</exception>
        By GetByLocator(string strategy, string value)
        {
            return strategy.ToLower() switch
            {
                "id" => By.Id(value),
                "xpath" => By.XPath(value),
                "classname" or "class_name" => By.ClassName(value),
                "name" => By.Name(value),
                "tagname" or "tag_name" => By.TagName(value),
                "cssselector" or "css_selector" => By.CssSelector(value),
                "accessibility_id" => MobileBy.AccessibilityId(value),
                "android_uiautomator" => MobileBy.AndroidUIAutomator(value),
                "ios_predicate" => MobileBy.IosNSPredicate(value),
                "ios_class_chain" => MobileBy.IosClassChain(value),
                // Platform-specific text finding strategies
                "text" => _currentPlatform switch
                {
                    "android" => By.XPath($"//*[@text='{value}']"),
                    "ios" => By.XPath($"//*[@name='{value}' or @label='{value}' or @value='{value}']"),
                    "windows" => By.Name(value),
                    _ => By.XPath($"//*[@text='{value}']")
                },
                _ => throw new ArgumentException($"Unsupported locator strategy: {strategy}")
            };
        }
    }
}