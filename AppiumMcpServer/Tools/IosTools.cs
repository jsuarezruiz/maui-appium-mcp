using AppiumMcpServer.Models;
using MobileDevMcpServer.Helpers;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace AppiumMcpServer.Tools
{
    /// <summary>
    /// Provides iOS-specific device management tools for interacting with iOS Simulator devices.
    /// Uses Xcode's simctl command-line tool to manage simulator lifecycle, device discovery, and state management.
    /// Requires Xcode and Xcode Command Line Tools to be installed on macOS.
    /// </summary>
    /// <remarks>
    /// This class is designed to work exclusively with iOS Simulator devices and requires:
    /// - macOS operating system
    /// - Xcode installed with Command Line Tools
    /// - xcrun and simctl tools available in PATH
    /// </remarks>
    [McpServerToolType]
    public class IosTools
    {
        /// <summary>
        /// Retrieves and lists all available simulator devices.
        /// </summary>
        /// <returns>
        /// A string formatted as a table, containing details of simulator devices such as name, ID, and runtime.
        /// If no devices are found or an error occurs, an appropriate message is returned.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when an error occurs during the device retrieval process.
        /// </exception>
        [McpServerTool(Name = "ios_list_devices")]
        [Description("Lists all available iOS simulator devices.")]
        public string ListDevices()
        {
            try
            {
                string resultJson = ProcessHelper.ExecuteCommand("xcrun simctl list devices --json");

                // Deserialize JSON data into SimulatorDevices object
                var result = JsonSerializer.Deserialize<SimulatorDevices>(resultJson);

                if (result?.Devices is null || result.Devices.Count == 0)
                {
                    return "No simulator devices available.";
                }

                // Prepare table header
                var devicesTable = new StringBuilder("# Simulator Devices\n\n");
                devicesTable.AppendLine("| Name             | Udid                | Runtime       |");
                devicesTable.AppendLine("|------------------|---------------------|---------------|");

                // Process devices and format table rows
                foreach (var runtime in result.Devices)
                {
                    string runtimeName = runtime.Key.Replace("com.apple.CoreSimulator.SimRuntime.", string.Empty);

                    foreach (var device in runtime.Value)
                    {
                        device.Runtime = runtimeName;
                        devicesTable.AppendLine($"| {device.Name,-16} | {device.Udid,-20} | {runtimeName,-13} |");
                    }
                }

                return devicesTable.ToString();
            }
            catch (JsonException jsonEx)
            {
                return $"Error parsing simulator devices: {jsonEx.Message}";
            }
            catch (Exception ex)
            {
                return $"Error retrieving simulator devices: {ex.Message}";
            }
        }

        /// <summary>
        /// Retrieves the name and ID of the first booted simulator device.
        /// </summary>
        /// <returns>
        /// The name and ID of the booted simulator device in a formatted string.
        /// If no booted simulator is found, an exception is thrown.
        /// </returns>
        [McpServerTool(Name = "ios_booted_device")]
        [Description("Retrieves the name and ID of the first booted simulator device.")]
        public static string GetBootedDevice()
        {
            try
            {
                string resultJson = ProcessHelper.ExecuteCommand("xcrun simctl list devices --json");

                // Deserialize JSON data into SimulatorDevices object
                var result = JsonSerializer.Deserialize<SimulatorDevices>(resultJson);

                if (result?.Devices is null || result.Devices.Count == 0)
                {
                    return "No simulator devices available.";
                }

                // Process devices and format table rows
                foreach (var runtime in result.Devices)
                {
                    string runtimeName = runtime.Key.Replace("com.apple.CoreSimulator.SimRuntime.", string.Empty);

                    foreach (var device in runtime.Value)
                    {
                        if (device.State == "Booted")
                        {
                            // Format the result as a table
                            var deviceStr = "# Booted Device\n\n";
                            deviceStr += "| Name            | Udid              |\n";
                            deviceStr += "|-----------------|-----------------|\n";
                            deviceStr += $"| {device.Name} | {device.Udid} |\n";

                            return deviceStr;
                        }
                    }
                }

                return "No booted devices found.";
            }
            catch (Exception ex)
            {
                return $"Error retrieving booted device: {ex.Message}";
            }
        }

        /// <summary>
        /// Boots a simulator device with the specified device ID.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the simulator device to be booted.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the provided <paramref name="deviceId"/> is null or empty.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when an error occurs during the boot operation.
        /// </exception>
        [McpServerTool(Name = "ios_boot_device")]
        [Description("Boots the specified iOS simulator device.")]
        public void BootDevice(string deviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    throw new ArgumentNullException(nameof(deviceId), "Error: Invalid or missing device ID.");
                }

                // Execute the command to boot the simulator device
                ProcessHelper.ExecuteCommand($"xcrun simctl boot {deviceId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error booting the simulator device: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Shuts down a simulator device with the specified device ID.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the simulator device to be shut down.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the provided <paramref name="deviceId"/> is null or empty.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when an error occurs during the shutdown operation.
        /// </exception>
        [McpServerTool(Name = "ios_shutdown_device")]
        [Description("Shuts down the specified iOS simulator device.")]
        public void ShutdownDevice(string deviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    throw new ArgumentNullException(nameof(deviceId), "Error: Invalid or missing device ID.");
                }

                // Execute the command to shut down the simulator device
                ProcessHelper.ExecuteCommand($"xcrun simctl shutdown {deviceId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error shutting down the simulator device: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves the list of installed application bundle identifiers from the specified iOS simulator device.
        /// Parses the plist format output from simctl and returns applications in a markdown table format.
        /// </summary>
        /// <param name="deviceId">
        /// The unique device identifier (UDID) of the iOS simulator to query.
        /// This should be the full UDID string as returned by ListDevices().
        /// Example format: "ABC12345-DEF6-7890-GHIJ-KLMNOPQRSTUV"
        /// </param>
        /// <param name="userAppsOnly">
        /// If true, returns only user-installed applications (excludes system apps like Safari, Settings, etc.).
        /// If false, returns all applications including system apps. Default is true.
        /// </param>
        /// <returns>
        /// A formatted markdown table containing application bundle identifiers and display names.
        /// Returns "No applications found on the device." if no apps match the criteria.
        /// Returns error message if the device ID is invalid or command execution fails.
        /// </returns>
        /// <remarks>
        /// This method requires:
        /// - macOS with Xcode installed
        /// - The specified simulator device to exist (use ListDevices() to get valid device IDs)
        /// - The device does not need to be booted to list applications
        /// 
        /// System applications are always present on iOS simulators and include built-in apps.
        /// User applications are those installed via Xcode, App Store, or development builds.
        /// 
        /// The bundle identifier format follows reverse domain notation (e.g., com.company.appname).
        /// 
        /// This method parses the plist format output from simctl, which uses "=" syntax instead of JSON.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when deviceId parameter is null, empty, or consists only of whitespace.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown when simctl command execution fails or device ID is invalid.
        /// </exception>
        /// <example>
        /// Example output format:
        /// <code>
        /// # Installed Applications
        /// 
        /// | Bundle Identifier           | Display Name    |
        /// |-----------------------------|-----------------|
        /// | com.companyname.WhatToEat   | WhatToEat       |
        /// | com.companyname.TestApp     | Test App        |
        /// | com.example.demoapp         | Demo Application|
        /// </code>
        /// </example>
        [McpServerTool(Name = "ios_list_packages")]
        [Description("Retrieves the list of installed application bundle identifiers from the specified iOS simulator device.")]
        public string ListApps(string deviceId, bool userAppsOnly = true)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    throw new ArgumentNullException(nameof(deviceId), "Error: Invalid or missing device ID.");
                }

                // Execute simctl command (returns plist format, not JSON)
                string plistResult = ProcessHelper.ExecuteCommand($"xcrun simctl listapps {deviceId}");

                return ParsePlistApps(plistResult, userAppsOnly);
            }
            catch (Exception ex)
            {
                return $"Error retrieving applications: {ex.Message}";
            }
        }

        /// <summary>
        /// Parses plist format output from simctl listapps command.
        /// Handles the property list format with "=" syntax used by iOS simulators.
        /// </summary>
        /// <param name="plistOutput">Raw plist text output from simctl command</param>
        /// <param name="userAppsOnly">Whether to filter out system applications</param>
        /// <returns>Formatted markdown table of applications</returns>
        private string ParsePlistApps(string plistOutput, bool userAppsOnly)
        {
            try
            {
                var applications = new List<(string BundleId, string DisplayName)>();
                string[] lines = plistOutput.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

                string currentBundleId = "";
                string currentDisplayName = "";
                string currentAppType = "";
                bool inAppSection = false;
                int braceDepth = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();

                    // Skip empty lines
                    if (string.IsNullOrEmpty(line))
                        continue;

                    // Check if this line contains a bundle identifier (top-level key)
                    // Format: "com.apple.Safari" = {
                    if (line.Contains("\" =") && line.Contains("{") && line.StartsWith("\""))
                    {
                        // Extract bundle ID from quoted string
                        int firstQuote = line.IndexOf('"');
                        int secondQuote = line.IndexOf('"', firstQuote + 1);
                        if (firstQuote >= 0 && secondQuote > firstQuote)
                        {
                            currentBundleId = line.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                            currentDisplayName = currentBundleId; // Default to bundle ID
                            currentAppType = "";
                            inAppSection = true;
                            braceDepth = 1;
                        }
                        continue;
                    }

                    if (inAppSection)
                    {
                        // Count braces to track nested structures
                        braceDepth += line.Count(c => c == '{');
                        braceDepth -= line.Count(c => c == '}');

                        // Only process top-level properties (braceDepth == 1)
                        if (braceDepth == 1)
                        {
                            // Look for ApplicationType = System; or ApplicationType = User;
                            if (line.Contains("ApplicationType = "))
                            {
                                string[] parts = line.Split('=');
                                if (parts.Length > 1)
                                {
                                    currentAppType = parts[1].Trim().TrimEnd(';').Trim();
                                }
                            }

                            // Look for CFBundleDisplayName = AppName;
                            else if (line.Contains("CFBundleDisplayName = "))
                            {
                                string[] parts = line.Split('=');
                                if (parts.Length > 1)
                                {
                                    currentDisplayName = parts[1].Trim().TrimEnd(';').Trim();
                                }
                            }

                            // Look for CFBundleName as fallback
                            else if (line.Contains("CFBundleName = ") && currentDisplayName == currentBundleId)
                            {
                                string[] parts = line.Split('=');
                                if (parts.Length > 1)
                                {
                                    currentDisplayName = parts[1].Trim().TrimEnd(';').Trim();
                                }
                            }
                        }

                        // Check if we're at the end of this app's section
                        if (braceDepth == 0)
                        {
                            inAppSection = false;

                            // Apply filtering
                            if (userAppsOnly && currentAppType.Equals("System", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            // Validate bundle ID format and add to list
                            if (!string.IsNullOrEmpty(currentBundleId) && currentBundleId.Contains("."))
                            {
                                applications.Add((currentBundleId, currentDisplayName));
                            }

                            // Reset for next app
                            currentBundleId = "";
                            currentDisplayName = "";
                            currentAppType = "";
                        }
                    }
                }

                if (applications.Count == 0)
                {
                    string appTypeText = userAppsOnly ? "user applications" : "applications";
                    return $"No {appTypeText} found on the device.";
                }

                // Sort applications by display name for better readability
                applications.Sort((x, y) => string.Compare(x.DisplayName, y.DisplayName, StringComparison.OrdinalIgnoreCase));

                // Format the result as a markdown table
                var applicationsStr = "# Installed Applications\n\n";
                applicationsStr += "| Bundle Identifier           | Display Name    |\n";
                applicationsStr += "|-----------------------------|---------------|\n";

                foreach (var app in applications)
                {
                    applicationsStr += $"| `{app.BundleId}` | {app.DisplayName} |\n";
                }

                return applicationsStr;
            }
            catch (Exception ex)
            {
                return $"Error parsing plist applications: {ex.Message}";
            }
        }
    }
}
