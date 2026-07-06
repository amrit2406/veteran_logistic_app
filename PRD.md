Product Requirements Document (PRD)
Veteran Logistics ERP Desktop Application

Version: 1.0
Status: In Development
Current Progress: Phase 1 Completed (Foundation + Authentication)

1. Project Overview
Product Name

Veteran Logistics

Product Type

Enterprise Desktop ERP

Industry

Logistics & Transport Management

Target Users
Transport Companies
Fleet Owners
Logistics Operators
Dispatch Team
Accounts Department
Billing Department
Operations Team
Management
2. Project Vision

Veteran Logistics is a production-grade desktop ERP designed to digitize and manage the complete workflow of transport and logistics companies.

The system will provide centralized management for:

Administration
Authentication & Security
Masters
Transport Operations
Billing
Payments
Reports
Dashboard & Analytics

The application is being built with long-term scalability in mind and should comfortably support 100+ screens while maintaining clean architecture, high performance, and maintainability.

3. Technology Stack
.NET 10
C#
WPF
MVVM
CommunityToolkit.Mvvm
SQL Server
Entity Framework Core (Code First)
Fluent API
Microsoft Generic Host
Dependency Injection
Microsoft.Extensions.Configuration
Microsoft.Extensions.Logging
Serilog
4. Architecture

The application follows a Feature-Based Architecture.

It intentionally does NOT use:

Clean Architecture
Onion Architecture
CQRS
MediatR
Vertical Slice Architecture

The goal is a simple, scalable, maintainable enterprise architecture.

5. Solution Structure
VeteranLogistics.sln

│
├── VeteranLogistics.Desktop
│
├── VeteranLogistics.Data
│
├── VeteranLogistics.Reporting
│
└── VeteranLogistics.Shared
6. Current Project Structure
veteran_logistic
│
├── Authentication
├── Authorization
├── Configuration
├── DependencyInjection
├── Desktop
├── FinancialYear
├── Logs
├── Migrations
├── MVVM
├── Options
├── Resources
├── Styles
├── Themes
├── VeteranLogistics.Data
├── VeteranLogistics.Shared
│
├── App.xaml
├── MainWindow.xaml
├── appsettings.json
├── AssemblyInfo.cs
└── Product_Requirements_Document.txt
7. Development Roadmap

The project is divided into 7 major phases.

Phase 0 — Foundation & Infrastructure

Status: ✅ Completed & Frozen

Objective

Build the complete technical foundation required before implementing any business functionality.

Completed
Application Foundation
Generic Host
Application Bootstrap
Configuration Infrastructure
Dependency Injection
Environment Configuration
Database
SQL Server Integration
Entity Framework Core
Code First Approach
DbContext
Fluent API
Migration Infrastructure
Shared Infrastructure
Result Pattern
Validation Models
Common Exceptions
Shared Utilities
Constants
Enums
Logging
Microsoft Logging
Serilog
Global Exception Handling
Centralized Logging
MVVM Infrastructure
CommunityToolkit.Mvvm
ViewModelBase
ObservableValidatorBase
Property Change Notification
Navigation
Shell
Navigation Service
ViewModel Factory
ViewModel-first Navigation
Desktop Infrastructure
MainWindow
ShellView
Dialog Service
Notification Service
UI Foundation
Theme Infrastructure
Resource Dictionaries
Colors
Brushes
Typography
Spacing
Control Styles
Design System
Phase 1 — Authentication & Security

Status: ✅ Completed & Frozen

Objective

Implement complete application security, authentication, authorization, session management, and startup workflow.

Phase 1.1 — Authentication Infrastructure
Completed
Authentication Contracts
Authentication Models
Runtime Authentication State
Session Contracts
Dependency Injection
Phase 1.2 — Authentication Persistence & Security
Completed
User Repository
Password Hashing (PBKDF2)
Password Policy
Authentication Audit
Authentication Options
Default Administrator Seeding
User Entity
Users DbSet
Phase 1.3 — Login UI
Completed
Login Screen
Username
Password
Password Visibility
Remember Me
Validation
Loading State
Error Message Area
Phase 1.4 — User Authentication
Completed
Authentication Service
Login Validation
Password Verification
Authentication Audit
Session Creation
Runtime Authentication State
Navigation Workflow
Phase 1.5 — Remember Me
Completed
Username Persistence
JSON Storage
Restore Username
Restore Checkbox
Security

Never stores:

Password
Password Hash
Salt
Session
Token
Phase 1.6 — Financial Year Selection
Completed
Financial Year Module
Financial Year Repository
Financial Year Service
Financial Year Context
Financial Year Selection Screen

Workflow

Login
    ↓
Financial Year
    ↓
Shell
Phase 1.7 — Session Management
Completed
Session Metadata
Session Validation
Session Lifecycle
Session Events
Session Authority
Phase 1.8 — Role Based Authorization
Completed
Application Roles
Role Authorization Service
Administrator Detection
Runtime Role Validation
Phase 1.9 — Permission Based Authorization
Completed
Strongly Typed Permissions
Permission Provider
Permission Authorization
Runtime Permission Validation
Phase 1.10 — Logout
Completed
Logout Service
Session Cleanup
Authentication Reset
Financial Year Cleanup
Navigation to Login
Current Application Workflow
Application

↓

Login

↓

Authenticate User

↓

Financial Year Selection

↓

Application Shell

↓

Business Modules

↓

Logout

↓

Back to Login
Phase 2 — Administration

Status: ⏳ Pending

Objective

Manage application users, security, roles, permissions, and financial years.

Planned Modules
Users
User List
Add User
Edit User
Delete User
Activate/Deactivate
Reset Password
Lock/Unlock
Search
Filters
Roles
Role Management
Create
Edit
Delete
Clone Role
Role Configuration
Permissions
Permission Matrix
Module Permissions
CRUD Permissions
Export Permissions
Print Permissions
Role Permission Mapping
Financial Year Administration
Financial Year CRUD
Open Financial Year
Close Financial Year
Lock Financial Year
Default Financial Year
Validation Rules
Phase 3 — Masters

Status: ⏳ Pending

Objective

Manage all master data used throughout the ERP.

Planned Modules
Company
Customer
Vendor
Source
Destination
Material
Fuel Pump
HSD Rate
Payment Location
Vehicle Owner
Vehicle
Vehicle Assignment
Vehicle Owner Transfer
DO Rate Setup

Each module will include:

List Screen
Add
Edit
Delete
Search
Filters
Validation
Active/Inactive Status
Phase 4 — Transactions

Status: ⏳ Pending

Objective

Implement all operational transport workflows.

Planned Modules
Loading Advice
Loading Register
Vehicle Block
Vehicle Release
Cash Advance
Fuel Pump Bill
Unloading Register
Payment Register
Party Billing

Each transaction will include:

Entry Screens
Edit
Delete
Search
Validation
Business Rules
Workflow Processing
Printing
Audit Logging
Phase 5 — Reports

Status: ⏳ Pending

Objective

Provide complete operational and management reporting.

Planned Reports
Loading Report
Unloading Report
Payment Report
Partywise Billing
TDS Report
Bank Report
Consolidated Report
DO Status Report
Query Builder

Each report will support:

Filters
Sorting
Searching
Print
PDF Export
Excel Export
Phase 6 — Dashboard

Status: ⏳ Pending

Objective

Provide management with a real-time overview of business operations.

Planned Dashboard Components
KPI Cards
Revenue
Expenses
Trips
Active Vehicles
Pending Payments
Charts
Revenue Trend
Monthly Trips
Fuel Expenses
Vehicle Utilization
Recent Activities
Latest Transactions
User Activity
System Events
Pending Work
Pending Loading
Pending Unloading
Pending Payments
Pending Bills
Notifications
Alerts
Expiring Documents
Vehicle Due Dates
System Notifications
8. Standard Feature Structure

Every business feature follows the same structure.

Feature
│
├── Views
│
├── ViewModels
│
├── Services
│
├── Validators
│
└── Models (when required)

This applies to:

Authentication
FinancialYear
Administration
Masters
Transactions
Reports
Dashboard
9. Development Standards

The following standards apply throughout the project:

Production-quality implementation
Feature-Based Architecture
MVVM Pattern
Constructor Dependency Injection
Async/Await
Entity Framework Core (Code First)
Fluent API
Nullable Reference Types
Structured Logging (Serilog)
Centralized Exception Handling
ViewModel-first Navigation
Resource Dictionary–based UI
No business logic in Views
No direct MessageBox usage outside the Dialog Service
No direct DbContext usage in Views or ViewModels
Database schema managed only through EF Core Migrations
Enterprise-grade coding standards and naming conventions
10. Project Status
Phase	Description	Status
Phase 0	Foundation & Infrastructure	✅ Completed & Frozen
Phase 1	Authentication & Security	✅ Completed & Frozen
Phase 2	Administration	⏳ Pending
Phase 3	Masters	⏳ Pending
Phase 4	Transactions	⏳ Pending
Phase 5	Reports	⏳ Pending
Phase 6	Dashboard	⏳ Pending
11. Current Completion Summary
✅ Completed
Enterprise Application Foundation
Configuration & Dependency Injection
Database Infrastructure (EF Core + SQL Server)
Shared Infrastructure
Logging & Exception Handling
MVVM Infrastructure
Navigation Framework
Shell & Desktop Infrastructure
Theme & Design System
Authentication
Login Workflow
Remember Me
Financial Year Selection
Session Management
Role-Based Authorization
Permission-Based Authorization
Logout Workflow
⏳ Pending
Administration Module
Master Data Management
Transaction Modules
Reporting Module
Dashboard & Analytics
12. Long-Term Goal

By the end of development, Veteran Logistics will be a complete enterprise-grade Logistics & Transport Management ERP capable of managing:

User Administration
Security & Authorization
Company & Master Data
Fleet & Vehicle Management
Loading & Unloading Operations
Billing & Payments
Financial Year Operations
Reporting & Analytics
Dashboard & KPIs

The project is being developed with a strong emphasis on scalability, maintainability, clean code, and production readiness, following a consistent Feature-Based Architecture suitable for a large-scale desktop ERP application.