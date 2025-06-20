# 🏥 Medical Consultation Booking System – My First Razor Pages Project

> 🎉 **Milestone**: This project is my first full implementation using **ASP.NET Core Razor Pages**, focused on building an online medical consultation and communication system.

---

## 📖 Project Overview

This web-based system enables **patients to book online medical consultations** and communicate with doctors through an internal messaging system. Additionally, doctors can collaborate with nurses to prepare medical examination rooms via integrated chat.

The system simulates a real-world **remote medical environment** with proper role separation and communication flow.

---

## 🛠️ Tech Stack

| Category        | Technology                            |
|----------------|----------------------------------------|
| **Framework**   | ASP.NET Core Razor Pages               |
| **Database**    | SQL Server (Code First - EF Core)      |
| **Frontend**    | HTML, CSS, JavaScript                  |
| **Architecture**| Separated configuration classes, clean structure |
| **Tools**       | Visual Studio 2022, SQL Server Mgmt Studio |

---

## 🚀 Key Features

- 🧑‍⚕️ **Patients** can:
  - Book medical consultations online
  - Send messages describing symptoms or concerns to doctors
  - View doctor responses and medical feedback

- 👨‍⚕️ **Doctors** can:
  - Reply to patient messages
  - Communicate directly with assigned nurses to coordinate preparations

- 👩‍⚕️ **Nurses** can:
  - Receive instructions from doctors
  - Prepare virtual examination environments

- ✉️ **Integrated Messaging System**:
  - Patient ↔ Doctor communication
  - Doctor ↔ Nurse coordination

---

## 📬 Communication Workflow

```plaintext
Patient → Message → Doctor → Respond

Doctor → Request → Nurse → Prepare Room → Confirm
