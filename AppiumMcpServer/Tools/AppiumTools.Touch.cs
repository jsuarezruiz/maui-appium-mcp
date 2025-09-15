using ModelContextProtocol.Server;
using OpenQA.Selenium.Interactions;
using System.ComponentModel;

namespace AppiumMcpServer.Tools
{
    /// <summary>
    /// Tap Actions for AppiumTools
    /// </summary>
    public partial class AppiumTools
    {
        /// <summary>
        /// Performs a single tap/click action on the specified UI element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_tap")]
        [Description("Tap on an element.")]
        public string Tap(string locatorStrategy, string locatorValue, int timeoutSeconds = 10)
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
        public string LongPress(string locatorStrategy, string locatorValue, int durationSeconds = 2, int timeoutSeconds = 10)
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

        /// <summary>
        /// Performs a right-click action on the specified UI element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_right_click")]
        [Description("Perform a right click on an element.")]
        public string RightClick(string locatorStrategy, string locatorValue, int timeoutSeconds = 10)
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
                actions.ContextClick(element).Perform();

                return $"Successfully right clicked element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to right click element: {ex.Message}";
            }
        }

        /// <summary>
        /// Performs a tap/click action on the specified coordinates.
        /// </summary>
        /// <param name="x">X-coordinate to tap</param>
        /// <param name="y">Y-coordinate to tap</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_tap_coordinates")]
        [Description("Tap on specific coordinates.")]
        public string TapCoordinates(float x, float y)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var actions = new Actions(_driver);
                actions.MoveByOffset((int)x, (int)y).Click().Perform();

                return $"Successfully tapped coordinates: ({x}, {y})";
            }
            catch (Exception ex)
            {
                return $"Failed to tap coordinates: {ex.Message}";
            }
        }

        /// <summary>
        /// Performs a double tap/click action on the specified coordinates.
        /// </summary>
        /// <param name="x">X-coordinate to double tap</param>
        /// <param name="y">Y-coordinate to double tap</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_double_tap_coordinates")]
        [Description("Double tap on specific coordinates.")]
        public string DoubleTapCoordinates(float x, float y)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var actions = new Actions(_driver);
                actions.MoveByOffset((int)x, (int)y).DoubleClick().Perform();

                return $"Successfully double tapped coordinates: ({x}, {y})";
            }
            catch (Exception ex)
            {
                return $"Failed to double tap coordinates: {ex.Message}";
            }
        }

        /// <summary>
        /// Performs a long press action on the specified coordinates.
        /// </summary>
        /// <param name="x">X-coordinate to long press</param>
        /// <param name="y">Y-coordinate to long press</param>
        /// <param name="durationSeconds">Duration to hold the press in seconds (default: 2)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_long_press_coordinates")]
        [Description("Long press on specific coordinates.")]
        public string LongPressCoordinates(float x, float y, int durationSeconds = 2)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var actions = new Actions(_driver);
                actions.MoveByOffset((int)x, (int)y)
                       .ClickAndHold()
                       .Pause(TimeSpan.FromSeconds(durationSeconds))
                       .Release()
                       .Perform();

                return $"Successfully long pressed coordinates: ({x}, {y}) for {durationSeconds}s";
            }
            catch (Exception ex)
            {
                return $"Failed to long press coordinates: {ex.Message}";
            }
        }
    }
}