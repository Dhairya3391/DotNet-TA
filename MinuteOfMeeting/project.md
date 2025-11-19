# 2301CS412 – ASP.NET Core Project Documentation

**Minutes of Meeting (MOM) Management**
Common Project Documentation

---

**PREPARED BY**
Prof. Madhuresh Fichadiya
Computer Science and Engineering
Darshan University

**Academic Year: 2025-26**

---

## Table of Contents

1. [Project Goals & Objectives](#1-project-goals--objectives)
   - Overview
   - Goals
   - Objectives
2. [Scope of the Project](#2-scope-of-the-project)
3. [Project Timeline](#3-project-timeline)
4. [Database Overview](#4-database-overview)
5. [Functional Requirements](#5-functional-requirements)
6. [Screen List](#6-screen-list)
7. [Screen Design Reference](#7-screen-design-reference)

---

## 1. Project Goals & Objectives

### Overview

- The MOM (Minutes of Meeting) Management System is designed to streamline how meetings are scheduled, recorded, and documented within an organization.
- The application should include CRUD operations, session management, filters & validations for all required screens.

### Goals

#### • Design a Normalized Database Structure:
- Create efficient and normalized tables to manage MeetingType, Staff(Member), Department, MeetingVenues, Meetings and MeetingMembers.
- Implement proper primary and foreign key relationships, including a many-to-many association as and when required.

#### • Implement CRUD & Authentication Functionality:
- Develop Create, Read, Update, and Delete operations for all major entities.
- Ensure smooth user interaction and data management through forms and listings with login/logout & user registration.

#### • Ensure Data Integrity and Validation:
- Apply appropriate validations on input fields and ensure data consistency.

#### • Develop an Intuitive User Interface:
- Provide a clean and easy-to-use frontend for managing hospital operations.

#### • Encourage Good Software Development Practices:
- Apply modular coding, separation of concerns, and basic MVC principles.

### Objectives

#### • Meeting Type Management:
- Enable users to create, view, edit, and delete various meeting types (e.g., review meetings, planning sessions, briefings) to categorize and structure organizational meetings.

#### • Member (Staff) Management:
- Allow creation and management of staff member profiles, including their personal information, roles, contact details, and departmental assignments.

#### • Department Management:
- Provide functionality to create and maintain departments, and associate one or more staff members with each department.

#### • Meeting Venue Management:
- Enable users to add, update, view, and delete meeting venues, including physical rooms or virtual meeting links, ensuring availability and proper resource allocation.

#### • Meeting Scheduling and Management:
- Facilitate the scheduling, modification, viewing, and cancellation of meetings, including assigning meeting types, venues, departments involved, and linking attending staff members.

#### • Meeting–Member Association:
- Maintain a many-to-many relationship between meetings and staff members using a mapping table (MeetingMembers), allowing multiple attendees per meeting and enabling tracking of participation.

#### • Data Integrity and Validation:
- Ensure accurate, consistent data entry through proper validations, relational constraints, and field checks to prevent scheduling conflicts, invalid staff assignments, or improper meeting configurations.

---

## 2. Scope of the Project

### • Member and Department Management
- Ability for administrators to manage staff/member records, including roles and contact details.
- Create and manage departments with functional or specialization tags.
- Associate staff members with one or more departments where applicable.

### • Meeting Type and Venue Management
- Create, update, and manage various meeting types (e.g., Review, Planning, Audit, Training).
- Manage meeting venues including physical rooms and virtual meeting links.
- Ensure venues can be selected and assigned for scheduled meetings.

### • Meeting Scheduling and Minutes Recording
- Schedule meetings by selecting meeting type, venue, departments involved, and participating staff.
- View meetings filtered by department, meeting type, venue, or staff member.
- Update, cancel, or reschedule meetings with status tracking.
- Record meeting minutes, decisions taken, issues raised, and action items.

### • Meeting Attendance and Participation Tracking
- Maintain attendance using MeetingMembers to associate multiple staff members with each meeting.
- Track who attended, who was absent, and roles of attendees (e.g., organizer, presenter, participant).

### • Dashboard and Data Overview/Statistics
- Display key information such as upcoming meetings, recently completed meetings, venue availability, and department-wise meeting counts.
- Show quick insights such as most frequent meeting types, busiest departments, or staff participation statistics.

### • Input Validation and Error Handling
- Ensure all meeting-related inputs (date/time, venue, participants) are validated for correctness and conflicts.
- Provide meaningful error messages, warnings, and user-friendly alerts for invalid or conflicting actions (e.g., double-booked venues or overlapping meetings).

---

## 3. Project Timeline

| Week No. | Week Date | Task List |
|----------|-----------|-----------|
| 1 | 01-12-25 to 06-12-25 | Study and Analysis of Existing MoM/Scheduling Systems for the understanding of project & Database Design |
| 2 | 08-12-25 to 13-12-25 | Database Schema Design and Stored Procedure Creation |
| 3 | 15-12-25 to 20-12-25 | Start Screen Design with a Common Layout |
| 4 | 22-12-25 to 27-12-25 | Screen Designs for List Pages and Add/Edit Pages |
| 5 | 29-12-25 to 03-01-26 | Model Class Preparation with Data Annotations and Validations |
| 6 | 05-01-26 to 10-01-26 | CRUD Operation for a Single Module/Table |
| 7 | 12-01-26 to 17-01-26 | CRUD Operations for 2-3 Modules/Tables |
| 8 | 19-01-26 to 24-01-26 | CRUD Operations for 2 Advanced Modules/Tables (Meetings & MeetingMembers) |
| 9 | 26-01-26 to 31-01-26 | Dashboard Design and Implementation |
| 10 | 02-02-26 to 07-02-26 | Export Functionality Implementation for All List Pages |
| 11 | 09-02-26 to 14-02-26 | Session Management – Login, Logout, and User Registration |
| 12 | 16-02-26 to 21-02-26 | Final Evaluation |

---

## 4. Database Overview

### MOM_MeetingType

| Column Name | Data Type | Remarks |
|-------------|-----------|---------|
| MeetingTypeID | Int | PK, AutoIncrement |
| MeetingTypeName | Nvarchar(100) | Not Null |
| Remarks | Nvarchar(100) | Not Null |
| Created | DateTime | Default GetDate() |
| Modified | DateTime | Not Null |

### MOM_Department

| Column Name | Data Type | Remarks |
|-------------|-----------|---------|
| DepartmentID | Int | PK, AutoIncrement |
| DepartmentName | Nvarchar(100) | Not Null |
| Created | DateTime | Default GetDate() |
| Modified | DateTime | Not Null |

### MOM_MeetingVenue

| Column Name | Data Type | Remarks |
|-------------|-----------|---------|
| MeetingVenueID | Int | PK, AutoIncrement |
| MeetingVenueName | Nvarchar(100) | Not Null |
| Created | DateTime | Default GetDate() |
| Modified | DateTime | Not Null |

### MOM_Meetings

| Column Name | Data Type | Remarks |
|-------------|-----------|---------|
| MeetingID | Int | PK, AutoIncrement |
| MeetingDate | DateTime | Not Null |
| MeetingVenueID | Int | FK MeetingVenue, Not Null |
| MeetingTypeID | Int | FK MeetingType, Not Null |
| DepartmentID | Int | FK Department, Not Null |
| MeetingDescription | Nvarchar(250) | Allow Null |
| DocumentPath | Nvarchar(250) | Allow Null |
| Created | DateTime | Default GetDate() |
| Modified | DateTime | Not Null |
| IsCancelled | Bit | Allow Null |
| CancellationDateTime | DateTime | Allow Null |
| CancellationReason | Nvarchar(250) | Allow Null |

### MOM_Staff

| Column Name | Data Type | Remarks |
|-------------|-----------|---------|
| StaffID | Int | PK, AutoIncrement |
| DepartmentID | Int | FK Department, Not Null |
| StaffName | Nvarchar(50) | Not Null |
| MobileNo | Nvarchar(20) | Not Null |
| EmailAddress | Nvarchar(50) | Not Null |
| Remarks | Nvarchar(250) | Allow Null |
| Created | DateTime | Not Null, Default GetDate() |
| Modified | DateTime | Not Null |

### MOM_MeetingMember

| Column Name | Data Type | Remarks |
|-------------|-----------|---------|
| MeetingMemberID | Int | PK, AutoIncrement |
| MeetingID | Int | FK Meeting, Not Null |
| StaffID | Int | FK Staff, Not Null |
| IsPresent | Bit | Not Null |
| Remarks | Nvarchar(250) | Allow Null |
| Created | DateTime | Not Null, Default GetDate() |
| Modified | DateTime | Not Null |

---

## 5. Functional Requirements

### Meeting Type Management (Priority – 3)
- Manage (Create, View, Edit, Delete) meeting types
- Prevent duplicate MeetingTypeName entries
- Maintain audit fields (Created, Modified) automatically

### Staff (Member) Management (Priority – 3)
- CRUD operations
- Prevent duplicate emails
- Ensure valid department assignment

### Department Management (Priority – 4)
- Manage all departments
- Prevent duplicate department names

### Meeting Venue Management (Priority – 3)
- Manage meeting room/venue records
- Prevent duplicate names
- Ensure valid capacity entry

### Meetings Management (Priority – 1)
- Complete meeting scheduling
- Conflict checking for venue & time
- Track modifications via timestamps

### Meeting Members / Attendance Management (Priority – 2)
- Many-to-many mapping between Meetings & Staff
- Track attendance
- Prevent duplicate Staff entries for same meeting

---

## 6. Screen List

### 1. Authentication & Dashboard
1. **Login Page**: Role-based authentication (Admin / Meeting Organizer (Convener) / Staff)
2. **Dashboard**: Overview of upcoming, completed, and cancelled meetings. Displays meeting stats, recent MOMs, and pending follow-ups
3. **Profile**: View/update user details (staff/student)

### 2. Master Configuration
- Meeting Type
- Staff
- Department
- Venue

### 3. Meeting Management
- Creation/Edit of Meetings
- Cancel Meetings
- Meeting List / Calendar View
- Meeting Detailed View

### 4. Attendance & Participants
- Add Meeting Members
- Mark Attendance
- View Attendance Summary

### 5. Reports & Analytics
- Meeting Summary Report
- Meeting Wise Report
- Export to Excel / PDF

---

## 7. Screen Design Reference

### Login Page
*[Design reference from original PDF - Page 9]*

### Sign-Up Page
*[Design reference from original PDF - Page 10]*

### Dashboard
*[Design reference from original PDF - Page 10]*

### List Pages
- Doctor List Page *(reference page 11)*
- Patient List Page *(reference page 12)*
- Doctor-Department List Page *(reference page 13)*
- Appointment List Page *(reference page 14)*

### Add/Edit Pages
- Doctor Add/Edit Page *(reference page 11)*
- Patient Add/Edit Page *(reference page 12)*
- Doctor-Department Add/Edit Page *(reference page 13)*
- Appointment Add/Edit Page *(reference page 14)*

---

**Note**: The screen design references (Doctor, Patient, Appointment) appear to be template examples from a hospital management system that can be adapted for the MOM system's corresponding screens (MeetingType, Staff, Department, Venue, Meetings, MeetingMembers).

---

*End of Document*
