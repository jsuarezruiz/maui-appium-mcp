using AppiumMcpServer.Helpers;
using AppiumMcpServer.Models;
using MobileDevMcpServer.Helpers;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Diagnostics;

namespace AppiumMcpServer.Tools
{
    /// <summary>
    /// Provides Android-specific device management tools for interacting with physical devices 
    /// and Android Virtual Devices (AVDs) through ADB (Android Debug Bridge).
    /// Supports device discovery, emulator lifecycle management, and device operations.
    /// </summary>
    [McpServerToolType]
    public class AndroidTools
    {
        /// <summary>
        /// Retrieves a list of connected Android devices.
        /// </summary>
        /// <returns>
        /// A string containing the list of connected devices and their details, such as serial numbers.
        /// </returns>
        /// <remarks>
        /// This method requires ADB (Android Debug Bridge) to be installed and accessible in the system PATH.
        /// The returned information includes both physical devices and running emulator instances.
        /// </remarks>
        /// <example>
        /// Example output format:
        /// <code>
        /// # Devices
        /// 
        /// | Serial          | Device           | Product          | Model            |
        /// |-----------------|------------------|------------------|------------------|
        /// | emulator-5554   | generic_x86_64   | sdk_gphone64_x86_64 | AOSP on x86_64 |
        /// | ABC123DEF456    | sailfish         | sailfish         | Pixel            |
        /// </code>
        /// </example>
        [McpServerTool(Name = "android_list_devices")]
        [Description("Lists all available Android devices.")]
        public string ListDevices()
        {
            try
            {
                if (!AdbHelper.CheckAdbInstalled())
                {
                    throw new Exception("ADB is not installed or not in PATH. Please install ADB and ensure it is in your PATH.");
                }

                var devices = new List<AdbDevice>();
                string result = ProcessHelper.ExecuteCommand("adb devices -l");

                string[] lines = result.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

                // Skip the first line (header)
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        // Parse each line to extract device details
                        string[] parts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                        var device = new AdbDevice
                        {
                            Product = GetPropertyFromParts(parts, "product:"),
                            Model = GetPropertyFromParts(parts, "model:"),
                            Device = GetPropertyFromParts(parts, "device:"),
                            SerialNumber = parts[0], // Assuming the serial number is the first part
                        };
                        devices.Add(device);
                    }
                }

                if (devices is null || devices.Count == 0)
                {
                    return "No devices found.";
                }

                // Format the result as a table
                var devicesStr = "# Devices\n\n";
                devicesStr += "| Serial          | Device           | Product          | Model            |\n";
                devicesStr += "|-----------------|------------------|------------------|------------------|\n";

                foreach (var device in devices)
                {
                    devicesStr += $"| `{device.SerialNumber}` | `{device.Device}` | `{device.Product}` | `{device.Model}` |\n";
                }

                return devicesStr;
            }
            catch (Exception ex)
            {
                return $"Error retrieving device list: {ex.Message}";
            }
        }

        /// <summary>
        /// Boots up an Android Virtual Device (AVD) emulator with the specified name.
        /// </summary>
        /// <param name="avdName">The name of the Android Virtual Device (AVD) to be booted.</param>
        [McpServerTool(Name = "android_boot_device")]
        [Description("Boots the specified Android device.")]
        public void BootDevice(string avdName)
        {
            try
            {
                if (string.IsNullOrEmpty(avdName))
                {
                    throw new ArgumentNullException(nameof(avdName), "Error: Device name is missing or invalid.");
                }

                // Execute the adb command to kill the emulator
                ProcessHelper.ExecuteCommand($"adb -s {avdName} emu kill");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error booting the device: {ex.Message}");
            }
        }

        /// <summary>
        /// Shuts down an Android Virtual Device (AVD) emulator with the specified name.
        /// </summary>
        /// <param name="avdName">The name of the Android Virtual Device (AVD) to be shut down.</param>
        [McpServerTool(Name = "android_shutdown_device")]
        [Description("Shuts down the specified Android device.")]
        public void ShutdownDevice(string avdName)
        {
            try
            {
                if (!AdbHelper.CheckAdbInstalled())
                {
                    throw new Exception("ADB is not installed or not in PATH. Please install ADB and ensure it is in your PATH.");
                }

                if (string.IsNullOrEmpty(avdName))
                {
                    throw new ArgumentNullException(nameof(avdName), "Error: Device name is missing or invalid.");
                }

                // Execute the adb command to kill the emulator
                ProcessHelper.ExecuteCommand($"adb -s {avdName} emu kill");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error shutting down the device: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a list of installed application packages from the specified Android device.
        /// </summary>
        /// <param name="deviceSerial">The serial number of the target Android device.</param>
        /// <returns>
        /// A string containing the list of installed application packages.
        /// </returns>
        [McpServerTool(Name = "android_list_packages")]
        [Description("Retrieves the list of installed package names from the specified Android device.")]
        public string ListPackages(string deviceSerial)
        {
            try
            {
                if (!AdbHelper.CheckAdbInstalled())
                {
                    throw new Exception("ADB is not installed or not in PATH. Please install ADB and ensure it is in your PATH.");
                }

                if (string.IsNullOrEmpty(deviceSerial))
                {
                    return $"Error: Invalid or missing device serial number.";
                }

                var packages = new List<string>();

                string result = ProcessHelper.ExecuteCommand($"adb -s {deviceSerial} shell pm list packages");

                string[] lines = result.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    if (line.StartsWith("package:"))
                    {
                        // Remove the "package:" prefix and add the package name to the list
                        string packageName = line.Replace("package:", "").Trim();
                        packages.Add(packageName);
                    }
                }

                if (packages is null || packages.Count == 0)
                {
                    return "No packages found on the device.";
                }

                // Format the result as a table
                var packagesStr = "# Installed Packages\n\n";
                packagesStr += "| Package Name |\n";
                packagesStr += "|--------------|\n";

                foreach (var app in packages)
                {
                    packagesStr += $"| `{app}` |\n";
                }

                return packagesStr;
            }
            catch (Exception ex)
            {
                return $"Error retrieving packages: {ex.Message}";
            }
        }

        // Extracts the value of a specific property from an array of strings.
        string GetPropertyFromParts(string[] parts, string propertyKey)
        {
            foreach (var part in parts)
            {
                if (part.StartsWith(propertyKey, StringComparison.OrdinalIgnoreCase))
                {
                    return part.Substring(propertyKey.Length);
                }
            }

            return string.Empty;
        }
    }
}
