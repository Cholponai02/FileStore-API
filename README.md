# FileStore-API

## 📌 Project Overview
This is a specialized **ASP.NET Core Web API** designed for managing file storage operations. The service provides a clean interface for saving, retrieving, and organizing files within a structured directory system.

## 🚀 Features
* **Asynchronous File Operations:** Uses `async/await` for non-blocking I/O performance.
* **Network Authentication:** Integrated `NetworkCredential` support for accessing protected storage locations.
* **Automated Directory Management:** Smart path generation and directory verification before file operations.
* **Scalable Architecture:** Built with a service-oriented approach (Dependency Injection).

## 🛠 Tech Stack
* **Language:** C#
* **Framework:** .NET 8 / ASP.NET Core (уточни свою версию)
* **API Style:** RESTful

## 🧪 How to Use
1. Clone the repository.
2. Configure your storage paths in `appsettings.json`.
3. Run the project and access the API via Swagger at `/swagger`.

## ⚠️ Security Note
*Ensure that sensitive credentials (usernames/passwords) are managed via **Environment Variables** or **User Secrets** and are never hardcoded in production.*
