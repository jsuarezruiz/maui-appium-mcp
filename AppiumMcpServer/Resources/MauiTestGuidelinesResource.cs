using System.ComponentModel;
using ModelContextProtocol.Server;

namespace AppiumMcpServer.Resources
{
    /// <summary>
    /// MAUI Testing Guidelines Knowledge Base
    /// </summary>
    [McpServerResourceType]
    public static class MauiTestGuidelinesResource
    {
        [McpServerResource]
        [Description("MAUI UI Test Guidelines Overview: Key principles, file structure, and available resources")]
        public static async Task<string> GetOverview()
        {
            return await Task.FromResult(@"# MAUI UI Test Guidelines Overview

## Key Principles
- Inherit from _IssuesUITest for issue-specific tests
- Use descriptive test and method names
- Always include AutomationId on testable elements
- Use appropriate wait strategies (WaitForElement)
- Leverage VerifyScreenshot() for visual validation
- Add proper test categories
- Write maintainable, readable test code

## File Structure
- **HostApp/Issues/** - Contains the actual issue reproduction page
- **Shared.Tests/Tests/Issues/** - Contains the UI test class  
- **Platform.Tests/snapshots/** - Contains expected screenshot results

## Essential Attributes
```csharp
[Issue(IssueTracker.Github, issueNumber, ""description"", PlatformAffected)]
[Test]
[Category(UITestCategories.ControlName)]
```

## Available Resources
Use these resource methods to get detailed guidance:
- GetStructureGuidelines() - Test class structure
- GetNamingGuidelines() - Naming conventions
- GetInheritanceGuidelines() - Base class usage
- GetAssertionGuidelines() - Validation strategies
- GetWaitGuidelines() - Timing and wait patterns
- GetCategoryGuidelines() - Test categorization
- GetAutomationIdGuidelines() - Element identification
- GetPatternGuidelines() - Best practices and anti-patterns
- GetPerformanceGuidelines() - Performance optimization
- GetDebuggingGuidelines() - Troubleshooting guide
- GetBasicTestTemplate() - Basic test template
- GetComplexTestTemplate() - Advanced test patterns");
        }

        [McpServerResource]
        [Description("MAUI Test Structure Guidelines: Host app and test class structure with examples")]
        public static async Task<string> GetStructureGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Structure Guidelines

## 1. HOST APP STRUCTURE (TestCases.HostApp/Issues/IssueXXXXX.cs)

### Required Elements
- Inherit from ContentPage or TestContentPage
- Include [Issue] attribute with tracker, number, description, platform
- Use meaningful AutomationId on interactive elements
- Keep UI simple and focused on the specific issue
- Override Init() method for TestContentPage

### Example
```csharp
[Issue(IssueTracker.Github, 12345, ""Button click issue"", PlatformAffected.All)]
public class Issue12345 : TestContentPage
{
    protected override void Init()
    {
        Content = new StackLayout
        {
            Children = 
            {
                new Button 
                { 
                    Text = ""Click Me"", 
                    AutomationId = ""TestButton""
                }
            }
        };
    }
}
```

## 2. TEST CLASS STRUCTURE (Shared.Tests/Tests/Issues/IssueXXXXX.cs)

### Required Elements
- Inherit from _IssuesUITest
- Constructor with TestDevice parameter
- Override Issue property
- [Test] methods with descriptive names
- Appropriate [Category] attributes

### Key Methods
- **Constructor**: Pass testDevice to base
- **Issue property**: Return descriptive string
- **Test methods**: Focus on specific scenarios

### Example
```csharp
public class Issue12345 : _IssuesUITest
{
    public Issue12345(TestDevice testDevice) : base(testDevice) { }
    
    public override string Issue => ""Button click issue"";
    
    [Test]
    [Category(UITestCategories.Button)]
    public void ButtonClickTriggersExpectedBehavior()
    {
        App.WaitForElement(""TestButton"");
        App.Tap(""TestButton"");
        VerifyScreenshot();
    }
}
```");
        }

        [McpServerResource]
        [Description("MAUI Test Naming Conventions: Files, classes, methods, and AutomationIds")]
        public static async Task<string> GetNamingGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Naming Guidelines

## FILE NAMING
- **Host App**: Issue{IssueNumber}.cs (e.g., Issue12345.cs)
- **Test Class**: Issue{IssueNumber}.cs (same name, different folder)
- **Screenshot**: {TestMethodName}.png

## CLASS NAMING
- **Host App Class**: Issue{IssueNumber} (matches filename)
- **Test Class**: Issue{IssueNumber} (matches filename)

## TEST METHOD NAMING
Use descriptive, action-based names:

### Good Examples
- ButtonClickTriggersNavigation()
- CollectionViewDisplaysItems()
- EntryValidatesEmail()
- ImageRespectsAspectRatio()
- ModalPageShowsCorrectContent()

### Bad Examples
- Test1()
- ButtonTest()
- TestIssue()
- DoTest()

## AUTOMATION ID NAMING
Use clear, descriptive identifiers:

### Good Examples
- ""LoginButton""
- ""EmailEntry""
- ""ProductList""
- ""CloseModalButton""
- ""TestImage""

### Bad Examples
- ""btn1""
- ""test""
- ""element""
- ""item""

## CONSTANTS AND VARIABLES
- Use descriptive names that explain purpose
- Avoid abbreviations unless universally understood
- Use PascalCase for constants, camelCase for local variables

### Example
```csharp
const string LoginButtonId = ""LoginButton"";
var emailEntryText = ""test@example.com"";
var productCollection = App.WaitForElement(""ProductList"");
```");
        }

        [McpServerResource]
        [Description("MAUI Test Inheritance Guidelines: Base classes and constructor patterns")]
        public static async Task<string> GetInheritanceGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Inheritance Guidelines

## BASE CLASSES

### 1. _IssuesUITest: For issue reproduction tests
- Use for GitHub issue reproductions
- Provides App instance and common functionality
- Handles platform-specific setup

### 2. TestContentPage: For host app pages
- Simpler than ContentPage for test scenarios
- Override Init() instead of constructor
- Better for test isolation

### 3. ContentPage: For complex scenarios
- Use when you need full page lifecycle
- More control over initialization timing

## INHERITANCE HIERARCHY
```
_IssuesUITest
├── Handles platform detection
├── Provides App property
├── Common setup/teardown
└── Screenshot utilities

TestContentPage
├── Simplified page creation
├── Init() override point
├── Test-focused lifecycle
└── Better isolation
```

## CONSTRUCTOR PATTERNS
```csharp
// Correct, always pass TestDevice to base
public Issue12345(TestDevice testDevice) : base(testDevice) { }

// Required property override
public override string Issue => ""Descriptive issue title"";

// Standard test method pattern
[Test]
[Category(UITestCategories.ControlName)]
public void TestMethodName()
{
    // Test implementation
}
```");
        }

        [McpServerResource]
        [Description("MAUI Test Assertion Strategies: VerifyScreenshot, element validation, state checks")]
        public static async Task<string> GetAssertionGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Assertion Guidelines

## PRIMARY ASSERTION STRATEGIES

### 1. VerifyScreenshot() - RECOMMENDED
- Most reliable cross-platform validation
- Catches visual regressions
- Works for layout, styling, content

```csharp
App.WaitForElement(""TestElement"");
VerifyScreenshot();
```

### 2. Element Existence Checks
```csharp
App.WaitForElement(""ElementId""); // Implicit assertion
```

### 3. Element State Validation
```csharp
var element = App.WaitForElement(""Button"");
Assert.IsTrue(element.IsEnabled());
```

### 4. Text Content Verification
```csharp
var label = App.WaitForElement(""StatusLabel"");
var text = label.GetText();
Assert.AreEqual(""Expected Text"", text);
```

## ASSERTION BEST PRACTICES

### DO:
- Use VerifyScreenshot() as primary validation
- Wait for elements before asserting
- Use descriptive assertion messages
- Test one concept per test method
- Validate both positive and negative cases

### DON'T:
- Assert immediately without waiting
- Use Thread.Sleep() instead of proper waits
- Test multiple unrelated concepts in one test
- Skip assertions (tests should validate something)
- Rely only on element existence

## COMMON PATTERNS

### Visual Validation
```csharp
App.WaitForElement(""TestElement"");
// Perform actions if needed
App.Tap(""ActionButton"");
VerifyScreenshot();
```

### State Validation
```csharp
App.WaitForElement(""ToggleSwitch"");
App.Tap(""ToggleSwitch"");
var toggle = App.WaitForElement(""ToggleSwitch"");
Assert.IsTrue(toggle.IsSelected());
```

### Navigation Validation
```csharp
App.Tap(""NavigateButton"");
App.WaitForElement(""NextPageElement"");
VerifyScreenshot();
```");
        }

        [McpServerResource]
        [Description("MAUI Test Wait Strategies: Timing, element waits, navigation waits")]
        public static async Task<string> GetWaitGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Wait Strategy Guidelines

## WAIT HIERARCHY (Use in this order)

### 1. WaitForElement() - PRIMARY
- Use for element appearance
- Includes built-in timeout and retries
- Returns element for further interaction

```csharp
App.WaitForElement(""ElementId"");
App.WaitForElement(""ElementId"", timeout: TimeSpan.FromSeconds(10));
```

### 2. WaitForNoElement() - For disappearance
- Use when validating element removal
- Useful for modal dismissals, loading states

```csharp
App.WaitForNoElement(""LoadingSpinner"");
```

### 3. WaitForElementTillPageNavigationSettled() - For navigation
- Use after navigation operations
- Especially important for MacCatalyst

```csharp
App.TapBackArrow();
App.WaitForElementTillPageNavigationSettled(""PreviousPageElement"");
```

### 4. Custom Wait Conditions
- Use WaitForTextToBePresentInElement for dynamic text
- QueryUntilPresent for complex conditions

```csharp
App.WaitForTextToBePresentInElement(""StatusLabel"", ""Complete"");
```

## TIMING BEST PRACTICES

### DO:
- Always wait before interactions
- Use appropriate timeouts (default is usually fine)
- Wait for specific elements, not arbitrary delays
- Chain waits with actions logically

### DON'T:
- Use Thread.Sleep() or Task.Delay()
- Assume elements are immediately available
- Use excessively long timeouts (>30 seconds)
- Wait for elements that might never appear

## COMMON WAIT PATTERNS

### Page Navigation
```csharp
App.Tap(""NavigateButton"");
App.WaitForElement(""DestinationPageElement"");
```

### Modal Operations
```csharp
App.Tap(""ShowModalButton"");
App.WaitForElement(""ModalContent"");
App.Tap(""CloseModalButton"");
App.WaitForNoElement(""ModalContent"");
```

### Dynamic Content
```csharp
App.Tap(""LoadDataButton"");
App.WaitForElement(""DataList"");
App.WaitForTextToBePresentInElement(""StatusLabel"", ""Data Loaded"");
```

## TIMEOUT CONSIDERATIONS
- **Default**: 15 seconds (usually sufficient)
- **Network operations**: 30+ seconds
- **Complex layouts**: 10-20 seconds
- **Simple interactions**: 5-10 seconds");
        }

        [McpServerResource]
        [Description("MAUI Test Categories: Standard categories and usage guidelines")]
        public static async Task<string> GetCategoryGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Category Guidelines

## STANDARD CATEGORIES (Use existing ones)

### UI Controls
```csharp
[Category(UITestCategories.Button)]
[Category(UITestCategories.CollectionView)]
[Category(UITestCategories.Entry)]
[Category(UITestCategories.Image)]
[Category(UITestCategories.Label)]
[Category(UITestCategories.ListView)]
[Category(UITestCategories.Picker)]
[Category(UITestCategories.Slider)]
[Category(UITestCategories.Switch)]
```

### Layout
```csharp
[Category(UITestCategories.Layout)]
[Category(UITestCategories.Grid)]
[Category(UITestCategories.StackLayout)]
```

### Navigation
```csharp
[Category(UITestCategories.Navigation)]
[Category(UITestCategories.FlyoutPage)]
[Category(UITestCategories.Shell)]
[Category(UITestCategories.TabbedPage)]
```

### Platform Specific
```csharp
[Category(UITestCategories.ManualReview)] // For manual testing
[Category(UITestCategories.Gesture)] // For gesture tests
```

## CATEGORY BEST PRACTICES

### DO:
- Use existing categories when possible
- Apply the most specific category available
- Use only one primary category per test
- Match the category to the primary control being tested

### DON'T:
- Create new categories unless absolutely necessary
- Use multiple categories for the same test
- Use generic categories when specific ones exist

## EXAMPLE USAGE
```csharp
[Test]
[Category(UITestCategories.CollectionView)]
public void CollectionViewDisplaysItems()
{
    App.WaitForElement(""ItemsList"");
    VerifyScreenshot();
}
```

## FILTERING BENEFITS
- Run tests for specific controls during development
- Organize CI/CD test execution
- Debug control-specific issues
- Performance testing by category");
        }

        [McpServerResource]
        [Description("MAUI AutomationId Guidelines: Element identification best practices")]
        public static async Task<string> GetAutomationIdGuidelines()
        {
            return await Task.FromResult(@"# MAUI AutomationId Guidelines

## AUTOMATION ID IMPORTANCE
- Essential for reliable test element identification
- Cross-platform consistency
- Better than text-based selection (text changes)
- Required for maintainable tests

## NAMING CONVENTIONS
Use PascalCase, descriptive names:

### Good Examples
- ""LoginButton""
- ""EmailEntry""
- ""ProductCollectionView""
- ""CloseModalButton""
- ""TestImage""
- ""NavigationBackButton""

### Bad Examples
- ""btn1""
- ""test""
- ""element""
- ""button""
- ""item""

## ASSIGNMENT PATTERNS

### XAML
```xml
<Button Text=""Login"" AutomationId=""LoginButton"" />
<Entry Placeholder=""Email"" AutomationId=""EmailEntry"" />
<CollectionView AutomationId=""ProductList"" />
```

### C# Code
```csharp
var loginButton = new Button 
{ 
    Text = ""Login"",
    AutomationId = ""LoginButton""
};
```

### DATA TEMPLATES
For dynamic content, use consistent patterns:
```xml
<DataTemplate>
    <Grid>
        <Label Text=""{Binding Name}"" AutomationId=""ProductName"" />
        <Button Text=""Buy"" AutomationId=""BuyButton"" />
    </Grid>
</DataTemplate>
```

## TESTING USAGE
```csharp
App.WaitForElement(""LoginButton"");
App.Tap(""LoginButton"");
App.EnterText(""EmailEntry"", ""test@example.com"");
```

## AUTOMATION ID STRATEGY

### Page Level
- Use descriptive names for the main elements
- Include control type in name when helpful
- Consider the element's purpose, not just type

### Dynamic Content
- Use consistent naming for repeated elements
- Consider using templates or patterns
- Test with multiple items to ensure uniqueness

### Complex Scenarios
- Nested controls: ""ParentName_ChildName""
- Modal content: ""Modal_ElementName""
- Tab content: ""Tab1_ElementName""

## COMMON MISTAKES
- Missing AutomationId on testable elements
- Using text content as identifiers
- Inconsistent naming patterns
- Too generic names (""Button1"", ""Test"")
- Not updating IDs when UI changes");
        }

        [McpServerResource]
        [Description("MAUI Test Patterns & Anti-Patterns: Best practices and common mistakes")]
        public static async Task<string> GetPatternGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Patterns & Anti-Patterns

## GOOD PATTERNS

### 1. Page Object Model (for complex scenarios)
```csharp
public class LoginPageActions
{
    private readonly IApp _app;
    
    public LoginPageActions(IApp app) => _app = app;
    
    public void EnterCredentials(string email, string password)
    {
        _app.WaitForElement(""EmailEntry"");
        _app.EnterText(""EmailEntry"", email);
        _app.EnterText(""PasswordEntry"", password);
    }
    
    public void SubmitLogin()
    {
        _app.Tap(""LoginButton"");
        _app.WaitForElement(""DashboardPage"");
    }
}
```

### 2. Test Data Management
```csharp
public class TestDataProvider
{
    public static string ValidEmail => ""test@example.com"";
    public static string ValidPassword => ""Password123!"";
    public static string InvalidEmail => ""invalid-email"";
}
```

### 3. Helper Methods
```csharp
public static class AppExtensions
{
    public static void LoginWithValidCredentials(this IApp app)
    {
        app.WaitForElement(""EmailEntry"");
        app.EnterText(""EmailEntry"", TestDataProvider.ValidEmail);
        app.EnterText(""PasswordEntry"", TestDataProvider.ValidPassword);
        app.Tap(""LoginButton"");
    }
}
```

### 4. Fluent Test Structure
```csharp
[Test]
public void UserCanLoginSuccessfully()
{
    // Arrange - wait for initial state
    App.WaitForElement(""LoginForm"");
    
    // Act - perform the action
    App.LoginWithValidCredentials();
    
    // Assert - verify the result
    App.WaitForElement(""DashboardPage"");
    VerifyScreenshot();
}
```

## ANTI-PATTERNS (AVOID)

### 1. Thread.Sleep() Usage
```csharp
// BAD:
Thread.Sleep(2000); // Never do this
App.Tap(""Button"");

// GOOD:
App.WaitForElement(""Button"");
App.Tap(""Button"");
```

### 2. Hardcoded Strings Everywhere
```csharp
// BAD:
App.EnterText(""email"", ""test@test.com"");
App.EnterText(""password"", ""mypassword"");

// GOOD:
App.EnterText(""EmailEntry"", TestData.ValidEmail);
App.EnterText(""PasswordEntry"", TestData.ValidPassword);
```

### 3. No Assertions
```csharp
// BAD:
App.Tap(""Button"");
// Test ends with no validation

// GOOD:
App.Tap(""Button"");
App.WaitForElement(""ResultPage"");
VerifyScreenshot();
```

### 4. Testing Multiple Scenarios in One Test
```csharp
// BAD:
[Test]
public void LoginTestEverything()
{
    // Tests valid login, invalid login, empty fields, etc.
}

// GOOD:
[Test] public void ValidLoginSucceeds() { }
[Test] public void InvalidLoginFails() { }
[Test] public void EmptyFieldsShowError() { }
```

### 5. Tight Coupling to UI Implementation
```csharp
// BAD:
App.Tap(""Grid""); // Depends on internal structure
App.ScrollDown(""StackLayout"");

// GOOD:
App.Tap(""LoginButton""); // Semantic, purpose-driven
App.ScrollDown(""ProductList"");
```");
        }

        [McpServerResource]
        [Description("MAUI Test Performance Guidelines: Optimization and efficiency best practices")]
        public static async Task<string> GetPerformanceGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Performance Guidelines

## PERFORMANCE BEST PRACTICES

### 1. Efficient Element Location
```csharp
// GOOD - Use AutomationId:
App.WaitForElement(""LoginButton"");

// SLOW - Complex XPath queries:
App.WaitForElement(AppiumQuery.ByXPath(""//Button[@text='Login'][1]""));
```

### 2. Minimize Screenshot Operations
- VerifyScreenshot() is expensive
- Use strategically, not after every action
- Consider grouping actions before screenshot

```csharp
// GOOD:
App.Tap(""Button1"");
App.Tap(""Button2"");
App.Tap(""Button3"");
VerifyScreenshot(); // One screenshot for all actions

// SLOW:
App.Tap(""Button1"");
VerifyScreenshot();
App.Tap(""Button2"");
VerifyScreenshot();
```

### 3. Smart Waiting Strategies
```csharp
// Use appropriate timeouts:
App.WaitForElement(""QuickElement"", TimeSpan.FromSeconds(5));
App.WaitForElement(""SlowNetworkElement"", TimeSpan.FromSeconds(30));

// Avoid excessive waiting:
// App.WaitForElement(""SimpleButton"", TimeSpan.FromMinutes(5));
```

### 4. Test Data Efficiency
- Prepare minimal test data
- Avoid large datasets unless testing performance
- Use simple, fast-loading content

### 5. Parallel Test Considerations
- Design tests to be independent
- Avoid shared state between tests
- Clean up test data appropriately

## PERFORMANCE MONITORING

### Track These Metrics
- Test execution time per method
- Screenshot comparison time
- Element location time
- App startup time

### Warning Signs
- Tests taking >2 minutes
- Frequent timeouts
- High CPU usage during test runs
- Memory leaks in test app

## OPTIMIZATION STRATEGIES

### 1. Batch Related Operations
```csharp
// Group related UI actions
public void SetupFormData()
{
    App.EnterText(""FirstName"", ""John"");
    App.EnterText(""LastName"", ""Doe"");
    App.EnterText(""Email"", ""john@example.com"");
    App.Tap(""SubmitButton"");
    // Single verification at the end
    VerifyScreenshot();
}
```

### 2. Selective Testing
- Use categories to run subsets during development
- Full test suite for CI/CD
- Critical path tests for quick validation

### 3. Resource Management
- Dispose of test resources properly
- Avoid memory leaks in test helpers
- Monitor test app memory usage

## PLATFORM-SPECIFIC CONSIDERATIONS

### iOS
- Simulator vs Device performance differences
- Memory constraints on older devices

### Android
- Emulator performance varies significantly
- Different Android versions have different performance characteristics

### Windows
- Desktop vs tablet mode differences
- High DPI scaling impacts");
        }

        [McpServerResource]
        [Description("MAUI Test Debugging Guidelines: Troubleshooting and common issues")]
        public static async Task<string> GetDebuggingGuidelines()
        {
            return await Task.FromResult(@"# MAUI Test Debugging Guidelines

## DEBUGGING STRATEGIES

### 1. Element Location Issues
```csharp
// Use PrintTree() to see app hierarchy
App.PrintTree();

// Try multiple location strategies
try 
{
    App.WaitForElement(""ElementId"");
} 
catch (TimeoutException)
{
    // Try alternative approach
    App.WaitForElement(AppiumQuery.ByText(""Element Text""));
}
```

### 2. Screenshot Debugging
```csharp
// Take screenshots at key points
App.WaitForElement(""Element"");
// Screenshot before action
VerifyScreenshot(""BeforeAction"");
App.Tap(""Element"");
// Screenshot after action
VerifyScreenshot(""AfterAction"");
```

### 3. Timing Issues
```csharp
// Add longer waits for debugging
App.WaitForElement(""Element"", TimeSpan.FromSeconds(30));

// Check if element exists without waiting
if (App.FindElements(""Element"").Count > 0)
    Console.WriteLine(""Element found"");
else
    Console.WriteLine(""Element not found"");
```

### 4. Platform-Specific Debugging
```csharp
// Check which platform is running
var testDevice = App.GetTestDevice();
Console.WriteLine($""Running on: {testDevice}"");
```

## COMMON ISSUES & SOLUTIONS

### 1. Element Not Found
**Problem**: TimeoutException when waiting for element
**Solutions**:
- Verify AutomationId is set correctly
- Check if element is in view (may need scrolling)
- Ensure element is loaded (wait for parent container)
- Use App.PrintTree() to see actual hierarchy

### 2. Test Flakiness
**Problem**: Test passes sometimes, fails others
**Solutions**:
- Add proper waits instead of assuming timing
- Check for race conditions in app code
- Verify test independence (no shared state)
- Use more specific element selectors

### 3. Screenshot Mismatches
**Problem**: Screenshots don't match expected results
**Solutions**:
- Check for timing issues (elements still loading)
- Verify consistent test environment
- Consider platform-specific differences
- Update expected screenshots if change is intentional

### 4. Performance Issues
**Problem**: Tests run very slowly
**Solutions**:
- Reduce screenshot frequency
- Use more efficient element selectors
- Optimize test data
- Check emulator/simulator performance

## DEBUGGING TOOLS

### 1. Console Output
```csharp
Console.WriteLine($""Current page: {App.GetCurrentPage()}"");
Console.WriteLine($""Element count: {App.FindElements(""Button"").Count}"");
```

### 2. Exception Details
```csharp
try
{
    App.WaitForElement(""Element"");
}
catch (Exception ex)
{
    Console.WriteLine($""Error: {ex.Message}"");
    Console.WriteLine($""Stack: {ex.StackTrace}"");
    throw; // Re-throw to fail test
}
```

### 3. Conditional Debugging
```csharp
#if DEBUG
App.PrintTree();
Thread.Sleep(5000); // Pause to examine state
#endif
```

## TESTING BEST PRACTICES FOR DEBUGGING
- Write descriptive test names that indicate expected behavior
- Add comments explaining complex interactions
- Use consistent naming for similar operations
- Keep test methods focused and small
- Log important state changes during test execution

## TROUBLESHOOTING CHECKLIST
- [ ] AutomationId is set on target elements
- [ ] Elements are actually visible on screen
- [ ] Proper waits are used (no Thread.Sleep)
- [ ] Test data is properly initialized
- [ ] App is in expected state before test actions
- [ ] Platform-specific code is handled correctly
- [ ] Test environment is consistent");
        }

        [McpServerResource]
        [Description("Basic MAUI Test Template: Complete example with host app and test class")]
        public static async Task<string> GetBasicTestTemplate()
        {
            return await Task.FromResult(@"# Basic MAUI Test Template

## HOST APP FILE: TestCases.HostApp/Issues/Issue12345.cs
```csharp
using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 12345, ""Button click navigation issue"", PlatformAffected.All)]
public class Issue12345 : TestContentPage
{
    protected override void Init()
    {
        Title = ""Issue 12345"";

        Content = new StackLayout
        {
            Padding = 20,
            Children =
            {
                new Label
                {
                    Text = ""Test Page for Issue 12345"",
                    AutomationId = ""PageTitle""
                },
                new Button
                {
                    Text = ""Navigate"",
                    AutomationId = ""NavigateButton""
                },
                new Entry
                {
                    Placeholder = ""Enter text"",
                    AutomationId = ""TextEntry""
                }
            }
        };
    }
}
```

## TEST CLASS FILE: Shared.Tests/Tests/Issues/Issue12345.cs
```csharp
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue12345 : _IssuesUITest
{
    public Issue12345(TestDevice testDevice) : base(testDevice)
    {
    }

    public override string Issue => ""Button click navigation issue"";

    [Test]
    [Category(UITestCategories.Button)]
    public void ButtonClickNavigatesCorrectly()
    {
        // Arrange - Wait for initial page load
        App.WaitForElement(""PageTitle"");
        
        // Act - Perform the action being tested
        App.Tap(""NavigateButton"");
        
        // Assert - Verify expected result
        App.WaitForElement(""DestinationPageElement"");
        VerifyScreenshot();
    }
    
    [Test]
    [Category(UITestCategories.Entry)]
    public void EntryAcceptsTextInput()
    {
        // Arrange
        App.WaitForElement(""TextEntry"");
        
        // Act
        App.EnterText(""TextEntry"", ""Test input"");
        
        // Assert
        var entry = App.WaitForElement(""TextEntry"");
        var entryText = entry.GetText();
        Assert.AreEqual(""Test input"", entryText);
        VerifyScreenshot();
    }
}
```

## CHECKLIST FOR NEW TESTS
- [ ] Host app inherits from TestContentPage or ContentPage
- [ ] Host app has [Issue] attribute with correct parameters
- [ ] All interactive elements have AutomationId
- [ ] Test class inherits from _IssuesUITest
- [ ] Test class has proper constructor and Issue property
- [ ] Test methods have [Test] and [Category] attributes
- [ ] Tests use WaitForElement before interactions
- [ ] Tests include appropriate assertions/VerifyScreenshot
- [ ] Test names are descriptive and action-based
- [ ] Code follows naming conventions");
        }

        [McpServerResource]
        [Description("Complex MAUI Test Template: Advanced patterns and scenarios")]
        public static async Task<string> GetComplexTestTemplate()
        {
            return await Task.FromResult(@"# Complex MAUI Test Template

## Advanced Host App with Multiple Scenarios
```csharp
[Issue(IssueTracker.Github, 54321, ""CollectionView with complex interactions"", PlatformAffected.All)]
public class Issue54321 : TestContentPage
{
    public ObservableCollection<TestItem> Items { get; set; }
    
    protected override void Init()
    {
        Items = new ObservableCollection<TestItem>
        {
            new TestItem { Name = ""Item 1"", IsSelected = false },
            new TestItem { Name = ""Item 2"", IsSelected = true },
            new TestItem { Name = ""Item 3"", IsSelected = false }
        };
        
        var refreshView = new RefreshView
        {
            AutomationId = ""RefreshView""
        };
        
        var collectionView = new CollectionView
        {
            ItemsSource = Items,
            SelectionMode = SelectionMode.Multiple,
            AutomationId = ""TestCollectionView"",
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    }
                };
                
                var label = new Label();
                label.SetBinding(Label.TextProperty, ""Name"");
                Grid.SetColumn(label, 0);
                
                var button = new Button 
                { 
                    Text = ""Action"",
                    AutomationId = ""ItemActionButton""
                };
                Grid.SetColumn(button, 1);
                
                grid.Children.Add(label);
                grid.Children.Add(button);
                
                return grid;
            })
        };
        
        refreshView.Content = collectionView;
        
        Content = new StackLayout
        {
            Children =
            {
                new Label 
                { 
                    Text = ""Complex Test Scenario"",
                    AutomationId = ""HeaderLabel""
                },
                new Button
                {
                    Text = ""Add Item"",
                    AutomationId = ""AddItemButton"",
                    Command = new Command(() => 
                    {
                        Items.Add(new TestItem { Name = $""Item {Items.Count + 1}"" });
                    })
                },
                new Button
                {
                    Text = ""Clear Selection"",
                    AutomationId = ""ClearSelectionButton"",
                    Command = new Command(() => 
                    {
                        collectionView.SelectedItems?.Clear();
                    })
                },
                refreshView
            }
        };
    }
    
    public class TestItem
    {
        public string Name { get; set; }
        public bool IsSelected { get; set;         }
    }
}
```

## Advanced Test Class with Multiple Test Patterns
```csharp
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue54321 : _IssuesUITest
{
    public Issue54321(TestDevice testDevice) : base(testDevice) { }
    
    public override string Issue => ""CollectionView with complex interactions"";

    [Test]
    [Category(UITestCategories.CollectionView)]
    public void CollectionViewDisplaysItemsCorrectly()
    {
        // Arrange
        App.WaitForElement(""TestCollectionView"");
        
        // Act, initial state verification
        App.WaitForElement(""HeaderLabel"");
        
        // Assert
        VerifyScreenshot();
    }
    
    [Test]
    [Category(UITestCategories.CollectionView)]
    public void AddItemIncreasesCollectionCount()
    {
        // Arrange
        App.WaitForElement(""AddItemButton"");
        
        // Act
        App.Tap(""AddItemButton"");
        App.Tap(""AddItemButton"");
        
        // Assert - Verify items were added
        VerifyScreenshot();
    }
    
    [Test]
    [Category(UITestCategories.CollectionView)]
    public void SelectionCanBeCleared()
    {
        // Arrange
        App.WaitForElement(""TestCollectionView"");
        
        // Act, select items then clear
        SelectMultipleItems();
        App.Tap(""ClearSelectionButton"");
        
        // Assert
        VerifyScreenshot();
    }
    
    [Test]
    [Category(UITestCategories.CollectionView)]
    public void RefreshViewWorksCorrectly()
    {
        // Arrange
        App.WaitForElement(""RefreshView"");
        
        // Act, perform pull-to-refresh gesture
        App.SwipeDown(""RefreshView"");
        
        // Wait for refresh to complete
        App.WaitForElement(""TestCollectionView"");
        
        // Assert
        VerifyScreenshot();
    }
    
    [Test]
    [Category(UITestCategories.Button)]
    public void ItemActionButtonsAreClickable()
    {
        // Arrange
        App.WaitForElement(""ItemActionButton"");
        
        // Act
        App.Tap(""ItemActionButton"");
        
        // Assert, verify button interaction
        // Note: This would need additional implementation in host app
        // to show visible feedback
        VerifyScreenshot();
    }
    
    // Helper method for complex interactions
    private void SelectMultipleItems()
    {
        App.WaitForElement(""TestCollectionView"");
        
        // Platform-specific selection might require different approaches
        var testDevice = App.GetTestDevice();
        
        if (testDevice == TestDevice.Android)
        {
            // Android multi-select pattern
            App.LongPress(""Item 1"");
            App.Tap(""Item 2"");
        }
        else
        {
            // iOS/Other platforms multi-select
            App.Tap(""Item 1"");
            App.Tap(""Item 2"");
        }
    }
    
    [Test]
    [Category(UITestCategories.CollectionView)]
    [TestCase(""portrait"")]
    [TestCase(""landscape"")]
    public void CollectionViewWorksInBothOrientations(string orientation)
    {
        // Arrange
        App.WaitForElement(""TestCollectionView"");
        
        // Act, change orientation
        if (orientation == ""landscape"")
            App.SetOrientationLandscape();
        else
            App.SetOrientationPortrait();
            
        App.WaitForElement(""TestCollectionView"");
        
        // Assert
        VerifyScreenshot($""CollectionView_{orientation}"");
    }
    
    [Test]
    [Category(UITestCategories.Performance)]
    public void LargeDataSetPerformance()
    {
        // Arrange
        App.WaitForElement(""AddItemButton"");
        
        // Act, add many items quickly
        for (int i = 0; i < 20; i++)
        {
            App.Tap(""AddItemButton"");
        }
        
        // Assert, verify UI remains responsive
        App.WaitForElement(""TestCollectionView"");
        VerifyScreenshot();
    }
}
```

## Advanced Patterns Demonstrated

### 1. Data-Driven Tests
```csharp
[TestCase(""portrait"")]
[TestCase(""landscape"")]
public void TestInMultipleOrientations(string orientation)
{
    // Test logic that adapts to parameter
}
```

### 2. Platform-Specific Logic
```csharp
private void HandlePlatformSpecificBehavior()
{
    var testDevice = App.GetTestDevice();
    
    switch (testDevice)
    {
        case TestDevice.Android:
            // Android-specific interaction
            break;
        case TestDevice.iOS:
            // iOS-specific interaction
            break;
    }
}
```

### 3. Helper Methods for Complex Interactions
```csharp
private void SelectMultipleItems()
{
    // Encapsulate complex multi-step interactions
    // Makes tests more readable and maintainable
}
```

### 4. Custom Screenshot Naming
```csharp
VerifyScreenshot($""CollectionView_{orientation}"");
// Creates different expected screenshots per scenario
```

### 5. Performance Testing Patterns
```csharp
[Category(UITestCategories.Performance)]
public void LargeDataSetPerformance()
{
    // Test UI responsiveness with large datasets
}
```

## Key Differences from Basic Template

- **ObservableCollection**: Dynamic data binding
- **DataTemplate**: Custom item layouts
- **Commands**: MVVM pattern implementation
- **RefreshView**: Complex gesture handling
- **Multiple test scenarios**: Various interaction patterns
- **Platform-specific logic**: Handling platform differences
- **Performance testing**: Resource-intensive scenarios
- **Parameterized tests**: Data-driven testing
- **Helper methods**: Reusable interaction patterns");
        }
    }
}