---
name: fix-mvc-architecture
description: Repairs ASP.NET Core MVC architecture by separating MVC Views from API endpoints, fixing routing, controllers and generated CRUD pages without changing business logic.
---

# Fix MVC Architecture Skill

You are a Senior ASP.NET Core MVC Architect.

Your responsibility is to repair incorrect MVC architecture that was generated previously.

Your goal is NOT to generate new features.

Your goal is to make the project follow the correct ASP.NET Core MVC architecture.

Backend logic already exists.

Business layer already exists.

Database already exists.

Authentication already exists.

Authorization already exists.

CRUD pages already exist.

Dashboard already exists.

Only repair architecture problems.

---

# Project Architecture

Solution:

CompanySystem

Projects:

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

# Agent Responsibility

This agent repairs:

- MVC Controllers
- MVC Routing
- View routing
- AJAX routing
- CRUD routing
- API separation
- MVC architecture consistency

This agent does NOT redesign UI.

This agent does NOT modify business logic.

---

# Main Goal

The project MUST follow the standard ASP.NET Core MVC architecture.

MVC endpoints return:

```csharp
return View();
```

API endpoints return:

```csharp
return Ok(...);
```

Never mix them.

---

# Correct Controller Pattern

Every entity controller must follow this pattern.

MVC Views:

GET

```text
/Entity
```

returns

```csharp
View();
```

---

GET

```text
/Entity/Create
```

returns

```csharp
View();
```

---

GET

```text
/Entity/Edit/{id}
```

returns

```csharp
View();
```

---

GET

```text
/Entity/Details/{id}
```

returns

```csharp
View();
```

---

GET

```text
/Entity/Delete/{id}
```

returns

```csharp
View();
```

---

API endpoints

GET

```text
/Entity/GetAll
```

returns JSON

---

GET

```text
/Entity/GetById/{id}
```

returns JSON

---

POST

```text
/Entity/Create
```

returns JSON

---

PUT

```text
/Entity/Edit
```

returns JSON

---

DELETE

```text
/Entity/Delete/{id}
```

returns JSON

---

# NEVER Allow

Never return JSON from

Index()

Never return JSON from

Details()

Never return JSON from

Create()

Never return JSON from

Edit()

Never return JSON from

Delete()

Those are MVC pages.

---

# Detect Broken Controllers

Inspect every controller inside

CompanySystem.Web/Controllers/

If Index() returns

```csharp
Ok(...)
```

replace it with

```csharp
return View();
```

and move the existing logic into

GetAll()

---

If Details() returns

```csharp
Ok(...)
```

replace it with

```csharp
return View();
```

and move existing logic into

GetById()

---

If Create()

contains View generation and API together,

split them.

---

If Edit()

contains View generation and API together,

split them.

---

If Delete()

contains View generation and API together,

split them.

---

# Preserve

Never modify

Business Layer

Never modify

Services

Never modify

Repositories

Never modify

DTOs

Never modify

Entities

Never modify

DbContext

Never modify

Migrations

Never modify

Authentication logic

Never modify

Authorization logic

Never modify

Claims

Never modify

JWT

Never modify

Password hashing

---

# Authorization

Keep every

[Authorize]

exactly as it is.

Keep every

[Authorize(Roles="...")]

exactly as it is.

Never add permissions.

Never remove permissions.

Never change roles.

Only repair routing.

---

# CRUD Views

Inspect every generated Razor View.

Especially

Index.cshtml

Create.cshtml

Edit.cshtml

Details.cshtml

Delete.cshtml

If JavaScript calls

```
/Entity
```

to retrieve JSON,

replace it with

```
/Entity/GetAll
```

If JavaScript calls

```
/Entity/Details/{id}
```

for JSON,

replace it with

```
/Entity/GetById/{id}
```

If JavaScript calls

```
/Entity
```

for Create,

replace it with

```
/Entity/Create
```

POST

Continue with every CRUD page.

---

# Detect Wrong AJAX Calls

Inspect every JavaScript file inside

CompanySystem.Web/wwwroot/js/

and every Razor View.

Replace incorrect AJAX routes.

Wrong:

```javascript
GET /Role
```

Correct:

```javascript
GET /Role/GetAll
```

---

Wrong:

```javascript
GET /Department
```

Correct:

```javascript
GET /Department/GetAll
```

---

Wrong:

```javascript
GET /User
```

Correct:

```javascript
GET /User/GetAll
```

---

Wrong:

```javascript
GET /Note
```

Correct:

```javascript
GET /Note/GetAll
```

---

Wrong:

```javascript
GET /MainPageSection
```

Correct:

```javascript
GET /MainPageSection/GetAll
```

---

Wrong:

```javascript
GET /Role/Details/{id}
```

when expecting JSON.

Correct:

```javascript
GET /Role/GetById/{id}
```

---

Wrong:

```javascript
GET /Department/Details/{id}
```

Correct:

```javascript
GET /Department/GetById/{id}
```

---

Wrong:

```javascript
GET /User/Details/{id}
```

Correct:

```javascript
GET /User/GetById/{id}
```

---

Wrong:

```javascript
GET /Note/Details/{id}
```

Correct:

```javascript
GET /Note/GetById/{id}
```

---

Wrong:

```javascript
GET /MainPageSection/Details/{id}
```

Correct:

```javascript
GET /MainPageSection/GetById/{id}
```

---

# View Rules

MVC pages always return

```csharp
View();
```

Data must be loaded using AJAX.

Never pass entity data directly from MVC Controller into Razor.

Always use existing API endpoints.

---

# Dashboard Compatibility

Do not break

build-dashboard-ui

Do not break

build-auth-ui

Do not break

build-ui

Only repair incorrect routing.

Dashboard links must continue pointing to

```
/User

/Department

/Role

/Note

/MainPageSection
```

Those URLs must open Razor pages.

Never return JSON from them.

---

# Preserve Existing Views

Do not regenerate UI.

Do not redesign pages.

Do not change HTML.

Do not change CSS.

Do not change JavaScript behavior except routing.

---

# Detect Missing MVC Views

If a controller contains

Create()

Edit()

Details()

Delete()

return View();

but the View file is missing,

create it using the existing CRUD UI pattern.

Do not invent layouts.

Reuse existing styles.

---

# Preserve Dashboard

Do not modify

Dashboard.cshtml

unless routing depends on it.

Do not modify

Layout

unless routing depends on it.

---

# Preserve Authentication

Do not modify

Login

Register

Logout

AccessDenied

unless routing depends on them.

Authentication logic is already correct.

---

# Final Validation

Before finishing verify

✓ Every Index() returns View()

✓ Every Details() returns View()

✓ Every Create() GET returns View()

✓ Every Edit() GET returns View()

✓ Every Delete() GET returns View()

✓ Every GetAll() returns JSON

✓ Every GetById() returns JSON

✓ Every POST Create returns JSON

✓ Every PUT Edit returns JSON

✓ Every DELETE returns JSON

✓ Every AJAX call uses GetAll()

✓ Every AJAX Details uses GetById()

✓ Dashboard links open Razor pages

✓ CRUD pages no longer display raw JSON

✓ Role page opens correctly

✓ Note page opens correctly

✓ Department page still works

✓ User page still works

✓ MainPageSection still works

✓ No Business Layer modified

✓ No Services modified

✓ No Repositories modified

✓ No DTOs modified

✓ No Entities modified

✓ No Authentication logic modified

✓ No Authorization logic modified

✓ No Dashboard UI broken

Run

```bash
dotnet build
```

Required result

```
0 Errors
0 Warnings
```

If any controller still mixes MVC Views and JSON responses,

continue repairing until the project follows the standard ASP.NET Core MVC architecture completely.

Never stop before every controller follows the correct MVC pattern.

---

# Missing View Detection

Inspect every MVC Controller.

For every entity controller, verify that the corresponding Razor Views exist.

Expected structure:

CompanySystem.Web/Views/{EntityName}/

Required files:

- Index.cshtml
- Create.cshtml
- Edit.cshtml
- Details.cshtml
- Delete.cshtml

Examples:

CompanySystem.Web/Views/User/

CompanySystem.Web/Views/Department/

CompanySystem.Web/Views/Role/

CompanySystem.Web/Views/Note/

CompanySystem.Web/Views/MainPageSection/

If any required View is missing:

Generate it.

Follow the existing CRUD UI style used by build-ui.

Never leave an MVC action without its corresponding View.

---

# Controller / View Consistency Check

For every controller verify:

MVC Actions:

```text
Index()

Create()

Edit(id)

Details(id)

Delete(id)
```

Each one MUST return:

```csharp
return View();
```

Each one MUST have a matching Razor View.

Example:

```
/Views/Role/Index.cshtml
```

must exist.

```
/Views/Role/Create.cshtml
```

must exist.

```
/Views/Role/Edit.cshtml
```

must exist.

```
/Views/Role/Details.cshtml
```

must exist.

```
/Views/Role/Delete.cshtml
```

must exist.

Repeat this verification for every entity.

Never assume the Views already exist.

---

# Repair Missing CRUD Pages

If a controller has MVC actions but its Views folder is missing or incomplete:

Create the missing Views.

Reuse the CRUD UI conventions generated by build-ui.

The generated Views must:

- Use Bootstrap 5
- Use jQuery AJAX
- Use existing GetAll/GetById/Create/Edit/Delete endpoints
- Respect current Authorization visibility
- Preserve Filtering
- Preserve Sorting
- Preserve Pagination
- Preserve Foreign Key dropdowns

Do not redesign the UI.

Only repair missing pages.

---

# MVC Navigation Verification

Verify that Dashboard navigation points to MVC pages instead of API endpoints.

For every module:

User

Department

Role

Note

MainPageSection

Navigation should open:

```
/Entity
```

which returns:

```csharp
View();
```

The Razor page itself must retrieve its data through:

```
/Entity/GetAll
```

using AJAX.

Never allow Dashboard links to display raw JSON.

---

# Final Missing View Verification

Before completing:

Verify the existence of:

✓ Views/User/Index.cshtml

✓ Views/User/Create.cshtml

✓ Views/User/Edit.cshtml

✓ Views/User/Details.cshtml

✓ Views/User/Delete.cshtml

✓ Views/Department/Index.cshtml

✓ Views/Department/Create.cshtml

✓ Views/Department/Edit.cshtml

✓ Views/Department/Details.cshtml

✓ Views/Department/Delete.cshtml

✓ Views/Role/Index.cshtml

✓ Views/Role/Create.cshtml

✓ Views/Role/Edit.cshtml

✓ Views/Role/Details.cshtml

✓ Views/Role/Delete.cshtml

✓ Views/Note/Index.cshtml

✓ Views/Note/Create.cshtml

✓ Views/Note/Edit.cshtml

✓ Views/Note/Details.cshtml

✓ Views/Note/Delete.cshtml

✓ Views/MainPageSection/Index.cshtml

✓ Views/MainPageSection/Create.cshtml

✓ Views/MainPageSection/Edit.cshtml

✓ Views/MainPageSection/Details.cshtml

✓ Views/MainPageSection/Delete.cshtml

If any file is missing:

Create it before finishing.

Never report success while any CRUD View is missing.