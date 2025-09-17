# MAUI Appium MCP Server

A Model Context Protocol (MCP) server that provides AI-powered mobile automation capabilities for .NET MAUI applications. This server enables tools like Visual Studio Code or Claude to perform intelligent mobile testing workflows with comprehensive device management and testing capabilities.

## Overview

The MAUI Appium MCP Server bridges AI agents with mobile automation through Appium WebDriver. It provides specialized tools for Android and iOS device management, comprehensive UI testing capabilities, and includes extensive MAUI testing guidelines to ensure best practices.

## Features

### Core Capabilities
- **Cross-Platform Mobile Automation**: Support for Android, iOS, Mac and Windows
- **AI-Powered Testing**: Built for agents using Model Context Protocol
- **Device Management**: Complete Android and iOS device lifecycle management
- **Appium Integration**: Full Appium support
- **Code Generation**: Auto-generate C# test code from MCP tool calls

### Device Support
- **Android Devices**: Physical devices and emulators via ADB
- **iOS Devices**: Physical devices and simulators via idb (iOS Device Bridge)
- **Device Discovery**: Automatic detection of connected devices
- **App Management**: Install, uninstall, and manage applications

### Testing Tools
- **Element Interactions**: Tap, swipe, scroll, text input
- **Advanced Gestures**: Multi-touch, drag & drop, pinch & zoom
- **Device Controls**: Orientation, keyboard, navigation
- **Screenshot Capture**: Visual verification and debugging
- **Wait Strategies**: Intelligent element waiting and synchronization

### MAUI Testing Knowledge Base
- **Comprehensive Guidelines**: Complete MAUI UI testing best practices
- **Test Structure**: Host app and test class patterns
- **Naming Conventions**: Consistent naming across tests
- **Performance Optimization**: Best practices for test efficiency
- **Debugging Support**: Troubleshooting and common issues

## Prerequisites

### System Requirements
- **.NET 9.0 SDK** or later
- **Visual Studio 2022 17.14+** or Visual Studio Code
- **Node.js 18+** (for Appium)
- **Java Development Kit (JDK) 8+**

### Platform-Specific Requirements

#### Android Testing
- **Android SDK** with platform tools
- **ADB (Android Debug Bridge)** in system PATH
- Android emulator or physical device with USB debugging enabled

#### iOS Testing (macOS only)
- **Xcode** with iOS Simulator
- **idb (iOS Device Bridge)** installed
- iOS device with developer profile (for physical device testing)

### Environment Setup

Ensure your system environment includes the necessary variables:

```bash
# Required environment variables
export JAVA_HOME=/path/to/your/java
export ANDROID_HOME=/path/to/your/android/sdk
export PATH=$PATH:$ANDROID_HOME/tools:$ANDROID_HOME/platform-tools
```

### Appium Installation

```bash
# Install Appium globally
npm install -g appium

# Install required drivers
appium driver install uiautomator2  # For Android
appium driver install xcuitest      # For iOS

# Verify installation
appium doctor --android  # Check Android setup
appium doctor --ios      # Check iOS setup
```

## Installation & Setup

### 1. Clone and Build
```bash
git clone https://github.com/PureWeen/maui-appium-mcp.git
cd maui-appium-mcp/AppiumMcpServer

# Restore and build
dotnet restore
dotnet build
```

### 2. Run the MCP Server
```bash
# Start with stdio transport (default)
dotnet run

# Or run the built executable
./bin/Debug/net9.0/AppiumMcpServer
```

### 3. Configure with Claude

Add to your Claude MCP configuration file:

**Visual Studio Code**:
```json
{
  "mcpServers": {
    "maui-appium-server": {
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/AppiumMcpServer"],
      "cwd": "/path/to/maui-appium-mcp/AppiumMcpServer"
    }
  }
}
```

## Available Tools

### Android Device Management

| Tool | Description | Usage |
|------|-------------|-------|
| `android_list_devices` | List connected Android devices | Get device serial numbers |
| `android_boot_device` | Boot Android emulator | Start AVD by name |
| `android_shutdown_device` | Shutdown Android emulator | Stop running AVD |
| `android_list_packages` | List installed apps | Get package names |
| `android_screenshot` | Capture device screenshot | Visual verification |

### Appium Core Operations

| Tool | Description | Parameters |
|------|-------------|------------|
| `appium_connect_app` | Connect to app | platform, deviceName, appPackage/bundleId |
| `appium_disconnect` | Disconnect session | - |
| `appium_tap` | Tap element | locatorStrategy, locatorValue |
| `appium_enter_text` | Send text to element | locatorStrategy, locatorValue, text |
| `appium_find_element` | Find UI element | locatorStrategy, locatorValue |
| `appium_wait_for_element` | Wait for element | locatorStrategy, locatorValue, timeout |

### Advanced Gestures

| Tool | Description | Parameters |
|------|-------------|------------|
| `appium_swipe_left_to_right` | Swipe left to right | locatorStrategy, locatorValue, swipePercentage |
| `appium_swipe_right_to_left` | Swipe right to left | locatorStrategy, locatorValue, swipePercentage |
| `appium_scroll_down` | Scroll down | locatorStrategy, locatorValue |
| `appium_scroll_up` | Scroll up | locatorStrategy, locatorValue |
| `appium_scroll_to` | Scroll to target element | targetLocator, containerLocator, direction |

### Device Controls

| Tool | Description |
|------|-------------|
| `appium_set_orientation_landscape` | Set landscape mode |
| `appium_set_orientation_portrait` | Set portrait mode |
| `appium_press_enter` | Press Enter key |
| `appium_dismiss_keyboard` | Hide soft keyboard |
| `appium_navigate_back` | Navigate back |

### App Management

| Tool | Description | Parameters |
|------|-------------|------------|
| `appium_install_app` | Install app | appPath |
| `appium_uninstall_app` | Uninstall app | bundleId/packageName |
| `appium_is_app_installed` | Check if app is installed | bundleId/packageName |

### Code Generation

| Tool | Description | Parameters |
|------|-------------|------------|
| `appium_generate_code` | Generate C# test code | toolName, parameters |
| `appium_generate_test_method` | Generate complete test method | testName, toolSequence |

## Knowledge Base Resources

The server includes comprehensive MAUI testing guidelines accessible as MCP resources:

### Available Resources
- **Overview**: Key principles and file structure
- **Structure Guidelines**: Host app and test class patterns
- **Naming Guidelines**: Consistent naming conventions
- **Inheritance Guidelines**: Base class usage patterns
- **Assertion Guidelines**: VerifyScreenshot and validation strategies
- **Wait Guidelines**: Timing and synchronization best practices
- **Category Guidelines**: Test categorization standards
- **AutomationId Guidelines**: Element identification patterns
- **Pattern Guidelines**: Best practices and anti-patterns
- **Performance Guidelines**: Optimization techniques
- **Debugging Guidelines**: Troubleshooting and common issues
- **Basic Test Template**: Simple test examples
- **Complex Test Template**: Advanced testing patterns

## Project Structure

```
AppiumMcpServer/
├── AppiumMcpServer.csproj          # Project file with dependencies
├── AppiumMcpServer.sln             # Solution file
├── Program.cs                      # Entry point and MCP server setup
├── Helpers/
│   ├── AdbHelper.cs               # Android ADB integration
│   ├── IdbHelper.cs               # iOS device bridge integration
│   └── ProcessHelper.cs           # Cross-platform process execution
├── Models/
│   ├── AdbDevice.cs               # Android device representation
│   └── SimulatorDevices.cs        # iOS simulator models
├── Resources/
│   └── MauiTestGuidelinesResource.cs  # Comprehensive testing knowledge
└── Tools/
    ├── AndroidTools.cs            # Android-specific operations
    ├── AppiumTools.cs             # Core Appium operations
    ├── AppiumTools.Code.cs        # Code generation utilities
    ├── AppiumTools.DragDrop.cs    # Drag and Drop operations
    ├── AppiumTools.Device.cs      # Device management
    ├── AppiumTools.Scroll.cs      # Scrolling operations
    ├── AppiumTools.Swipe.cs       # Swipe gestures
    └── AppiumTools.Text.cs        # Text input operations
```

## Dependencies

- **Appium.WebDriver** - Core Appium integration
- **Microsoft.Extensions.Hosting** - Application hosting
- **ModelContextProtocol** - MCP server implementation

## Troubleshooting

### Common Issues

#### ADB Not Found
```bash
# Ensure ADB is in PATH
which adb  # Linux/macOS
where adb  # Windows

# Add Android SDK platform-tools to PATH
export PATH=$PATH:$ANDROID_HOME/platform-tools
```

#### Device Not Detected
```bash
# Enable USB debugging on device
# Check device connection
adb devices

# Restart ADB server if needed
adb kill-server
adb start-server
```

#### Appium Connection Issues
```bash
# Verify Appium installation
appium doctor

# Start Appium server manually for debugging
appium --log-level debug
```

## Contributing

This project provides a comprehensive foundation for AI-powered mobile testing. Contributions are welcome for:

- Additional platform support
- Enhanced gesture recognition
- Extended device management capabilities
- Improved testing patterns and guidelines