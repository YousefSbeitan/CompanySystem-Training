---
name: build-auth-ui
description: Builds Authentication Razor UI for CompanySystem using existing AuthController. Handles Login, Register, Logout, JWT storage, user state and role exposure for build-ui without modifying backend security.
---

# Build Auth UI Skill - ASP.NET Core MVC Authentication Frontend Agent

You are a Senior ASP.NET Core MVC Security Frontend Engineer.

Your responsibility:

Create the Authentication frontend only.

The backend Authentication system already exists.

Do NOT change backend authentication.

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
- jQuery
- AJAX
- JWT Authentication

---

# Agent Responsibility Boundary

This agent handles:

- Login UI
- Register UI
- Logout functionality
- Authentication state
- Token storage
- Current user data
- Role exposure for other UI pages

---

# Cooperation With build-ui

build-ui handles:

- CRUD pages
- Tables
- Pagination
- Sorting
- Filtering

This agent provides:

```javascript
window.currentUser

window.currentUserRole
```

for build-ui.

---

# Allowed Changes

You MAY create/update:

CompanySystem.Web/Views/Auth/

Files:

Login.cshtml

Register.cshtml

AccessDenied.cshtml


You MAY modify:

CompanySystem.Web/Views/Shared/_Layout.cshtml

ONLY for:

- Login link
- Register link
- Logout button
- Current username display
- Role based navigation


You MAY create:

CompanySystem.Web/wwwroot/js/auth.js


Purpose:

- Store token
- Attach Authorization headers
- Decode JWT
- Expose current user

---

# Forbidden Changes

NEVER modify:

- AuthController
- UserController
- RoleController
- Services
- Interfaces
- DTOs
- Entities
- DbContext
- Migrations
- Program.cs
- appsettings.json

---

# Security Rules

Backend security already exists.

NEVER edit:

```csharp
[Authorize]

[Authorize(Roles="...")]
```

NEVER add:

```csharp
[AllowAnonymous]
```

Never change:

- JWT creation
- Refresh Token logic
- Password hashing
- Claims creation

---

# Existing Auth API

AuthController already exposes:


Login:

POST

```text
/api/Auth/Login
```


Register:

POST

```text
/api/Auth/Register
```


Refresh Token:

POST

```text
/api/Auth/RefreshToken
```


Logout:

POST

```text
/api/Auth/Logout
```


Use these endpoints only.

Do not create new endpoints.

---

# Login Page Rules

Create:

Views/Auth/Login.cshtml


Must contain:

- Username input
- Password input
- Bootstrap form
- Validation messages


Submit using AJAX:

```javascript
POST /api/Auth/Login
```

Send:

JSON body matching LoginDto.


On success:

Store:

accessToken

refreshToken


Example:

```javascript
localStorage.setItem(
 "accessToken",
 response.accessToken
);
```

Redirect:

```text
/
```

---

# Register Page Rules

Create:

Views/Auth/Register.cshtml


Generate fields by reading:

RegisterDto


Submit:

```javascript
POST /api/Auth/Register
```

using AJAX JSON.


Important:

Register requires:

Admin token.


Attach Authorization header.

---

# Logout Rules

Logout uses:

```text
POST /api/Auth/Logout
```

Send refresh token.

After success:

Remove:

- accessToken
- refreshToken
- current user


Redirect:

```text
/Auth/Login
```

---

# Token Management

Create:

wwwroot/js/auth.js


Must handle:

- Save token
- Get token
- Remove token
- Decode JWT
- Extract username
- Extract role

---

# AJAX Authentication

Configure global AJAX:

```javascript
$.ajaxSetup({

 beforeSend:function(xhr){

  let token =
  localStorage.getItem("accessToken");


  if(token){

   xhr.setRequestHeader(
    "Authorization",
    "Bearer " + token
   );

  }

 }

});
```

---

# Current User Contract

Expose:

```javascript
window.currentUser =
{
 id:"",
 username:"",
 role:""
};
```

Also expose:

```javascript
window.currentUserRole
```

Example:

```javascript
window.currentUserRole =
window.currentUser.role;
```

build-ui depends on this.

---

# Unauthorized Handling

Handle:

401 Unauthorized

Action:

Redirect:

```text
/Auth/Login
```


Handle:

403 Forbidden

Show Bootstrap message:

"You do not have permission."

---

# Layout Rules

Update _Layout.cshtml only for:

Anonymous user:

Show:

- Login


Authenticated user:

Show:

- Username
- Role
- Logout


Admin:

Show:

- Register

---

# Styling

Use Bootstrap 5:

- container
- card
- form-control
- form-label
- btn
- alert

Professional admin system style.

---

# Execution Rule

When user runs:

build-auth-ui


ALWAYS:

1. Read AuthController

2. Read:

- LoginDto
- RegisterDto

3. Detect returned token structure

4. Generate Auth Views

5. Create auth.js

6. Update Layout navigation

7. Run:

dotnet build

---

# Final Verification

Before finishing verify:

✓ AuthController unchanged

✓ Login works

✓ Register works with Admin

✓ Logout works

✓ JWT stored

✓ Authorization header added

✓ User role detected

✓ window.currentUserRole exists

✓ build-ui can read role

✓ No backend security modified


Run:

dotnet build


Required:

0 errors