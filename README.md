# DonationHub API - Sprint 1

DonationHub is a feature-rich, secure donation and campaign management platform designed to streamline charitable contributions efficiently. Built with modern web technologies and following industry best practices, this project demonstrates a scalable, maintainable architecture suitable for real-world enterprise applications.

---

## 📋 Table of Contents
- [About The Project](#-about-the-project)
- [Architecture Overview](#-architecture-overview)
- [Layer Breakdown](#-layer-breakdown)
- [Key Features](#-key-features)
- [Getting Started](#-getting-started)
- [Try It (Scalar / Swagger & Authentication)](#-try-it-scalar--swagger--authentication)
- [Design Decisions](#-design-decisions)
- [What is Included in Sprint 1](#-what-is-included-in-sprint-1)

---

## 🎯 About The Project

**DonationHub** is a backend application designed to manage donations, campaigns, and categories securely and efficiently using modern .NET architecture and clean code principles.

### Why This Project?

* ✅ **Clean Architecture** - Clear separation of concerns across enterprise layers
* ✅ **Generic Repository Pattern** - Abstracted data access layer for flexibility
* ✅ **Unit of Work** - Centralized transaction management and atomic operations
* ✅ **Role-Based Access Control** - Secure Admin and user authorization via JWT
* ✅ **Robust Validation & Invariants** - Strict business rule enforcement (e.g., preventing category deletion if tied to active campaigns)
* ✅ **API-First & Documentation** - Integrated interactive API documentation using Scalar and OpenAPI

---

## 🏗 Architecture Overview

The project follows a strict **4-tier Clean Architecture** layered structure:

```text
┌────────────────────────────────────────────────────────┐
│               DonationHub.Api (Presentation)           │
│         Controllers, Program.cs, Scalar / Swagger      │
└────────────────────────────────────────────────────────┘
                           │
                           ▼
┌────────────────────────────────────────────────────────┐
│            DonationHub.Application (Business)          │
│       Services, DTOs, Interfaces, AutoMapper, Options  │
└────────────────────────────────────────────────────────┘
                           │
                           ▼
┌────────────────────────────────────────────────────────┐
│             DonationHub.Domain (Core Domain)           │
│        Entities, Common Abstractions, Value Objects    │
└────────────────────────────────────────────────────────┘
                           │
                           ▼
┌────────────────────────────────────────────────────────┐
│         DonationHub.Infrastructure (Persistence)       │
│    AppDbContext, EF Core Repositories, UnitOfWork, DB  │
└────────────────────────────────────────────────────────┘
```

---

## 📂 Layer Breakdown

### 1. DonationHub.Api - Presentation Layer
The ASP.NET Core Web API application serving as the presentation interface, integrating OpenAPI and Scalar.

*   `AuthController.cs` - Handles user authentication, login flow, and JWT access token generation
*   `CategoriesController.cs` - Manages public read operations and admin-only category endpoints
*   `Program.cs` - Configures middleware pipeline, DI services, CORS, and authentication schemes
*   `BearerSecuritySchemeTransformer.cs` - Custom OpenAPI transformer for security definitions

### 2. DonationHub.Application - Business Logic Layer
The core business logic layer containing services, DTOs, interfaces, and mapping profiles.

*   `CategoryService.cs` - Implements category business logic, validation rules, and Invariant 10 checks
*   `CategoryDTO.cs` - Data transfer object for category request/response payloads
*   `ICategoryService.cs` - Contract definition for category operations
*   `IGenericRepository.cs` - Generic contract for data repository actions
*   `IUnitOfWork.cs` - Contract for centralized transaction management
*   `CategoryMappingProfile.cs` - AutoMapper configurations with reverse mapping

### 3. DonationHub.Domain - Core Domain Layer
The foundational enterprise layer containing core entities and shared abstractions with zero external dependencies.

*   `Category.cs` - Core category entity model
*   `Campaign.cs` - Core fundraising campaign entity model
*   `Donation.cs` - Core donation transaction entity model
*   `BaseEntity.cs` - Base abstract entity with shared identifiers
*   `ISoftDeletable.cs` - Interface for soft-delete auditing support
*   `CampaignStatus.cs` - Enum for campaign lifecycle states

### 4. DonationHub.Infrastructure - Persistence & External Layer
The data access layer responsible for database context, migrations, repositories, and persistence logic.

*   `AppDbContext.cs` - Entity Framework Core database session context
*   `GenericRepository.cs` - Implementation of generic data operations
*   `UnitOfWork.cs` - Implementation of atomic transaction handling
*   `DataSeeder.cs` - Seeds initial admin and test user credentials
*   `Configurations/` - Fluent API entity configuration classes
*   `Interceptors/` - Custom database interceptors for auditing and soft deletes

---

## 🚀 Key Features

*   **Clean Architecture:** Strict separation of concerns across Api, Application, Domain, and Infrastructure layers.
*   **Generic Repository & Unit of Work:** Centralized data access abstraction and atomic transaction management ensuring data integrity.
*   **Hybrid Data Access Strategy:** Optimized read operations bypassing repositories to query directly via `DbContext` using `AsNoTracking()` and explicit DTO projections.
*   **JWT-Based Authentication:** Secure token generation and validation flow for user login and protected endpoints.
*   **Role-Based Access Control (RBAC):** Distinct permission boundaries allowing public read access while restricting sensitive operations (POST, PUT, DELETE) exclusively to Admin users.
*   **Domain Invariant Enforcement (Invariant 10 / CAT-04):** Strict business validation inside the service layer preventing category deletion if tied to active campaigns, returning a `409 Conflict`.
*   **Modern API Documentation:** Interactive Scalar API reference and Swagger UI.

---

## 🚀 Getting Started

### Prerequisites
Before running this project, ensure you have the following installed:
* [.NET 9/10 SDK](https://dotnet.microsoft.com/) or later
* [SQL Server](https://www.microsoft.com/sql-server) (Express/Developer Edition)
* Visual Studio 2022 or VS Code
* SQL Server Management Studio (Optional)

---

### Installation & Setup

#### 1. Clone the repository
```bash
git clone https://github.com/yourusername/DonationHub.git
cd DonationHub
```

#### 2. Restore NuGet packages
```bash
dotnet restore
```

#### 3. Update database connection string
Open `DonationHub.Api/appsettings.json` and update your database connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=DonationHubDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

#### 4. Configure User Secrets (Recommended for JWT & Security)
Initialize and set up your local user secrets in the API project directory:

```bash
cd DonationHub.Api
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "YOUR_SUPER_SECRET_KEY_HERE_MINIMUM_32_BYTES"
dotnet user-secrets set "Jwt:Issuer" "DonationHub.Api"
dotnet user-secrets set "Jwt:Audience" "DonationHub.Users"
```

#### 5. Apply database migrations
Navigate to the Infrastructure project or specify paths to update the database schema:

```bash
dotnet ef database update --project ../DonationHub.Infrastructure --startup-project .
```

#### 6. Run the application
```bash
dotnet run
```

---

## 🧪 Try It (Scalar / Swagger & Authentication)

### Opening API Documentation
* **Scalar API Reference:** Open your browser and navigate to:
  ```text
  https://localhost:YourPort/scalar/v1
  ```
* **Swagger UI:** Navigate to:
  ```text
  https://localhost:YourPort/index.html
  ```

### Authentication Steps
1. Execute the `POST /api/Auth/login` endpoint using seeded admin credentials:
   ```json
   {
     "email": "your-email@gmail.com",
     "password": "your-password"
   }
   ```
2. Copy the **`accessToken`** from the response body.
3. In Scalar or Postman, pass the token in the request headers:
   ```text
   Authorization: Bearer <your_access_token>
   ```
4. Test protected endpoints (Admin-only POST, PUT, DELETE for Categories).

---

## 🏛 Design Decisions

### 1. Hybrid Data Access Strategy (Generic Repository/UoW for Writes vs. Direct DbContext for Reads)
* **Decision:** Write operations (Create, Update, Delete) are channeled through `IGenericRepository<T>` combined with `IUnitOfWork` to maintain transactional integrity and domain invariant tracking. Conversely, read operations query directly via `DbContext` using `AsNoTracking()` and explicit DTO projections (`ProjectTo` / AutoMapper).
* **Alternative Considered:** Using the Generic Repository for all read operations as well.
* **Reasoning:** Pure repository patterns for reads often lead to performance bottlenecks and unnecessary change tracking overhead. Direct `DbContext` querying with `AsNoTracking()` significantly optimizes read performance and memory allocation (SRS §3.3 rule 5).

### 2. Invariant 10 / CAT-04 Validation Rule on Deletion
* **Decision:** Enforcing a strict check inside `CategoryService` before deleting a category to ensure no campaigns are actively bound to it (returning `409 Conflict` if dependencies exist).
* **Alternative Considered:** Cascading deletes automatically in the database.
* **Reasoning:** Cascading deletes for core categorization entities risks catastrophic data loss of active campaigns. Returning a clean conflict response preserves data integrity.

---
