using System;
using System.ComponentModel;
using System.IO;
using ModelContextProtocol.Server;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Android.Enums;
using OpenQA.Selenium.Appium.Interfaces;
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
        /// Uninstalls an application from the connected device.
        /// </summary>
        /// <param name="identifier">The bundle identifier (iOS) or package name (Android) of the app to uninstall</param>
        /// <returns>Uninstallation status message</returns>
        [McpServerTool(Name = "appium_uninstall_app")]
        [Description("Uninstall an app from the connected device using bundle ID (iOS) or package name (Android).")]
        public string UninstallApp(string identifier)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (string.IsNullOrWhiteSpace(identifier))
                {
                    return "Bundle ID/Package name cannot be null or empty";
                }

                // Check if app is installed before attempting uninstall
                bool isInstalled = _driver.IsAppInstalled(identifier);
                if (!isInstalled)
                {
                    return $"App with bundle ID '{identifier}' is not installed on the device";
                }

                _driver.RemoveApp(identifier);

                return $"Successfully uninstalled app: {identifier}";
            }
            catch (Exception ex)
            {
                return $"Failed to uninstall app '{identifier}': {ex.Message}";
            }
        }

        /// <summary>
        /// Checks if an application is installed on the device.
        /// </summary>
        /// <param name="identifier">The bundle identifier (iOS) or package name (Android) to check</param>
        /// <returns>Installation status information</returns>
        [McpServerTool(Name = "appium_is_app_installed")]
        [Description("Check if an app is installed on the device.")]
        public string IsAppInstalled(string identifier)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (string.IsNullOrWhiteSpace(identifier))
                {
                    return "Bundle ID/Package name cannot be null or empty";
                }

                bool isInstalled = _driver.IsAppInstalled(identifier);
                return $"App '{identifier}' is {(isInstalled ? "installed" : "not installed")} on the device";
            }
            catch (Exception ex)
            {
                return $"Failed to check app installation status for '{identifier}': {ex.Message}";
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
        /// Sends text input to the currently focused element or active input field.
        /// </summary>
        /// <param name="text">The text to send to the active element</param>
        /// <param name="elementId">Optional element ID to target specific element. If not provided, sends to currently focused element</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_send_keys")]
        [Description("Send text input to the focused element or specified element.")]
        public string SendKeys(string text, string? elementId = null)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (string.IsNullOrEmpty(text))
                {
                    return "Text parameter cannot be null or empty";
                }

                if (!string.IsNullOrEmpty(elementId))
                {
                    // Send keys to specific element
                    var element = _driver.FindElement(By.Id(elementId));

                    if (element == null)
                    {
                        // Try other locator strategies
                        element = _driver.FindElement(By.XPath($"//*[@content-desc='{elementId}']")) ??
                                  _driver.FindElement(By.XPath($"//*[@text='{elementId}']")) ??
                                  _driver.FindElement(By.Id(elementId));
                    }

                    if (element != null)
                    {
                        element.SendKeys(text);
                        return $"Successfully sent text '{text}' to element '{elementId}'";
                    }
                    else
                    {
                        return $"Element with ID '{elementId}' not found";
                    }
                }
                else
                {
                    // Send keys to currently focused element
                    var activeElement = _driver.SwitchTo().ActiveElement();
                    if (activeElement != null)
                    {
                        activeElement.SendKeys(text);
                        return $"Successfully sent text '{text}' to active element";
                    }
                    else
                    {
                        // Fallback: use Actions to send keys
                        var actions = new Actions(_driver);
                        actions.SendKeys(text).Perform();
                        return $"Successfully sent text '{text}' using Actions";
                    }
                }
            }
            catch (NoSuchElementException ex)
            {
                return $"Element not found: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Failed to send keys: {ex.Message}";
            }
        }

        /// <summary>
        /// Presses the Enter key on the device.
        /// </summary>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_press_enter")]
        [Description("Press the Enter key on the device.")]
        public string PressEnter()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (_currentPlatform == "android" && _driver is AndroidDriver androidDriver)
                {
                    // Android: Use key event for Enter key (keycode 66)
                    if (androidDriver is ISendsKeyEvents keyEventDriver)
                    {
                        keyEventDriver.PressKeyCode(AndroidKeyCode.Enter);
                        return "Successfully pressed Enter key (Android)";
                    }
                    else
                    {
                        // Fallback to Actions
                        var actions = new Actions(_driver);
                        actions.SendKeys(Keys.Enter).Perform();
                        return "Successfully pressed Enter key using Actions (Android)";
                    }
                }
                else if (_currentPlatform == "ios" && _driver is IOSDriver iosDriver)
                {
                    // iOS: Use Actions to send Enter key
                    var actions = new Actions(_driver);
                    actions.SendKeys(Keys.Enter).Perform();
                    return "Successfully pressed Enter key (iOS)";
                }
                else
                {
                    // Generic approach for other platforms
                    var actions = new Actions(_driver);
                    actions.SendKeys(Keys.Enter).Perform();
                    return "Successfully pressed Enter key (Generic)";
                }
            }
            catch (Exception ex)
            {
                return $"Failed to press Enter key: {ex.Message}";
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

        /// <summary>
        /// Sets the device orientation to landscape mode.
        /// </summary>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_set_orientation_landscape")]
        [Description("Change device orientation to landscape mode.")]
        public string SetOrientationLandscape()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (_currentPlatform == "android" && _driver is AndroidDriver androidDriver)
                {
                    // Android: Set orientation to landscape
                    androidDriver.Orientation = ScreenOrientation.Landscape;
                    return "Successfully set orientation to landscape (Android)";
                }
                else if (_currentPlatform == "ios" && _driver is IOSDriver iosDriver)
                {
                    // iOS: Set orientation to landscape
                    iosDriver.Orientation = ScreenOrientation.Landscape;
                    return "Successfully set orientation to landscape (iOS)";
                }
                else
                {
                    // Generic approach for other platforms
                    try
                    {
                        _driver.Manage().Window.Size = new System.Drawing.Size(
                            Math.Max(_driver.Manage().Window.Size.Width, _driver.Manage().Window.Size.Height),
                            Math.Min(_driver.Manage().Window.Size.Width, _driver.Manage().Window.Size.Height)
                        );
                        return "Attempted to set orientation to landscape (Generic - may not work on all platforms)";
                    }
                    catch (Exception)
                    {
                        return "Orientation change not supported on this platform";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Failed to set orientation to landscape: {ex.Message}";
            }
        }

        /// <summary>
        /// Sets the device orientation to portrait mode.
        /// </summary>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_set_orientation_portrait")]
        [Description("Change device orientation to portrait mode.")]
        public string SetOrientationPortrait()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                if (_currentPlatform == "android" && _driver is AndroidDriver androidDriver)
                {
                    // Android: Set orientation to portrait
                    androidDriver.Orientation = ScreenOrientation.Portrait;
                    return "Successfully set orientation to portrait (Android)";
                }
                else if (_currentPlatform == "ios" && _driver is IOSDriver iosDriver)
                {
                    // iOS: Set orientation to portrait
                    iosDriver.Orientation = ScreenOrientation.Portrait;
                    return "Successfully set orientation to portrait (iOS)";
                }
                else
                {
                    // Generic approach for other platforms
                    try
                    {
                        _driver.Manage().Window.Size = new System.Drawing.Size(
                            Math.Min(_driver.Manage().Window.Size.Width, _driver.Manage().Window.Size.Height),
                            Math.Max(_driver.Manage().Window.Size.Width, _driver.Manage().Window.Size.Height)
                        );
                        return "Attempted to set orientation to portrait (Generic - may not work on all platforms)";
                    }
                    catch (Exception)
                    {
                        return "Orientation change not supported on this platform";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Failed to set orientation to portrait: {ex.Message}";
            }
        }

        /// <summary>
        /// Lock the screen.
        /// Functionality that's only available on Android and iOS.
        /// </summary>
        [McpServerTool(Name = "appium_lock_screen")]
        [Description("Locks the device screen. Only available on Android and iOS devices.")]
        public string LockScreen()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                // Check if driver supports lock functionality
                if (_driver is not AndroidDriver && _driver is not IOSDriver)
                {
                    return "Lock screen is only supported on Android and iOS devices.";
                }

                // Execute lock command
                _driver.ExecuteScript("mobile: lock", new Dictionary<string, object>());

                return "Successfully locked the device screen.";
            }
            catch (Exception ex)
            {
                return $"Failed to lock screen: {ex.Message}";
            }
        }

        /// <summary>
        /// Unlock the screen.
        /// Functionality that's only available on Android and iOS.
        /// </summary>
        [McpServerTool(Name = "appium_unlock_screen")]
        [Description(
            "Unlocks the device screen with optional unlock type and key. Only available on Android and iOS devices.")]
        public string UnlockScreen(string unlockType = "", string unlockKey = "")
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                // Check if driver supports unlock functionality
                if (_driver is not AndroidDriver && _driver is not IOSDriver)
                {
                    return "Unlock screen is only supported on Android and iOS devices.";
                }

                // Validate unlock type if provided
                if (!string.IsNullOrEmpty(unlockType))
                {
                    var validUnlockTypes = new[]
                        { "pin", "pinWithKeyEvent", "password", "pattern", "fingerprint", "face" };
                    if (!validUnlockTypes.Contains(unlockType.ToLower()))
                    {
                        return
                            $"Invalid unlock type '{unlockType}'. Valid types are: {string.Join(", ", validUnlockTypes)}";
                    }
                }

                // Execute unlock command
                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(unlockType))
                {
                    parameters["unlockType"] = unlockType;
                }

                if (!string.IsNullOrEmpty(unlockKey))
                {
                    parameters["unlockKey"] = unlockKey;
                }

                _driver.ExecuteScript("mobile: unlock", parameters);

                var message = "Successfully unlocked the device screen";

                if (!string.IsNullOrEmpty(unlockType))
                {
                    message += $" using {unlockType}";
                }

                message += ".";

                return message;
            }
            catch (Exception ex)
            {
                return $"Failed to unlock screen: {ex.Message}";
            }
        }

        /// <summary>
        /// Toggle airplane mode on device.
        /// Functionality that's only available on Android.
        /// </summary>
        [McpServerTool(Name = "appium_toggle_airplane_mode")]
        [Description("Toggles airplane mode on the Android device. Only available on Android devices.")]
        public string ToggleAirplaneMode()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                // Check if driver is Android
                if (_driver is not AndroidDriver)
                {
                    return "Toggle airplane mode is only supported on Android devices.";
                }

                // Execute airplane mode toggle command
                _driver.ExecuteScript("mobile: toggleAirplaneMode", new Dictionary<string, object>());

                return "Successfully toggled airplane mode on the device.";
            }
            catch (Exception ex)
            {
                return $"Failed to toggle airplane mode: {ex.Message}";
            }
        }

        /// <summary>
        /// Switch the state of the wifi service.
        /// Functionality that's only available on Android.
        /// </summary>
        [McpServerTool(Name = "appium_toggle_wifi")]
        [Description("Toggles WiFi on the Android device. Only available on Android devices.")]
        public string ToggleWifi()
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                // Check if driver is Android
                if (_driver is not AndroidDriver)
                {
                    return "Toggle WiFi is only supported on Android devices.";
                }

                // Execute WiFi toggle command
                _driver.ExecuteScript("mobile: toggleWifi", new Dictionary<string, object>());

                return "Successfully toggled WiFi on the device.";
            }
            catch (Exception ex)
            {
                return $"Failed to toggle WiFi: {ex.Message}";
            }
        }
    }
}