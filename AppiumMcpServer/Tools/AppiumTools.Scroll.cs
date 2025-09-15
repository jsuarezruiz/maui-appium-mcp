using System;
using ModelContextProtocol.Server;
using OpenQA.Selenium.Interactions;
using System.ComponentModel;
using System.Threading;

namespace AppiumMcpServer.Tools
{
    /// <summary>
    /// Swipe Actions for AppiumTools
    /// </summary>
    public partial class AppiumTools
    {
        /// <summary>
        /// Scrolls down on the specified element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy</param>
        /// <param name="locatorValue">Element location value</param>
        /// <param name="swipePercentage">How far to scroll (from 0.0 to 1.0)</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_scroll_down")]
        [Description("Scroll down on an element.")]
        public string ScrollDown(string locatorStrategy, string locatorValue, double swipePercentage = 0.67, int timeoutSeconds = 10)
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

                var elementRect = element.Rect;
                var startY = elementRect.Y + (int)(elementRect.Height * 0.8);
                var endY = elementRect.Y + (int)(elementRect.Height * (1.0 - swipePercentage));
                var x = elementRect.X + elementRect.Width / 2;

                var actions = new Actions(_driver);
                actions.MoveToElement(element, x - elementRect.X, startY - elementRect.Y)
                       .ClickAndHold()
                       .MoveByOffset(0, endY - startY)
                       .Release()
                       .Perform();

                return $"Successfully scrolled down on element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to scroll down: {ex.Message}";
            }
        }

        /// <summary>
        /// Scrolls up on the specified element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy</param>
        /// <param name="locatorValue">Element location value</param>
        /// <param name="swipePercentage">How far to scroll (from 0.0 to 1.0)</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_scroll_up")]
        [Description("Scroll up on an element.")]
        public string ScrollUp(string locatorStrategy, string locatorValue, double swipePercentage = 0.67, int timeoutSeconds = 10)
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

                var elementRect = element.Rect;
                var startY = elementRect.Y + (int)(elementRect.Height * 0.2);
                var endY = elementRect.Y + (int)(elementRect.Height * swipePercentage);
                var x = elementRect.X + elementRect.Width / 2;

                var actions = new Actions(_driver);
                actions.MoveToElement(element, x - elementRect.X, startY - elementRect.Y)
                       .ClickAndHold()
                       .MoveByOffset(0, endY - startY)
                       .Release()
                       .Perform();

                return $"Successfully scrolled up on element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to scroll up: {ex.Message}";
            }
        }

        /// <summary>
        /// Scrolls left on the specified element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy</param>
        /// <param name="locatorValue">Element location value</param>
        /// <param name="swipePercentage">How far to scroll (from 0.0 to 1.0)</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_scroll_left")]
        [Description("Scroll left on an element.")]
        public string ScrollLeft(string locatorStrategy, string locatorValue, double swipePercentage = 0.67, int timeoutSeconds = 10)
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

                var elementRect = element.Rect;
                var startX = elementRect.X + (int)(elementRect.Width * 0.8);
                var endX = elementRect.X + (int)(elementRect.Width * (1.0 - swipePercentage));
                var y = elementRect.Y + elementRect.Height / 2;

                var actions = new Actions(_driver);
                actions.MoveToElement(element, startX - elementRect.X, y - elementRect.Y)
                       .ClickAndHold()
                       .MoveByOffset(endX - startX, 0)
                       .Release()
                       .Perform();

                return $"Successfully scrolled left on element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to scroll left: {ex.Message}";
            }
        }

        /// <summary>
        /// Scrolls right on the specified element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy</param>
        /// <param name="locatorValue">Element location value</param>
        /// <param name="swipePercentage">How far to scroll (from 0.0 to 1.0)</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element to be found (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_scroll_right")]
        [Description("Scroll right on an element.")]
        public string ScrollRight(string locatorStrategy, string locatorValue, double swipePercentage = 0.67, int timeoutSeconds = 10)
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

                var elementRect = element.Rect;
                var startX = elementRect.X + (int)(elementRect.Width * 0.2);
                var endX = elementRect.X + (int)(elementRect.Width * swipePercentage);
                var y = elementRect.Y + elementRect.Height / 2;

                var actions = new Actions(_driver);
                actions.MoveToElement(element, startX - elementRect.X, y - elementRect.Y)
                       .ClickAndHold()
                       .MoveByOffset(endX - startX, 0)
                       .Release()
                       .Perform();

                return $"Successfully scrolled right on element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to scroll right: {ex.Message}";
            }
        }

        /// <summary>
        /// Scrolls until a target element becomes visible within a scrollable container.
        /// </summary>
        /// <param name="targetLocatorStrategy">Target element location strategy</param>
        /// <param name="targetLocatorValue">Target element location value</param>
        /// <param name="containerLocatorStrategy">Container element location strategy</param>
        /// <param name="containerLocatorValue">Container element location value</param>
        /// <param name="direction">Scroll direction: up, down, left, right</param>
        /// <param name="maxScrolls">Maximum number of scroll attempts (default: 10)</param>
        /// <param name="timeoutSeconds">Maximum time to wait for elements (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_scroll_to")]
        [Description("Scroll until a target element becomes visible.")]
        public string ScrollTo(string targetLocatorStrategy, string targetLocatorValue, string containerLocatorStrategy, string containerLocatorValue, string direction = "down", int maxScrolls = 10, int timeoutSeconds = 10)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var container = FindElementWithTimeout(containerLocatorStrategy, containerLocatorValue, timeoutSeconds);
                if (container == null)
                {
                    return $"Container element not found using {containerLocatorStrategy}: {containerLocatorValue}";
                }

                for (int i = 0; i < maxScrolls; i++)
                {
                    var targetElement = FindElementWithTimeout(targetLocatorStrategy, targetLocatorValue, 1);
                    if (targetElement != null && targetElement.Displayed)
                    {
                        return $"Successfully found target element after {i} scrolls: {targetLocatorStrategy}={targetLocatorValue}";
                    }

                    switch (direction.ToLower())
                    {
                        case "down":
                            ScrollDown(containerLocatorStrategy, containerLocatorValue);
                            break;
                        case "up":
                            ScrollUp(containerLocatorStrategy, containerLocatorValue);
                            break;
                        case "left":
                            ScrollLeft(containerLocatorStrategy, containerLocatorValue);
                            break;
                        case "right":
                            ScrollRight(containerLocatorStrategy, containerLocatorValue);
                            break;
                        default:
                            return $"Invalid scroll direction: {direction}. Use: up, down, left, right";
                    }

                    Thread.Sleep(500);
                }

                return $"Target element not found after {maxScrolls} scrolls: {targetLocatorStrategy}={targetLocatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to scroll to element: {ex.Message}";
            }
        }
    }
}