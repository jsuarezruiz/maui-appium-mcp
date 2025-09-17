using System.ComponentModel;
using ModelContextProtocol.Server;

namespace AppiumMcpServer.Tools
{
    public partial class AppiumTools
    {
        [McpServerTool(Name = "appium_drag_and_drop")]
        [Description(
            "Performs a long touch on a source element, followed by dragging it to a target element and dropping it.")]
        public string DragAndDrop(
            string sourceLocatorStrategy,
            string sourceLocatorValue,
            string targetLocatorStrategy,
            string targetLocatorValue,
            int timeoutSeconds = 10)
        {
            try
            {
                if (_driver == null)
                {
                    return "No active connection. Use appium_connect_app first.";
                }

                var sourceElement = FindElementWithTimeout(sourceLocatorStrategy, sourceLocatorValue, timeoutSeconds);
                if (sourceElement == null)
                {
                    return $"Source element not found using {sourceLocatorStrategy}: {sourceLocatorValue}";
                }

                var targetElement = FindElementWithTimeout(targetLocatorStrategy, targetLocatorValue, timeoutSeconds);
                if (targetElement == null)
                {
                    return $"Target element not found using {targetLocatorStrategy}: {targetLocatorValue}";
                }

                // Execute native drag and drop command
                _driver.ExecuteScript("mobile: dragAndDrop", new Dictionary<string, object>
                {
                    ["sourceElement"] = sourceElement,
                    ["destinationElement"] = targetElement
                });

                return
                    $"Successfully performed native drag and drop from {sourceLocatorStrategy}={sourceLocatorValue} to {targetLocatorStrategy}={targetLocatorValue}";
            }
            catch (Exception ex)
            {
                return $"Failed to perform native drag and drop: {ex.Message}.";
            }
        }
    }
}