// ── CompanySystem – Dashboard JavaScript ──────────────────────────

(function () {
    'use strict';

    // ── Module Definitions ─────────────────────────────────────────
    var modules = [
        {
            id: 'users',
            title: 'Users',
            desc: 'Manage system users and employees',
            url: '/User',
            icon: 'users',
            roles: ['Admin', 'Manager'],
            section: 'Management'
        },
        {
            id: 'departments',
            title: 'Departments',
            desc: 'Organize company departments',
            url: '/Department',
            icon: 'departments',
            roles: ['Admin', 'Manager'],
            section: 'Management'
        },
        {
            id: 'roles',
            title: 'Roles',
            desc: 'Configure access permissions',
            url: '/Role',
            icon: 'roles',
            roles: ['Admin'],
            section: 'Administration'
        },
        {
            id: 'notes',
            title: 'Notes',
            desc: 'View and manage notes',
            url: '/Note',
            icon: 'notes',
            roles: ['Admin', 'Manager', 'User'],
            section: 'Collaboration'
        },
        {
            id: 'content',
            title: 'Main Page Sections',
            desc: 'Edit homepage content blocks',
            url: '/MainPageSection',
            icon: 'content',
            roles: ['Admin', 'Manager', 'User'],
            section: 'Content'
        }
    ];

    // ── SVG Icons ──────────────────────────────────────────────────
    var icons = {
        dashboard: '<svg viewBox="0 0 24 24"><path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/></svg>',
        users: '<svg viewBox="0 0 24 24"><path d="M16 11c1.66 0 2.99-1.34 2.99-3S17.66 5 16 5c-1.66 0-3 1.34-3 3s1.34 3 3 3zm-8 0c1.66 0 2.99-1.34 2.99-3S9.66 5 8 5C6.34 5 5 6.34 5 8s1.34 3 3 3zm0 2c-2.33 0-7 1.17-7 3.5V19h14v-2.5c0-2.33-4.67-3.5-7-3.5zm8 0c-.29 0-.62.02-.97.05 1.16.84 1.97 1.97 1.97 3.45V19h6v-2.5c0-2.33-4.67-3.5-7-3.5z"/></svg>',
        departments: '<svg viewBox="0 0 24 24"><path d="M20 6h-8l-2-2H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2zm0 12H4V8h16v10z"/></svg>',
        roles: '<svg viewBox="0 0 24 24"><path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V5l-9-4zm0 10.99h7c-.53 4.12-3.28 7.79-7 8.94V12H5V6.3l7-3.11v8.8z"/></svg>',
        notes: '<svg viewBox="0 0 24 24"><path d="M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zm-1 7V3.5L18.5 9H13zM6 20V4h5v7h7v9H6z"/></svg>',
        content: '<svg viewBox="0 0 24 24"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 16H5V5h14v14zM7 10h10v2H7v-2zm0 4h6v2H7v-2zm0-8h10v2H7V6z"/></svg>',
        menu: '<svg viewBox="0 0 24 24"><path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"/></svg>',
        logout: '<svg viewBox="0 0 24 24"><path d="M17 7l-1.41 1.41L18.17 11H8v2h10.17l-2.58 2.58L17 17l5-5zM4 5h8V3H4c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h8v-2H4V5z"/></svg>'
    };

    // ── Initialize Dashboard ───────────────────────────────────────
    function initDashboard() {
        var user = window.currentUser || { id: '', username: '', role: '' };
        var role = user.role || '';
        var isLoggedIn = !!user.username;
        var isAdmin = (role === 'Admin');

        // Build sidebar navigation
        buildSidebarNav(role, isAdmin);

        // Build dashboard module cards (if on dashboard page)
        if ($('#dashboardModulesContainer').length) {
            buildModuleCards(role, isAdmin);
        }

        // Build user menu in top navbar (always for non-auth pages)
        if ($('#topNavRight').length) {
            buildUserMenu(user, isLoggedIn);
        }

        // Load statistics (if on dashboard page)
        if ($('#statCardsContainer').length) {
            loadStatistics(role);
        }

        // Toggle body class for admin
        document.body.classList.toggle('has-admin', isAdmin);

        // Update footer year
        var yearEl = document.getElementById('footerYear');
        if (yearEl) {
            yearEl.textContent = new Date().getFullYear();
        }
    }

    // ── Build Sidebar Navigation ───────────────────────────────────
    function buildSidebarNav(role, isAdmin) {
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

        // Group modules by section
        var sections = {};
        modules.forEach(function (mod) {
            if (hasAccess(mod.roles, role)) {
                if (!sections[mod.section]) {
                    sections[mod.section] = [];
                }
                sections[mod.section].push(mod);
            }
        });

        // Render sections
        Object.keys(sections).forEach(function (section) {
            html += '<li class="nav-section">' + escapeHtml(section) + '</li>';
            sections[section].forEach(function (mod) {
                var isActive = (currentPath === mod.url || currentPath.indexOf(mod.url + '/') === 0);
                html += '<li class="nav-item">';
                html += '<a class="nav-link' + (isActive ? ' active' : '') + '" href="' + escapeHtml(mod.url) + '" role="menuitem">';
                html += '<span class="nav-link-icon"><i class="bi ' + getModuleIcon(mod.icon) + '"></i></span>';
                html += '<span>' + escapeHtml(mod.title) + '</span>';
                html += '</a>';
                html += '</li>';
            });
        });

        navContainer.innerHTML = html;
    }

    function getModuleIcon(iconName) {
        var iconMap = {
            'dashboard': 'bi-speedometer2',
            'users': 'bi-people-fill',
            'departments': 'bi-building-fill',
            'roles': 'bi-shield-fill-check',
            'notes': 'bi-sticky-fill',
            'content': 'bi-layout-text-window-reverse'
        };
        return iconMap[iconName] || 'bi-circle-fill';
    }

    // ── Build Module Cards (Dashboard) ─────────────────────────────
    function buildModuleCards(role, isAdmin) {
        var container = document.getElementById('dashboardModulesContainer');
        if (!container) return;

        var html = '';
        var visibleModules = modules.filter(function (mod) {
            return hasAccess(mod.roles, role);
        });

        if (visibleModules.length === 0) {
            html = '<div class="col-12"><div class="empty-state"><i class="bi bi-grid-3x3-gap empty-icon"></i><h4>No Modules Available</h4><p>You don\'t have access to any management modules.</p></div></div>';
            container.innerHTML = html;
            return;
        }

        visibleModules.forEach(function (mod) {
            var icon = getModuleIcon(mod.icon);
            html += '<div class="col-md-6 col-lg-4">';
            html += '<a href="' + escapeHtml(mod.url) + '" class="module-card">';
            html += '<div class="module-icon mod-' + mod.icon + '"><i class="bi ' + icon + '"></i></div>';
            html += '<div class="module-title">' + escapeHtml(mod.title) + '</div>';
            html += '<div class="module-desc">' + escapeHtml(mod.desc) + '</div>';
            html += '</a>';
            html += '</div>';
        });

        container.innerHTML = html;
    }

    // ── Build User Menu ────────────────────────────────────────────
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
        html += '<div class="user-avatar" aria-hidden="true"><i class="bi bi-person-fill"></i></div>';
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

        // Bind user menu toggle (remove old listeners by replacing HTML)
        var menuToggle = document.getElementById('userMenuToggle');
        var dropdown = document.getElementById('userDropdown');
        if (menuToggle && dropdown) {
            menuToggle.addEventListener('click', function (e) {
                e.stopPropagation();
                var isOpen = dropdown.classList.contains('show');
                dropdown.classList.toggle('show');
                menuToggle.setAttribute('aria-expanded', !isOpen);
            });

            // Close on outside click
            var closeHandler = function () {
                dropdown.classList.remove('show');
                if (menuToggle) menuToggle.setAttribute('aria-expanded', 'false');
            };
            document.removeEventListener('click', closeHandler);
            document.addEventListener('click', closeHandler);

            // Prevent dropdown from closing when clicking inside it
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

    // ── Load Statistics ────────────────────────────────────────────
    function loadStatistics(role) {
        var endpoints = [];

        if (hasAccess(['Admin', 'Manager'], role)) {
            endpoints.push({ key: 'users', url: '/User/GetAll?pageNumber=1&pageSize=1' });
            endpoints.push({ key: 'departments', url: '/Department/GetAll?pageNumber=1&pageSize=1' });
        }
        if (hasAccess(['Admin'], role)) {
            endpoints.push({ key: 'roles', url: '/Role/GetAll?pageNumber=1&pageSize=1' });
        }
        endpoints.push({ key: 'content', url: '/MainPageSection/GetAll?pageNumber=1&pageSize=1' });

        var completedCount = 0;
        var totalRequests = endpoints.length;

        if (totalRequests === 0) {
            // No stats to show - hide stat cards section
            var container = document.getElementById('statCardsContainer');
            if (container) container.style.display = 'none';
            return;
        }

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
                },
                complete: function () {
                    completedCount++;
                }
            });
        });
    }

    // ── Extract Total Count from Response ─────────────────────────
    function extractTotalCount(response) {
        if (!response) return 0;

        if (typeof response.totalCount === 'number') {
            return response.totalCount;
        }

        if (response.items && Array.isArray(response.items)) {
            if (typeof response.totalRecords === 'number') return response.totalRecords;
            if (typeof response.totalCount === 'number') return response.totalCount;
            if (typeof response.totalPages === 'number' && typeof response.pageSize === 'number') {
                return response.totalPages * response.pageSize;
            }
            if (response.items.length > 0) {
                return response.items.length;
            }
            return response.items.length;
        }

        if (response.data && Array.isArray(response.data)) {
            if (typeof response.totalRecords === 'number') return response.totalRecords;
            if (typeof response.totalCount === 'number') return response.totalCount;
            if (typeof response.total === 'number') return response.total;
            return response.data.length;
        }

        if (Array.isArray(response)) {
            return response.length;
        }

        if (typeof response.total === 'number') return response.total;
        if (typeof response.recordsTotal === 'number') return response.recordsTotal;
        if (typeof response.recordCount === 'number') return response.recordCount;

        return 0;
    }

    // ── Update Stat Card Value ────────────────────────────────────
    function updateStatValue(key, value) {
        var el = document.getElementById('statValue_' + key);
        if (el) {
            var displayValue = typeof value === 'number' ? value.toLocaleString() : value;
            el.textContent = displayValue;
        }
    }

    // ── Check Role Access ─────────────────────────────────────────
    function hasAccess(allowedRoles, userRole) {
        if (!userRole) return false;
        return allowedRoles.some(function (r) {
            return r.toLowerCase() === userRole.toLowerCase();
        });
    }

    // ── Logout ─────────────────────────────────────────────────────
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
        // Clear the JWT cookie by expiring it
        document.cookie = 'CompanySystem.Jwt=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
        document.cookie = 'CompanySystem.Auth=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
        $(window).trigger('authStateChanged');
        window.location.href = '/Auth/Login';
    }

    // ── Sidebar Toggle ─────────────────────────────────────────────
    function initSidebarToggle() {
        var toggleBtn = document.getElementById('sidebarToggle');
        var sidebar = document.getElementById('sidebar');
        var overlay = document.getElementById('sidebarOverlay');
        var wrapper = document.getElementById('contentWrapper');

        if (!toggleBtn || !sidebar) return;

        // Remove old listener by cloning/replacing won't work,
        // but we use a flag to prevent double-binding
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

        // Handle escape key
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                sidebar.classList.remove('mobile-show');
                if (overlay) overlay.classList.remove('show');
                document.body.classList.remove('sidebar-open');
            }
        });
    }

    // ── Escape HTML ────────────────────────────────────────────────
    function escapeHtml(str) {
        if (!str) return '';
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(str));
        return div.innerHTML;
    }

    // ── Set Active Nav Link Based on Current URL ───────────────────
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

    // ── Document Ready ─────────────────────────────────────────────
    $(document).ready(function () {
        initSidebarToggle();
        initDashboard();

        // Re-run when auth state changes
        $(window).on('authStateChanged', function () {
            initDashboard();
        });

        // Set active nav link on initial load and after navigation
        setActiveNavLink();

        // Listen for navigation events to update active link
        $(document).on('click', '.sidebar-nav .nav-link', function () {
            setTimeout(setActiveNavLink, 50);
        });
    });

})();
