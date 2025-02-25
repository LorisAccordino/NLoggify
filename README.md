# NLoggify
![Description](https://raw.githubusercontent.com/LorisAccordino/NLoggify/master/Imgs/preview2.png)

NLoggify is a lightweight, customizable logging library for .NET that supports multiple log levels (Info, Warning, Error) and outputs logs to JSON/TXT files. It provides a singleton-based logger with event handling for easy integration into your projects. Ideal for logging in .NET applications, with a focus on simplicity and flexibility.

## Features
- Simple logging interface.
- Support for different log levels (`Info`, `Warn`, `Error`, `Debug`, `Trace`, etc.).
- Easy configuration for different log outputs (e.g., file, console, json).
- Lightweight and efficient.

## Installation
You can install NLoggify via [NuGet](https://www.nuget.org/packages/NLoggify) using the following command:

```bash
dotnet add package NLoggify --version 1.0.4
```

Alternatively, you can use the NuGet Package Manager in Visual Studio to search for NLoggify and install it.

## Usage
### Setup and Initialization
To use NLoggify in your project, follow these steps:
1. Install the NuGet package.
2. Initialize NLoggify by configuring the logging settings in your application.

Here is a simple example to get you started:
```C#
using NLoggify.Logging.Loggers;

// Initialize the logger
var logger = Logger.GetLogger();

// Log messages with different log levels
logger.Log(LogLevel.Info, "This is an informational message.");
logger.Log(LogLevel.Warn, "This is a warning message.");
logger.Log(LogLevel.Error, "An error occurred.");
```

## Contributing
We welcome contributions to NLoggify! If you have suggestions, improvements, or bug fixes, feel free to open an issue or submit a pull request.

## Issues
If you encounter any issues or bugs, please report them in the [GitHub issues](https://github.com/LorisAccordino/NLoggify/issues).

## Changelog  
See the [CHANGELOG](https://github.com/LorisAccordino/NLoggify/blob/master/CHANGELOG.md) for details on past and upcoming changes.

## Documentation
For detailed usage instructions, configuration options, and advanced features, please refer to the [NLoggify Documentation](https://github.com/LorisAccordino/NLoggify/wiki).

Full documentation will be available soon. Stay tuned for updates!

## Project URL
https://lorisaccordino.github.io/NLoggify

## License
This project is licensed under the MIT License - see the [LICENSE](https://github.com/LorisAccordino/NLoggify/blob/master/LICENSE.txt) file for details.

## Authors
- **Loris Accordino** - Developer and creator of NLoggify.