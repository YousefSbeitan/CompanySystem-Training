---
name: build-ui
description: Incremental ASP.NET Core MVC Razor CRUD UI generator for CompanySystem. Reads existing views, detects changes from backend, modifies only what changed. Supports AJAX CRUD, Filtering, Sorting, Pagination and Permission Based UI.
---

# Build UI Skill - Incremental CRUD Frontend Agent

You are a Senior ASP.NET Core MVC Frontend Engineer.

Your responsibility:

Incrementally generate or update CRUD Razor frontend.

The backend is the source of truth.

Do NOT change:

- Authentication
- Authorization
- Security
- Business logic

---

# Guiding Principles

IDEMPOTENT:

Running build-ui twice without backend changes produces zero file modifications.

INCREMENTAL:

Existing views are read, analyzed, compared against the backend, and modified in place.

Only missing files are created from scratch.

PRESERVING:

Custom HTML, CSS, JavaScript, modals, event handlers, comments, styling, and layout are kept intact.

Only the parts affected by backend changes are updated.

REPORTING:

The agent reports exactly what changed for each view and why.

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
- JSON

---

# Agent Responsibility Boundary

This agent handles:

CRUD UI:

- Index
- Create
- Edit
- Details
- Delete

Features:

- AJAX loading
- Searching
- Filtering
- Sorting
- Pagination
- FK dropdowns
- Permission based visibility

---

# Cooperation With build-auth-ui

Authentication UI is handled by:

build-auth-ui

This agent MUST NOT create or modify:

- Login pages
- Logout functionality
- Authentication flow
- Token handling
- Cookie handling
- Claims creation

This agent READS:

```javascript
window.currentUserPermissions
window.currentUserRole (fallback)
```

from auth.js for permission checks.

---

# Allowed Changes

You MAY modify:

CompanySystem.Web/Controllers/{EntityName}Controller.cs

ONLY for:

- Add missing Razor View actions (GET Index, Create, Edit, Details, Delete)
- Add missing JSON endpoints (GetAll, GetById)
- Any new actions must use [RequirePermission] not [Authorize(Roles)]
- Fix routing conflicts between MVC and API

You MAY create/update:

CompanySystem.Web/Views/{EntityName}/

Files:

Index.cshtml
Create.cshtml
Edit.cshtml
Details.cshtml
Delete.cshtml

You MAY create/update:

Entity-specific JavaScript in views (inline @section Scripts)

---

# Forbidden Changes

NEVER modify:

- AuthController
- Authentication services
- Authorization services
- JWT logic
- Cookie configuration
- Claims
- Password hashing
- Role management logic
- Existing [RequirePermission] values
- Existing [Authorize] attributes
- Entities
- DTOs (read only)
- Services
- Interfaces
- Repositories
- DbContext
- Migrations
- Program.cs
- appsettings.json

---

# Security Rules (CRITICAL)

Authentication and Authorization already exist.

NEVER remove or edit:

```csharp
[Authorize]

[Authorize(Roles="...")]

[RequirePermission("...")]
```

When adding new MVC GET actions or JSON endpoints:

Use [RequirePermission("Entity.Action")] matching the pattern:

- Entity.View for GET view/GetAll/GetById
- Entity.Create for POST Create
- Entity.Edit for PUT Edit
- Entity.Delete for DELETE Delete

NEVER add permissions for existing actions.

NEVER reduce permissions.

NEVER change backend security.

Backend is the source of truth.

---

# The Incremental Pipeline

When user runs:

build-ui EntityName

Example:

build-ui User

EXECUTE THIS PIPELINE:

```
╔══════════════════════════════════════════════════╗
║              BUILD-UI INCREMENTAL PIPELINE        ║
╠══════════════════════════════════════════════════╣
║  1. READ CONTROLLER                              ║
║     → Actions, Routes, [RequirePermission]       ║
║     → GetAll params, Response shape              ║
╠══════════════════════════════════════════════════╣
║  2. READ DTOs                                    ║
║     → CreateDto, EditDto, ReadDto                ║
║     → Fields, Types, DataAnnotations             ║
╠══════════════════════════════════════════════════╣
║  3. SCAN EXISTING VIEWS                          ║
║     → List existing .cshtml files                ║
║     → Read each file completely                  ║
╠══════════════════════════════════════════════════╣
║  4. SCAN EXISTING JAVASCRIPT                     ║
║     → Inline @section Scripts in views           ║
║     → Entity-specific JS files in wwwroot        ║
╠══════════════════════════════════════════════════╣
║  5. COMPARE (for each existing view)             ║
║     → Backend state vs Frontend state            ║
║     → Detect: missing columns, wrong URLs,       ║
║       changed permissions, new fields            ║
╠══════════════════════════════════════════════════╣
║  6. MODIFY existing files (targeted edits)       ║
║     → Only change affected parts                 ║
║     → Preserve everything else                   ║
╠══════════════════════════════════════════════════╣
║  7. CREATE missing files (from scratch)          ║
║     → Use standard permission-based templates    ║
╠══════════════════════════════════════════════════╣
║  8. REPORT changes                               ║
║     → Per-file: "updated", "created",            ║
║       "no changes needed"                        ║
╠══════════════════════════════════════════════════╣
║  9. dotnet build → 0 errors                      ║
╚══════════════════════════════════════════════════╝
```

---

# Step 1: Read Controller

Read CompanySystem.Web/Controllers/{EntityName}Controller.cs

Detect and record:

## MVC GET Actions

```
GET /Entity       → [Authorize] or [RequirePermission("Entity.View")]
GET /Entity/Create → [Authorize] or [RequirePermission("Entity.Create")]
GET /Entity/Edit/{id} → [Authorize] or [RequirePermission("Entity.Edit")]
GET /Entity/Details/{id} → [Authorize] or [RequirePermission("Entity.View")]
GET /Entity/Delete/{id} → [Authorize] or [RequirePermission("Entity.Delete")]
```

## JSON API Actions

```
GET    /Entity/GetAll    → [RequirePermission("Entity.View")]
GET    /Entity/GetById/{id} → [RequirePermission("Entity.View")]
POST   /Entity/Create    → [RequirePermission("Entity.Create")]
PUT    /Entity/Edit      → [RequirePermission("Entity.Edit")]
DELETE /Entity/Delete/{id} → [RequirePermission("Entity.Delete")]
```

## GetAll Parameters

Read the GetAll action signature.

Detect which filtering/sorting/pagination parameters it accepts:

```csharp
public async Task<IActionResult> GetAll(
    string? search,
    string? sortBy,
    string? sortDirection,
    int? pageNumber,
    int? pageSize,
    ...)
```

Build an exact list of parameter names.

## Response Shape

Read the return type.

Detect:

- PagedResponse<T> → items[], totalPages, pageNumber, totalRecords
- List<T> → direct array
- Single object

Check the response property names:

```csharp
public class PagedResponse<T>
{
    public List<T> Items { get; set; }
    // or: public List<T> Data { get; set; }
    // or: public List<T> Results { get; set; }
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int? TotalRecords { get; set; }
}
```

---

# Step 2: Read DTOs

Read the DTOs used by this entity:

CompanySystem.Business/DTOs/{EntityName}Dtos.cs

Detect:

## CreateDto

Fields, types, DataAnnotations:

```csharp
public class CreateDepartmentDto
{
    [Required]
    [StringLength(100)]
    public string DepartmentName { get; set; }

    public int? ManagerId { get; set; }
}
```

## EditDto

Same structure as CreateDto, but includes the ID field.

## ReadDto (or Dto)

Fields displayed in Index table and Details page.

Record for each field:

- Name
- Type (string, int, decimal, DateTime, bool, enum)
- Is FK (RoleId, DepartmentId, ManagerId, LeaderId → needs dropdown)
- Validation attributes (Required, StringLength, Range, Phone)
- Display attribute (label text)

---

# Step 3: Scan Existing Views

List all existing view files:

```
Views/{EntityName}/
├── Index.cshtml (exists / missing)
├── Create.cshtml (exists / missing)
├── Edit.cshtml (exists / missing)
├── Details.cshtml (exists / missing)
└── Delete.cshtml (exists / missing)
```

For each file that exists:

Read the COMPLETE file content.

Analyze its structure:

### Index.cshtml Analysis

Parse:

- Table structure: headers (columns), rows, action links
- AJAX call: URL, parameters (search, sort, page), response handling
- Permission checks: which permissions are checked for buttons
- Search input: id, name, event binding
- Sortable headers: click handlers
- Pagination: structure, event handlers
- Custom elements: modals, extra buttons, custom CSS classes, inline styles
- Custom JavaScript: event handlers, utility functions, variable declarations

### Create.cshtml Analysis

Parse:

- Form fields: inputs, selects, textareas, labels
- Validation: error message containers, validation classes
- AJAX call: URL, data serialization, success handler, error handler
- FK dropdowns: load URLs, data sources, option rendering
- Permission checks
- Custom elements: additional fields, modals, conditional sections
- Custom JavaScript: formatters, validators, event handlers

### Edit.cshtml Analysis

Same as Create, plus:

- Data load AJAX: URL (GetById), response handling, field population

### Details.cshtml Analysis

Parse:

- Display fields: labels, values, formatting
- AJAX call: URL (GetById), response handling
- Custom elements: additional info, related data, actions

### Delete.cshtml Analysis

Parse:

- Display fields: read-only summary
- AJAX calls: load URL (GetById), delete URL (Delete)
- Confirmation: modal or inline
- Custom elements

---

# Step 4: Scan Existing JavaScript

Check for:

- Inline JavaScript in @section Scripts of each view
- Entity-specific JS file: wwwroot/js/{entityName}.js (if exists)
- Shared JS that may contain entity-specific logic

Read any found JS files.

Extract:

- AJAX function signatures (URLs, params, callbacks)
- Event handler bindings
- Permission check functions
- Custom utility functions
- Data transformation logic
- Modal show/hide logic

---

# Step 5: Compare Backend vs Frontend

For each existing view, compare the current backend state against what the view contains.

## Index.cshtml Comparison

Compare:

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Table columns | ReadDto fields | Table header cells |
| Search field | GetAll "search" param | Search input presence |
| Sort columns | GetAll "sortBy" param | Sortable headers |
| Page parameters | "pageNumber", "pageSize" | Pagination controls |
| AJAX URL | GET /Entity/GetAll | AJAX url in script |
| Permission: Create | [RequirePermission("Entity.Create")] on POST Create | Create button permission check |
| Permission: Edit | [RequirePermission("Entity.Edit")] on PUT Edit | Edit button permission check |
| Permission: Delete | [RequirePermission("Entity.Delete")] on DELETE Delete | Delete button permission check |
| Response shape | PagedResponse<T> | Response.items / data parsing |

If ALL aspects match: Index.cshtml → NO CHANGES NEEDED.

If ANY aspect differs: Targeted edit of that specific part.

## Create.cshtml Comparison

Compare:

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Form fields | CreateDto properties | Input/select/textarea elements |
| Field types | DTO property types | Input type (text, number, date, checkbox, select) |
| Validation | DataAnnotations | HTML5 validation attrs, error containers |
| FK dropdowns | FK fields in DTO | Select with load URL |
| AJAX URL | POST /Entity/Create | AJAX url in script |
| Permission | [RequirePermission("Entity.Create")] | Permission check in script/button |

## Edit.cshtml Comparison

Same as Create, plus:

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Load URL | GET /Entity/GetById/{id} | AJAX load url |
| Submit URL | PUT /Entity/Edit | AJAX submit url |

## Details.cshtml Comparison

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Display fields | ReadDto properties | Label/value elements |
| Load URL | GET /Entity/GetById/{id} | AJAX url |

## Delete.cshtml Comparison

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Load URL | GET /Entity/GetById/{id} | AJAX url |
| Delete URL | DELETE /Entity/Delete/{id} | AJAX url |

---

# Step 6: Modify Existing Files

When a difference is detected, make a targeted edit.

DO NOT rewrite the entire file.

DO replace only the affected section.

## Table Column Update (Index.cshtml)

If DTO has new fields or removed fields:

```html
<!-- BEFORE -->
<thead>
    <tr>
        <th>ID</th>
        <th>Name</th>
    </tr>
</thead>

<!-- AFTER: added Email column -->
<thead>
    <tr>
        <th>ID</th>
        <th>Name</th>
        <th>Email</th>
    </tr>
</thead>
```

Update the corresponding data cells in tbody.

## AJAX URL Update

If controller route changed:

```javascript
// BEFORE
url: "/Department/GetAll"

// AFTER
url: "/Department/GetAll"
// (only change if route actually changed)
```

## Permission Check Update

If [RequirePermission] attribute changed:

```javascript
// BEFORE
if (permissions.includes("Departments.Create")) {

// AFTER
if (permissions.includes("Departments.Create")) {
// (change only if permission name changed)
```

## Form Field Update (Create.cshtml)

If DTO has new field:

```html
<!-- ADD: new Email field -->
<div class="mb-3">
    <label class="form-label">Email</label>
    <input type="email" class="form-control" name="Email" />
    <span class="text-danger field-validation-valid" data-valmsg-for="Email"></span>
</div>
```

If DTO removed a field:

Remove the corresponding HTML element and its validation.

## FK Dropdown Update

If FK target entity changed:

```html
<!-- UPDATE: data source URL if changed -->
<select class="form-select" name="ManagerId">
</select>
<script>
    $.get("/User/GetAll", function(res) {
        // populate dropdown
    });
</script>
```

## What to PRESERVE

KEEP intact:

- All custom CSS classes (container, row, col, custom-class)
- All custom HTML structure (wrapping divs, modals, cards, alerts)
- All custom JavaScript (event handlers, utility functions, formatters)
- All custom styling (inline styles, style blocks)
- All comments
- All Bootstrap layout structure
- All modal dialogs
- All custom validation logic
- All custom UI elements (tabs, accordions, progress bars, badges)
- All data transformation or formatting code

Only MODIFY:

- Table headers and cells (column changes)
- Form inputs (field changes)
- AJAX URLs (route changes)
- Permission check strings (permission name changes)
- Validation attributes (data annotations changes)
- FK dropdown data sources (entity changes)
- Search/filter parameters (controller param changes)
- Response property names (items vs data vs results)

## Verification After Each Edit

After each modification:

Re-read the file to verify the edit was applied correctly.

Ensure the file still has valid Razor syntax.

Ensure @section Scripts, @RenderSection, and other Razor directives are intact.

---

# Step 7: Create Missing Files

If a view file does not exist:

Create it from scratch.

Use the standard template patterns.

Include:

- Permission-based UI visibility
- Bootstrap 5 classes
- jQuery AJAX
- Loading states
- Error handling

## Index.cshtml Template

```html
@{
    ViewData["Title"] = "EntityName";
}

<div class="container-fluid">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h1>EntityName</h1>
        <div>
            <button class="btn btn-primary create-btn" style="display:none"
                    onclick="location.href='/Entity/Create'">Create New</button>
        </div>
    </div>

    <div class="card">
        <div class="card-body">
            <div class="row mb-3">
                <div class="col-md-4">
                    <input type="text" id="searchInput" class="form-control"
                           placeholder="Search..." />
                </div>
            </div>

            <div class="table-responsive">
                <table class="table table-striped table-hover" id="entityTable">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody id="tableBody">
                    </tbody>
                </table>
            </div>

            <nav>
                <ul class="pagination justify-content-center" id="pagination">
                </ul>
            </nav>
        </div>
    </div>
</div>

@section Scripts {
<script>
    let permissions = window.currentUserPermissions || [];

    if (permissions.includes("Entity.Create")) $(".create-btn").show();
    if (permissions.includes("Entity.View")) loadData();

    function loadData(page, search, sortBy, sortDir) {
        $.ajax({
            url: "/Entity/GetAll",
            data: { pageNumber: page, search: search, sortBy: sortBy, sortDirection: sortDir },
            success: function(res) {
                var items = res.items || res.data || res.results || res;
                renderTable(items);
                renderPagination(res.totalPages, res.pageNumber);
            }
        });
    }

    function renderTable(items) {
        var html = "";
        items.forEach(function(item) {
            html += "<tr>";
            html += "<td>" + item.id + "</td>";
            html += "<td>" + item.name + "</td>";
            html += "<td>";
            if (permissions.includes("Entity.View"))
                html += "<a href='/Entity/Details/" + item.id + "' class='btn btn-sm btn-info'>Details</a> ";
            if (permissions.includes("Entity.Edit"))
                html += "<a href='/Entity/Edit/" + item.id + "' class='btn btn-sm btn-warning'>Edit</a> ";
            if (permissions.includes("Entity.Delete"))
                html += "<a href='/Entity/Delete/" + item.id + "' class='btn btn-sm btn-danger'>Delete</a> ";
            html += "</td></tr>";
        });
        $("#tableBody").html(html);
    }

    function renderPagination(totalPages, currentPage) { ... }
</script>
}
```

## Create.cshtml / Edit.cshtml Template

Form with:

- Input for each CreateDto/EditDto field
- FK fields as select dropdowns loaded via /Target/GetAll
- Bootstrap validation
- Permission check on submit
- AJAX submit with loading state

## Details.cshtml / Delete.cshtml Template

Display with:

- Load via GET /Entity/GetById/{id}
- Read-only display for Details
- Confirmation modal for Delete
- Submit via DELETE /Entity/Delete/{id}

---

# Step 8: Report Changes

After all modifications and creations:

Print a summary report:

```
=== build-ui Report for EntityName ===

Index.cshtml: UPDATED
  - Added column: Email (new field in DTO)
  - Updated AJAX URL: /Department/GetAll → /Department/List
  - No changes to: search, sort, pagination, permission checks

Create.cshtml: NO CHANGES NEEDED
  - All fields match backend
  - AJAX endpoint matches
  - Permission checks match

Edit.cshtml: UPDATED
  - Updated permission check: "Departments.Edit" → "Department.Edit"
  - No other changes

Details.cshtml: CREATED (new file)

Delete.cshtml: NO CHANGES NEEDED

=== Summary: 2 updated, 1 created, 2 unchanged ===
```

If NO changes were needed for any file:

```
build-ui: No changes required. Frontend already matches backend for EntityName.
```

---

# Step 9: Build Verification

Run:

dotnet build

Required:

0 errors

If build fails:

Report the errors.

Do NOT commit broken code.

Fix the issues and rebuild.

---

# DTO Rules

Always inspect:

CompanySystem.Business/DTOs/

Use:

CreateDto for Create page form fields

UpdateDto / EditDto for Edit page form fields

ReadDto / Dto for Index table and Details display

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
- jQuery AJAX

---

# Index Page Rules

Must support:

- Table
- Search
- Filters
- Sorting
- Pagination
- Permission based buttons
- Loading state
- Error state

---

# GetAll Detection

Inspect parameters.

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

Generate matching AJAX with exact parameter names.

---

# Pagination

Support:

```json
{
 items:[],
 totalPages:5,
 pageNumber:1
}
```

and:

```json
{
 data:[],
 totalPages:5
}
```

and:

```json
{
 results:[],
 totalPages:5
}
```

Never assume array only.

---

# Sorting

Clickable headers.

Send:

```javascript
sortBy:name

sortDirection:asc/desc
```

Match exact parameter names from controller.

---

# Data Loading

Try:

response.items

then:

response.data

then:

response.results

then:

response (direct array)

---

# Create Page

Generate from:

CreateDto

Submit:

```javascript
POST /Entity/Create
```

using JSON AJAX.

---

# Edit Page

Load:

```text
GET /Entity/GetById/id
```

Submit:

```text
PUT /Entity/Edit
```

---

# Details Page

Load:

```text
GET /Entity/GetById/id
```

Readonly.

---

# Delete Page

Load:

```text
GET /Entity/GetById/id
```

Delete:

```text
DELETE /Entity/Delete/id
```

---

# Foreign Key Rules

Never textbox:

- RoleId
- DepartmentId
- ManagerId
- LeaderId
- CreatedBy
- UpdatedBy

Use:

select dropdown

Load:

Role: /Role/GetAll

Department: /Department/GetAll

Users: /User/GetAll

Show names.

Not IDs.

---

# Type Mapping

string: text input

large string: textarea

int: number input

decimal: number input step="0.01"

DateTime: date input

bool: checkbox

enum: select dropdown

---

# Validation

Read DataAnnotations.

Support:

- Required
- StringLength
- Range
- Phone
- Display
- EmailAddress
- Compare

Use Bootstrap validation classes:

- is-valid
- is-invalid
- invalid-feedback

Never:

alert()

---

# Permission Based UI Rules

Primary: Read [RequirePermission] on controller actions.

Map to permission check:

If POST Create has [RequirePermission("Entity.Create")]:

```javascript
if (permissions.includes("Entity.Create")) { showCreateButton(); }
```

If PUT Edit has [RequirePermission("Entity.Edit")]:

```javascript
if (permissions.includes("Entity.Edit")) { showEditButton(); }
```

If DELETE Delete has [RequirePermission("Entity.Delete")]:

```javascript
if (permissions.includes("Entity.Delete")) { showDeleteButton(); }
```

If GET GetAll has [RequirePermission("Entity.View")]:

```javascript
if (permissions.includes("Entity.View")) { showDetailsButton(); }
```

Fallback: [Authorize(Roles="Admin")] → Admin = all permissions.

---

# Frontend Authorization

Read from build-auth-ui:

```javascript
let permissions = window.currentUserPermissions || [];
```

Fallback:

```javascript
let role = window.currentUserRole || "";
```

If neither is available:

Hide all restricted actions.

---

# Styling

Use Bootstrap 5:

- container / container-fluid
- row / col-md-*
- form-control / form-select / form-label
- table / table-striped / table-hover
- btn / btn-primary / btn-warning / btn-danger / btn-info
- alert / alert-success / alert-danger
- pagination
- card / card-body
- modal / modal-dialog / modal-content
- spinner-border

---

# Final Verification

Before finishing verify:

✓ No Auth files modified

✓ No security attributes changed

✓ Controller still protected

✓ [RequirePermission] attributes preserved

✓ MVC pages work

✓ JSON APIs work

✓ Permission UI visibility works

✓ Fallback role UI works if permissions unavailable

✓ Search works

✓ Sorting works

✓ Pagination works

✓ FK dropdowns work

✓ 5 Views exist (Index, Create, Edit, Details, Delete)

✓ Existing views were NOT overwritten (only updated)

✓ Custom code in existing views was preserved

✓ dotnet build produces 0 errors

Run:

dotnet build

Required:

0 errors
