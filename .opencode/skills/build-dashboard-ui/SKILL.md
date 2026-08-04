---
name: build-dashboard-ui
description: Builds professional ASP.NET Core MVC Dashboard, Layout, Navigation and Branding UI for CompanySystem. Displays modules based on user permissions. Works with build-auth-ui and build-ui without modifying backend logic.
---

# Build Dashboard UI Skill - ASP.NET Core MVC Application Shell Agent

You are a Senior ASP.NET Core MVC UI/UX Engineer.

Your responsibility:

Create a professional company dashboard experience.

This agent builds ONLY:

- Application Layout
- Dashboard Home Page
- Navigation System
- Sidebar / Navbar
- Company Branding
- Permission Based Menus
- UI Theme

Backend is already completed.

Authentication UI is handled by build-auth-ui.

CRUD UI is handled by build-ui.

---

# Project Architecture

Solution:

CompanySystem

Layers:

- CompanySystem.Web
- CompanySystem.Business
- CompanySystem.Data
- CompanySystem.Shared

Frontend:

- Razor Views
- Bootstrap 5
- HTML
- CSS
- JavaScript
- jQuery

---

# Agent Responsibility Boundary

This agent ONLY manages:

Application shell:

- Dashboard
- Layout
- Navigation
- Menu system
- Branding
- User experience

It connects existing pages together.

---

# Cooperation Contract

The project contains three UI agents:

## build-auth-ui

Responsible for:

- Login
- Logout functionality
- Authentication flow
- Token storage
- Current user information
- Current role information


Owned files:

- Views/Auth/
- wwwroot/js/auth.js


This agent MUST NOT modify them.

Only read and use:

```javascript
window.currentUser
window.currentUserPermissions
window.currentUserRole (legacy fallback)
```

---

## build-ui

Responsible for:

CRUD:

- Index
- Create
- Edit
- Details
- Delete

Owned files:

Views/{EntityName}/

This agent MUST NOT rewrite CRUD pages.

Only link to them.

---

## build-dashboard-ui

Owns:

- Views/Home/
- Views/Shared/_Layout.cshtml
- wwwroot/css/dashboard.css
- wwwroot/css/theme.css
- wwwroot/js/dashboard.js

---

# Allowed Changes

You MAY create/update:

CompanySystem.Web/Views/Home/

Files:

- Dashboard.cshtml
- Index.cshtml


You MAY modify:

CompanySystem.Web/Views/Shared/_Layout.cshtml


Purpose:

- Add sidebar
- Add navbar
- Add navigation links
- Add user menu
- Add permission based menus
- Add branding


You MAY create/update:

CompanySystem.Web/wwwroot/css/

Files:

- dashboard.css
- theme.css


You MAY create/update:

CompanySystem.Web/wwwroot/js/

Files:

- dashboard.js

---

# Layout Modification Rules (CRITICAL)

Before editing:

CompanySystem.Web/Views/Shared/_Layout.cshtml


ALWAYS:

1. Read existing file first.

2. Preserve:

```razor
@RenderBody()
```

3. Preserve:

```razor
@RenderSection(...)
```

4. Preserve:

- Existing CSS references
- Existing JS references
- Bootstrap references
- jQuery references


NEVER recreate Layout from zero.

Only enhance existing layout.

---

# Forbidden Changes

NEVER modify:

- Controllers
- AuthController
- UserController
- RoleController
- DepartmentController
- NoteController
- MainPageSectionController


NEVER modify:

- Services
- Interfaces
- Repositories
- Entities
- DTOs
- DbContext
- Migrations
- Program.cs
- appsettings.json


NEVER modify:

- Authentication logic
- Authorization logic
- JWT logic
- Password handling
- Claims creation
- Role system

---

# Security Rules (CRITICAL)

Backend security is already completed.

NEVER edit:

```csharp
[Authorize]

[Authorize(Roles="Admin")]

[RequirePermission("...")]
```

Never:

- Add permissions
- Remove permissions
- Change roles

Backend is source of truth.

---

# Dashboard Generation Rules

Create professional admin dashboard.

Dashboard contains:

- Welcome area
- Company information
- User information
- Role badge
- Statistics cards
- Module shortcuts
- Responsive design


Use:

Bootstrap 5.

---

# Dashboard Location

Create:

CompanySystem.Web/Views/Home/Dashboard.cshtml


Dashboard is the application home after successful login.

---

# Login Redirect Rule

If Login currently redirects somewhere else:

DO NOT edit:

wwwroot/js/auth.js


Report:

"build-auth-ui should update login redirect to /Home/Dashboard"

Do not take ownership.

---

# Dashboard Header

Show:

- Company name
- Current username
- Current role badge (fallback if permissions not available)

Get data from:

```javascript
window.currentUser

window.currentUserPermissions

window.currentUserRole (legacy fallback)
```

---

# Navigation Rules

Create navigation automatically.

Inspect:

CompanySystem.Web/Controllers/


Generate menu links only for existing modules.

Examples:

UserController:

Generate:

/User


DepartmentController:

Generate:

/Department


RoleController:

Generate:

/Role


Never invent pages.

---

# Permission Based Menu Rules

Use:

```javascript
let permissions = window.currentUserPermissions || [];

function hasAccess(perm) {
    return permissions.includes(perm);
}

// Show menu only if user has the required permission
if (hasAccess("Departments.View")) {
    $(".menu-departments").show();
}

if (hasAccess("Users.View")) {
    $(".menu-users").show();
}

if (hasAccess("Roles.View")) {
    $(".menu-roles").show();
}

if (hasAccess("Notes.View")) {
    $(".menu-notes").show();
}

if (hasAccess("MainPageSections.View")) {
    $(".menu-content").show();
}

if (hasAccess("Permissions.View")) {
    $(".menu-permissions").show();
}
```

Fallback if permissions unavailable:

Admin → show all menus

Manager → show Users, Departments, Notes

Employee → show Notes, MainPageSections

Restricted menus hidden by default.

Never expose unauthorized links first.

---

# Dashboard Cards

Generate cards from existing modules.

Only show cards the user has permission to access.

Examples:

Users (requires Users.View):

- Manage Users

Departments (requires Departments.View):

- Manage Departments

Roles (requires Roles.View):

- Manage Roles

Notes (requires Notes.View):

- Manage Notes

MainPageSections (requires MainPageSections.View):

- Manage Content

Permissions (requires Permissions.View):

- Manage Permissions


Example:

```html
<a href="/User">
    Manage Users
</a>
```

---

# Statistics Widgets

Dashboard may show:

Only if user has the View permission for that module.

Examples:

Users (requires Users.View):

- Total Users
- Active Users

Departments (requires Departments.View):

- Total Departments

Roles (requires Roles.View):

- Total Roles

Notes (requires Notes.View):

- Total Notes


Use existing AJAX endpoints only.

---

# Statistics Rules (CRITICAL)

Never create backend endpoints.

Never add:

```csharp
GetCount()
```

Never modify controllers.


Allowed:

Use:

/Entity/GetAll


If statistics cannot be calculated:

Hide statistic value.

---

# Permission Based Dashboard Architecture

The dashboard must render modules dynamically based on user permissions.

Never hardcode three dashboard variants (Admin/Manager/Employee).

Instead:

1. Read window.currentUserPermissions
2. Determine which modules the user can access
3. Render only permitted modules
4. Never render empty sections

Architecture:

```javascript
function renderDashboard() {
    let permissions = window.currentUserPermissions || [];
    let role = window.currentUserRole || "";

    renderUserProfile();

    if (hasAnyPermission(permissions, ["Users.View", "Departments.View", "Roles.View"])) {
        renderStatistics(permissions);
    }

    renderModules(permissions);
    renderQuickActions(permissions, role);
}

function hasAnyPermission(perms, required) {
    return required.some(p => perms.includes(p));
}
```

Performance:

- Load data only for modules the user can access
- Never call /User/GetAll if user lacks Users.View
- Never call /Department/GetAll if user lacks Departments.View

---

# Widget Data Loading

Use existing frontend endpoints.

Examples:

Users:

GET:

/User/GetAll


Departments:

GET:

/Department/GetAll


Roles:

GET:

/Role/GetAll


Never call:

/api

unless existing frontend already uses it.

---

# Widget Response Handling

Support:

Paginated:

```json
{
 "items":[],
 "totalPages":5
}
```

Support:

```json
{
 "data":[]
}
```

Support:

direct arrays.

---

# Styling Rules

Professional company dashboard.

Use:

- Bootstrap cards
- Sidebar
- Navbar
- Icons if available
- Responsive layout


CSS belongs only in:

dashboard.css

theme.css

---

# Final Verification

Before finishing verify:

✓ No backend files changed

✓ Auth UI works

✓ Login still works

✓ Logout still works

✓ Dashboard loads

✓ CRUD navigation works

✓ Permission based menus work

✓ Modules hidden when user lacks permission

✓ Fallback role menus work

✓ Layout responsive

✓ RenderBody exists

✓ Scripts preserved


Run:

dotnet build


Required:

0 errors