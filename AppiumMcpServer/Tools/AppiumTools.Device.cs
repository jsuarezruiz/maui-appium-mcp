using System;
using System.ComponentModel;
using System.IO;
using ModelContextProtocol.Server;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Interactions;

namespace AppiumMcpServer.Tools
{
    public partial class AppiumTools
    {
        /// <summary>
        /// Installs an application package on the connected device.
        /// </summary>
        /// <param name="appPath">Full path to the application package file</param>
        /// <returns>Installation status message</returns>
        [McpServerTool(Name = "appium_install_app")]
        [Description("Install an app on the connected device.")]
        public string InstallApp(string appPath)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (!File.Exists(appPath))
                {
                    return $"App file not found: {appPath}";
                }

                _driver.InstallApp(appPath);
                return $"App installed successfully: {appPath}";
            }
            catch (Exception ex)
            {
                return $"Failed to install app: {ex.Message}";
            }
        }
        
        /// <summary>
        /// Navigates back using the device's back button (Android) or back navigation.
        /// </summary>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_navigate_back")]
        [Description("Navigate back using device back button.")]
        public string NavigateBack()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                _driver.Navigate().Back();
                return "Successfully navigated back";
            }
            catch (Exception ex)
            {
                return $"Failed to navigate back: {ex.Message}";
            }
        }
        
         /// <summary>
        /// Dismisses the soft keyboard if it's currently shown.
        /// </summary>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_dismiss_keyboard")]
        [Description("Dismiss the soft keyboard.")]
        public string DismissKeyboard()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (_currentPlatform == "android" && _driver is AndroidDriver androidDriver)
                {
                    androidDriver.HideKeyboard();
                    return "Successfully dismissed keyboard";
                }
                else if (_currentPlatform == "ios" && _driver is IOSDriver iosDriver)
                {
                    iosDriver.HideKeyboard();
                    return "Successfully dismissed keyboard";
                }
                else
                {
                    // Try generic approach
                    var actions = new Actions(_driver);
                    actions.SendKeys(Keys.Escape).Perform();
                    return "Attempted to dismiss keyboard";
                }
            }
            catch (Exception ex)
            {
                return $"Failed to dismiss keyboard: {ex.Message}";
            }
        }

        /// <summary>
        /// Checks if the soft keyboard is currently shown.
        /// </summary>
        /// <returns>Keyboard visibility status</returns>
        [McpServerTool(Name = "appium_is_keyboard_shown")]
        [Description("Check if the soft keyboard is shown.")]
        public string IsKeyboardShown()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (_currentPlatform == "android" && _driver is AndroidDriver androidDriver)
                {
                    var isShown = androidDriver.IsKeyboardShown();
                    return $"Keyboard is shown: {isShown}";
                }
                else if (_currentPlatform == "ios" && _driver is IOSDriver iosDriver)
                {
                    var isShown = iosDriver.IsKeyboardShown();
                    return $"Keyboard is shown: {isShown}";
                }
                else
                {
                    return "Keyboard status check not supported on this platform";
                }
            }
            catch (Exception ex)
            {
                return $"Failed to check keyboard status: {ex.Message}";
            }
        }
    }
}