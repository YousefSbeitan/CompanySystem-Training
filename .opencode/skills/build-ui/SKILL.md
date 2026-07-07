---
name: build-ui
description: Builds complete ASP.NET Core MVC Razor UI for CompanySystem. Supports CRUD, AJAX, Filtering, Sorting, Pagination and converts API Controllers to hybrid MVC Controllers.
---

# Build UI Skill - ASP.NET Core MVC Frontend Agent

You are a Senior ASP.NET Core MVC Engineer.

Your responsibility:

Create a complete working Razor MVC frontend from the existing backend.

Backend is the source of truth.

Do NOT change business logic.

---

# Project Architecture

Solution:

CompanySystem

Layers:

- CompanySystem.Web
- CompanySystem.Business
- CompanySystem.Data
- CompanySystem.Shared

Frontend stack:

- Razor Views
- Bootstrap 5
- jQuery
- AJAX
- JSON

Controllers must support:

1. MVC Razor pages
2. JSON API endpoints

---

# Allowed Changes

You MAY modify ONLY:

CompanySystem.Web/Controllers/{EntityName}Controller.cs

Allowed controller changes:

- Add MVC View actions
- Rename GET JSON endpoints
- Add missing GET API endpoints
- Fix routing conflicts

You MAY create/update:

CompanySystem.Web/Views/{EntityName}/

Files:

Index.cshtml
Create.cshtml
Edit.cshtml
Details.cshtml
Delete.cshtml

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

Never modify:

- database schema
- validation rules
- business logic

---

# Execution Rule

When user runs:

build-ui EntityName

Example:

build-ui User


ALWAYS:

1. Read Controller
2. Read DTOs
3. Detect routes
4. Detect GetAll parameters
5. Detect pagination/filter/sort support
6. Update controller routing
7. Generate Views
8. Run dotnet build

Never skip controller inspection.

---

# Controller MVC Requirements

Controller MUST contain:

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
public IActionResult Edit(id)
{
    return View();
}


[HttpGet]
public IActionResult Details(id)
{
    return View();
}


[HttpGet]
public IActionResult Delete(id)
{
    return View();
}
```

Use correct ID type:

- string
- int
- Guid

based on entity.

---

# API GET Requirements

Controller MUST expose:

GetAll:

```csharp
[HttpGet]
public async Task<IActionResult> GetAll(...)
{
    var result = await _service.GetAllAsync(...);
    return Ok(result);
}
```


GetById:

```csharp
[HttpGet]
public async Task<IActionResult> GetById(id)
{
    var result = await _service.GetByIdAsync(id);
    return Ok(result);
}
```

---

# Convert Existing API Controllers

If:

```csharp
Index()
{
 return Ok(data);
}
```

Convert:

Index → MVC View

Move API code to:

GetAll()


If:

```csharp
Details(id)
{
 return Ok(data);
}
```

Convert:

Details → MVC View

Move API code to:

GetById(id)

---

# Keep Existing Commands

Never rename:

POST Create(dto)

PUT Edit(dto)

DELETE Delete(id)

They remain AJAX endpoints.

---

# DTO Rules

Always inspect:

CompanySystem.Business/DTOs/

Use:

CreateDto → Create page

UpdateDto → Edit page

ReadDto → Index + Details

Never invent fields.

---

# Razor Rules

NEVER use:

@model

Html.BeginForm

Normal form submit


Use:

- HTML
- Bootstrap
- jQuery
- AJAX

---

# Index.cshtml Rules

Index must support:

CRUD table

PLUS:

- Search
- Filtering
- Sorting
- Pagination

---

# GetAll Detection Rules

Before writing Index:

Inspect GetAllAsync parameters.

Detect:

- search
- keyword
- filter
- pageNumber
- pageSize
- sortBy
- sortColumn
- sortDirection
- orderBy

Generate matching UI.

---

# Search UI

If search exists:

Generate:

```html
<input id="searchInput"
       class="form-control"
       placeholder="Search..." />
```

Send:

```javascript
search: $("#searchInput").val()
```

---

# Pagination Rules

If pagination exists:

Generate:

Bootstrap pagination:

```html
<ul class="pagination"></ul>
```

AJAX:

```javascript
$.getJSON('/Entity/GetAll',
{
 pageNumber: currentPage,
 pageSize: pageSize
})
```

Support responses:

```json
{
 items: [],
 totalPages: 5,
 pageNumber: 1,
 totalCount: 50
}
```

OR:

```json
{
 data: [],
 totalPages: 5
}
```

Never assume GetAll returns array.

---

# Sorting Rules

Tables must support sorting.

Headers should be clickable.

Example:

Name ↑ ↓

Send:

```javascript
sortBy: columnName,
sortDirection: "asc"
```

or:

```javascript
sortDirection: "desc"
```

---

# Table Loading Rules

Detect collection:

Try:

response.items

then:

response.data

then:

response.results

then:

response directly

---

# Create.cshtml

Generate fields from CreateDto.

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
GET /Entity/GetById/id
```

Submit:

```javascript
PUT /Entity/Edit
```

---

# Details.cshtml

Load:

```javascript
GET /Entity/GetById/id
```

Show readonly data.

---

# Delete.cshtml

Load:

```javascript
GET /Entity/GetById/id
```

Delete:

```javascript
DELETE /Entity/Delete/id
```

---

# Foreign Keys

Never textbox:

- RoleId
- DepartmentId
- ManagerId
- LeaderId
- CreatedBy
- UpdatedBy

Always:

<select>

Load:

Role:

/Role/GetAll

Department:

/Department/GetAll

Users:

/User/GetAll

Display names not IDs.

---

# Input Mapping

string:

text


large string:

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

Read DataAnnotations.

Support:

- Required
- StringLength
- Range
- Phone
- Display

Generate HTML validation.

Show errors using:

Bootstrap alert

Never:

alert()

---

# UI Style

Use Bootstrap 5:

container
row
col-md
form-control
form-label
table
btn
alert
pagination

Clean admin dashboard style.

---

# Final Verification

Before finishing:

Check:

✓ Controller supports MVC

✓ API still works

✓ GetAll supports filters

✓ Sorting works

✓ Pagination works

✓ 5 Views created

✓ FK dropdowns load

Run:

dotnet build

Required result:

0 errors