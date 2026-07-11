---
description: Polish and fix the complete ASP.NET Core MVC frontend. Improve Dashboard, Authentication UI, Layout, Sidebar, Navigation, UX, JavaScript interactions and visual consistency.
agent: ui-builder
---

Load the fix-dashboard-ui skill and execute it.

Analyze the current frontend before making any changes.

Inspect:

- CompanySystem.Web/Views/
- CompanySystem.Web/wwwroot/css/
- CompanySystem.Web/wwwroot/js/

Inspect existing pages:

Authentication

- Views/Auth/Login.cshtml
- Views/Auth/Register.cshtml
- Views/Auth/AccessDenied.cshtml

Dashboard

- Views/Home/
- Views/Shared/_Layout.cshtml

CRUD

- Views/User/
- Views/Department/
- Views/Role/
- Views/Note/
- Views/MainPageSection/

Inspect JavaScript:

- auth.js
- dashboard.js
- site.js

Inspect CSS:

- dashboard.css
- theme.css
- site.css

---

Tasks

1.

Improve Login UI.

Generate a professional company login page.

Requirements:

- Company branding
- Modern appearance
- Responsive layout
- Better typography
- Better spacing
- Better buttons

Do NOT display:

- Sidebar
- Navbar
- Dashboard menu

before authentication.

---

2.

Improve password fields.

Requirements:

Always display:

Show / Hide password icon.

Implement using JavaScript.

Never rely on browser implementation.

Apply to:

- Login
- Register

---

3.

Protect Register UI.

Requirements:

Anonymous users:

Never see Register button.

Non Admin users:

Never see Register button.

If a non-admin manually visits:

/Auth/Register

Show:

AccessDenied

or

redirect according to the existing authentication flow.

Never expose Admin functionality.

---

4.

Inspect Dashboard.

Improve:

- Cards
- Widgets
- Statistics
- Colors
- Icons
- Typography
- Animations

Remove:

- Empty widgets
- Placeholder cards
- Broken statistics
- Infinite Loading
- Empty IDs
- Dummy data

---

5.

Inspect Sidebar.

Fix:

- Active menu
- Collapse
- Expand
- Responsive behavior
- Mobile behavior
- Scroll
- Navigation highlighting

---

6.

Inspect Navbar.

Improve:

- User information
- Role badge
- Logout
- Company branding
- Responsive behavior

---

7.

Inspect all loading components.

Fix:

Loading indicators.

Requirements:

Loading appears only while AJAX request is running.

Loading disappears after:

Success

or

Failure.

Never remain visible forever.

---

8.

Inspect every Bootstrap Collapse.

Fix:

Arrow rotation.

Expand.

Collapse.

Animation.

Synchronization.

Arrow must always match the actual collapsed state.

---

9.

Inspect every CRUD page.

Improve:

Tables.

Forms.

Cards.

Buttons.

Pagination.

Sorting.

Filtering.

Validation.

Responsive behavior.

Never change backend logic.

---

10.

Replace poor UI behavior.

Replace:

alert()

with:

Bootstrap Alerts

or

Bootstrap Toasts.

Never expose raw exceptions.

Display user friendly messages.

---

11.

Inspect Navigation.

Verify every menu item.

Navigation must always open MVC Razor pages.

Never navigate directly to:

- GetAll
- GetById
- API endpoints
- JSON endpoints

Fix incorrect links if found.

---

12.

Improve responsive behavior.

Verify:

Desktop.

Tablet.

Mobile.

Fix:

Overflow.

Broken spacing.

Broken cards.

Broken forms.

Broken tables.

Broken navigation.

---

13.

Improve visual consistency.

Use one design language.

Verify:

Buttons.

Typography.

Cards.

Borders.

Icons.

Spacing.

Colors.

Professional enterprise appearance.

---

14.

Inspect JavaScript.

Fix:

Duplicated event handlers.

Duplicated AJAX calls.

Broken selectors.

Broken click handlers.

Broken toggle handlers.

Broken modal handlers.

Broken initialization.

Avoid duplicate code.

---

15.

Inspect CSS.

Remove:

Unused CSS.

Duplicated styles.

Conflicting styles.

Fix:

Alignment.

Spacing.

Responsive layout.

Hover effects.

Focus styles.

---

16.

Final inspection.

Verify:

✓ Login UI

✓ Register UI

✓ Dashboard

✓ Sidebar

✓ Navbar

✓ Layout

✓ Tables

✓ Forms

✓ Loading

✓ Collapse

✓ Responsive layout

✓ Role visibility

✓ Navigation

✓ JavaScript

✓ CSS

✓ Bootstrap

✓ Authentication still works

✓ Authorization still works

✓ No backend files modified

Follow the fix-dashboard-ui skill instructions exactly.

Run:

dotnet build

Required result:

0 errors.