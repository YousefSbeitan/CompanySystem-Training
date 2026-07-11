---
description: Build a fully role-based dashboard experience for CompanySystem. Creates a different dashboard for Admin, Manager and Employee without modifying backend logic.
agent: ui-builder
---

Load the role-based-dashboard skill and execute it.

Analyze the existing project before making changes.

Read:

- CompanySystem.Web/Controllers/HomeController.cs
- CompanySystem.Web/Views/Home/Dashboard.cshtml
- CompanySystem.Web/Views/Shared/_Layout.cshtml
- CompanySystem.Web/wwwroot/js/dashboard.js
- CompanySystem.Web/wwwroot/css/dashboard.css
- CompanySystem.Web/wwwroot/js/auth.js

Also inspect all MVC controllers to discover existing modules.

Examples:

- UserController
- DepartmentController
- RoleController
- NoteController
- MainPageSectionController

Do NOT assume modules.

Use only existing MVC controllers.

---

Objective

Transform the dashboard into a professional role-based dashboard.

Each role must receive its own experience.

Do NOT simply hide cards.

Generate different dashboard sections according to the authenticated user's role.

---

Authentication

Use only:

window.currentUser

window.currentUserRole

provided by auth.js.

Never decode JWT again.

Never create another authentication system.

---

Admin Dashboard

Generate a complete administration dashboard.

Display:

- Company Branding
- Welcome message
- User profile
- Role badge
- User ID
- Statistics cards
- Quick actions
- Management modules

Allowed modules:

- Users
- Departments
- Roles
- Notes
- Main Page Sections

Sidebar should contain only these modules.

---

Manager Dashboard

Generate a manager dashboard.

Display:

- Welcome message
- User profile
- Team overview
- Quick actions

Visible modules:

- Users
- Departments
- Notes

Hide:

- Roles
- Main Page Sections administration features

Statistics should include only manager-related information.

---

Employee Dashboard

Generate a lightweight employee dashboard.

Do NOT generate admin widgets.

Display:

- Welcome message
- Personal profile
- Current role
- User ID
- Personal notes shortcut
- Company announcements shortcut

Visible modules only:

- Notes
- Main Page Sections

Hide completely:

- Users
- Departments
- Roles

Employee dashboard should feel different from the admin dashboard.

---

Dashboard Rendering Rules

Render UI according to the current role.

Never render every section and hide it later.

Instead:

Generate only the required HTML.

---

Sidebar Rules

Sidebar must also become role-aware.

Admin:

Dashboard

Users

Departments

Roles

Notes

Main Page Sections

Manager:

Dashboard

Users

Departments

Notes

Employee:

Dashboard

Notes

Main Page Sections

Remove empty categories.

---

Profile Card

Always display:

Username

Role

User ID

Avatar

If more profile information exists and can be loaded from existing APIs, display it.

Otherwise display only available information.

Never display:

Loading...

Placeholder

Undefined

Null

---

Statistics

Load only statistics that belong to the current role.

Admin:

Users

Departments

Roles

Sections

Manager:

Users

Departments

Employee:

No management statistics.

Instead display personal shortcuts.

Never perform unnecessary AJAX requests.

---

Navigation

Every dashboard card must navigate to MVC pages only.

Examples:

/User

/Department

/Role

/Note

/MainPageSection

Never navigate directly to:

/GetAll

/GetById

/Create

/Edit

/Delete

Never open JSON pages.

---

JavaScript Refactoring

Refactor dashboard.js into smaller functions.

Suggested structure:

renderAdminDashboard()

renderManagerDashboard()

renderEmployeeDashboard()

renderSidebar()

renderProfile()

renderStatistics()

renderModules()

renderQuickActions()

renderCompanyBrand()

Each function should have one responsibility.

---

CSS Improvements

Improve:

Spacing

Cards

Typography

Responsiveness

Hover effects

Sidebar

Top navigation

Profile card

Statistics cards

Module cards

Keep Bootstrap 5 compatibility.

Do not modify Bootstrap.

---

Responsive Rules

Desktop

Tablet

Mobile

must all work correctly.

Sidebar should collapse properly.

Dashboard cards should wrap automatically.

---

Performance

Avoid unnecessary AJAX requests.

Load only data required by the current role.

Do not query endpoints that will never be shown.

---

Do NOT Modify

Never modify:

Controllers

Services

Repositories

Entities

DTOs

Business Layer

Data Layer

Authentication

Authorization

JWT

Claims

Program.cs

appsettings.json

DbContext

Migrations

---

Compatibility

Remain fully compatible with:

build-auth-ui

build-ui

build-dashboard-ui

fix-ui-integration

fix-mvc-architecture

fix-dashboard-ui

Do not overwrite their responsibilities.

Only improve the dashboard experience.

---

Final Validation

Before finishing verify:

✓ Admin dashboard is fully functional

✓ Manager dashboard is fully functional

✓ Employee dashboard is fully functional

✓ Sidebar changes according to role

✓ User profile displays correctly

✓ No placeholders remain

✓ Statistics are correct

✓ Only MVC pages are opened

✓ CRUD pages still work

✓ Login still works

✓ Register still works

✓ Logout still works

✓ No backend files modified

Run:

dotnet build

Build result must contain:

0 Errors

0 Razor Errors

0 JavaScript Errors