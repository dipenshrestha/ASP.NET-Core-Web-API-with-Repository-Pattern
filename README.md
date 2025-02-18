# Dapper Based Employee MAnagement API using ASP.NET Core Web API with Repository Pattern

## **Overview**
This project is a Web API built using **ASP.NET Core**, implementing the **Repository Pattern** for maintainability and scalability. It provides **CRUD operations** with **Dapper** for database interaction and uses **SQL Server Management Studio (SSMS)** as the database.

## **Features**
- Built with **ASP.NET Core Web API** using **ControllerBase**.
- Implemented **Repository Pattern** for better separation of concerns.
- Utilized **Dapper** for lightweight and high-performance database operations.
- Integrated **Swagger** for API documentation and testing.
- Applied **Data Annotations** for model validation.
- Used **try-catch blocks** for error handling.
- Marked controllers with `[ApiController]` for automatic request validation.
- Followed **asynchronous programming** principles using **async/await**.
- Implemented **CRUD operations** for managing Employee, Department, and Designation.

## **Technologies Used**
- **ASP.NET Core Web API**
- **Dapper** (Micro-ORM)
- **SQL Server (SSMS)**
- **Swagger (Swashbuckle)**
- **C#**
- **REST API principles**

## **Database Schema**
This project consists of three tables:  
1. **Designation**  
   - `DesignationId` (Primary Key, Auto-increment)  
   - `DesignationName` (VARCHAR 500)  

2. **Department**  
   - `DepartmentId` (Primary Key, Auto-increment)  
   - `DepartmentName` (VARCHAR 500)  

3. **Employee**  
   - `EmployeeId` (Primary Key, Auto-increment)  
   - `FirstName` (VARCHAR 500)  
   - `LastName` (VARCHAR 500)  
   - `Email` (VARCHAR 500)  
   - `PhoneNumber` (VARCHAR 500)  
   - `DepartmentId` (Foreign Key referencing `DepartmentId`, ON DELETE SET NULL)  
   - `DesignationId` (Foreign Key referencing `DesignationId`, ON DELETE SET NULL)  
