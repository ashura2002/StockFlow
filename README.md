# Inventory & Order Management API

A backend REST API for inventory and order management built with ASP.NET Core Web API. The system manages products, inventory, suppliers, categories, and customer orders while following Clean Architecture, CQRS, and Domain-Driven Design (DDD) principles. It also includes JWT authentication, role-based authorization, password recovery, and cloud-based image management.

## Tech Stack

* **Backend:** .NET 10 (ASP.NET Core), C#, MediatR (CQRS), Entity Framework Core
* **Database:** PostgreSQL
* **Architecture:** Clean Architecture, Domain-Driven Design (DDD), Repository Pattern, Unit of Work
* **Authentication:** JWT, Role-Based Authorization
* **Email:** Resend — password reset and recovery
* **File Storage:** Cloudinary — profile and product image management

## Key Features

* **Role-Based Access:** Supports `Admin` and `Customer` roles with protected role-specific operations.
* **Inventory Management:** Product stock reservation, quantity adjustment, stock restoration, and inventory validation during order operations.
* **Order Management:** Customers can create, view, update, and cancel their own orders while maintaining order ownership and domain rules.
* **Secure Authentication:** JWT-based authentication with role-based authorization and current-user context.
* **Password Recovery:** Forgot-password and reset-password workflow using secure reset tokens and Resend email delivery.
* **Profile Management:** Customer profile management with profile image uploads through Cloudinary.
* **Product Management:** Product, category, and supplier management with product image upload and cloud storage.
* **Domain-Driven Design:** Business rules and invariants are encapsulated within domain entities and value objects.
* **Soft Delete:** Supports soft deletion for preserving important historical records.
* **Transactional Consistency:** Order and inventory changes are persisted together through the Unit of Work pattern.

## Architectural Highlights

The system follows **Clean Architecture** to maintain a strict separation between business logic, application use cases, infrastructure, and API concerns:

* **Domain:** Entities, Value Objects, domain rules, domain exceptions, and business behavior.
* **Application:** CQRS Commands/Queries, use cases, DTOs, and application interfaces.
* **Infrastructure:** EF Core persistence, PostgreSQL, repositories, Unit of Work, Cloudinary, and Resend integrations.
* **API:** RESTful endpoints, authentication, authorization, middleware, and HTTP request handling.

## Getting Started

### Prerequisites

* .NET 10 SDK
* PostgreSQL 16+
* Cloudinary account
* Resend account
