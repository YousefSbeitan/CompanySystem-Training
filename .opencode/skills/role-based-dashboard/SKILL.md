---
name: role-based-dashboard
description: Build a professional role-based dashboard for the Company System. Render different dashboard experiences for Admin, Manager, and Employee instead of hiding elements. Improve user profile, statistics, quick actions, and dashboard layout while preserving existing backend APIs and authorization.
---

# Role-Based Dashboard Skill

## Goal

Transform the current dashboard from a single generic page into a role-aware dashboard.

Instead of displaying one dashboard and hiding some cards using JavaScript, build a dedicated experience for every role.

The dashboard should immediately communicate to the logged-in user:

- Who they are
- What they can access
- What actions they can perform
- What information matters to them

The dashboard must feel like a real enterprise system.

---

# General Rules

Do NOT modify:

- Authentication
- Authorization
- JWT
- Services
- Business Layer
- Repository Layer
- Database

Only modify:

Views

wwwroot/js

wwwroot/css

Layout rendering if necessary.

---

# Current Problem

The current dashboard renders:

User Card

↓

Statistics

↓

Management Modules

for every authenticated user.

After rendering, JavaScript hides some modules according to role.

This creates several problems:

Employee users see empty sections.

Employees see management-related titles.

Statistics remain visible even when meaningless.

Dashboard cards are empty when no permissions exist.

Large blank spaces appear.

Overall UX feels unfinished.

---

# Required Architecture

Dashboard rendering must become role-aware.

The page should determine the logged-in role.

Then render one of three dashboard layouts.

Admin Dashboard

Manager Dashboard

Employee Dashboard

Never render unnecessary sections.

Do not rely on hiding UI with CSS.

Instead:

Render only what belongs to the current role.

---

# Admin Dashboard

Admin is responsible for the whole company.

Dashboard should contain:

User Profile Card

System Statistics

Users

Departments

Roles

Content Sections

Management Modules

Users

Departments

Roles

Notes

Main Page Sections

Quick Management Actions

Create User

Create Department

Create Role

Create Section

Visual style should feel administrative.

---

# Manager Dashboard

Manager is responsible for his own team.

Dashboard should NOT look like Admin.

It should focus on daily management.

Show:

User Profile Card

Team Statistics

My Employees

Departments I Manage

Notes

Quick Actions

Add Note

View Team

My Department

Employee List

Visible Modules

Users

Departments

Notes

Hide completely:

Roles

Main Page Sections

Content Management

Role statistics

System-wide actions

Manager dashboard should communicate:

"I manage my team."

---

# Employee Dashboard

Employee should never see management interfaces.

Employee dashboard should feel personal.

Show:

User Profile

Username

Role

Department

Leader

Years Of Experience

Phone Number

Status

Personal Actions

My Notes

My Profile

Company Content

Recent Company Announcements

About Company

Services

Latest News

Useful Links

Quick Actions

View Notes

Edit Profile (if allowed)

Company Information

Do NOT display:

Users

Departments

Roles

Management Modules

System Statistics

Admin Cards

Manager Cards

No empty placeholders.

No blank sections.

No hidden cards.

The employee dashboard should resemble an employee portal instead of an administration panel.

---

# User Profile Card

Replace the current placeholder implementation.

Never display:

Loading...

Placeholder bars

Empty ID badges

Instead show real data:

Avatar

Username

Role

Department

Leader

User ID

Years Of Experience

Phone Number

Account Status

If any field is unavailable display:

—

instead of Loading...

---

# Statistics

Statistics should depend on role.

Admin

Users

Departments

Roles

Content Sections

Manager

My Employees

Departments Managed

My Notes

Employee

No management statistics.

Instead display:

Personal Information

Last Login (if available)

Company Information

Recent Notes

If there are no statistics for a role,

do not render the statistics section.

---

# Dashboard JavaScript Architecture

Refactor dashboard.js into small rendering functions.

Avoid one large initialization function.

Required functions:

renderAdminDashboard()

renderManagerDashboard()

renderEmployeeDashboard()

renderUserProfile()

renderStatistics()

renderQuickActions()

renderModules()

renderCompanyInformation()

renderRecentNotes()

Each renderer should only build the UI that belongs to its role.

Never build all components and hide them later.

---

# Role Detection

Use the authenticated user information provided by build-auth-ui.

Read:

window.currentUser

window.currentUserRole

Never decode JWT again.

Never create another authentication system.

---

# Module Rendering Rules

Build modules dynamically.

Only include modules that actually exist.

Inspect existing MVC controllers.

Examples:

UserController

DepartmentController

RoleController

NoteController

MainPageSectionController

Generate cards only for controllers that exist.

Never invent modules.

---

# Navigation

Sidebar should also become role-aware.

Admin

Dashboard

Users

Departments

Roles

Notes

Main Page Sections

Manager

Dashboard

Users

Departments

Notes

Employee

Dashboard

Notes

Main Page Sections

Hide everything else.

Do not leave empty sections.

---

# Dashboard Cards

Cards should display:

Icon

Title

Description

Button

Hover animation

Responsive layout

Cards must never navigate to JSON endpoints.

Always navigate to MVC pages.

Examples:

/User

/Department

/Role

/Note

/MainPageSection

Never:

/GetAll

/GetById

/Create

/Edit

---

# User Information

Populate profile card using:

window.currentUser

Display:

Username

Role

User ID

Avatar Letter

If additional information is available from existing APIs,

display it.

Otherwise display:

—

Never display:

Loading...

Placeholder

Undefined

Null

---

# CSS Rules

Improve dashboard appearance.

Use modern cards.

Rounded corners.

Soft shadows.

Consistent spacing.

Responsive layout.

Bootstrap 5 compatible.

Do not modify Bootstrap.

Only extend it.

---

# Performance

Do not load data that will never be shown.

Example:

Employee dashboard

must not call:

/User/GetAll

/Department/GetAll

/Role/GetAll

Only request endpoints required by the current role.

Reduce unnecessary AJAX requests.

---

# Accessibility

Buttons must be keyboard accessible.

Navigation must support focus.

Icons must have accessible labels where appropriate.

Use semantic HTML.

---

# Error Handling

If an endpoint fails:

Show a friendly message.

Do not leave placeholders.

Do not leave empty cards.

Do not display JavaScript errors.

---

# Cooperation With Existing Skills

This skill works after:

build-auth-ui

build-ui

build-dashboard-ui

fix-ui-integration

fix-mvc-architecture

fix-dashboard-ui

It must not duplicate their responsibilities.

It only improves dashboard rendering and user experience.

---

# Final Verification

Before finishing verify:

✓ Dashboard renders correctly for Admin

✓ Dashboard renders correctly for Manager

✓ Dashboard renders correctly for Employee

✓ User profile displays real information

✓ Empty placeholders removed

✓ No unnecessary sections rendered

✓ Sidebar changes according to role

✓ Navigation links open MVC pages only

✓ No JSON endpoints are opened by navigation

✓ No backend code modified

✓ No authentication logic modified

✓ Existing CRUD pages still work

✓ Existing Login/Register still work

Run:

dotnet build

Required result:

0 build errors

0 Razor errors

0 JavaScript errors

Dashboard production-ready.