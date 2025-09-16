using System.ComponentModel;
using ModelContextProtocol.Server;

namespace AppiumMcpServer.Tools
{
    public partial class AppiumTools
    {
        /// <summary>
        /// Generates UI test code from MCP tool calls for easy copy-paste into test methods.
        /// </summary>
        /// <param name="toolName">Name of the MCP tool to generate code for</param>
        /// <param name="parameters">Comma-separated parameters for the tool call</param>
        /// <returns>Generated C# code line</returns>
        [McpServerTool(Name = "appium_generate_code")]
        [Description("Generate C# UI test code from MCP tool calls. Format: toolName, param1, param2, ...")]
        public string GenerateCode(string toolName, string parameters = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toolName))
                {
                    return "Tool name cannot be null or empty";
                }

                var paramList = string.IsNullOrEmpty(parameters)
                    ? new string[0]
                    : parameters.Split(',', StringSplitOptions.RemoveEmptyEntries);

                // Clean parameters (trim whitespace)
                for (int i = 0; i < paramList.Length; i++)
                {
                    paramList[i] = paramList[i].Trim();
                }

                return toolName.ToLower() switch
                {
                    // Basic interactions
                    "appium_tap" or "tap" => GenerateTapCode(paramList),
                    "appium_enter_text" or "enter_text" => GenerateEnterTextCode(paramList),
                    "appium_clear_text" or "clear_text" => GenerateClearTextCode(paramList),
                    "appium_send_keys" or "send_keys" => GenerateSendKeysCode(paramList),

                    // Element queries
                    "appium_find_element" or "find_element" => GenerateFindElementCode(paramList),
                    "appium_wait_for_element" or "wait_for_element" => GenerateWaitForElementCode(paramList),
                    "appium_wait_for_no_element" or "wait_for_no_element" => GenerateWaitForNoElementCode(paramList),

                    // Gestures
                    "appium_swipe_left_to_right" or "swipe_left_to_right" => GenerateSwipeCode("SwipeLeftToRight",
                        paramList),
                    "appium_swipe_right_to_left" or "swipe_right_to_left" => GenerateSwipeCode("SwipeRightToLeft",
                        paramList),
                    "appium_scroll_down" or "scroll_down" => GenerateScrollCode("ScrollDown", paramList),
                    "appium_scroll_up" or "scroll_up" => GenerateScrollCode("ScrollUp", paramList),

                    // Hardware keys
                    "appium_press_enter" or "press_enter" => "App.PressEnter();",
                    "appium_press_back" or "press_back" => "App.Back();",

                    // Keyboard
                    "appium_dismiss_keyboard" or "dismiss_keyboard" => "App.DismissKeyboard();",
                    "appium_is_keyboard_shown" or "is_keyboard_shown" => "var isKeyboardShown = App.IsKeyboardShown();",

                    // Orientation
                    "appium_set_orientation_landscape" or "set_orientation_landscape" =>
                        "App.SetOrientationLandscape();",
                    "appium_set_orientation_portrait" or "set_orientation_portrait" => "App.SetOrientationPortrait();",
                    "appium_get_orientation" or "get_orientation" => "var orientation = App.GetOrientation();",

                    // Clipboard
                    "appium_get_clipboard_text" or "get_clipboard_text" =>
                        "var clipboardText = App.GetClipboardText();",
                    "appium_set_clipboard_text" or "set_clipboard_text" => GenerateSetClipboardCode(paramList),

                    // App management
                    "appium_activate_app" or "activate_app" => GenerateActivateAppCode(paramList),
                    "appium_terminate_app" or "terminate_app" => GenerateTerminateAppCode(paramList),

                    // Navigation
                    "appium_navigate_back" or "navigate_back" => "App.Back();",

                    _ => $"// Unknown tool: {toolName}"
                };
            }
            catch (Exception ex)
            {
                return $"// Error generating code: {ex.Message}";
            }
        }

        string GenerateTapCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing element ID";

            return $"App.Tap(\"{parameters[0]}\");";
        }

        string GenerateEnterTextCode(string[] parameters)
        {
            if (parameters.Length < 2)
                return string.Empty; // Missing parameters";

            return $"App.EnterText(\"{parameters[0]}\", \"{parameters[1]}\");";
        }

        string GenerateClearTextCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing element ID";

            return $"App.ClearText(\"{parameters[0]}\");";
        }

        string GenerateSendKeysCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing text parameter";

            if (parameters.Length == 1)
                return
                    $"// Send keys to active element\nvar activeElement = App.WaitForElement(() => App.Driver.SwitchTo().ActiveElement());\nactiveElement.SendKeys(\"{parameters[0]}\");";

            return $"App.FindElement(\"{parameters[1]}\").SendKeys(\"{parameters[0]}\");";
        }

        string GenerateFindElementCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing element ID";

            return $"var element = App.FindElement(\"{parameters[0]}\");";
        } 
        
        string GenerateWaitForElementCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing element ID";
            
            if (parameters.Length > 1 && int.TryParse(parameters[1], out int timeout))
                return $"App.WaitForElement(\"{parameters[0]}\", timeout: TimeSpan.FromSeconds({timeout}));";
            
            return $"App.WaitForElement(\"{parameters[0]}\");";
        }

        string GenerateWaitForNoElementCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing element ID";
            
            return $"App.WaitForNoElement(\"{parameters[0]}\");";
        }

        string GenerateSwipeCode(string methodName, string[] parameters)
        {
            if (parameters.Length == 0)
                return $"App.{methodName}();";
            
            if (parameters.Length == 1)
                return $"App.{methodName}(\"{parameters[0]}\");";
            
            // With additional parameters like swipePercentage
            return $"App.{methodName}(\"{parameters[0]}\", swipePercentage: {parameters[1]});";
        }
        
        string GenerateScrollCode(string methodName, string[] parameters)
        {
            if (parameters.Length == 0)
                return $"App.{methodName}();";
            
            return $"App.{methodName}(\"{parameters[0]}\");";
        }
        
        string GenerateSetClipboardCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing text parameter";
            
            if (parameters.Length == 1)
                return $"App.SetClipboardText(\"{parameters[0]}\");";
            
            return $"App.SetClipboardText(\"{parameters[0]}\", \"{parameters[1]}\");";
        }
        
        string GenerateActivateAppCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return string.Empty; // Missing app identifier";
            
            return $"App.LaunchApp(); // Note: Use config for app: {parameters[0]}";
        }

        string GenerateTerminateAppCode(string[] parameters)
        {
            if (parameters.Length == 0)
                return "App.CloseApp();";
            
            return $"App.CloseApp(); // Note: Terminating app: {parameters[0]}";
        }

        /// <summary>
        /// Generates a complete test method from a sequence of MCP tool calls.
        /// </summary>
        /// <param name="testName">Name of the test method</param>
        /// <param name="toolSequence">Semicolon-separated sequence of tool calls in format: toolName(param1,param2);toolName2(param3)</param>
        /// <returns>Complete C# test method</returns>
        [McpServerTool(Name = "appium_generate_test_method")]
        [Description("Generate complete test method from tool sequence. Format: testName;toolName(param1,param2);toolName2(param3)")]
        public string GenerateTestMethod(string testName, string toolSequence)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(testName))
                {
                    return "Test name cannot be null or empty";
                }

                if (string.IsNullOrWhiteSpace(toolSequence))
                {
                    return "Tool sequence cannot be null or empty";
                }

                var result = $@"[Test]
public void {testName}()
{{

";

                var toolCalls = toolSequence.Split(';', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var toolCall in toolCalls)
                {
                    var trimmed = toolCall.Trim();
                    if (string.IsNullOrEmpty(trimmed)) continue;

                    // Parse toolName(param1,param2) format
                    var openParen = trimmed.IndexOf('(');
                    var closeParen = trimmed.LastIndexOf(')');
                    
                    string toolName;
                    string parameters = "";
                    
                    if (openParen > 0 && closeParen > openParen)
                    {
                        toolName = trimmed.Substring(0, openParen);
                        parameters = trimmed.Substring(openParen + 1, closeParen - openParen - 1);
                    }
                    else
                    {
                        toolName = trimmed;
                    }
                    
                    var codeString = GenerateCode(toolName, parameters);
                    result += $"    {codeString}\n";
                }

                result += "}";
                return result;
            }
            catch (Exception ex)
            {
                return $"Error generating test method: {ex.Message}";
            }
        }
    }
}