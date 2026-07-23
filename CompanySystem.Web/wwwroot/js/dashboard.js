// ── CompanySystem – Permission-Based Dashboard ─────────────────────
// Uses window.currentUserPermissions (primary) with
// window.currentUserRole (fallback for legacy compatibility).
// ─────────────────────────────────────────────────────────────────────

(function () {
    'use strict';

    // ── Module Definitions (inferred from controller [RequirePermission]) ──
    var modules = [
        {
            id: 'users',
            title: 'Users',
            desc: 'Manage system users and employees',
            url: '/User',
            icon: 'users',
            permission: 'Users.View',
            // Fallback roles when permissions not available
            roles: ['Admin', 'Manager', 'HR']
        },
        {
            id: 'departments',
            title: 'Departments',
            desc: 'Organize company departments',
            url: '/Department',
            icon: 'departments',
            permission: 'Departments.View',
            roles: ['Admin', 'Manager']
        },
        {
            id: 'roles',
            title: 'Roles',
            desc: 'Configure access permissions',
            url: '/Role',
            icon: 'roles',
            permission: 'Roles.View',
            roles: ['Admin']
        },
        {
            id: 'notes',
            title: 'Notes',
            desc: 'View and manage notes',
            url: '/Note',
            icon: 'notes',
            permission: 'Notes.View',
            roles: ['Admin', 'Manager', 'HR']
        },
        {
            id: 'content',
            title: 'Main Page Sections',
            desc: 'Edit homepage content blocks',
            url: '/MainPageSection',
            icon: 'content',
            permission: 'MainPageSections.View',
            roles: ['Admin', 'Manager', 'HR', 'User']
        },
        {
            id: 'permissions',
            title: 'Permissions',
            desc: 'Manage user permissions',
            url: '/Permission',
            icon: 'permissions',
            permission: 'Permissions.View',
            roles: ['Admin']
        }
    ];

    // ── Icon Map ──
    var iconMap = {
        'dashboard': 'bi-speedometer2',
        'users': 'bi-people-fill',
        'departments': 'bi-building-fill',
        'roles': 'bi-shield-fill-check',
        'notes': 'bi-sticky-fill',
        'content': 'bi-layout-text-window-reverse',
        'permissions': 'bi-shield-lock-fill'
    };

    function getModuleIcon(iconName) {
        return iconMap[iconName] || 'bi-circle-fill';
    }

    // ── Permission / Role Helpers ──────────────────────────────────

    /**
     * Get current permissions array (with fallback).
     */
    function getPermissions() {
        return window.currentUserPermissions || [];
    }

    /**
     * Get current role (legacy fallback).
     */
    function getRole() {
        var user = window.currentUser || { role: '' };
        return user.role || '';
    }

    /**
     * Check whether the user has a specific permission.
     */
    function hasPermission(perm) {
        if (!perm) return false;
        var perms = getPermissions();
        return perms.indexOf(perm) !== -1;
    }

    /**
     * Check whether the user has any of the given permissions.
     */
    function hasAnyPermission(permList) {
        if (!permList || permList.length === 0) return false;
        var perms = getPermissions();
        return permList.some(function (p) { return perms.indexOf(p) !== -1; });
    }

    /**
     * Check whether the user has access to a module.
     * Primary: permission check via window.currentUserPermissions.
     * Fallback: role check (legacy).
     */
    function canAccessModule(mod) {
        if (!mod) return false;

        // Permission-based check (primary)
        var perms = getPermissions();
        if (perms.length > 0) {
            return hasPermission(mod.permission);
        }

        // Role-based fallback
        var role = getRole().toLowerCase();
        if (!role) return false;
        if (!mod.roles) return false;
        return mod.roles.some(function (r) { return r.toLowerCase() === role; });
    }

    /**
     * Get accessible modules based on permissions (or role fallback).
     */
    function getAccessibleModules() {
        return modules.filter(function (m) { return canAccessModule(m); });
    }

    /**
     * Get role badge CSS class.
     */
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

    // ═══════════════════════════════════════════════════════════════════
    //  INITIALIZATION
    // ═══════════════════════════════════════════════════════════════════

    function initDashboard() {
        var user = window.currentUser || { id: '', username: '', role: '' };
        var isLoggedIn = !!user.username;

        // Build sidebar navigation
        buildSidebarNav();

        // Build user menu in top navbar
        if ($('#topNavRight').length) {
            buildUserMenu(user, isLoggedIn);
        }

        // Render dashboard content
        if ($('#dashboardContent').length) {
            renderDashboardContent(user);
        }

        // Set body role class for CSS targeting
        var role = user.role || '';
        document.body.classList.remove('role-admin', 'role-manager', 'role-hr', 'role-user');
        if (role) {
            document.body.classList.add('role-' + role.toLowerCase());
        }

        // Update footer year
        var yearEl = document.getElementById('footerYear');
        if (yearEl) {
            yearEl.textContent = new Date().getFullYear();
        }

        // Set active nav link
        setActiveNavLink();
    }

    // ═══════════════════════════════════════════════════════════════════
    //  WELCOME
    // ═══════════════════════════════════════════════════════════════════

    function setWelcome(title, subtitle) {
        var subEl = document.getElementById('welcomeSubtitle');
        if (subEl) {
            subEl.textContent = subtitle || '';
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  DASHBOARD CONTENT (permission-based)
    // ═══════════════════════════════════════════════════════════════════

    function renderDashboardContent(user) {
        var permissions = getPermissions();
        var role = getRole();
        var accessibleModules = getAccessibleModules();
        var hasPerms = permissions.length > 0;

        // ── Welcome ────────────────────────────────────────────────
        var welcomeTitle = 'Dashboard';
        var welcomeSubtitle = 'Welcome, ' + (user.username || 'User') + '!';

        if (hasPerms || role) {
            var displayRole = role || 'User';
            welcomeSubtitle += ' | ' + displayRole;
        }

        setWelcome(welcomeTitle, welcomeSubtitle);

        // ── User Profile ───────────────────────────────────────────
        renderUserProfile(user);

        var html = '';
        var container = document.getElementById('dashboardContent');
        if (!container) return;

        // ── Statistics Cards ───────────────────────────────────────
        var statModules = accessibleModules.filter(function (m) {
            return m.id !== 'permissions'; // permissions doesn't have a count endpoint
        });

        if (statModules.length > 0) {
            html += '<div class="row g-4 mb-4" id="statCardsContainer">';
            statModules.forEach(function (mod) {
                html += buildStatCard(mod);
            });
            html += '</div>';
        }

        // ── Quick Actions ──────────────────────────────────────────
        var hasCreateAny = hasAnyPermission([
            'Users.Create', 'Departments.Create', 'Roles.Create',
            'Notes.Create', 'MainPageSections.Create', 'Permissions.Create'
        ]);

        if (hasCreateAny || accessibleModules.length > 0) {
            html += '<h2 class="section-title"><i class="bi bi-lightning-charge-fill me-2"></i>Quick Actions</h2>';
            html += '<div class="row g-3 mb-4" id="quickActionsContainer">';
            html += buildQuickActions(accessibleModules, permissions);
            html += '</div>';
        }

        // ── Management Modules ─────────────────────────────────────
        if (accessibleModules.length > 0) {
            html += '<h2 class="section-title"><i class="bi bi-grid-3x3-gap-fill me-2"></i>Management Modules</h2>';
            html += '<div class="row g-4" id="dashboardModulesContainer">';
            accessibleModules.forEach(function (mod) {
                html += buildModuleCard(mod);
            });
            html += '</div>';
        }

        container.innerHTML = html;

        // ── Load Statistics ────────────────────────────────────────
        loadDashboardStatistics(accessibleModules);
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
        html += '</div>';
        html += '</div>';
        html += '</div>';
        html += '</div>';

        var container = document.getElementById('userProfileContainer');
        if (container) {
            container.innerHTML = html;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    //  BUILDERS
    // ═══════════════════════════════════════════════════════════════════

    function buildStatCard(mod) {
        var icon = getModuleIcon(mod.icon);
        return '<div class="col-md-6 col-lg-3">' +
            '<div class="stat-card" id="statCard_' + mod.id + '">' +
            '<div class="stat-icon icon-' + mod.icon + '"><i class="bi ' + icon + '"></i></div>' +
            '<div class="stat-value" id="statValue_' + mod.id + '">—</div>' +
            '<div class="stat-label">' + escapeHtml('Total ' + mod.title) + '</div>' +
            '<a href="' + escapeHtml(mod.url) + '" class="stat-link">Manage <i class="bi bi-arrow-right"></i></a>' +
            '</div></div>';
    }

    function buildQuickActions(accessibleModules, permissions) {
        var actions = [];
        var hasPerms = permissions.length > 0;

        accessibleModules.forEach(function (mod) {
            var createPerm = mod.id.charAt(0).toUpperCase() + mod.id.slice(1);
            // Convert 'content' to 'MainPageSections'
            var createPermName;
            if (mod.id === 'content') {
                createPermName = 'MainPageSections.Create';
            } else {
                createPermName = createPerm.replace(/([a-z])([A-Z])/g, '$1$2');
                // Capitalize first letter
                createPermName = mod.id.charAt(0).toUpperCase() + mod.id.slice(1) + '.Create';
            }

            var canCreate = hasPerms
                ? hasPermission(createPermName)
                : true; // fallback: show quick actions

            if (canCreate) {
                var actionUrls = {
                    'users': '/User/Create',
                    'departments': '/Department/Create',
                    'roles': '/Role/Create',
                    'notes': '/Note/Create',
                    'content': '/MainPageSection/Create',
                    'permissions': '/Permission/Create'
                };
                var actionColors = {
                    'users': 'primary',
                    'departments': 'success',
                    'roles': 'warning',
                    'notes': 'purple',
                    'content': 'info',
                    'permissions': 'danger'
                };
                var actionIcons = {
                    'users': 'bi-person-plus-fill',
                    'departments': 'bi-building-add-fill',
                    'roles': 'bi-shield-plus-fill',
                    'notes': 'bi-plus-circle-fill',
                    'content': 'bi-file-plus-fill',
                    'permissions': 'bi-shield-plus-fill'
                };

                actions.push({
                    title: 'New ' + mod.title.replace('Main Page Sections', 'Section'),
                    icon: actionIcons[mod.id] || 'bi-plus-circle-fill',
                    color: actionColors[mod.id] || 'primary',
                    url: actionUrls[mod.id] || (mod.url + '/Create'),
                    desc: 'Create a new ' + mod.title.toLowerCase().replace('main page sections', 'section')
                });
            }
        });

        // Limit to 4 quick actions
        var displayActions = actions.slice(0, 4);
        var html = '';
        displayActions.forEach(function (a) {
            html += buildQuickActionCard(a.title, a.icon, a.color, a.url, a.desc);
        });
        return html;
    }

    function buildQuickActionCard(title, icon, color, link, desc) {
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

    function loadDashboardStatistics(accessibleModules) {
        var endpointMap = {
            'users': '/User/GetAll?pageNumber=1&pageSize=1',
            'departments': '/Department/GetAll?pageNumber=1&pageSize=1',
            'roles': '/Role/GetAll?pageNumber=1&pageSize=1',
            'notes': '/Note/GetAll?pageNumber=1&pageSize=1',
            'content': '/MainPageSection/GetAll?pageNumber=1&pageSize=1'
        };

        var endpoints = [];
        accessibleModules.forEach(function (mod) {
            if (endpointMap[mod.id]) {
                endpoints.push({ key: mod.id, url: endpointMap[mod.id] });
            }
        });

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
    //  SIDEBAR NAVIGATION
    // ═══════════════════════════════════════════════════════════════════

    function buildSidebarNav() {
        var navContainer = document.getElementById('sidebarNav');
        if (!navContainer) return;

        var currentPath = window.location.pathname;
        var accessibleModules = getAccessibleModules();

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
    //  ACTIVE NAV LINK
    // ═══════════════════════════════════════════════════════════════════

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
    //  EXPOSE FOR LAYOUT SCRIPT
    // ═══════════════════════════════════════════════════════════════════

    // Expose these functions globally so _Layout.cshtml can call them
    window.buildSidebarNav = buildSidebarNav;
    window.buildUserMenu = buildUserMenu;
    window.initDashboard = initDashboard;

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
