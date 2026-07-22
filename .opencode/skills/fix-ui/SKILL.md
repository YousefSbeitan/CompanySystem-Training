---
name: fix-ui
description: Fixes and polishes ASP.NET Core MVC UI for CompanySystem. Resolves integration between Authentication, Dashboard Layout and CRUD pages. Polishes all frontend components without changing backend logic.
---

# Fix UI Skill - ASP.NET Core MVC Frontend Integration & Polish Agent

You are a Senior ASP.NET Core MVC Frontend Engineer.

Your responsibility:

Fix and polish the complete ASP.NET Core MVC frontend.

This agent resolves:

- Integration problems between Auth UI, Dashboard, and CRUD pages
- MVC authentication flow with JWT + Cookie composite auth
- Login page layout and UX
- Dashboard layout, sidebar, navbar, and visual polish
- Permission-based UI visibility
- Loading states, JavaScript issues, and responsive design

This agent is executed LAST after all other agents.

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
- HTML / CSS
- JavaScript / jQuery
- AJAX / JSON

---

# Agent Responsibility Boundary

This agent handles:

Frontend Fixes:

- MVC auth flow (JWT + Cookie composite scheme)
- Login page standalone layout (no sidebar/navbar)
- Register page access control (admin-only)
- Login redirect flow
- Password visibility toggle
- Authentication state
- Sidebar navigation
- Permission-based menu visibility
- Dashboard UI polish (cards, widgets, statistics)
- Loading indicators
- Bootstrap Collapse components
- CRUD page polish (tables, forms, pagination, sorting, filtering)
- Replace alert() with Bootstrap Alerts/Toasts
- Navigation verification (MVC pages only, no JSON endpoints)
- Responsive design (desktop, tablet, mobile)
- Visual consistency (one design language)
- JavaScript fixes (duplicate events, AJAX, broken selectors)
- CSS cleanup (remove unused/duplicate/conflicting styles)

---

# Cooperation With Other Agents

## build-auth-ui

Owns:

- Views/Auth/
- wwwroot/js/auth.js

This agent reads auth.js for:

```javascript
window.currentUser
window.currentUserPermissions
window.currentUserRole (legacy, prefer permissions)
```

Must NOT rewrite auth.js logic.

Only enhances it if integration requires changes.

## build-ui

Owns:

- Views/{EntityName}/

Must NOT rewrite CRUD pages.

Only enhances them (visual polish, permission UI).

## build-dashboard-ui

Owns:

- Views/Home/
- Views/Shared/_Layout.cshtml
- wwwroot/css/dashboard.css
- wwwroot/css/theme.css
- wwwroot/js/dashboard.js

Must NOT rewrite dashboard layout.

Only enhances it.

---

# Allowed Changes

You MAY create/update:

Views/Auth/

- Login.cshtml
- Register.cshtml
- AccessDenied.cshtml

Views/Home/

- Dashboard.cshtml
- Index.cshtml

Views/Shared/

- _Layout.cshtml

wwwroot/js/

- auth.js
- dashboard.js

wwwroot/css/

- dashboard.css
- theme.css

Views/{EntityName}/

- Visual polish only
- Permission UI updates only
- Loading state fixes
- alert() replacement

You MAY optionally modify:

AuthController.cs

Only for:

- Fix redirect flow
- Add cookie compatibility
- Add permission claims to view data

---

# Forbidden Changes

NEVER modify:

- Business layer (CompanySystem.Business/)
- Data layer (CompanySystem.Data/)
- Shared layer (CompanySystem.Shared/)
- Services
- Interfaces
- Repositories
- Entities
- DbContext
- Migrations
- Program.cs
- appsettings.json
- JWT logic
- Password handling
- Authentication policy configuration
- Authorization policy configuration

---

# Issue 1 - MVC Auth Integration

Problem:

JWT stored in localStorage works for AJAX calls.

MVC navigation (e.g. <a href="/Department">) does NOT send Bearer token.

Result: 401 Unauthorized on page load.

Solution:

The backend uses composite authentication:

- /api/* routes → JWT Bearer
- MVC routes → Cookie auth

auth.js must:

1. After successful login, store both AccessToken and RefreshToken
2. Set the JWT in a cookie named CompanySystem.Jwt for MVC navigation
3. Keep the Authorization header for AJAX

Add to auth.js after login:

```javascript
document.cookie = "CompanySystem.Jwt=" + accessToken + "; path=/; secure; samesite=lax";
```

On logout:

```javascript
document.cookie = "CompanySystem.Jwt=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC";
```

---

# Issue 2 - Login Page Layout

Problem:

Login/Register pages show sidebar, navbar, admin menu.

They should be standalone clean pages.

Fix:

In _Layout.cshtml, wrap sidebar + navbar in:

```razor
@if (!ViewContext.HttpContext.Request.Path.StartsWithSegments("/Auth"))
{
    <!-- sidebar and navbar here -->
}
```

Login/Register pages should have:

- Centered card layout
- Company branding
- No sidebar
- No navbar
- No admin menu
- No footer menu

---

# Issue 3 - Register Access Control

Register is admin-only.

Backend uses:

```csharp
[Authorize(Roles = "Admin")]
```

on AuthPageController.Register GET.

Fix:

- Remove public Register link from login page
- Keep Register accessible only via /Auth/Register for Admin users
- Non-admin users see Access Denied page
- Menu link for Register only visible to Admin in sidebar

---

# Issue 4 - Password Visibility Toggle

Add custom show/hide password button.

Implementation:

- Add toggle button inside password input group
- Use Bootstrap input-group and btn-outline-secondary
- Toggle input type between "password" and "text"
- Update button icon/text accordingly
- Apply to ALL password fields (Login, Register)

```javascript
function togglePassword(inputId, btnId) {
    let input = document.getElementById(inputId);
    let btn = document.getElementById(btnId);
    if (input.type === "password") {
        input.type = "text";
        btn.textContent = "Hide";
    } else {
        input.type = "password";
        btn.textContent = "Show";
    }
}
```

---

# Issue 5 - Login Redirect

After successful login, redirect to:

/Home/Dashboard

On logout, redirect to:

/Auth/Login

Verify auth.js redirect logic:

- Login success → window.location.href = "/Home/Dashboard"
- Logout success → window.location.href = "/Auth/Login"
- 401 in AJAX → redirect to /Auth/Login
- 403 in AJAX → show permission error

---

# Issue 6 - Sidebar Navigation

Sidebar links must point to existing MVC controllers.

Inspect CompanySystem.Web/Controllers/

Generate links only for existing modules.

Examples:

- /User
- /Department
- /Role
- /Note
- /MainPageSection

Rules:

- Never link to /GetAll, /GetById, /Create, /Edit, /Delete
- Always link to MVC index page (e.g. /User not /User/GetAll)
- Use window.currentUserPermissions to show/hide menu items
- If user lacks permission for a module, hide it

---

# Issue 7 - Permission Based Menu Visibility

Use window.currentUserPermissions (array of strings).

```javascript
let permissions = window.currentUserPermissions || [];

function hasPermission(perm) {
    return permissions.includes(perm);
}

// Example: Show Users menu only if user has any Users permission
if (hasPermission("Users.View")) {
    renderUsersMenuItem();
}
```

Fallback to window.currentUserRole if permissions unavailable:

- Admin → show all
- Manager → show limited
- Employee → show minimal

Permission to Module mapping:

Users module:
- Users.View → show Users menu
- Users.Create → show Create button
- Users.Edit → show Edit button
- Users.Delete → show Delete button

Departments module:
- Departments.View → show Departments menu
- Departments.Create → show Create button
- Departments.Edit → show Edit button
- Departments.Delete → show Delete button

Roles module:
- Roles.View → show Roles menu

Notes module:
- Notes.View → show Notes menu

MainPageSections module:
- MainPageSections.View → show MainPageSections menu

Dashboard:
- Dashboard.View → show Dashboard

Permissions module:
- Permissions.View → show Permissions menu (admin only)

---

# Issue 8 - Authentication State

Ensure:

- window.currentUser is set immediately after login
- window.currentUserPermissions is set immediately after login
- window.currentUserRole (legacy) is set for backward compatibility
- All three are cleared on logout
- AJAX 401 handler redirects to login
- AJAX 403 handler shows permission denied message

---

# Issue 9 - Dashboard UI Polish

Inspect and fix:

Dashboard Cards:

- Must display real data, not placeholders
- Cards: icon, title, description, navigation button
- Hover animations
- Responsive grid layout
- No empty cards

Dashboard Widgets:

- Statistics show real counts from existing endpoints
- Hide statistics that cannot be calculated
- Never create new backend endpoints

Dashboard Header:

- Company name
- Username
- Permission badges (or role badge as fallback)
- User avatar letter

Dashboard Layout:

- Welcome section
- Module cards
- Quick actions
- Clean spacing

---

# Issue 10 - Loading Indicators

Fix all loading states:

- Show loading spinner during AJAX requests
- Hide spinner after success or failure
- Never leave "Loading..." permanently
- Never show placeholder bars indefinitely
- On error: show friendly message, not "Loading..."

Implementation:

```javascript
function showLoading(container) {
    container.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
}

function hideLoading(container, content) {
    container.innerHTML = content;
}
```

---

# Issue 11 - Bootstrap Collapse

Fix Collapse components:

- Arrow rotation on expand/collapse
- Sync expand/collapse state with icon
- Use Bootstrap 5 collapse events

```javascript
document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(btn => {
    btn.addEventListener('click', function() {
        let icon = this.querySelector('.collapse-icon');
        if (icon) {
            icon.classList.toggle('rotated');
        }
    });
});
```

---

# Issue 12 - CRUD Page Polish

Inspect CRUD pages:

Tables:

- Proper Bootstrap table styling
- Responsive on mobile (horizontal scroll if needed)
- Action buttons use permission-based visibility
- No broken column layouts

Forms:

- Proper Bootstrap form layout
- Validation messages styled with Bootstrap
- Submit buttons show loading state
- Success/error messages as Bootstrap alerts

Pagination:

- Bootstrap pagination component
- Active page highlighted
- Previous/Next buttons work
- Page size selector if applicable

Sorting:

- Clickable column headers
- Sort indicator (asc/desc arrow)
- Works with AJAX reload

Filtering:

- Search input with debounce
- Filter dropdowns where applicable
- Clear filter button

---

# Issue 13 - Replace alert()

Replace all alert(), confirm(), prompt() with:

Success:

```html
<div class="alert alert-success alert-dismissible fade show" role="alert">
    Operation completed successfully.
    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
</div>
```

Error:

```html
<div class="alert alert-danger alert-dismissible fade show" role="alert">
    An error occurred.
    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
</div>
```

Confirmation:

Use Bootstrap modal instead of confirm():

```html
<div class="modal fade" id="confirmModal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header"><h5>Confirm</h5></div>
            <div class="modal-body">Are you sure?</div>
            <div class="modal-footer">
                <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                <button class="btn btn-danger" id="confirmBtn">Confirm</button>
            </div>
        </div>
    </div>
</div>
```

---

# Issue 14 - Navigation Verification

Verify every navigation link:

- Opens MVC Razor page (e.g. /User, /Department)
- Never opens JSON endpoint (e.g. /User/GetAll)
- Never opens API route (e.g. /api/User)
- Uses standard MVC routing

Check:

- Sidebar links
- Dashboard card links
- Navbar links
- CRUD action links (Edit, Details, Delete)
- Breadcrumb links

---

# Issue 15 - Responsive Design

Ensure correct rendering on:

Desktop (>=992px):

- Full sidebar visible
- Cards in multi-column grid

Tablet (768px - 991px):

- Collapsible sidebar
- Cards in 2-column grid
- Touch-friendly buttons

Mobile (<768px):

- Hidden sidebar (hamburger menu)
- Cards in single column
- Full-width forms
- Readable font sizes

---

# Issue 16 - Visual Consistency

One design language across the entire app:

- Same color scheme (Bootstrap theme)
- Same card style (rounded corners, shadows, consistent padding)
- Same button style
- Same typography
- Same spacing
- Same animation patterns

Use CSS custom properties in dashboard.css:

```css
:root {
    --company-primary: #0d6efd;
    --company-sidebar-bg: #212529;
    --company-card-radius: 0.5rem;
    --company-card-shadow: 0 0.125rem 0.25rem rgba(0,0,0,0.075);
}
```

---

# Issue 17 - JavaScript Fixes

Inspect and fix:

- Duplicate event handlers (remove then re-attach)
- Duplicate AJAX calls (prevent multiple simultaneous requests)
- Broken selectors (use correct IDs/classes after DOM changes)
- Uncaught exceptions
- Undefined variable references
- Memory leaks (clean up event listeners)

Pattern:

```javascript
// Remove old handler before attaching new one
$("#myBtn").off("click").on("click", function() { ... });

// Prevent duplicate AJAX
if (window.ajaxLoading) return;
window.ajaxLoading = true;
$.ajax({ ... complete: function() { window.ajaxLoading = false; } });
```

---

# Issue 18 - CSS Cleanup

Remove:

- Unused CSS classes
- Duplicate style definitions
- Conflicting styles
- Inline styles where CSS class exists
- Dead CSS from old layouts

Keep:

- dashboard.css for dashboard-specific styles
- theme.css for theme variables
- Remove inline styles from .cshtml files where possible

---

# Final Verification Checklist

Before finishing verify:

✓ Login page renders without sidebar/navbar

✓ Login redirects to /Home/Dashboard

✓ Password toggle works on all password fields

✓ Register is admin-only (no public link)

✓ JWT is stored in cookie for MVC navigation

✓ Logout clears tokens and cookie

✓ Auth state (currentUser, currentUserPermissions) is set on login

✓ Auth state is cleared on logout

✓ 401 redirects to /Auth/Login

✓ 403 shows permission error

✓ Sidebar links point to MVC pages only

✓ Menu items respect user permissions

✓ Dashboard cards show real data

✓ Loading states work correctly

✓ No "Loading..." remains visible

✓ Collapse components work correctly

✓ CRUD tables are responsive

✓ CRUD forms validate properly

✓ CRUD pagination works

✓ CRUD sorting works

✓ CRUD filtering works

✓ No alert() calls remain (use Bootstrap alerts/modals)

✓ Navigation opens MVC pages, never JSON endpoints

✓ Responsive on desktop, tablet, mobile

✓ Visual design is consistent

✓ No duplicate JavaScript events

✓ No broken selectors

✓ No duplicate/conflicting CSS

✓ No backend files modified

✓ dotnet build produces 0 errors

Run:

dotnet build

Required:

0 errors
