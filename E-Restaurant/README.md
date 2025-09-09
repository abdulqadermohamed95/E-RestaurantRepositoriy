 ## E-Restaurant API

A clean architecture–based ASP.NET Core Web API for managing Materials, Combos, and Orders with localization (Arabic/English), validation, and EF Core.

 ## Features

ASP.NET Core 8 Web API

Entity Framework Core with SQL Server

Repository + Unit of Work pattern

FluentValidation for input validation

AutoMapper for DTO mapping

Global exception handling middleware

Tenant middleware (multi-tenant ready)

Localization (Arabic/English)

Swagger/OpenAPI documentation

In-memory database support for testing

## Prerequisites

Make sure you have installed:

.NET 8 SDK

SQL Server

Visual Studio 2022

## Setup
1. Clone Repository
git clone 

2. Configure Database

Update appsettings.json with your SQL Server connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ERestaurantDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

3. Apply Migrations
Add-Migration InitialCreate -StartupProject ERestaurant.Api -Project ERestaurant.Infrastructure
Update-Database -StartupProject ERestaurant.Api -Project ERestaurant.Infrastructure
4. Seed Data

Database is automatically seeded on first run with sample materials, combos, and orders.
 
Run the Application

Using Visual Studio

 Set ERestaurant.API as Startup Project

Open Swagger UI at this Link : https://localhost:44332/swagger

## Running Tests

This project uses xUnit with an in-memory EF Core database.

Run tests:

dotnet test

Localization

Pass Accept-Language header in requests:

Accept-Language: ar → Returns Arabic names

Accept-Language: en → Returns English names

## Project Structure

ERestaurant/
│── ERestaurant.API/                → API project (controllers, Program.cs)
│── ERestaurant.Application/        → Application layer (DTOs, services, validators)
│── ERestaurant.Infrastructure/     → EF Core, repositories, persistence
│── ERestaurant.Domain/             → Entities, enums, value objects
│── ERestaurant.Tests/              → Unit/integration tests