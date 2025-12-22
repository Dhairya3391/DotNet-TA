# Getting Started with .NET Development

Welcome to .NET development! This comprehensive guide will walk you through setting up your development environment with .NET SDK and Visual Studio Code for .NET programming.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installing .NET SDK](#installing-net-sdk)
- [Installing Visual Studio Code](#installing-visual-studio-code)
- [Configuring VS Code for .NET Development](#configuring-vs-code-for-net-development)
- [Creating Your First .NET Project](#creating-your-first-net-project)
- [Running and Debugging](#running-and-debugging)
- [Next Steps](#next-steps)

## Prerequisites

Before you begin, ensure you have:

- A Windows, macOS, or Linux machine
- Administrative privileges (for installation)
- An internet connection

## Installing .NET SDK

The .NET SDK (Software Development Kit) includes everything you need to build and run .NET applications.

### Windows

1. **Download the .NET SDK**

   - Visit [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
   - Select the latest LTS (Long Term Support) version
   - Download the SDK (not just the Runtime)

2. **Run the Installer**

   - Double-click the downloaded `.exe` file
   - Follow the installation wizard
   - Accept the license agreement
   - Complete the installation

3. **Verify Installation**
   ```bash
   dotnet --version
   dotnet --info
   ```

### macOS

1. **Using Homebrew (Recommended)**

   ```bash
   brew install dotnet
   ```

2. **Manual Installation**

   - Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
   - Download the `.pkg` file for macOS
   - Double-click and follow the installation wizard

3. **Verify Installation**
   ```bash
   dotnet --version
   dotnet --info
   ```

### Linux (Ubuntu/Debian)

1. **Add Microsoft Package Repository**

   ```bash
   wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   ```

2. **Install .NET SDK**

   ```bash
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-8.0
   ```

3. **Verify Installation**
   ```bash
   dotnet --version
   dotnet --info
   ```

## Installing Visual Studio Code

Visual Studio Code is a lightweight, powerful code editor that works perfectly for .NET development.

### Installation

1. **Download VS Code**

   - Visit [https://code.visualstudio.com](https://code.visualstudio.com)
   - Download the appropriate version for your operating system

2. **Install VS Code**

   - **Windows**: Run the `.exe` installer and follow the setup wizard
   - **macOS**: Drag VS Code to your Applications folder
   - **Linux**: Follow the distribution-specific instructions

3. **Launch VS Code** and complete the initial setup

## Configuring VS Code for .NET Development

To make VS Code a powerful .NET IDE, you need to install essential extensions.

### Required Extensions

1. **C# Dev Kit**

   - Extension ID: `ms-dotnettools.csdevkit`
   - Provides C# language support, project management, and testing capabilities

2. **C#**
   - Extension ID: `ms-dotnettools.csharp`
   - Core C# language support with IntelliSense and debugging

### Installing Extensions

**Method 1: From Extensions Marketplace**

1. Open VS Code
2. Click the Extensions icon in the sidebar (Ctrl+Shift+X)
3. Search for "C# Dev Kit"
4. Click Install on the C# Dev Kit extension
5. The C# extension will be installed automatically

**Method 2: From Command Palette**

1. Press `Ctrl+Shift+P` (Windows/Linux) or `Cmd+Shift+P` (macOS)
2. Type "Extensions: Install Extensions"
3. Search for and install the required extensions

### Additional Recommended Extensions

- **.NET Runtime Install Tool** (`ms-dotnettools.vscode-dotnet-runtime`): Helps manage .NET runtimes
- **IntelliCode for C# Dev Kit** (`ms-dotnettools.vscode-intellicode-csharp`): AI-assisted development
- **Prettier - Code formatter** (`esbenp.prettier-vscode`): Code formatting

## Creating Your First .NET Project

Now let's create your first .NET console application to verify everything is working correctly.

### Method 1: Using VS Code Terminal

1. **Open VS Code**
2. **Open Terminal**: `Ctrl+`` (backtick) or Terminal → New Terminal
3. **Create a new project folder**:

   ```bash
   mkdir MyFirstApp
   cd MyFirstApp
   ```

4. **Create a new console application**:

   ```bash
   dotnet new console
   ```

5. **Open the project in VS Code**:
   ```bash
   code .
   ```

### Method 2: Using VS Code Command Palette

1. Open VS Code
2. Press `Ctrl+Shift+P` (Windows/Linux) or `Cmd+Shift+P` (macOS)
3. Type ".NET: New Project"
4. Select "Console Application"
5. Choose your preferred location and project name

### Understanding the Project Structure

Your new project will have these files:

```
MyFirstApp/
├── MyFirstApp.csproj    # Project file
├── Program.cs          # Main application code
└── obj/               # Build output folder
```

**Program.cs** contains your main application code:

```csharp
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
```

## Running and Debugging

### Running Your Application

1. **Using Terminal**:

   ```bash
   dotnet run
   ```

2. **Using VS Code Run Button**:

   - Open Program.cs
   - Click the "Run" button above the main method
   - Select "Run and Debug"

3. **Using VS Code Command Palette**:
   - `Ctrl+Shift+P`
   - Type ".NET: Run Project"

### Debugging Your Application

1. **Set a Breakpoint**:

   - Click in the gutter to the left of the line number in Program.cs
   - Or press F9 on a line

2. **Start Debugging**:

   - Press F5
   - Or click the Run and Debug button in the sidebar
   - Or go to Run → Start Debugging

3. **Debugging Controls**:
   - **F5**: Continue/Start debugging
   - **F10**: Step over
   - **F11**: Step into
   - **Shift+F11**: Step out
   - **Shift+F5**: Stop debugging

### Modifying Your Code

Let's modify the program to make it more interactive:

```csharp
using System;

Console.WriteLine("Welcome to .NET Development!");
Console.Write("Please enter your name: ");

string name = Console.ReadLine();

Console.WriteLine($"Hello, {name}! Welcome to the world of .NET programming.");
Console.WriteLine("This is your first step in becoming a .NET developer.");
```

Run the program again (`dotnet run`) and see your changes in action!

## Common .NET CLI Commands

Here are the essential .NET CLI commands you'll use frequently:

### Project Management

```bash
# Create new projects
dotnet new console          # Console application
dotnet new webapi           # Web API
dotnet new mvc              # MVC web application
dotnet new classlib         # Class library

# Build and run
dotnet build                # Build the project
dotnet run                  # Build and run
dotnet clean                # Clean build artifacts
dotnet restore              # Restore NuGet packages
```

### Package Management

```bash
# Add NuGet packages
dotnet add package Newtonsoft.Json
dotnet add package Microsoft.Extensions.Logging

# List packages
dotnet list package

# Remove packages
dotnet remove package Newtonsoft.Json
```

### Testing

```bash
# Run tests
dotnet test

# Create test project
dotnet new xunit -n MyProject.Tests
```

## Troubleshooting Common Issues

### "dotnet command not found"

- Ensure the .NET SDK is installed correctly
- Restart your terminal/command prompt
- On Windows, you may need to restart your computer
- Check that the .NET installation path is in your system PATH

### Extension Issues

- Restart VS Code after installing extensions
- Ensure you have the latest version of VS Code
- Check the Extensions view for any error notifications

### Build Errors

- Run `dotnet restore` to ensure all packages are restored
- Check for syntax errors in your code
- Ensure you're using a compatible .NET version

### Intellisense Not Working

- Make sure C# Dev Kit is installed and enabled
- Try reloading the VS Code window (`Ctrl+Shift+P` → "Developer: Reload Window")
- Check that your project file (.csproj) is properly configured

## Best Practices for .NET Development

1. **Use Meaningful Names**: Choose descriptive names for variables, methods, and classes
2. **Follow C# Conventions**: Use PascalCase for public members, camelCase for local variables
3. **Write Clean Code**: Keep methods small and focused on single responsibilities
4. **Use Source Control**: Initialize Git repositories for your projects
5. **Test Your Code**: Write unit tests for your logic
6. **Stay Updated**: Keep your .NET SDK and tools updated to the latest versions

## Next Steps

Congratulations! You now have a fully functional .NET development environment. Here's what you can explore next:

1. **Explore Different Project Types**: Try creating web APIs, web applications, or class libraries
2. **Learn C# Fundamentals**: Dive into C# language features and syntax
3. **Explore the Labs**: Continue with our hands-on labs to practice your skills
4. **Build Real Projects**: Start building small applications to practice what you've learned
5. **Join the Community**: Engage with the .NET community through forums and discussions

## Additional Resources

- [Official .NET Documentation](https://docs.microsoft.com/dotnet/)
- [C# Programming Guide](https://docs.microsoft.com/dotnet/csharp/)
- [VS Code Documentation](https://code.visualstudio.com/docs)
- [.NET samples and tutorials](https://learn.microsoft.com/dotnet/samples-and-tutorials/)

---

**Happy coding!** You're now ready to start your journey into .NET development.
