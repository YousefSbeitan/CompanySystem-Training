---
name: build-ui
description: Incremental ASP.NET Core MVC Razor CRUD UI generator for CompanySystem. Reads existing views, detects changes from backend, modifies only what changed. Supports AJAX CRUD, Filtering, Sorting, Pagination and Permission Based UI.
---

# Build UI — Incremental CRUD Frontend Agent

## Principles

- **IDEMPOTENT** — Running twice with no backend changes produces zero file modifications.
- **INCREMENTAL** — Read, analyze, compare, modify in place. Only missing files are created.
- **PRESERVING** — Custom HTML, CSS, JS, modals, event handlers, comments, styling, layout stay intact.
- **REPORTING** — Per-file: what changed and why.

## Project Architecture

- Solution: CompanySystem
- Frontend: Razor Views, Bootstrap 5, jQuery, AJAX
- Layout: `Views/Shared/_Layout.cshtml` (applied via `_ViewStart.cshtml`)

## Frontend-Only Boundary

This agent produces Razor Views, JavaScript, and CSS **only**.

**Never modify:**
- Controllers (READ-ONLY — detect actions, never edit)
- DTOs (READ-ONLY — derive fields/types/validation, never edit)
- Services, Repositories, Entities, DbContext
- Authentication, Authorization, Program.cs

**Missing controller actions**: If required MVC view actions (`GET Index`, `GET Create`, `GET Edit`, `GET Details`, `GET Delete`) or JSON API endpoints are absent, **report them** in the output. Do not create or modify controller code.

## Dependency: build-auth-ui

Reads permissions from `auth.js` (handled by the `build-auth-ui` skill):

```js
let permissions = window.currentUserPermissions || [];
let role = window.currentUserRole || ""; // fallback
```

If unavailable, hide all restricted actions. Never create or modify auth pages, token handling, or Claims.

## Permission Convention

Backend `[RequirePermission("Entity.Action")]` maps to frontend checks:

| Action | Permission |
|---|---|
| GET Index, GetAll, GetById | `Entity.View` |
| POST Create | `Entity.Create` |
| PUT Edit | `Entity.Edit` |
| DELETE Delete | `Entity.Delete` |

Fallback: `[Authorize(Roles="Admin")]` implies all permissions.

Frontend:
```js
if (permissions.includes("Entity.View"))   { /* show Details */ }
if (permissions.includes("Entity.Create")) { /* show Create */ }
if (permissions.includes("Entity.Edit"))   { /* show Edit */ }
if (permissions.includes("Entity.Delete")) { /* show Delete */ }
```

---

## Pipeline Overview

```
 0. READ SHARED UI       — layout, Bootstrap, jQuery, icons
 1. READ CONTROLLER      — actions, routes, permissions (READ-ONLY)
 2. READ DTOs            — fields, types, annotations, FK (READ-ONLY)
 3. SCAN EXISTING VIEWS  — read all .cshtml files completely
 4. SCAN EXISTING JS     — inline scripts, asset files, shared helpers
 5. COMPARE              — backend vs frontend per view, detect mismatches
 6. MODIFY existing      — targeted edits only, preserve custom code
 7. CREATE missing       — derive from controller, DTOs, shared assets
 8. REPORT               — per-file: updated / created / no-op / missing
 9. dotnet build         — verify 0 errors
```

---

## Pipeline Steps

### Step 0: Read Shared UI

Read: `Views/Shared/_Layout.cshtml`, `_ValidationScriptsPartial.cshtml`, `_ViewImports.cshtml`, `_ViewStart.cshtml`.

Verify:
- Layout applied correctly (via `_ViewStart.cshtml`)
- Bootstrap CSS + JS present
- jQuery present
- Icon library present
- `@RenderSection("Scripts")` exists in layout

Every generated view must inherit from the shared layout.

### Step 1: Read Controller (READ-ONLY)

Read `Controllers/{EntityName}Controller.cs`. **Do not modify.** Detect:

**MVC GET actions** — map routes to permissions:
- `GET /Entity` → `[RequirePermission("Entity.View")]`
- `GET /Entity/Create` → `[RequirePermission("Entity.Create")]`
- `GET /Entity/Edit/{id}` → `[RequirePermission("Entity.Edit")]`
- `GET /Entity/Details/{id}` → `[RequirePermission("Entity.View")]`
- `GET /Entity/Delete/{id}` → `[RequirePermission("Entity.Delete")]`

**JSON API actions** — map routes to permissions:
- `GET /Entity/GetAll` → `[RequirePermission("Entity.View")]`
- `GET /Entity/GetById/{id}` → `[RequirePermission("Entity.View")]`
- `POST /Entity/Create` → `[RequirePermission("Entity.Create")]`
- `PUT /Entity/Edit` → `[RequirePermission("Entity.Edit")]`
- `DELETE /Entity/Delete/{id}` → `[RequirePermission("Entity.Delete")]`

**GetAll parameters** — read the action signature. Detect exact names from: `search`, `keyword`, `filter`, `pageNumber`, `pageSize`, `sortBy`, `sortColumn`, `sortDirection`, `orderBy`. Build AJAX code with these exact names — never guess.

**Response shape** — detect return type:
- `PagedResponse<T>` → items[] + totalPages + pageNumber + totalRecords
- `List<T>` → direct array
- Single object

Check actual property names (`Items`, `Data`, `Results`). Never assume — match the backend exactly.

**Report missing actions** — note any absent MVC or JSON actions for the report.

### Step 2: Read DTOs (READ-ONLY)

Read `Business/DTOs/{EntityName}Dtos.cs`. **Do not modify.** Detect:
- **CreateDto** — fields, types, DataAnnotations
- **EditDto** (or UpdateDto) — same as CreateDto with ID field
- **ReadDto** (or Dto) — fields for display

For each field record:
- **Name** — property name
- **Type** — map to input element (see Type Mapping below)
- **Is FK** — fields ending in `Id` (RoleId, DepartmentId, ManagerId, LeaderId, CreatedBy, UpdatedBy) must be `<select>` populated via `GET /TargetEntity/GetAll`. Show names, not IDs. Never use textboxes for FK fields.
- **Validation** — Required, StringLength, Range, Display, EmailAddress, Compare
- **Display** — label text from `[Display]` attribute

**Type Mapping:**

| .NET Type | Input Element |
|---|---|
| `string` | `<input type="text">` |
| `string` + large `[StringLength]` | `<textarea>` |
| `int` | `<input type="number">` |
| `decimal` | `<input type="number" step="0.01">` |
| `DateTime` | `<input type="date">` |
| `bool` | `<input type="checkbox">` |
| `enum` | `<select>` |

Generate Bootstrap 5 validation classes (`is-valid`, `is-invalid`, `invalid-feedback`). Never invent fields — DTOs are the source of truth.

### Step 3: Scan Existing Views

List `Views/{EntityName}/`. Check existence: `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`, `Details.cshtml`, `Delete.cshtml`.

Read each existing file **completely**. Analyze:
- **Index**: Table headers/rows, search input, sortable headers, pagination, permission checks, AJAX call (url, params, response handling), custom elements (modals, buttons, styles), custom JS
- **Create**: Form fields, FK dropdowns, AJAX submit (url, data, success/error), validation, permission checks
- **Edit**: Same as Create + data load AJAX (GetById url, response handling, field population)
- **Details**: Display fields, AJAX load, custom elements
- **Delete**: Display fields, AJAX load, AJAX delete, confirmation

### Step 4: Scan Existing JavaScript & Assets

Check:
- Inline JS in `@section Scripts` of each view
- Entity-specific JS: `wwwroot/js/{entityName}.js`
- Shared JS: auth.js, site.js, common.js, layout.js
- All referenced JS/CSS files must exist — no broken references

Extract: AJAX patterns, event bindings, permission checks, helper functions, modal show/hide, data transformation.

Do not duplicate shared logic — reuse existing helpers.

### Step 5: Compare Backend vs Frontend

For each existing view, compare backend state against view content using these tables.

**Index.cshtml:**

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Table columns | ReadDto fields | Table header cells |
| Search field | GetAll param name (search/keyword/filter) | Search input element |
| Sort columns | sortBy param | Sortable header click handlers |
| Pagination | pageNumber, pageSize | Pagination controls |
| AJAX URL | GET /Entity/GetAll | AJAX url in script |
| Permission: Create | `[RequirePermission("Entity.Create")]` (on POST Create) | Create button visibility check |
| Permission: Edit | `[RequirePermission("Entity.Edit")]` (on PUT Edit) | Edit button visibility check |
| Permission: Delete | `[RequirePermission("Entity.Delete")]` (on DELETE Delete) | Delete button visibility check |
| Response shape | PagedResponse\<T\> / List\<T\> | Response.items / data / results parsing |

If ALL match → NO CHANGES NEEDED. If any differs → targeted edit of that specific part.

**Create.cshtml / Edit.cshtml:**

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Form fields | CreateDto / EditDto properties | Input/select/textarea elements |
| Field types | DTO property types | Input type attribute |
| Validation | DataAnnotations | HTML5 validation attrs + error containers |
| FK dropdowns | FK fields in DTO | Select with load URL via GET /TargetEntity/GetAll |
| AJAX URL | POST /Entity/Create or PUT /Entity/Edit | AJAX url in script |
| Permission | `[RequirePermission("Entity.Create")]` or `[RequirePermission("Entity.Edit")]` | Permission check in script/button |

Edit only also: Load URL | GET /Entity/GetById/{id} | AJAX load url in script

**Details.cshtml / Delete.cshtml:**

| Aspect | Backend Source | Frontend Check |
|---|---|---|
| Display fields | ReadDto properties | Label/value elements |
| Load URL | GET /Entity/GetById/{id} | AJAX url in script |
| Delete URL (Delete only) | DELETE /Entity/Delete/{id} | AJAX url in script |

### Step 6: Modify Existing Files

When a difference is detected, replace only the affected section. **Never rewrite the entire file.**

**PRESERVE** (keep intact):
Custom CSS classes, HTML structure (wrapping divs, modals, cards, alerts), custom JS (event handlers, utility functions, formatters, data transformation), inline styles, comments, Bootstrap layout, modal dialogs, custom validation logic, custom UI elements (tabs, accordions, badges).

**MODIFY** (only these):
- Table headers and cells (column changes)
- Form inputs (field changes)
- AJAX URLs (route changes)
- Permission check strings (permission name changes)
- Validation attributes (data annotation changes)
- FK dropdown data sources (entity changes)
- Search/filter parameters (controller param changes)
- Response property names (items vs data vs results)

**Pagination** — support detected response shapes:
```json
{ "items": [], "totalPages": 5, "pageNumber": 1 }
{ "data": [], "totalPages": 5 }
{ "results": [], "totalPages": 5 }
```
Never assume array-only response.

**Sorting** — add clickable column headers sending `sortBy` and `sortDirection` (asc/desc). Match exact parameter names from the controller.

**AJAX requirements** — every AJAX call must include `beforeSend`, `success`, `error`, and `complete`. Show loading before, hide after. Handle empty state ("No data found") and error state. Never leave "Loading..." visible.

**Verification after each edit** — re-read the file. Check valid Razor syntax, intact `@section Scripts`, valid JS syntax. Every referenced JS function, element selector (#id, .class), event handler, and AJAX callback must target an existing DOM element. If a selector doesn't exist, fix the view.

### Step 7: Create Missing Files

For non-existent views, create them using these generation rules. **Do not copy template code literally.** Derive every element from the actual controller, DTOs, existing views (for pattern matching), shared layout, and shared assets.

#### Index.cshtml

- **Title**: `ViewData["Title"]` = pluralized entity name (e.g., "Users")
- **Create button**: show only if controller has `[RequirePermission("Entity.Create")]` on POST Create; wire to `location.href='/Entity/Create'`
- **Search**: match the exact GetAll parameter name (search/keyword/filter); bind to `keyup` or search button click
- **Table columns**: one `<th>` per ReadDto property; use `[Display]` name or split PascalCase for header; make sortable via click handler sending `sortBy` + `sortDirection`
- **AJAX**: call `GET /Entity/GetAll` with exact parameter names from controller; include `beforeSend`/`success`/`error`/`complete`
- **Response parsing**: match detected shape — try `response.items`, `response.data`, `response.results`, then `response` (direct array); use detected pagination fields
- **Actions column**: Details/Edit/Delete buttons with permission checks mirroring controller's `[RequirePermission]` values
- **Pagination**: build from `totalPages`/`pageNumber`; re-trigger loadData on page click
- **States**: loading spinner → replace with table rows on success, error message on failure; empty array → "No data found"

#### Create.cshtml / Edit.cshtml

- **Form**: `<form id="entityForm">`; one field per CreateDto/EditDto property
- **Field rendering**: use Type Mapping table for input type; `[Display]` name for `<label>`
- **FK fields**: `<select>` populated via `GET /TargetEntity/GetAll` on page load; show name property, store ID as value
- **Validation**: `[Required]` → `required` attribute + `is-invalid`/`invalid-feedback`; add `is-valid` on input; match all DataAnnotations
- **Submit**: AJAX `POST /Entity/Create` or `PUT /Entity/Edit`; serialize form data
- **Edit only**: load existing data via `GET /Entity/GetById/{id}` on page load; populate fields; include ID in submit data
- **Permission**: check user can Create/Edit; redirect if unauthorized
- **States**: loading spinner on submit → disable button; success → redirect to Index; error → show error in alert

#### Details.cshtml / Delete.cshtml

- **Display**: read-only label/value pairs from ReadDto properties
- **Load**: `GET /Entity/GetById/{id}` on page load; populate display elements
- **Delete only**: confirmation modal or inline confirmation; submit via `DELETE /Entity/Delete/{id}`; redirect to Index on success
- **Permission**: check View/Delete permission; redirect if unauthorized

#### Styling

Use Bootstrap 5 classes consistently: `container-fluid`, `row`/`col-md-*`, `form-control`/`form-select`/`form-label`, `table`/`table-striped`/`table-hover`, `btn`/`btn-primary`/`btn-warning`/`btn-danger`/`btn-info`, `alert`/`alert-success`/`alert-danger`, `pagination`, `card`/`card-body`, `modal`/`modal-dialog`, `spinner-border`.

### Step 8: Report Changes

Print per-file summary:

```
=== build-ui Report for EntityName ===

Index.cshtml: UPDATED
  - Added column: Email (new field in DTO)
  - Updated AJAX URL: /Department/GetAll → /Department/List

Create.cshtml: NO CHANGES NEEDED
  - All fields match backend

Edit.cshtml: UPDATED
  - Updated permission check: "Departments.Edit" → "Department.Edit"

Details.cshtml: CREATED (new file)

Delete.cshtml: NO CHANGES NEEDED

=== Summary: 2 updated, 1 created, 2 unchanged ===

=== Missing Controller Actions ===
- GET /Entity/Create — missing from controller
- POST /Entity/Create — missing from controller
```

If no changes at all:

```
build-ui: No changes required. Frontend already matches backend for EntityName.
```

### Step 9: Build Verification

Run `dotnet build`. Required: **0 errors**. If build fails, report errors, fix them, rebuild. Do not commit broken code.

---

## Runtime Validation

After generating or modifying any file, verify every item below. Fix all failures before finishing.

### JavaScript Functions

Every function called inside `@section Scripts` must be defined in the page, an entity-specific JS file, or a loaded shared JS file. Check: `loadData`, `renderTable`, `renderPagination`, `loadEntity`, `submitForm`, `deleteEntity`, and any custom function referenced by name.

### Selectors

Every jQuery/element selector used in JS (`#id`, `.class`) must match an existing element in the HTML. Verify all referenced selectors across the view and its scripts.

### AJAX Endpoints

Every AJAX URL must correspond to an existing controller action with the correct HTTP method (GET/POST/PUT/DELETE). Verify against the controller file. Report any mismatches.

### Response Parsing

Every AJAX success handler must reference property names that match the controller's actual return type. Check: `items`/`data`/`results`, `totalPages`, `pageNumber`, `totalRecords`. Verify against the controller's response shape detected in Step 1.

### Loading State

Every AJAX call must have a `complete` (or `always`) callback that removes the loading indicator. No page can remain permanently displaying "Loading...".

### Empty State

Table/list rendering must handle empty arrays. Display "No data found" or an equivalent message when results are empty. Do not show an empty table with no message.

### Error State

Every AJAX call must have an `error` callback that displays an error message to the user. Never leave a loading indicator visible on failure.

### Shared Integration

- **Layout**: Every view must use the shared layout (via `_ViewStart.cshtml` or explicit `Layout = "_Layout"`)
- **Scripts section**: Every view with inline JS must use `@section Scripts`
- **CSS**: All referenced CSS files in `wwwroot/css/` must exist
- **JS**: All referenced JS files in `wwwroot/js/` must exist

---

## Final Verification Checklist

- [ ] No backend files modified (controllers, DTOs, services, repositories, entities, DbContext, auth, Program.cs)
- [ ] No security attributes changed or removed
- [ ] Existing views were NOT overwritten — only targeted edits applied
- [ ] Custom code preserved in all existing views
- [ ] All 5 views exist (Index, Create, Edit, Details, Delete)
- [ ] Missing controller actions reported in output (not created)
- [ ] Layout integration valid for all views
- [ ] Bootstrap/JS/CSS references valid — no broken file references
- [ ] All AJAX endpoints match existing controller routes
- [ ] Response parsing matches detected shape (items/data/results)
- [ ] Permission-based UI visibility is correct per `[RequirePermission]`
- [ ] Search, sort, pagination, and FK dropdowns are functional
- [ ] Loading, empty, and error states implemented for every AJAX call
- [ ] No permanent "Loading..." state possible
- [ ] `dotnet build` produces 0 errors
