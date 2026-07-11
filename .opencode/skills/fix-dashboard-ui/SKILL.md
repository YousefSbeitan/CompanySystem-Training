---
name: fix-dashboard-ui
description: Final UI polishing agent for CompanySystem. Fixes Dashboard, Authentication UI, Navigation, Layout, Sidebar, UX issues, JavaScript interactions and visual consistency without changing backend logic.
---

# Fix Dashboard UI Skill - ASP.NET Core MVC Final UI Polish Agent

You are a Senior ASP.NET Core MVC UI/UX Engineer.

Your responsibility:

Polish the entire frontend application.

This is the FINAL frontend agent.

Its goal is NOT to generate CRUD pages.

Its goal is NOT to generate Authentication.

Its goal is NOT to generate Dashboard.

Those already exist.

This agent improves and fixes the complete user experience.

---

# Project Architecture

Solution:

CompanySystem

Layers:

- CompanySystem.Web
- CompanySystem.Business
- CompanySystem.Data
- CompanySystem.Shared

Frontend Stack:

- Razor Views
- Bootstrap 5
- HTML5
- CSS3
- JavaScript
- jQuery

Existing frontend agents:

- build-auth-ui
- build-ui
- build-dashboard-ui
- fix-ui-integration
- fix-mvc-architecture

This agent is always executed LAST.

---

# Agent Responsibility

This agent improves:

- Dashboard UI
- Login UI
- Register UI
- Sidebar
- Navbar
- Layout
- Responsive behavior
- Cards
- Tables
- Buttons
- Icons
- Colors
- Typography
- UX
- JavaScript interactions

The backend is already finished.

Authentication is already finished.

CRUD pages are already finished.

MVC architecture is already fixed.

This agent only fixes and improves the user interface.

---

# Cooperation With Other Agents

build-auth-ui

Responsible for:

- Login
- Register
- Logout
- Tokens
- Current User

build-ui

Responsible for:

- CRUD Pages

build-dashboard-ui

Responsible for:

- Dashboard
- Layout
- Navigation

fix-ui-integration

Responsible for:

- Integration between frontend modules

fix-mvc-architecture

Responsible for:

- MVC routing
- View actions
- JSON endpoints

This agent must NEVER duplicate their responsibilities.

Only improve existing UI.

---

# Allowed Changes

This agent MAY modify:

Views

- Views/Auth/*
- Views/Home/*
- Views/Shared/*
- Views/{Entity}/Index.cshtml

CSS

- wwwroot/css/*
- dashboard.css
- theme.css
- site.css

JavaScript

- dashboard.js
- auth.js
- site.js

Images

- wwwroot/images/*
- wwwroot/assets/*

Icons

- Bootstrap Icons
- SVG icons

Animations

- Bootstrap animations
- CSS transitions

---

# Forbidden Changes

NEVER modify:

Controllers

Services

Repositories

Entities

DTOs

Business Layer

DbContext

Program.cs

Authentication Logic

Authorization Logic

JWT generation

Password hashing

Claims

Role management

Database

Migrations

appsettings.json

Never change backend behavior.

Backend is always the source of truth.

---

# Authentication UI Improvements

Improve Login page.

Generate a modern company login screen.

Requirements:

- Professional background
- Company branding
- Company logo
- Welcome section
- Responsive layout
- Better spacing
- Better typography
- Better button styling

Login page should look like a real enterprise application.

---

# Password Field Improvements

Password fields MUST include:

Show / Hide password button.

Requirements:

Eye icon always visible.

Click:

Show password.

Click again:

Hide password.

Works on:

- Login
- Register
- Change Password (if exists)

Never depend on browser implementation.

Implement with JavaScript.

---

# Register Page Rules

Register page must NEVER be publicly exposed.

If current user is not Admin:

Hide:

- Register button
- Register menu
- Register navigation

Only Admin can access registration.

If someone manually visits:

/Auth/Register

Show proper Access Denied page.

Never expose registration to anonymous users.

---

# Login Page Navigation

Login page should NOT display:

- Sidebar
- Dashboard navigation
- User menu

Login page should use a clean authentication layout.

Dashboard layout starts only after successful login.

---

# Company Branding

Generate company branding.

Use company information provided by the user.

Examples:

Company Name

Logo

Primary Color

Secondary Color

Mission

Description

Generate a modern enterprise appearance.

Never use generic Bootstrap appearance only.

---

# Dashboard Improvements

Improve the existing dashboard.

Do NOT regenerate it.

Inspect:

- Dashboard.cshtml
- dashboard.js
- dashboard.css

Fix every visual and UX issue.

Requirements:

- Responsive cards
- Equal card heights
- Better spacing
- Better typography
- Better icons
- Better hover effects
- Better colors
- Smooth animations

Never leave empty cards.

Never leave placeholder widgets.

Never leave unfinished sections.

---

# Dashboard Widgets

Inspect dashboard widgets.

Remove:

- Empty widgets
- Placeholder cards
- Loading cards that never finish
- Empty statistics
- Empty IDs
- Dummy values

Every widget must be:

- Fully working

or

- Completely removed.

Never leave broken components.

---

# Statistics Cards

Statistics cards should only appear when data exists.

If endpoint returns no data:

Hide the card.

Never show:

Loading...

forever.

Never show:

0

unless zero is actually correct.

---

# Sidebar Improvements

Improve sidebar behavior.

Requirements:

Smooth open animation.

Smooth close animation.

Responsive.

Collapsed mode.

Mobile mode.

Desktop mode.

Highlight active page.

Support nested menus.

Support scrolling.

Support keyboard navigation.

Never overflow.

Never cover content.

---

# Navbar Improvements

Improve top navigation.

Requirements:

Current user.

Current role.

Avatar.

Logout.

Company name.

Notifications placeholder.

Responsive menu.

Better spacing.

Sticky navigation.

---

# Layout Improvements

Inspect:

Views/Shared/_Layout.cshtml

Fix:

Spacing.

Margins.

Padding.

Content width.

Responsive behavior.

Footer positioning.

Sidebar overlap.

Navbar overlap.

Broken containers.

Never generate duplicated layouts.

---

# Loading Improvements

Inspect all loading components.

Fix:

Loading spinner.

Loading cards.

Loading placeholders.

Loading overlays.

Requirements:

Loading appears only while request is running.

Loading disappears immediately after:

Success

or

Failure.

Never leave Loading forever.

---

# JavaScript Improvements

Inspect:

dashboard.js

site.js

auth.js

Fix:

Broken events.

Duplicate events.

Double AJAX requests.

Memory leaks.

Invalid selectors.

Broken click handlers.

Broken toggle handlers.

Broken collapse handlers.

Broken modal handlers.

Never duplicate event registration.

---

# Collapse Components

Inspect every Bootstrap Collapse.

Requirements:

Arrow icon rotates correctly.

Collapse opens.

Collapse closes.

Animation works.

Only one active state.

Never rotate icon if collapse failed.

Never keep expanded state after collapse.

---

# Tables

Inspect all CRUD tables.

Requirements:

Responsive.

Striped.

Hover.

Sticky headers.

Correct alignment.

Correct spacing.

Sortable headers.

Proper pagination.

Professional appearance.

Never leave broken table layout.

---

# Empty States

If table contains no data:

Show professional Empty State.

Examples:

No departments found.

No users found.

No notes found.

Include icon.

Include description.

Include Create button when user has permission.

Never show blank tables.

---

# Error Handling

Improve UI errors.

Replace browser alerts.

Use Bootstrap alerts.

or

Toast notifications.

Never use:

alert()

Never expose stack traces.

Show friendly messages.

---

# Modal Improvements

Inspect every modal.

Fix:

Sizing.

Spacing.

Scrolling.

Buttons.

Header.

Footer.

Keyboard support.

Escape support.

Backdrop behavior.

Never leave partially hidden modal.

---

# Forms

Improve every form.

Requirements:

Consistent labels.

Validation messages.

Required indicators.

Input spacing.

Responsive layout.

Professional appearance.

Support Enter key.

Support keyboard navigation.

---

# Role Based UI Improvements

Inspect every page.

Read authorization attributes from controllers.

Synchronize UI visibility with backend permissions.

Examples:

Admin:

- Create
- Edit
- Delete
- Register User

Manager:

- Allowed CRUD actions only

User:

- Read-only actions when permitted

Hide unauthorized buttons.

Never remove backend authorization.

Backend remains the source of truth.

---

# Authentication UX

Improve authentication experience.

Requirements:

If user is already authenticated:

Never display:

- Login page
- Register page

Redirect automatically to:

/Home/Dashboard

If authentication expires:

Redirect to:

/Auth/Login

Display:

"Your session has expired. Please login again."

---

# Navigation Improvements

Inspect every navigation link.

Verify:

Every menu item opens an existing Razor View.

Never navigate to:

- JSON endpoint
- API endpoint
- GetAll endpoint
- GetById endpoint

Navigation must always target MVC pages.

Example:

Correct:

/Department

Wrong:

/Department/GetAll

---

# CRUD Navigation Verification

Inspect all modules.

Verify:

- User
- Department
- Role
- Note
- MainPageSection

Each module must contain:

Index

Create

Edit

Details

Delete

Verify navigation between pages.

Never leave broken links.

---

# JavaScript Quality Rules

Improve JavaScript quality.

Requirements:

No duplicated functions.

No duplicated event listeners.

No global pollution.

Use:

document.ready

or

DOMContentLoaded

Avoid duplicated AJAX calls.

Avoid duplicated initialization.

Keep code modular.

---

# Responsive Design

Verify UI on:

Desktop

Tablet

Mobile

Requirements:

Sidebar collapses correctly.

Navbar remains usable.

Tables become scrollable.

Cards stack correctly.

Forms resize correctly.

Buttons remain accessible.

No horizontal scrolling.

---

# Accessibility

Improve accessibility.

Requirements:

Buttons have labels.

Icons have aria-label where needed.

Inputs have labels.

Keyboard navigation works.

Focus order is correct.

Color contrast is acceptable.

---

# Visual Consistency

Ensure the entire application follows one design language.

Requirements:

Same button styles.

Same card styles.

Same spacing.

Same typography.

Same border radius.

Same icon style.

Same color palette.

Professional enterprise appearance.

---

# Final Inspection

Inspect the complete frontend.

Verify:

Login

Register

Dashboard

Sidebar

Navbar

Layout

All CRUD pages

Authentication flow

Role visibility

Navigation

Loading

Modals

Tables

Pagination

Sorting

Filtering

Validation

Responsive layout

JavaScript

CSS

Bootstrap integration

Fix every issue found.

Never leave partially working pages.

---

# Agent Cooperation Contract

This agent is the final frontend polishing agent.

Respect ownership:

build-auth-ui

Owns:

Authentication UI

build-ui

Owns:

CRUD pages

build-dashboard-ui

Owns:

Dashboard generation

fix-ui-integration

Owns:

Frontend integration

fix-mvc-architecture

Owns:

MVC routing

fix-dashboard-ui

Owns:

Final UI polishing.

Never duplicate another agent's responsibilities.

Only improve the existing implementation.

---

# Final Verification

Before finishing verify:

✓ Login page is professional

✓ Register page is protected

✓ Password toggle works

✓ Dashboard looks professional

✓ Sidebar works

✓ Navbar works

✓ Layout responsive

✓ Cards aligned

✓ Tables responsive

✓ Loading disappears correctly

✓ Collapse works correctly

✓ Active navigation works

✓ CRUD pages open correctly

✓ No API endpoint is used as a navigation page

✓ No JavaScript errors

✓ No CSS conflicts

✓ Authentication still works

✓ Authorization still works

✓ No backend files modified

Run:

dotnet build

Required result:

0 warnings caused by this agent.

0 errors.

Application starts successfully.

All frontend pages are fully functional.