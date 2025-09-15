using System;
using System.ComponentModel;
using ModelContextProtocol.Server;
using OpenQA.Selenium.Interactions;

namespace AppiumMcpServer.Tools
{
    public partial class AppiumTools
    {
/// <summary>
        /// Performs a tap/click action on the specified UI element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_tap")]
        [Description("Tap on an element.")]
        public string Tap(
            string locatorStrategy,
            string locatorValue,
            int timeoutSeconds = 10)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var element = FindElementWithTimeout(locatorStrategy, locatorValue, timeoutSeconds);
                if (element == null)
                {
                    return $"Element not found using {locatorStrategy}: {locatorValue}";
                }

                element.Click();
                return $"Successfully tapped element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to tap element: {ex.Message}";
            }
        }

        /// <summary>
        /// Performs a double tap/click action on the specified UI element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_double_tap")]
        [Description("Perform a double tap on an element.")]
        public string DoubleTap(string locatorStrategy, string locatorValue, int timeoutSeconds = 10)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var element = FindElementWithTimeout(locatorStrategy, locatorValue, timeoutSeconds);
                if (element == null)
                {
                    return $"Element not found using {locatorStrategy}: {locatorValue}";
                }

                var actions = new Actions(_driver);
                actions.DoubleClick(element).Perform();

                return $"Successfully double tapped element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to double tap element: {ex.Message}";
            }
        }

        /// <summary>
        /// Performs a long press/touch and hold action on the specified UI element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="durationSeconds">Duration to hold the press in seconds (default: 2)</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_long_press")]
        [Description("Perform a long press on an element.")]
        public string LongPress(string locatorStrategy, string locatorValue, int durationSeconds = 2,
            int timeoutSeconds = 10)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var element = FindElementWithTimeout(locatorStrategy, locatorValue, timeoutSeconds);
                if (element == null)
                {
                    return $"Element not found using {locatorStrategy}: {locatorValue}";
                }

                var actions = new Actions(_driver);
                actions.ClickAndHold(element)
                    .Pause(TimeSpan.FromSeconds(durationSeconds))
                    .Release()
                    .Perform();

                return $"Successfully long pressed element for {durationSeconds}s: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to long press element: {ex.Message}";
            }
        }
    }
}