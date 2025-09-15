using System;
using ModelContextProtocol.Server;
using OpenQA.Selenium.Interactions;
using System.ComponentModel;

namespace AppiumMcpServer.Tools
{
    /// <summary>
    /// Swipe Actions for AppiumTools
    /// Comprehensive implementation of swipe gesture operations for screen and element-level interactions
    /// </summary>
    public partial class AppiumTools
    {
        /// <summary>
        /// Performs a left to right swipe gesture on a specific element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy</param>
        /// <param name="locatorValue">Element location value</param>
        /// <param name="swipePercentage">How far across the element to swipe (from 0.0 to 1.0)</param>
        /// <param name="swipeSpeed">Speed of the gesture in milliseconds</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_swipe_left_to_right")]
        [Description("Swipe from left to right on a specific element.")]
        public string SwipeLeftToRight(string locatorStrategy, string locatorValue, double swipePercentage = 0.67, int swipeSpeed = 500, int timeoutSeconds = 10)
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
                var startX = elementRect.X + (int)(elementRect.Width * 0.1);
                var endX = elementRect.X + (int)(elementRect.Width * swipePercentage);
                var y = elementRect.Y + elementRect.Height / 2;

                var actions = new Actions(_driver);
                actions.MoveToElement(element, startX - elementRect.X, y - elementRect.Y)
                       .ClickAndHold()
                       .Pause(TimeSpan.FromMilliseconds(100))
                       .MoveByOffset(endX - startX, 0)
                       .Release()
                       .Perform();

                return $"Successfully swiped left to right on element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to swipe element left to right: {ex.Message}";
            }
        }

        /// <summary>
        /// Performs a right to left swipe gesture on a specific element.
        /// </summary>
        /// <param name="locatorStrategy">Element location strategy</param>
        /// <param name="locatorValue">Element location value</param>
        /// <param name="swipePercentage">How far across the element to swipe (from 0.0 to 1.0)</param>
        /// <param name="swipeSpeed">Speed of the gesture in milliseconds</param>
        /// <param name="timeoutSeconds">Maximum time to wait for element (default: 10)</param>
        /// <returns>Action status message</returns>
        [McpServerTool(Name = "appium_swipe_element_right_to_left")]
        [Description("Swipe from right to left on a specific element.")]
        public string SwipeRightToLeft(string locatorStrategy, string locatorValue, double swipePercentage = 0.67, int swipeSpeed = 500, int timeoutSeconds = 10)
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
                var startX = elementRect.X + (int)(elementRect.Width * 0.9);
                var endX = elementRect.X + (int)(elementRect.Width * (1.0 - swipePercentage));
                var y = elementRect.Y + elementRect.Height / 2;

                var actions = new Actions(_driver);
                actions.MoveToElement(element, startX - elementRect.X, y - elementRect.Y)
                       .ClickAndHold()
                       .Pause(TimeSpan.FromMilliseconds(100))
                       .MoveByOffset(endX - startX, 0)
                       .Release()
                       .Perform();

                return $"Successfully swiped right to left on element: {locatorStrategy}={locatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to swipe element right to left: {ex.Message}";
            }
        }
    }
}