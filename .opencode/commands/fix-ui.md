---
description: Fix and polish the complete ASP.NET Core MVC frontend. Resolves Auth UI integration, Dashboard UX, CRUD polish, permission-based UI, loading states, JavaScript issues and visual consistency.
agent: ui-builder
---

Load the fix-ui skill and execute it.

Read the existing project before making changes.

Read:

- CompanySystem.Web/Views/Shared/_Layout.cshtml
- CompanySystem.Web/Views/Auth/Login.cshtml
- CompanySystem.Web/Views/Auth/AccessDenied.cshtml
- CompanySystem.Web/Views/Home/Dashboard.cshtml
- CompanySystem.Web/wwwroot/js/auth.js
- CompanySystem.Web/wwwroot/js/dashboard.js
- CompanySystem.Web/wwwroot/css/dashboard.css
- CompanySystem.Web/wwwroot/css/theme.css

Inspect all CRUD views for visual and functional issues.

Inspect all JavaScript for duplicate events, broken selectors, alert() usage.

Tasks:

1. Fix MVC Auth Integration

Ensure JWT is stored in cookie for MVC navigation:

document.cookie = "CompanySystem.Jwt=" + token + "; path=/; secure; samesite=lax";

Clear cookie on logout.

2. Fix Login Page Layout

Wrap sidebar and navbar in _Layout.cshtml with:

@if (!ViewContext.HttpContext.Request.Path.StartsWithSegments("/Auth"))

Login page must be a standalone centered card.

3. Add Password Toggle

Add show/hide button to all password fields.

Use Bootstrap input-group and btn-outline-secondary.

4. Fix Login Redirect

Login → /Home/Dashboard

Logout → /Auth/Login

401 → /Auth/Login

5. Fix Sidebar Navigation

Links point to MVC controllers only.

Examples:

/User

/Department

/Role

/Note

/MainPageSection

Never link to /GetAll, /GetById, /Create, /Edit, /Delete.

6. Fix Permission Based Menu Visibility

Use window.currentUserPermissions to show/hide menu items.

Fallback to window.currentUserRole if permissions unavailable.

7. Fix Authentication State

Ensure currentUser, currentUserPermissions, currentUserRole are set on login.

Clear on logout.

8. Fix Dashboard UI

Polish cards, widgets, statistics, header, layout.

Remove placeholders.

Fix loading indicators.

9. Fix Loading States

Add spinners during AJAX.

Hide on success/failure.

Never leave "Loading..." permanently visible.

10. Fix Bootstrap Collapse

Arrow rotation on expand/collapse.

Sync state with icon.

11. Fix CRUD Pages

Polish tables, forms, pagination, sorting, filtering.

Permission-based button visibility.

12. Replace alert()

Use Bootstrap alerts for messages.

Use Bootstrap modals for confirmations.

13. Verify Navigation

Every link opens an MVC Razor page, never a JSON endpoint.

14. Fix Responsive Design

Desktop, tablet, mobile all work correctly.

15. Ensure Visual Consistency

Same color scheme, card style, typography across all pages.

16. Fix JavaScript Issues

Remove duplicate events, duplicate AJAX calls, broken selectors.

17. Clean CSS

Remove unused, duplicate, conflicting styles.

Do NOT Modify:

- Business layer
- Data layer
- Shared layer
- Services
- Interfaces
- Repositories
- Entities
- DTOs
- DbContext
- Migrations
- Program.cs
- appsettings.json
- JWT logic
- Password handling
- Auth policy configuration

Final verification:

✓ Login page renders without sidebar/navbar

✓ Login redirects to /Home/Dashboard

✓ Password toggle works

✓ JWT cookie set for MVC navigation

✓ Logout clears tokens

✓ Auth state set on login, cleared on logout

✓ 401 redirects to login

✓ 403 shows permission error

✓ Sidebar links to MVC pages

✓ Permission-based menu works

✓ Dashboard shows real data

✓ Loading states work

✓ No "Loading..." remains

✓ CRUD pages polished

✓ No alert() remains

✓ Navigation opens MVC pages

✓ Responsive design works

✓ Visual design consistent

✓ No JS errors

✓ No backend files modified

Run:

dotnet build

Result:

0 errors
