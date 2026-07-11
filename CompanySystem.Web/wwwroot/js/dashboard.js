// ── CompanySystem – Role-Based Dashboard ────────────────────────────
// Supports Admin, Manager, HR, Employee (User) roles.
// Module visibility is inferred from controllers via Authorize attributes.

(function () {
    'use strict';

    // ── Module Definitions (inferred from controller Authorize attributes) ──
    // Users:     Index=[Authorize], GetAll=[Authorize(Roles="Admin,Manager,HR")]
    // Departments: Index=[Authorize(Roles="Admin,Manager")]
    // Roles:     All actions=[Authorize(Roles="Admin")]
    // Notes:     Index=[Authorize] (all authenticated), but Employee explicitly excluded per business rule
    // MainPageSections: Index=[AllowAnonymous]
    var modules = [
        {
            id: 'users',
            title: 'Users',
            desc: 'Manage system users and employees',
            url: '/User',
            icon: 'users',
            roles: ['Admin', 'Manager', 'HR']
        },
        {
            id: 'departments',
            title: 'Departments',
            desc: 'Organize company departments',
            url: '/Department',
            icon: 'departments',
            roles: ['Admin', 'Manager']
        },
        {
            id: 'roles',
            title: 'Roles',
            desc: 'Configure access permissions',
            url: '/Role',
            icon: 'roles',
            roles: ['Admin']
        },
        {
            id: 'notes',
            title: 'Notes',
            desc: 'View and manage notes',
            url: '/Note',
            icon: 'notes',
            roles: ['Admin', 'Manager', 'HR']
        },
        {
            id: 'content',
            title: 'Main Page Sections',
            desc: 'Edit homepage content blocks',
            url: '/MainPageSection',
            icon: 'content',
            roles: ['Admin', 'Manager', 'HR', 'User']
        }
    ];

    // ── Icon Map ──
    var iconMap = {
        'dashboard': 'bi-speedometer2',
        'users': 'bi-people-fill',
        'departments': 'bi-building-fill',
        'roles': 'bi-shield-fill-check',
        'notes': 'bi-sticky-fill',
        'content': 'bi-layout-text-window-reverse'
    };

    function getModuleIcon(iconName) {
        return iconMap[iconName] || 'bi-circle-fill';
    }

    // ═══════════════════════════════════════════════════════════════════
    //  INITIALIZATION
    // ═══════════════════════════════════════════════════════════════════

    function initDashboard() {
        var user = window.currentUser || { id: '', username: '', role: '' };
        var role = user.role || '';
        var isLoggedIn = !!user.username;

        // Build sidebar navigation
        buildSidebarNav(role);

        // Build user menu in top navbar
        if ($('#topNavRight').length) {
            buildUserMenu(user, isLoggedIn);
        }

        // Render role-specific dashboard content
        if ($('#dashboardContent').length) {
            renderRoleDashboard(role, user);
        }

        // Toggle body classes for role-based CSS
        document.body.classList.remove('role-admin', 'role-manager', 'role-hr', 'role-user');
        var roleClass = 'role-' + (role || 'user').toLowerCase();
        document.body.classList.add(roleClass);

        // Update footer year
        var yearEl = document.getElementById('footerYear');
        if (yearEl) {
            yearEl.textContent = new Date().getFullYear();
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  ROLE DISPATCH
    // ═══════════════════════════════════════════════════════════════════

    function renderRoleDashboard(role, user) {
        var roleLower = (role || '').toLowerCase();

        switch (roleLower) {
            case 'admin':
                renderAdminDashboard(user);
                break;
            case 'manager':
                renderManagerDashboard(user);
                break;
            case 'hr':
                renderHRDashboard(user);
                break;
            default:
                renderEmployeeDashboard(user);
                break;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  HELPERS
    // ═══════════════════════════════════════════════════════════════════

    function setWelcome(title, subtitle) {
        var subEl = document.getElementById('welcomeSubtitle');
        if (subEl) {
            subEl.textContent = subtitle || '';
        }
    }

    function hasAccess(allowedRoles, userRole) {
        if (!userRole) return false;
        return allowedRoles.some(function (r) {
            return r.toLowerCase() === userRole.toLowerCase();
        });
    }

    function getRoleBadgeClass(role) {
        var r = (role || '').toLowerCase();
        if (r === 'admin') return 'bg-danger';
        if (r === 'manager') return 'bg-warning text-dark';
        if (r === 'hr') return 'bg-purple text-white';
        return 'bg-info text-dark';
    }

    function escapeHtml(str) {
        if (!str) return '';
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(str));
        return div.innerHTML;
    }

    function setActiveNavLink() {
        var currentPath = window.location.pathname;
        var links = document.querySelectorAll('.sidebar-nav .nav-link');
        links.forEach(function (link) {
            link.classList.remove('active');
            var href = link.getAttribute('href');
            if (href === currentPath || (currentPath.indexOf(href + '/') === 0 && href !== '/')) {
                link.classList.add('active');
            }
        });
    }

    // ═══════════════════════════════════════════════════════════════════
    //  USER PROFILE
    // ═══════════════════════════════════════════════════════════════════

    function renderUserProfile(user) {
        var avatarLetter = user.username ? user.username.charAt(0).toUpperCase() : '?';
        var roleBadgeClass = getRoleBadgeClass(user.role);

        var html = '<div class="user-profile-card">';
        html += '<div class="row align-items-center">';
        html += '<div class="col-auto">';
        html += '<div class="user-avatar-large">' + escapeHtml(avatarLetter) + '</div>';
        html += '</div>';
        html += '<div class="col">';
        html += '<h3>' + escapeHtml(user.username || '—') + '</h3>';
        html += '<div class="user-meta">';
        html += '<span class="badge ' + roleBadgeClass + '">' + escapeHtml(user.role || 'User') + '</span>';
        html += '<span class="badge bg-white text-dark">ID: ' + escapeHtml(user.id || '—') + '</span>';
        html += '</div>';
        html += '</div>';
        html += '</div>';
        html += '</div>';

        var container = document.getElementById('userProfileContainer');
        if (container) {
            container.innerHTML = html;
        }
    }

    function loadDetailedProfile(user) {
        $.ajax({
            url: '/User/GetById?id=' + encodeURIComponent(user.id),
            type: 'GET',
            success: function (response) {
                renderDetailedProfile(response);
            },
            error: function () {
                renderUserProfile(user);
            }
        });
    }

    function renderDetailedProfile(userData) {
        if (!userData) return;

        var username = userData.username || userData.userName || '—';
        var roleDisplay = window.currentUserRole || 'User';
        var userId = userData.userId || userData.id || '—';
        var avatarLetter = username.charAt(0).toUpperCase();
        var roleBadgeClass = getRoleBadgeClass(roleDisplay);

        var yearsExp = '—';
        if (userData.startDate) {
            var startDate = new Date(userData.startDate);
            if (!isNaN(startDate.getTime())) {
                var diffYears = Math.floor((Date.now() - startDate.getTime()) / (1000 * 60 * 60 * 24 * 365.25));
                yearsExp = diffYears + ' year' + (diffYears !== 1 ? 's' : '');
            }
        }

        var statusBadge = userData.isActive
            ? '<span class="badge bg-success">Active</span>'
            : '<span class="badge bg-secondary">Inactive</span>';

        var html = '<div class="user-profile-card detailed-profile">';
        html += '<div class="row align-items-center">';
        html += '<div class="col-auto">';
        html += '<div class="user-avatar-large">' + escapeHtml(avatarLetter) + '</div>';
        html += '</div>';
        html += '<div class="col">';
        html += '<h3>' + escapeHtml(username) + '</h3>';
        html += '<div class="user-meta">';
        html += '<span class="badge ' + roleBadgeClass + '">' + escapeHtml(roleDisplay) + '</span>';
        html += '<span class="badge bg-white text-dark">ID: ' + escapeHtml(userId) + '</span>';
        html += statusBadge;
        html += '</div>';
        html += '</div>';
        html += '</div>';

        // Additional details
        html += '<div class="row mt-3 g-3">';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Phone</span><span class="detail-value">' + escapeHtml(userData.phoneNumber || '—') + '</span></div></div>';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Experience</span><span class="detail-value">' + escapeHtml(yearsExp) + '</span></div></div>';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Department</span><span class="detail-value">' + escapeHtml(userData.departmentName || userData.departmentId || '—') + '</span></div></div>';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Leader</span><span class="detail-value">' + escapeHtml(userData.leaderName || userData.leaderId || 'None') + '</span></div></div>';
        html += '</div>';

        html += '</div>';

        var container = document.getElementById('userProfileContainer');
        if (container) {
            container.innerHTML = html;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  ADMIN DASHBOARD
    // ═══════════════════════════════════════════════════════════════════

    function renderAdminDashboard(user) {
        setWelcome('Administration Dashboard', 'Full system control and management');
        renderUserProfile(user);

        var container = document.getElementById('dashboardContent');
        if (!container) return;

        var html = '';

        // Statistics Cards
        html += '<div class="row g-4 mb-4" id="statCardsContainer">';
        html += buildStatCard('users', 'Total Users', 'bi-people-fill', 'users', '/User');
        html += buildStatCard('departments', 'Total Departments', 'bi-building-fill', 'departments', '/Department');
        html += buildStatCard('roles', 'Total Roles', 'bi-shield-fill-check', 'roles', '/Role');
        html += buildStatCard('content', 'Content Sections', 'bi-layout-text-window-reverse', 'content', '/MainPageSection');
        html += '</div>';

        // Quick Actions
        html += '<h2 class="section-title"><i class="bi bi-lightning-charge-fill me-2"></i>Quick Actions</h2>';
        html += '<div class="row g-3 mb-4">';
        html += buildQuickAction('New User', 'bi-person-plus-fill', 'primary', '/User/Create', 'Create a new user account');
        html += buildQuickAction('New Department', 'bi-building-add-fill', 'success', '/Department/Create', 'Add a new department');
        html += buildQuickAction('New Role', 'bi-shield-plus-fill', 'warning', '/Role/Create', 'Create a new role');
        html += buildQuickAction('New Section', 'bi-file-plus-fill', 'info', '/MainPageSection/Create', 'Add homepage content');
        html += '</div>';

        // Management Modules
        html += '<h2 class="section-title"><i class="bi bi-grid-3x3-gap-fill me-2"></i>Management Modules</h2>';
        html += '<div class="row g-4" id="dashboardModulesContainer">';
        var adminModules = modules.filter(function (m) { return hasAccess(m.roles, 'Admin'); });
        adminModules.forEach(function (mod) {
            html += buildModuleCard(mod);
        });
        html += '</div>';

        container.innerHTML = html;

        // Load statistics
        loadAdminStatistics();
    }

    // ═══════════════════════════════════════════════════════════════════
    //  MANAGER DASHBOARD
    // ═══════════════════════════════════════════════════════════════════

    function renderManagerDashboard(user) {
        setWelcome('Team Management Dashboard', 'Manage your team and departments');
        renderUserProfile(user);

        var container = document.getElementById('dashboardContent');
        if (!container) return;

        var html = '';

        // Statistics Cards
        html += '<div class="row g-4 mb-4" id="statCardsContainer">';
        html += buildStatCard('users', 'Total Users', 'bi-people-fill', 'users', '/User');
        html += buildStatCard('departments', 'Total Departments', 'bi-building-fill', 'departments', '/Department');
        html += '</div>';

        // Quick Actions
        html += '<h2 class="section-title"><i class="bi bi-lightning-charge-fill me-2"></i>Quick Actions</h2>';
        html += '<div class="row g-3 mb-4">';
        html += buildQuickAction('Add Note', 'bi-plus-circle-fill', 'purple', '/Note/Create', 'Create a new note');
        html += buildQuickAction('View Team', 'bi-people-fill', 'primary', '/User', 'See all team members');
        html += buildQuickAction('Departments', 'bi-building-fill', 'success', '/Department', 'View departments');
        html += '</div>';

        // Management Modules
        html += '<h2 class="section-title"><i class="bi bi-grid-3x3-gap-fill me-2"></i>Management Modules</h2>';
        html += '<div class="row g-4" id="dashboardModulesContainer">';
        var managerModules = modules.filter(function (m) { return hasAccess(m.roles, 'Manager'); });
        managerModules.forEach(function (mod) {
            html += buildModuleCard(mod);
        });
        html += '</div>';

        container.innerHTML = html;

        // Load statistics
        loadManagerStatistics();
    }

    // ═══════════════════════════════════════════════════════════════════
    //  HR DASHBOARD
    // ═══════════════════════════════════════════════════════════════════

    function renderHRDashboard(user) {
        setWelcome('Human Resources Dashboard', 'Manage personnel and company content');
        renderUserProfile(user);

        var container = document.getElementById('dashboardContent');
        if (!container) return;

        var html = '';

        // Statistics Cards
        html += '<div class="row g-4 mb-4" id="statCardsContainer">';
        html += buildStatCard('users', 'Total Users', 'bi-people-fill', 'users', '/User');
        html += buildStatCard('content', 'Content Sections', 'bi-layout-text-window-reverse', 'content', '/MainPageSection');
        html += '</div>';

        // Quick Actions
        html += '<h2 class="section-title"><i class="bi bi-lightning-charge-fill me-2"></i>Quick Actions</h2>';
        html += '<div class="row g-3 mb-4">';
        html += buildQuickAction('New User', 'bi-person-plus-fill', 'primary', '/User/Create', 'Create a new user account');
        html += buildQuickAction('Add Note', 'bi-plus-circle-fill', 'purple', '/Note/Create', 'Create a new note');
        html += '</div>';

        // Management Modules
        html += '<h2 class="section-title"><i class="bi bi-grid-3x3-gap-fill me-2"></i>Management Modules</h2>';
        html += '<div class="row g-4" id="dashboardModulesContainer">';
        var hrModules = modules.filter(function (m) { return hasAccess(m.roles, 'HR'); });
        hrModules.forEach(function (mod) {
            html += buildModuleCard(mod);
        });
        html += '</div>';

        container.innerHTML = html;

        // Load statistics
        loadHRStatistics();
    }

    // ═══════════════════════════════════════════════════════════════════
    //  EMPLOYEE DASHBOARD
    // ═══════════════════════════════════════════════════════════════════

    function renderEmployeeDashboard(user) {
        setWelcome('Employee Portal', 'Welcome back! Here\'s your personal workspace');

        // Show basic profile immediately, enhance with details from API
        renderUserProfile(user);
        if (user.id) {
            loadDetailedProfile(user);
        }

        var container = document.getElementById('dashboardContent');
        if (!container) return;

        var html = '';

        // Quick Personal Actions
        html += '<h2 class="section-title"><i class="bi bi-person-lines-fill me-2"></i>Quick Actions</h2>';
        html += '<div class="row g-3 mb-4">';
        html += buildQuickAction('Company Info', 'bi-building-fill', 'info', '/MainPageSection', 'Browse company information');
        html += '</div>';

        // Company Information
        html += '<h2 class="section-title"><i class="bi bi-megaphone-fill me-2"></i>Company Information</h2>';
        html += '<div class="row g-4" id="companyInfoContainer">';
        html += '<div class="col-12"><div class="text-center py-4"><div class="spinner-border spinner-border-sm text-primary me-2" role="status"></div>Loading company info...</div></div>';
        html += '</div>';

        container.innerHTML = html;

        // Load data
        loadCompanyInfo();
    }

    // ═══════════════════════════════════════════════════════════════════
    //  BUILDERS – Stat Card, Quick Action, Module Card
    // ═══════════════════════════════════════════════════════════════════

    function buildStatCard(key, label, icon, iconColor, link) {
        var colorClass = 'icon-' + iconColor;
        return '<div class="col-md-6 col-lg-3">' +
            '<div class="stat-card" id="statCard_' + key + '">' +
            '<div class="stat-icon ' + colorClass + '"><i class="bi ' + icon + '"></i></div>' +
            '<div class="stat-value" id="statValue_' + key + '">—</div>' +
            '<div class="stat-label">' + escapeHtml(label) + '</div>' +
            '<a href="' + escapeHtml(link) + '" class="stat-link">Manage <i class="bi bi-arrow-right"></i></a>' +
            '</div></div>';
    }

    function buildQuickAction(title, icon, color, link, desc) {
        var colorMap = {
            'primary': 'btn-primary',
            'success': 'btn-success',
            'warning': 'btn-warning',
            'info': 'btn-info',
            'danger': 'btn-danger',
            'purple': 'btn-purple'
        };
        var btnClass = colorMap[color] || 'btn-primary';
        return '<div class="col-md-4 col-lg-3">' +
            '<a href="' + escapeHtml(link) + '" class="quick-action-card text-decoration-none">' +
            '<div class="quick-action-icon ' + btnClass + '"><i class="bi ' + icon + '"></i></div>' +
            '<div class="quick-action-title">' + escapeHtml(title) + '</div>' +
            '<div class="quick-action-desc">' + escapeHtml(desc || '') + '</div>' +
            '</a></div>';
    }

    function buildModuleCard(mod) {
        var icon = getModuleIcon(mod.icon);
        return '<div class="col-md-6 col-lg-4">' +
            '<a href="' + escapeHtml(mod.url) + '" class="module-card">' +
            '<div class="module-icon mod-' + mod.icon + '"><i class="bi ' + icon + '"></i></div>' +
            '<div class="module-title">' + escapeHtml(mod.title) + '</div>' +
            '<div class="module-desc">' + escapeHtml(mod.desc) + '</div>' +
            '</a></div>';
    }

    // ═══════════════════════════════════════════════════════════════════
    //  STATISTICS
    // ═══════════════════════════════════════════════════════════════════

    function loadAdminStatistics() {
        var endpoints = [
            { key: 'users', url: '/User/GetAll?pageNumber=1&pageSize=1' },
            { key: 'departments', url: '/Department/GetAll?pageNumber=1&pageSize=1' },
            { key: 'roles', url: '/Role/GetAll?pageNumber=1&pageSize=1' },
            { key: 'content', url: '/MainPageSection/GetAll?pageNumber=1&pageSize=1' }
        ];
        loadStatValues(endpoints);
    }

    function loadManagerStatistics() {
        var endpoints = [
            { key: 'users', url: '/User/GetAll?pageNumber=1&pageSize=1' },
            { key: 'departments', url: '/Department/GetAll?pageNumber=1&pageSize=1' }
        ];
        loadStatValues(endpoints);
    }

    function loadHRStatistics() {
        var endpoints = [
            { key: 'users', url: '/User/GetAll?pageNumber=1&pageSize=1' },
            { key: 'content', url: '/MainPageSection/GetAll?pageNumber=1&pageSize=1' }
        ];
        loadStatValues(endpoints);
    }

    function loadStatValues(endpoints) {
        endpoints.forEach(function (ep) {
            $.ajax({
                url: ep.url,
                type: 'GET',
                success: function (response) {
                    var totalCount = extractTotalCount(response);
                    updateStatValue(ep.key, totalCount);
                },
                error: function () {
                    updateStatValue(ep.key, '—');
                }
            });
        });
    }

    function extractTotalCount(response) {
        if (!response) return 0;

        if (typeof response.totalRecords === 'number') {
            return response.totalRecords;
        }

        if (response.data && Array.isArray(response.data)) {
            if (typeof response.totalRecords === 'number') return response.totalRecords;
            if (typeof response.totalCount === 'number') return response.totalCount;
            if (typeof response.total === 'number') return response.total;
            return response.data.length;
        }

        if (response.items && Array.isArray(response.items)) {
            if (typeof response.totalRecords === 'number') return response.totalRecords;
            if (typeof response.totalCount === 'number') return response.totalCount;
            if (typeof response.totalPages === 'number' && typeof response.pageSize === 'number') {
                return response.totalPages * response.pageSize;
            }
            return response.items.length;
        }

        if (Array.isArray(response)) {
            return response.length;
        }

        if (typeof response.totalCount === 'number') return response.totalCount;
        if (typeof response.total === 'number') return response.total;
        if (typeof response.recordsTotal === 'number') return response.recordsTotal;
        if (typeof response.recordCount === 'number') return response.recordCount;

        return 0;
    }

    function updateStatValue(key, value) {
        var el = document.getElementById('statValue_' + key);
        if (el) {
            var displayValue = typeof value === 'number' ? value.toLocaleString() : value;
            el.textContent = displayValue;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  COMPANY INFORMATION (Employee)
    // ═══════════════════════════════════════════════════════════════════

    function loadCompanyInfo() {
        $.ajax({
            url: '/MainPageSection/GetAll?pageNumber=1&pageSize=50',
            type: 'GET',
            success: function (response) {
                var sections = [];
                if (response && response.data && Array.isArray(response.data)) {
                    sections = response.data;
                } else if (Array.isArray(response)) {
                    sections = response;
                }
                renderCompanyInfo(sections);
            },
            error: function () {
                var container = document.getElementById('companyInfoContainer');
                if (container) {
                    container.innerHTML = '<div class="col-12"><div class="empty-state"><i class="bi bi-building empty-icon"></i><h4>Company Information</h4><p>No announcements available at this time.</p></div></div>';
                }
            }
        });
    }

    function renderCompanyInfo(sections) {
        var container = document.getElementById('companyInfoContainer');
        if (!container) return;

        if (!sections || sections.length === 0) {
            container.innerHTML = '<div class="col-12"><div class="empty-state"><i class="bi bi-building empty-icon"></i><h4>Company Information</h4><p>No company content available at this time.</p><a href="/MainPageSection" class="btn btn-outline-primary btn-sm">Browse Sections</a></div></div>';
            return;
        }

        var html = '';
        sections.forEach(function (section) {
            var title = section.title || section.name || 'Section';
            var description = section.description || section.content || '';
            var truncatedDesc = description.length > 200 ? description.substring(0, 200) + '...' : description;

            html += '<div class="col-md-6 col-lg-4">';
            html += '<div class="company-info-card">';
            html += '<div class="company-info-icon"><i class="bi bi-file-text-fill"></i></div>';
            html += '<h5 class="company-info-title">' + escapeHtml(title) + '</h5>';
            if (truncatedDesc) {
                html += '<p class="company-info-desc">' + escapeHtml(truncatedDesc) + '</p>';
            }
            html += '<a href="/MainPageSection/Details/' + section.sectionId + '" class="company-info-link">Read More <i class="bi bi-arrow-right"></i></a>';
            html += '</div></div>';
        });

        container.innerHTML = html;
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SIDEBAR NAVIGATION
    // ═══════════════════════════════════════════════════════════════════

    function buildSidebarNav(role) {
        var navContainer = document.getElementById('sidebarNav');
        if (!navContainer) return;

        var currentPath = window.location.pathname;

        var html = '';

        // Dashboard link (always for authenticated)
        var isDashboardActive = (currentPath === '/Home/Dashboard' || currentPath === '/');
        html += '<li class="nav-item">';
        html += '<a class="nav-link' + (isDashboardActive ? ' active' : '') + '" href="/Home/Dashboard" role="menuitem">';
        html += '<span class="nav-link-icon"><i class="bi bi-speedometer2"></i></span>';
        html += '<span>Dashboard</span>';
        html += '</a>';
        html += '</li>';

        // Render accessible modules
        var accessibleModules = modules.filter(function (mod) {
            return hasAccess(mod.roles, role);
        });

        accessibleModules.forEach(function (mod) {
            var isActive = (currentPath === mod.url || currentPath.indexOf(mod.url + '/') === 0);
            html += '<li class="nav-item">';
            html += '<a class="nav-link' + (isActive ? ' active' : '') + '" href="' + escapeHtml(mod.url) + '" role="menuitem">';
            html += '<span class="nav-link-icon"><i class="bi ' + getModuleIcon(mod.icon) + '"></i></span>';
            html += '<span>' + escapeHtml(mod.title) + '</span>';
            html += '</a>';
            html += '</li>';
        });

        navContainer.innerHTML = html;
    }

    // ═══════════════════════════════════════════════════════════════════
    //  USER MENU (Top Navbar)
    // ═══════════════════════════════════════════════════════════════════

    function buildUserMenu(user, isLoggedIn) {
        var container = document.getElementById('topNavRight');
        if (!container) return;

        if (!isLoggedIn) {
            container.innerHTML = '<a class="nav-link" href="/Auth/Login"><i class="bi bi-box-arrow-in-right me-1"></i>Login</a>';
            return;
        }

        var avatarLetter = user.username ? user.username.charAt(0).toUpperCase() : 'U';
        var roleClass = 'role-' + (user.role ? user.role.toLowerCase() : 'user');

        var html = '';
        html += '<div class="user-menu-container">';
        html += '<div class="user-menu" id="userMenuToggle" role="button" tabindex="0" aria-haspopup="true" aria-expanded="false">';
        html += '<div class="user-avatar" aria-hidden="true">' + escapeHtml(avatarLetter) + '</div>';
        html += '<div class="user-info d-none d-md-flex">';
        html += '<span class="user-name">' + escapeHtml(user.username) + '</span>';
        html += '<span class="user-role-badge ' + roleClass + '">' + escapeHtml(user.role || 'User') + '</span>';
        html += '</div>';
        html += '</div>';

        // Dropdown
        html += '<div class="user-dropdown" id="userDropdown" role="menu">';
        html += '<div class="dropdown-header d-md-none">';
        html += '<div class="fw-bold">' + escapeHtml(user.username) + '</div>';
        html += '<small class="text-muted">' + escapeHtml(user.role || 'User') + '</small>';
        html += '</div>';
        html += '<div class="dropdown-divider d-md-none"></div>';
        html += '<button class="dropdown-item" id="dashboardLogoutBtn" role="menuitem">';
        html += '<i class="bi bi-box-arrow-right"></i>';
        html += 'Logout';
        html += '</button>';
        html += '</div>';
        html += '</div>';

        container.innerHTML = html;

        // Bind user menu toggle
        var menuToggle = document.getElementById('userMenuToggle');
        var dropdown = document.getElementById('userDropdown');
        if (menuToggle && dropdown) {
            menuToggle.addEventListener('click', function (e) {
                e.stopPropagation();
                var isOpen = dropdown.classList.contains('show');
                dropdown.classList.toggle('show');
                menuToggle.setAttribute('aria-expanded', !isOpen);
            });

            var closeHandler = function () {
                dropdown.classList.remove('show');
                if (menuToggle) menuToggle.setAttribute('aria-expanded', 'false');
            };
            document.removeEventListener('click', closeHandler);
            document.addEventListener('click', closeHandler);

            dropdown.addEventListener('click', function (e) {
                e.stopPropagation();
            });
        }

        // Bind logout button
        var logoutBtn = document.getElementById('dashboardLogoutBtn');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', function () {
                performLogout();
            });
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  LOGOUT
    // ═══════════════════════════════════════════════════════════════════

    function performLogout() {
        var refreshToken = window.auth
            ? window.auth.getRefreshToken()
            : localStorage.getItem('refreshToken');

        if (!refreshToken) {
            clearAuthAndRedirect();
            return;
        }

        $.ajax({
            url: '/api/Auth/Logout',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ refreshToken: refreshToken }),
            success: function () {
                clearAuthAndRedirect();
            },
            error: function () {
                clearAuthAndRedirect();
            }
        });
    }

    function clearAuthAndRedirect() {
        if (window.auth) {
            window.auth.removeTokens();
            window.auth.refreshUserGlobals();
        } else {
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
        }
        document.cookie = 'CompanySystem.Jwt=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
        document.cookie = 'CompanySystem.Auth=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
        $(window).trigger('authStateChanged');
        window.location.href = '/Auth/Login';
    }

    // ═══════════════════════════════════════════════════════════════════
    //  SIDEBAR TOGGLE
    // ═══════════════════════════════════════════════════════════════════

    function initSidebarToggle() {
        var toggleBtn = document.getElementById('sidebarToggle');
        var sidebar = document.getElementById('sidebar');
        var overlay = document.getElementById('sidebarOverlay');
        var wrapper = document.getElementById('contentWrapper');

        if (!toggleBtn || !sidebar) return;

        if (toggleBtn.dataset.sidebarInitialized) return;
        toggleBtn.dataset.sidebarInitialized = 'true';

        toggleBtn.addEventListener('click', function () {
            var isDesktop = window.innerWidth >= 992;

            if (isDesktop) {
                sidebar.classList.toggle('collapsed');
                if (wrapper) wrapper.classList.toggle('expanded');
            } else {
                sidebar.classList.toggle('mobile-show');
                if (overlay) overlay.classList.toggle('show');
                document.body.classList.toggle('sidebar-open');
            }
        });

        if (overlay) {
            overlay.addEventListener('click', function () {
                sidebar.classList.remove('mobile-show');
                overlay.classList.remove('show');
                document.body.classList.remove('sidebar-open');
            });
        }

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                sidebar.classList.remove('mobile-show');
                if (overlay) overlay.classList.remove('show');
                document.body.classList.remove('sidebar-open');
            }
        });
    }

    // ═══════════════════════════════════════════════════════════════════
    //  DOCUMENT READY
    // ═══════════════════════════════════════════════════════════════════

    $(document).ready(function () {
        initSidebarToggle();
        initDashboard();

        $(window).on('authStateChanged', function () {
            initDashboard();
        });

        setActiveNavLink();

        $(document).on('click', '.sidebar-nav .nav-link', function () {
            setTimeout(setActiveNavLink, 50);
        });
    });

})();
