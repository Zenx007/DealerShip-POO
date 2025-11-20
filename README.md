# Modular Dealership System 🚗

A robust C# Console Application designed to manage a vehicle dealership. 

**This project serves as an educational comparison** to the [Original Dealership Project](https://github.com/Zenx007/Dealership), demonstrating the transition from a monolithic script to a fully modularized, Object-Oriented architecture.

---

## 🎯 Purpose & Comparison

The main goal of this repository is to illustrate **why** software architecture matters. While the original version kept all logic, data, and UI in a single `Program.cs` file, this version separates concerns to follow **SOLID principles**.

### 🆚 Monolithic (Old) vs. Modular (New)

| Feature | ❌ Original Version (Monolithic) | ✅ This Version (Modular) |
| :--- | :--- | :--- |
| **Structure** | Single file (`Program.cs`) containing 200+ lines. | Multiple layers: `Models`, `Services`, `Interfaces`, `UI`. |
| **Coupling** | High coupling. The UI knew directly about file paths and logic. | Loose coupling via **Dependency Injection**. |
| **State** | Heavy use of `static` methods and global variables. | Instance-based services managing their own state. |
| **Scalability** | Hard to add features without breaking existing code. | Easy to extend (e.g., adding a database just requires a new Service). |
| **Language** | Portuguese code & Mixed UI. | **English Code (Standard)** & Portuguese UI (Localization). |

---

## 🏗️ Project Architecture

The application follows a **Layered Architecture** to ensure Separation of Concerns (SoC):

```text
Shop/
├── Interfaces/        # Contracts (What the system does)
│   ├── ICartService.cs
│   ├── IInventoryService.cs
│   └── IFileService.cs
├── Models/            # Data Structures (What the system processes)
│   └── Vehicle.cs
├── Services/          # Business Logic (How the system works)
│   ├── CartService.cs
│   ├── InventoryService.cs
│   └── FileService.cs
├── UI/                # Presentation Layer (How the user interacts)
│   └── UserInterface.cs
└── Program.cs         # Entry Point (Dependency Injection Wiring)
