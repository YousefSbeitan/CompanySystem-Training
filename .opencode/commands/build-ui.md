---
description: Incrementally generate or update MVC Razor Views for an entity. Reads existing views, compares against backend, modifies only what changed. Usage: build-ui <EntityName>
agent: ui-builder
---

Load the build-ui skill, then execute it for entity: $ARGUMENTS

The entity name is: $1

This is an INCREMENTAL generator. Do NOT scaffold from scratch.

EXECUTE THIS EXACT PIPELINE:

1. READ CONTROLLER

Read CompanySystem.Web/Controllers/$1Controller.cs

Detect:

- All MVC GET actions and their [RequirePermission] / [Authorize] attributes
- All JSON API actions and their [RequirePermission] values
- GetAll parameters (search, sortBy, sortDirection, pageNumber, pageSize, etc.)
- Response shape (PagedResponse, List, direct array)
- Check property names: Items, Data, Results

2. READ DTOs

Read CompanySystem.Business/DTOs/$1*Dtos.cs

Detect:

- CreateDto fields, types, DataAnnotations
- EditDto fields, types, DataAnnotations
- ReadDto (or Dto) fields for display
- FK fields that need dropdowns (RoleId, DepartmentId, ManagerId, LeaderId)

3. SCAN EXISTING VIEWS

List files in: CompanySystem.Web/Views/$1/

Check which exist: Index.cshtml, Create.cshtml, Edit.cshtml, Details.cshtml, Delete.cshtml

Read EVERY existing file COMPLETELY.

Parse its structure: columns, form fields, AJAX calls, permission checks, custom JS, modals.

4. COMPARE (for each existing view)

Compare backend state against frontend state.

Check:

- Table columns vs DTO fields
- Form fields vs CreateDto/EditDto fields
- AJAX URLs vs controller routes
- Permission checks vs [RequirePermission] values
- Validation vs DataAnnotations
- FK dropdown sources vs entity endpoints
- Search/filter params vs controller GetAll params
- Response parsing vs actual response shape

5. MODIFY existing files

Make targeted edits only.

Do NOT rewrite entire files.

Preserve: custom HTML, CSS, JS, modals, comments, layout, styling.

Update only: columns, fields, URLs, permission checks, validation.

6. CREATE missing files

Only for views that do not exist.

Use standard permission-based templates.

7. REPORT

Report per-file status:

- "Updated: changed X, Y, Z"
- "Created: new file"
- "No changes needed"

If NO changes needed: "No changes required. Frontend already matches backend."

8. BUILD

Run:

dotnet build

Result:

0 errors

CRITICAL RULES:

- Read EVERY existing view before making ANY changes
- Never overwrite existing work
- Only modify what the backend changed
- Preserve ALL custom developer code
- Targeted edits only, never full file rewrites
- If nothing changed, report "no changes required" and exit
