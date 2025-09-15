using System;
using System.ComponentModel;
using ModelContextProtocol.Server;

namespace AppiumMcpServer.Tools
{
    public partial class AppiumTools
    {
        /// <summary>
        /// Sends text input to the specified UI element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="text">Text to send to the element</param>
        /// <param name="clearFirst">Whether to clear existing text before sending new text (default: true)</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_enter_text")]
        [Description("Send text to an element.")]
        public string EnterText(
            string locatorStrategy,
            string locatorValue,
            string text,
            bool clearFirst = true,
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

                if (clearFirst)
                {
                    element.Clear();
                }

                element.SendKeys(text);
                return $"Successfully sent text '{text}' to element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to send keys: {ex.Message}";
            }
        }
        
        /// <summary>
        /// Clears text from the specified input element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_clear_text")]
        [Description("Clear text from an input element.")]
        public string ClearText(string locatorStrategy, string locatorValue, int timeoutSeconds = 10)
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

                element.Clear();
                return $"Successfully cleared element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to clear element: {ex.Message}";
            }
        }
        
        /// <summary>
        /// Retrieves the text content from the specified UI element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy (id, xpath, text, etc.)</param>
        /// <param name="locatorValue">Value used with the locator strategy to find the element</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>The text content of the element or error message</returns>
        [McpServerTool(Name = "appium_get_element_text")]
        [Description("Get text from an element.")]
        public string GetElementText(
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

                var text = element.Text;
                return $"Element text: '{text}'";
            }
            catch (Exception ex)
            {
                return $"Failed to get element text: {ex.Message}";
            }
        }
    }
}