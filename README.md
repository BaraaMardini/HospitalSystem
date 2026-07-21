# Hospital Management System API

## Overview
An ASP.NET Core Web API backend for managing hospital operations — patients, doctors, appointments, rooms, and billing — built on a SQL Server database with over 100 stored procedures. The API is front-end agnostic and designed to serve any client application.

## Main Features
- Full CRUD endpoints for core hospital entities (appointments, rooms, patients, doctors, invoices).
- Appointment scheduling with doctor availability checks, room availability checks, and time-conflict validation.
- Transactional appointment booking that atomically creates an appointment and its associated room booking.
- Standardized API responses with consistent success/error payloads across all endpoints.
- Foreign key and business-rule validation enforced at the stored procedure level (e.g., invalid patient, invalid doctor schedule, overlapping bookings).

## Architecture / System Design
The project follows a layered backend architecture:

```
Controllers → Services → Data Access Layer → SQL Server (Stored Procedures)
Models: DTO / UpdateDTO / ViewDTO
```

- **Controllers** expose REST endpoints and delegate all logic to the Service layer.
- **Services** apply business rules (e.g., "no results found" handling) before/after calling the Data Access Layer.
- **Data Access Layer** communicates with SQL Server via ADO.NET, using stored procedures exclusively — no inline dynamic SQL in application code.
- **Models** are split into `DTO` (input for create), `UpdateDTO` (partial/nullable fields for update), and `ViewDTO` (read-optimized, joined data for display).

A shared `ApiResult<T>` / `ErrorType` / `ApiResponseHelper` pattern maps database-level error codes to appropriate HTTP status codes (200, 201, 400, 404, 409, 500) consistently across the entire API.

## Technologies Used
- ASP.NET Core Web API
- C#
- SQL Server / T-SQL
- ADO.NET (`SqlConnection`, `SqlCommand`, output parameters)
- Stored Procedures with explicit transaction handling

## Database Design Explanation
The schema models real hospital relationships: `Appointment` links `Patient`, `Doctor`, `Room`, and `Status`; `RoomBookings` tracks time-based room reservations tied to an `Appointment`; `Invoices` and `InvoicePayments` handle billing tied to appointments and patients; `MedicalHistory` links back to `Patient` and the creating `User`. Foreign keys enforce referential integrity across roughly 20+ relationships (e.g., `Appointment.PatientID → Patient.ID`, `Appointment.RoomID → Room.ID`, `Invoices.AppointmentID → Appointment.ID`).

## Key Technical Implementations
- `SP_AddAppointment`: a transactional stored procedure that validates patient existence, doctor working days, room existence, requesting user, and time overlap (both doctor and room) before inserting into `Appointment` and `RoomBookings` inside a single SQL transaction with rollback on failure.
- Output parameters (`@Message`, `@ErrorType`, `@NewID`) used consistently across stored procedures to communicate result status back to the Data Access Layer without relying on exceptions for expected business failures.
- `ErrorTypeMapper` translates numeric database error codes into a strongly typed `ErrorType` enum consumed by the API layer.
- `ViewDTO` classes built from multi-table JOIN stored procedures (e.g., `SP_GetAllAppointment` joins `Doctor`, `Patient`, `People`, `Room`, `Status`, and `Users`) to avoid N+1 query patterns on read endpoints.

## Challenges Solved
- Preventing double-booking of a doctor or a room by validating time overlaps against existing `RoomBookings` before insert, inside a transaction to avoid race conditions.
- Keeping partial updates safe by using nullable fields in `UpdateDTO` combined with `COALESCE` in the `UPDATE` statements, so only supplied fields are changed.
- Returning meaningful, structured error information (not just generic 500s) for business rule violations like "doctor does not work this day" or "room already booked."

## What I Learned
- How to design and use SQL Server transactions correctly with `BEGIN TRY / BEGIN TRANSACTION / COMMIT / ROLLBACK` and `TRY/CATCH`.
- How to structure a layered .NET API so business logic, data access, and HTTP concerns stay separated.
- Practical experience mapping database error states to HTTP status codes through a centralized response helper.
- How to design DTOs specifically for their use case (create vs. update vs. read) rather than reusing one model everywhere.

## Future Improvements
- Add authentication and role-based authorization (doctor, receptionist, admin roles).
- Add pagination and filtering to list endpoints.
- Add automated integration tests around the stored procedure validation logic.

## How to Run
1. Restore the SQL Server database using the provided schema and stored procedures.
2. Update the connection string in `appsettings.json` (or `ConnectionString.cs`).
3. Build and run the ASP.NET Core project.
4. Use the exposed REST endpoints (e.g., `/api/appointments`) via Postman, Swagger, or any front-end client.
