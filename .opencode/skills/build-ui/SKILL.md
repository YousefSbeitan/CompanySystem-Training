---
name: build-ui
description: Builds complete ASP.NET Core MVC Razor UI for CompanySystem from existing backend. Converts API Controllers to hybrid MVC + AJAX Controllers and generates Bootstrap Views.
---

# Build UI Skill - ASP.NET Core MVC Frontend Agent

You are a Senior ASP.NET Core MVC Engineer.

Your goal:

Generate a fully working Razor MVC frontend.

The backend is already completed.

Do NOT change business logic.

---

# Architecture

Project:

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
- JSON

Pattern:

Controllers must work as:

1. MVC page providers
2. JSON API providers

---

# Allowed Changes

You MAY modify:

CompanySystem.Web/Controllers/{EntityName}Controller.cs

ONLY to:

- Add Razor View actions
- Rename GET API actions
- Add missing JSON GET endpoints

You MAY create/update:

CompanySystem.Web/Views/{EntityName}/

Required files:

- Index.cshtml
- Create.cshtml
- Edit.cshtml
- Details.cshtml
- Delete.cshtml

---

# Forbidden Changes

NEVER modify:

- Entities
- DTOs
- Services
- Interfaces
- Repositories
- DbContext
- Migrations
- Program.cs
- appsettings.json

Never change:

- validation rules
- business logic
- database structure

---

# MAIN RULE

When user runs:

build-ui EntityName

Example:

build-ui Department

You MUST ALWAYS:

1. Open Controller
2. Inspect existing actions
3. Fix routing
4. Generate Views
5. Run build

Never generate Views only.

---

# Controller Verification (MANDATORY)

Before finishing, Controller MUST contain:

## MVC Actions

```csharp
[HttpGet]
public IActionResult Index()
{
    return View();
}

[HttpGet]
public IActionResult Create()
{
    return View();
}

[HttpGet]
public IActionResult Edit(string id)
{
    return View();
}

[HttpGet]
public IActionResult Details(string id)
{
    return View();
}

[HttpGet]
public IActionResult Delete(string id)
{
    return View();
}
```

Use:

int id

instead of string id if entity key is int.

---

# JSON GET Rules

Every Controller MUST have:

```csharp
[HttpGet]
public async Task<IActionResult> GetAll()
{
    var result = await _service.GetAllAsync();
    return Ok(result);
}
```

And:

```csharp
[HttpGet]
public async Task<IActionResult> GetById(id)
{
    var result = await _service.GetByIdAsync(id);
    return Ok(result);
}
```

Use correct ID type:

- UserId / GUID strings → string
- Identity columns → int

---

# Convert Existing Controllers

If existing:

```csharp
[HttpGet]
public async Task<IActionResult> Index()
{
    return Ok(result);
}
```

Convert:

Index → View

Move old code into:

GetAll()


---

If existing:

```csharp
[HttpGet]
public async Task<IActionResult> Details(id)
{
    return Ok(result);
}
```

Convert:

Details → View

Move old code into:

GetById(id)

---

# Important

Never say:

"No controller changes needed"

unless these exist:

✓ Index returns View

✓ Create returns View

✓ Edit returns View

✓ Details returns View

✓ Delete returns View

✓ GetAll returns JSON

✓ GetById returns JSON


If missing:

ADD THEM.

---

# Keep Existing API Commands

Never rename:

POST Create(dto)

PUT Edit(dto)

DELETE Delete(id)

They stay AJAX endpoints.

---

# DTO Rules

Always read:

CompanySystem.Business/DTOs/

Use:

Create DTO → Create page

Update/Edit DTO → Edit page

Read DTO → Index + Details

Never invent properties.

---

# Razor Rules

NEVER use:

@model

NEVER use:

Html.BeginForm

NEVER use:

normal form submit


Use only:

- HTML
- Bootstrap
- jQuery AJAX

---

# Index.cshtml

Must:

Load:

```javascript
$.getJSON('/Entity/GetAll')
```

Generate:

- Bootstrap table
- Create button
- Edit button
- Details button
- Delete button

Navigation:

```
/Entity/Create
/Entity/Edit/id
/Entity/Details/id
/Entity/Delete/id
```

---

# Create.cshtml

Generate fields from:

CreateEntityDto

Submit:

```javascript
$.ajax({
 type:"POST",
 url:"/Entity/Create",
 contentType:"application/json",
 data:JSON.stringify(data)
})
```

Success:

```javascript
window.location.href="/Entity";
```

---

# Edit.cshtml

Extract id:

```javascript
window.location.pathname.split('/').pop()
```

Load:

```javascript
/Entity/GetById/id
```

Submit:

```javascript
PUT /Entity/Edit
```

---

# Details.cshtml

Load:

```javascript
/Entity/GetById/id
```

Display readonly information.

---

# Delete.cshtml

Load:

```javascript
/Entity/GetById/id
```

Delete:

```javascript
DELETE /Entity/Delete/id
```

---

# Foreign Keys

NEVER use textboxes for:

- RoleId
- DepartmentId
- ManagerId
- LeaderId
- UserId references
- CreatedBy
- UpdatedBy


Always generate:

```html
<select>
```

Load:

Role:

```javascript
/Role/GetAll
```

Department:

```javascript
/Department/GetAll
```

Users:

```javascript
/User/GetAll
```

Show names.

Do not show raw IDs unless no name exists.

---

# Type Mapping

string:

text input


long string:

textarea


int:

number


decimal:

number step="0.01"


DateTime:

date


bool:

checkbox


enum:

select

---

# Validation

Read DTO DataAnnotations.

Support:

- Required
- StringLength
- Range
- Phone
- Display

Generate:

- required
- maxlength
- min
- max

Errors:

Use Bootstrap alerts.

Never use:

alert()

---

# Styling

Use Bootstrap 5:

- container
- row
- col-md
- form-control
- form-label
- table
- btn
- alert

Professional simple layout.

---

# Final Check

Before saying completed:

Verify:

✓ Controller updated

✓ MVC routes work

✓ JSON routes work

✓ All 5 Views exist

✓ AJAX URLs correct

✓ FK dropdowns load

Then run:

dotnet build


Success:

0 errors