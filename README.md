## Overview

Welcome to the documentation for the Program-Infos-Manager. This class library is specifically designed for all operating systems and serves a crucial purpose in retrieving and managing programs. By leveraging the Windows Registry, this library already provides an efficient and robust way to access, manipulate, and store program information in Windows. Though through its plugin system it can support any way of managing program information on any operating system.

## Key Features

- **Plugin-Support**: Through its design the library supports loading plugins in form of compiled .dll libraries to support custom ways to retrieve program information.
- **Dependency-Injection**: The library uses the singleton pattern in combination with dependency-injection to ensure correct plugin structure.
- **Error Handling**: Robust error handling mechanisms to ensure stability and reliability, even when dealing with inconsistent registry data.
- **Compatibility and Performance**: Optimized for Windows environments but also supporting other operating systems, ensuring compatibility and high performance.

### Windows Registry Plugin
- **Registry Data Extraction**: Seamlessly extracts program information from various registry keys, especially focusing on installed program details.
- **Class-Based Representation**: Converts registry data into well-structured C# classes, making it easy to handle and manipulate program data within your applications.

## Intended Audience

This library is intended for developers and IT professionals who are working with applications that need to interact with the installed programs, especially for managing of program information. It abstracts the complexities of interactions and provides a simple yet powerful interface for managing program data.

## Getting Started

To get started with the Program-Infos-Manager Library, please refer to the [Getting Started](https://der-floh.github.io/Programs.Manager/docs/getting-started.html) guide. For more detailed information on specific classes and methods, please explore the [API Documentation](https://der-floh.github.io/Programs.Manager/api/Programs.html).

## Contributing and Feedback

Contributions and feedback are invaluable to the ongoing development and improvement of this library. Please refer to the [Contributing Guidelines](https://der-floh.github.io/Programs.Manager/docs/contributing.html) for more information on how you can participate in the development process.

---

I hope this documentation provides you with all the information you need to effectively utilize this Library in your projects. For any further assistance or queries, please reach out to me.
