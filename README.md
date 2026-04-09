# 🏥 Hospital Management System - Backend

## 📌 Overview

This project is a **robust backend system** for managing hospital operations, built using **ASP.NET Core Web API** and **SQL Server**.
It follows a clean, scalable architecture and handles complex business logic such as appointments, room bookings, and validations.

---

## 🚀 Features

* Full CRUD operations using **108+ Stored Procedures**
* Advanced validation at database level
* Transaction handling for critical operations
* Clean layered architecture:

  * Controllers
  * Services
  * Data Access Layer
* Unified API response structure
* DTO pattern implementation:

  * AddDTO
  * UpdateDTO
  * ViewDTO
* Error handling with mapped error types
* High-performance data access using **ADO.NET**

---

## 🧱 Architecture

```
Controller → Service → Data Access → SQL Server (Stored Procedures)
```

* **Controller**: Handles HTTP requests & responses
* **Service Layer**: Business logic & validation
* **Data Layer**: Executes Stored Procedures using ADO.NET
* **Database**: Contains all business rules and constraints

---
## 🗄️ Database Setup

The project contains a **DataBase** folder with SQL scripts to create the schema.  

1. Navigate to: `DataBase/SQLScript/HospitalSchema.sql`  
2. Manually create a new database in SQL Server called **Hospital**  
3. Run the script `HospitalSchema.sql` to create all tables, relations, and stored procedures.  

> ⚠️ Note: The backend will not work unless the database is created and the script is executed.

---

### 🔹 Connection String Configuration

Before running the project, make sure to update your connection string in `appsettings.json` according to your SQL Server setup:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Hospital;Trusted_Connection=True;Encrypt=False"
}

```




## 🗄️ Database Design

* Built using **SQL Server**
* Uses **Foreign Keys** to maintain relational integrity
* Includes entities such as:

  * Patients
  * Doctors
  * Appointments
  * Rooms
  * Invoices
  * Users

### 🔹 Key Features:

* Strong relational structure
* Business rules enforced inside Stored Procedures
* Conflict handling (Doctor schedule, Room booking)

---

## ⚙️ Example: Appointment Creation Logic

When creating an appointment:

* ✅ Validate Patient exists
* ✅ Validate Doctor schedule
* ✅ Validate Room availability
* ✅ Check time conflicts
* ✅ Ensure StartDate < EndDate
* ✅ Insert Appointment + RoomBooking inside a **Transaction**

---

## 🔁 API Response Structure

All endpoints return a unified response:

```json
{
  "data": {},
  "message": "Operation status message",
  "errorCode": 0
}
```

### Error Types:

| Code | Meaning            |
| ---- | ------------------ |
| 0    | Success            |
| 1    | Invalid ID         |
| 2    | Not Found          |
| 3    | Already Exists     |
| 4    | Logical Dependency |
| 5    | Database Error     |

---

## 📡 API Endpoints (Example)

### Appointment

* `GET /api/appointments/all`
* `GET /api/appointments/{id}`
* `POST /api/appointments`
* `PUT /api/appointments/{id}`
* `DELETE /api/appointments/{id}`

---

## 🧪 Technologies Used

* ASP.NET Core Web API
* ADO.NET
* SQL Server
* Stored Procedures
* C#

---

## 💡 Highlights

* Strong focus on **data integrity**
* Clean and maintainable architecture
* Scalable backend ready for any frontend integration
* Enterprise-level error handling approach

---

## 🔮 Future Improvements

* Add frontend (Angular / React)
* Implement authentication & authorization (JWT)
* Add caching for performance optimization
* Logging & monitoring

---

## 👨‍💻 Baraa Mardini

Backend Developer passionate about building scalable and clean systems.

---

## ⭐ Project Status

✔ Backend Completed
⏳ Frontend Coming Soon

---
