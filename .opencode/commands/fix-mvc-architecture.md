---
description: Repair ASP.NET Core MVC architecture. Separates MVC Views from JSON API endpoints, repairs controllers, routing, AJAX calls and missing CRUD Views.
agent: ui-builder
---

Load the fix-mvc-architecture skill, then execute it.

Inspect the entire CompanySystem solution before making any changes.

Project:

CompanySystem

Repair the MVC architecture without changing any business logic.

Read and inspect:

- CompanySystem.Web/Controllers/
- CompanySystem.Web/Views/
- CompanySystem.Web/wwwroot/js/
- CompanySystem.Business/DTOs/

Tasks:

## 1. Inspect Every Controller

Read every MVC Controller.

Detect controllers that incorrectly return JSON from MVC actions.

Repair them.

MVC actions must return:

```csharp
return View();
```

Never JSON.

---

## 2. Separate MVC From API

Ensure every controller follows this pattern.

MVC

GET

```
/Entity
```

returns

```
View()
```

GET

```
/Entity/Create
```

returns

```
View()
```

GET

```
/Entity/Edit/{id}
```

returns

```
View()
```

GET

```
/Entity/Details/{id}
```

returns

```
View()
```

GET

```
/Entity/Delete/{id}
```

returns

```
View()
```

API

GET

```
/Entity/GetAll
```

returns JSON.

GET

```
/Entity/GetById/{id}
```

returns JSON.

POST

```
/Entity/Create
```

returns JSON.

PUT

```
/Entity/Edit
```

returns JSON.

DELETE

```
/Entity/Delete/{id}
```

returns JSON.

Never mix MVC and API.

---

## 3. Preserve Authorization

Never remove

```
[Authorize]
```

Never remove

```
[Authorize(Roles="...")]
```

Never remove

```
[RequirePermission("...")]
```

Never change permissions.

Never change roles.

Never change security.

---

## 4. Repair AJAX Routes

Inspect every:

- Index.cshtml
- Create.cshtml
- Edit.cshtml
- Details.cshtml
- Delete.cshtml

Inspect every JavaScript block.

Replace incorrect routes.

Examples

Wrong

```
GET /Role
```

Correct

```
GET /Role/GetAll
```

Wrong

```
GET /Department
```

Correct

```
GET /Department/GetAll
```

Wrong

```
GET /User
```

Correct

```
GET /User/GetAll
```

Wrong

```
GET /Note
```

Correct

```
GET /Note/GetAll
```

Wrong

```
GET /MainPageSection
```

Correct

```
GET /MainPageSection/GetAll
```

Wrong

```
GET /Role/Details/5
```

when expecting JSON.

Correct

```
GET /Role/GetById/5
```

Repeat for every entity.

---

## 5. Verify CRUD Views

Inspect every controller.

Verify these files exist.

Views/User/

Views/Department/

Views/Role/

Views/Note/

Views/MainPageSection/

Required files

Index.cshtml

Create.cshtml

Edit.cshtml

Details.cshtml

Delete.cshtml

If any file is missing

Create it.

Reuse the build-ui conventions.

Do not redesign the UI.

---

## 6. Verify MVC Navigation

Inspect:

Dashboard

Sidebar

Navbar

Navigation links.

Ensure links open

```
/Entity
```

never

```
/Entity/GetAll
```

The Razor page itself should call GetAll by AJAX.

Never let clicking a menu item display JSON.

---

## 7. Verify Dashboard Compatibility

Do not modify

Authentication.

Do not modify

Dashboard design.

Do not modify

CRUD layouts.

Only repair broken routing.

---

## 8. Verify Authentication Compatibility

Do not modify

AuthController

Login

Register

Logout

JWT

Cookies

Claims

Tokens

Password hashing

Authorization

---

## 9. Verify CRUD Integration

Ensure generated CRUD pages still support

Search

Filtering

Sorting

Pagination

Foreign Key dropdowns

Bootstrap styling

Role based visibility

Do not remove any functionality.

---

## 10. Final MVC Validation

Before finishing verify

✓ Every MVC action returns View()

✓ Every API action returns JSON

✓ Every Dashboard link opens Razor pages

✓ No menu opens raw JSON

✓ AJAX uses GetAll()

✓ AJAX uses GetById()

✓ Role module opens correctly

✓ Note module opens correctly

✓ Department module opens correctly

✓ User module opens correctly

✓ MainPageSection module opens correctly

✓ Missing CRUD Views generated

✓ No business logic modified

✓ No services modified

✓ No DTOs modified

✓ No entities modified

✓ No repositories modified

✓ No authentication modified

✓ No authorization modified

✓ Dashboard still works

✓ build-ui compatibility preserved

✓ build-auth-ui compatibility preserved

✓ build-dashboard-ui compatibility preserved

Run

```bash
dotnet build
```

Do not finish until the build succeeds.

Required result

```
0 Errors
0 Warnings
```

If any MVC architecture problem still exists,

continue repairing until the project follows the standard ASP.NET Core MVC architecture completely.