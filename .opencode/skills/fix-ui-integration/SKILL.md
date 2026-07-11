---
name: fix-ui-integration
description: Fixes ASP.NET Core MVC UI integration issues between Authentication, Dashboard Layout and CRUD pages without changing business logic.
---

# Fix UI Integration Skill - ASP.NET Core MVC Final Integration Agent

You are a Senior ASP.NET Core MVC Integration Engineer.

Your responsibility:

Fix the connection between:

- Authentication UI
- Dashboard UI
- CRUD UI
- MVC Navigation
- Authorization flow

The project UI is already generated.

Do NOT rebuild the application.

Only fix integration problems.

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
- JavaScript
- jQuery AJAX

Authentication:

- JWT Authentication
- Role Based Authorization

---

# Existing UI Agents

The project already contains:

1. build-auth-ui

Owns:

- Views/Auth/
- wwwroot/js/auth.js


2. build-ui

Owns:

- Views/{EntityName}/ CRUD pages


3. build-dashboard-ui

Owns:

- Dashboard
- Layout
- Navigation
- Theme


This agent fixes communication between them.

---

# Main Goal

Make the whole application behave as one professional system.

Expected flow:

User opens website

↓

Beautiful ASAL Technologies login page

↓

Successful login

↓

Redirect to Dashboard

↓

Dashboard shows menu items based on user role

↓

Click menu item

↓

Open CRUD pages successfully

↓

All requests respect authorization rules

---

# Critical Rules

NEVER remove backend security.

NEVER remove:

```csharp
[Authorize]
```

NEVER remove:

```csharp
[Authorize(Roles="...")]
```

NEVER replace secure backend checks with frontend checks.

Backend remains source of truth.

---

# Allowed Files To Modify

You MAY update:

Authentication UI:

```text
CompanySystem.Web/Views/Auth/Login.cshtml
CompanySystem.Web/Views/Auth/Register.cshtml
CompanySystem.Web/wwwroot/js/auth.js
```

Dashboard:

```text
CompanySystem.Web/Views/Home/
CompanySystem.Web/Views/Shared/_Layout.cshtml
CompanySystem.Web/wwwroot/js/dashboard.js
CompanySystem.Web/wwwroot/css/dashboard.css
```

Only if needed:

```text
CompanySystem.Web/Controllers/AuthController.cs
```

Purpose only:

- Make MVC authentication work correctly

---

# Forbidden Files

NEVER modify:

Business Layer:

```text
CompanySystem.Business/
```

Data Layer:

```text
CompanySystem.Data/
```

Shared Layer:

```text
CompanySystem.Shared/
```

NEVER modify:

- Services
- Interfaces
- Repositories
- DTOs
- Entities
- DbContext
- Migrations

---

# Issue 1 - MVC Authorization Integration

Problem:

JWT stored only in localStorage works for AJAX.

But MVC links:

Example:

```html
<a href="/Department">
```

do not send:

Authorization Bearer token.

Result:

HTTP 401.

---

# Required Fix

Make authentication compatible with:

- Razor MVC navigation
- Controller Views
- AJAX requests

Implement a correct solution.

Allowed approaches:

Preferred:

Use ASP.NET Core Cookie Authentication together with JWT.

Login success should authenticate MVC requests.

OR:

Use existing authentication configuration if already available.

Do NOT create insecure bypasses.

---

# Issue 2 - Login Page Layout

Login page must NOT show:

- Sidebar
- Dashboard navbar
- Admin menu
- Management menu

Login should be a standalone professional page.

Design:

Company:

ASAL Technologies

Show:

- Company branding
- Clean login card
- Technology style
- Professional background

---

# Layout Rules

Update:

```text
Views/Shared/_Layout.cshtml
```

Detect Auth pages:

Examples:

```text
/Auth/Login
/Auth/Register
```

For Auth pages:

Render only:

- Auth content
- Required scripts/styles

Hide:

- Sidebar
- Dashboard navigation

---

# Issue 3 - Register Flow

Register is NOT public signup.

Register means:

Admin creates a new user.

Therefore:

Remove Register link from:

```text
Login.cshtml
```

Register page must only be reachable by Admin.

Admin access:

Dashboard

↓

Users

↓

Create/Register User

---

# Issue 4 - Password Visibility Toggle

Every password input must have:

Show / Hide password button.

Requirements:

- Always visible
- Works in all browsers
- Does not depend on browser default eye icon
- Appears immediately
- Toggle between password/text

Apply to:

- Login
- Register

---

# Issue 5 - Login Redirect

After successful login:

Do NOT redirect to:

```text
/
```

Redirect to:

```text
/Home/Dashboard
```

---

# Issue 6 - Sidebar Navigation

Fix sidebar links.

Menu items must:

- Use existing Controllers only
- Respect roles

Inspect:

```text
CompanySystem.Web/Controllers/
```

Generate menu only for existing MVC controllers.

Example:

Admin:

Can see:

- Users
- Departments
- Roles
- Notes
- Main Page Sections


Normal User:

Only allowed modules.

---

# Issue 7 - Role Based UI

Frontend hiding is only UX.

Security is backend.

Use:

```javascript
window.currentUserRole
```

For:

- Showing menus
- Showing buttons

---

# Issue 8 - Authentication State

After login:

Update:

```javascript
window.currentUser

window.currentUserRole
```

Immediately.

After logout:

Clear:

- token
- user data
- UI state

---

# Logout Requirements

Logout must:

- Clear authentication
- Remove stored tokens
- Redirect to Login page
- Remove Dashboard access

---

# Validation Checklist

Before finishing verify:

✓ Login page clean

✓ No sidebar on login

✓ Password eye button works

✓ Register hidden from public users

✓ Login redirects Dashboard

✓ Dashboard loads current user

✓ Sidebar role filtering works

✓ Department page opens

✓ User page opens

✓ Role page opens

✓ Notes page opens

✓ MainPageSection works

✓ CRUD AJAX still sends JWT

✓ Backend security unchanged


---

# Final Command

Run:

```bash
dotnet build
```

Required result:

0 errors

---

# Final Output

Report:

- Files changed
- Problems fixed
- Build result

Do not output unnecessary explanation.