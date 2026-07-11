// ── CompanySystem – Role-Based Dashboard ────────────────────────────
// Renders Admin, Manager, Employee dashboards according to user role.

(function () {
    'use strict';

    // ── Module Definitions ─────────────────────────────────────────
    // Each module: id, title, desc, url, icon, roles (who can see it)
    var modules = [
        {
            id: 'users',
            title: 'Users',
            desc: 'Manage system users and employees',
            url: '/User',
            icon: 'users',
            roles: ['Admin', 'Manager']
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
            roles: ['Admin', 'Manager', 'User']
        },
        {
            id: 'content',
            title: 'Main Page Sections',
            desc: 'Edit homepage content blocks',
            url: '/MainPageSection',
            icon: 'content',
            roles: ['Admin', 'Manager', 'User']
        }
    ];

    // ── SVG Icons (for sidebar) ────────────────────────────────────
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

    // ═══════════════════════════════════════════════════════════════
    //  INITIALIZATION
    // ═══════════════════════════════════════════════════════════════

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

        // Toggle body classes
        document.body.classList.toggle('has-admin', role === 'Admin');
        document.body.classList.toggle('is-manager', role === 'Manager');

        // Update footer year
        var yearEl = document.getElementById('footerYear');
        if (yearEl) {
            yearEl.textContent = new Date().getFullYear();
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  ROLE DISPATCH
    // ═══════════════════════════════════════════════════════════════

    function renderRoleDashboard(role, user) {
        var roleLower = (role || '').toLowerCase();

        if (roleLower === 'admin') {
            renderAdminDashboard(user);
        } else if (roleLower === 'manager') {
            renderManagerDashboard(user);
        } else {
            renderEmployeeDashboard(user);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  ADMIN DASHBOARD
    // ═══════════════════════════════════════════════════════════════

    function renderAdminDashboard(user) {
        // Update welcome
        setWelcome('Administration Dashboard', 'Full system control and management');

        // Render profile
        renderUserProfile(user);

        var container = document.getElementById('dashboardContent');
        if (!container) return;

        var html = '';

        // ── Statistics Cards ──
        html += '<div class="row g-4 mb-4" id="statCardsContainer">';
        html += buildStatCard('users', 'Total Users', 'bi-people-fill', 'users', '/User');
        html += buildStatCard('departments', 'Total Departments', 'bi-building-fill', 'departments', '/Department');
        html += buildStatCard('roles', 'Total Roles', 'bi-shield-fill-check', 'roles', '/Role');
        html += buildStatCard('content', 'Content Sections', 'bi-layout-text-window-reverse', 'content', '/MainPageSection');
        html += '</div>';

        // ── Quick Actions ──
        html += '<h2 class="section-title"><i class="bi bi-lightning-charge-fill me-2"></i>Quick Actions</h2>';
        html += '<div class="row g-3 mb-4">';
        html += buildQuickAction('New User', 'bi-person-plus-fill', 'primary', '/User/Create', 'Create a new user account');
        html += buildQuickAction('New Department', 'bi-building-add-fill', 'success', '/Department/Create', 'Add a new department');
        html += buildQuickAction('New Role', 'bi-shield-plus-fill', 'warning', '/Role/Create', 'Create a new role');
        html += buildQuickAction('New Section', 'bi-file-plus-fill', 'info', '/MainPageSection/Create', 'Add homepage content');
        html += '</div>';

        // ── Management Modules ──
        html += '<h2 class="section-title"><i class="bi bi-grid-3x3-gap-fill me-2"></i>Management Modules</h2>';
        html += '<div class="row g-4" id="dashboardModulesContainer">';
        var adminModules = modules.filter(function (m) { return hasAccess(m.roles, 'Admin'); });
        adminModules.forEach(function (mod) {
            html += buildModuleCard(mod);
        });
        html += '</div>';

        container.innerHTML = html;

        // Load statistics after rendering
        loadAdminStatistics();
    }

    // ═══════════════════════════════════════════════════════════════
    //  MANAGER DASHBOARD
    // ═══════════════════════════════════════════════════════════════

    function renderManagerDashboard(user) {
        setWelcome('Team Management Dashboard', 'Manage your team and departments');

        renderUserProfile(user);

        var container = document.getElementById('dashboardContent');
        if (!container) return;

        var html = '';

        // ── Statistics Cards ──
        html += '<div class="row g-4 mb-4" id="statCardsContainer">';
        html += buildStatCard('users', 'Total Users', 'bi-people-fill', 'users', '/User');
        html += buildStatCard('departments', 'Total Departments', 'bi-building-fill', 'departments', '/Department');
        html += '</div>';

        // ── Quick Actions ──
        html += '<h2 class="section-title"><i class="bi bi-lightning-charge-fill me-2"></i>Quick Actions</h2>';
        html += '<div class="row g-3 mb-4">';
        html += buildQuickAction('Add Note', 'bi-plus-circle-fill', 'purple', '/Note/Create', 'Create a new note');
        html += buildQuickAction('View Team', 'bi-people-fill', 'primary', '/User', 'See all team members');
        html += buildQuickAction('Departments', 'bi-building-fill', 'success', '/Department', 'View departments');
        html += '</div>';

        // ── Management Modules ──
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

    // ═══════════════════════════════════════════════════════════════
    //  EMPLOYEE DASHBOARD
    // ═══════════════════════════════════════════════════════════════

    function renderEmployeeDashboard(user) {
        setWelcome('Employee Portal', 'Welcome back! Here\'s your personal workspace');

        // Show basic profile immediately, then enhance with details from API
        renderUserProfile(user);
        if (user.id) {
            loadEmployeeProfile(user);
        }

        var container = document.getElementById('dashboardContent');
        if (!container) return;

        var html = '';

        // ── Quick Personal Actions ──
        html += '<h2 class="section-title"><i class="bi bi-person-lines-fill me-2"></i>Quick Actions</h2>';
        html += '<div class="row g-3 mb-4">';
        html += buildQuickAction('My Notes', 'bi-sticky-fill', 'purple', '/Note', 'View and manage your notes');
        html += buildQuickAction('Company Info', 'bi-building-fill', 'info', '/MainPageSection', 'Browse company information');
        html += '</div>';

        // ── Recent Notes ──
        html += '<h2 class="section-title"><i class="bi bi-clock-history me-2"></i>Recent Notes</h2>';
        html += '<div class="row g-4 mb-4" id="recentNotesContainer">';
        html += '<div class="col-12"><div class="text-center py-4"><div class="spinner-border spinner-border-sm text-primary me-2" role="status"></div>Loading notes...</div></div>';
        html += '</div>';

        // ── Company Information ──
        html += '<h2 class="section-title"><i class="bi bi-megaphone-fill me-2"></i>Company Information</h2>';
        html += '<div class="row g-4" id="companyInfoContainer">';
        html += '<div class="col-12"><div class="text-center py-4"><div class="spinner-border spinner-border-sm text-primary me-2" role="status"></div>Loading company info...</div></div>';
        html += '</div>';

        container.innerHTML = html;

        // Load data
        loadRecentNotes(user);
        loadCompanyInfo();
    }

    // ═══════════════════════════════════════════════════════════════
    //  USER PROFILE
    // ═══════════════════════════════════════════════════════════════

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

    function loadEmployeeProfile(user) {
        // Load full user details from API for richer profile
        $.ajax({
            url: '/User/GetById?id=' + encodeURIComponent(user.id),
            type: 'GET',
            success: function (response) {
                renderDetailedProfile(response);
            },
            error: function () {
                // Fallback to basic profile
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

        // Calculate years of experience from start date
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

        // Additional details section
        html += '<div class="row mt-3 g-3">';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Phone</span><span class="detail-value">' + escapeHtml(userData.phoneNumber || '—') + '</span></div></div>';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Experience</span><span class="detail-value">' + escapeHtml(yearsExp) + '</span></div></div>';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Department ID</span><span class="detail-value">' + (userData.departmentId || '—') + '</span></div></div>';
        html += '<div class="col-md-3 col-6"><div class="profile-detail"><span class="detail-label">Leader ID</span><span class="detail-value">' + escapeHtml(userData.leaderId || 'None') + '</span></div></div>';
        html += '</div>';

        html += '</div>';

        var container = document.getElementById('userProfileContainer');
        if (container) {
            container.innerHTML = html;
        }
    }

    function getRoleBadgeClass(role) {
        var r = (role || '').toLowerCase();
        if (r === 'admin') return 'bg-danger';
        if (r === 'manager') return 'bg-warning text-dark';
        return 'bg-info text-dark';
    }

    // ═══════════════════════════════════════════════════════════════
    //  BUILDERS – Stat Card, Quick Action, Module Card
    // ═══════════════════════════════════════════════════════════════

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

    // ═══════════════════════════════════════════════════════════════
    //  WELCOME HELPER
    // ═══════════════════════════════════════════════════════════════

    function setWelcome(title, subtitle) {
        var subEl = document.getElementById('welcomeSubtitle');
        if (subEl) {
            subEl.textContent = subtitle || '';
        }
    }

    // ═══════════════════════════════════════════════════════════════
    //  STATISTICS
    // ═══════════════════════════════════════════════════════════════

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

    // ═══════════════════════════════════════════════════════════════
    //  RECENT NOTES (Employee)
    // ═══════════════════════════════════════════════════════════════

    function loadRecentNotes(user) {
        $.ajax({
            url: '/Note/GetAll?pageNumber=1&pageSize=5&sortBy=CreatedDate&isDescending=true',
            type: 'GET',
            success: function (response) {
                var notes = [];
                if (response && response.data && Array.isArray(response.data)) {
                    notes = response.data;
                } else if (Array.isArray(response)) {
                    notes = response;
                }
                renderRecentNotes(notes);
            },
            error: function () {
                var container = document.getElementById('recentNotesContainer');
                if (container) {
                    container.innerHTML = '<div class="col-12"><div class="empty-state"><i class="bi bi-journal-text empty-icon"></i><h4>No Notes Available</h4><p>You don\'t have any notes yet.</p><a href="/Note" class="btn btn-primary btn-sm">Go to Notes</a></div></div>';
                }
            }
        });
    }

    function renderRecentNotes(notes) {
        var container = document.getElementById('recentNotesContainer');
        if (!container) return;

        if (!notes || notes.length === 0) {
            container.innerHTML = '<div class="col-12"><div class="empty-state"><i class="bi bi-journal-text empty-icon"></i><h4>No Notes Yet</h4><p>You don\'t have any recent notes.</p><a href="/Note" class="btn btn-primary btn-sm">Go to Notes</a></div></div>';
            return;
        }

        var html = '';
        notes.forEach(function (note) {
            var title = note.title || 'Untitled';
            var content = note.content || '';
            var truncatedContent = content.length > 120 ? content.substring(0, 120) + '...' : content;
            var noteType = note.noteType || 'General';
            var createdDate = note.createdDate ? new Date(note.createdDate).toLocaleDateString() : '—';

            html += '<div class="col-md-6">';
            html += '<div class="note-mini-card">';
            html += '<div class="note-mini-header">';
            html += '<span class="note-mini-type badge bg-purple-light">' + escapeHtml(noteType) + '</span>';
            html += '<small class="text-muted">' + escapeHtml(createdDate) + '</small>';
            html += '</div>';
            html += '<h5 class="note-mini-title">' + escapeHtml(title) + '</h5>';
            html += '<p class="note-mini-content">' + escapeHtml(truncatedContent) + '</p>';
            html += '<a href="/Note/Details/' + note.noteId + '" class="note-mini-link">View Details <i class="bi bi-arrow-right"></i></a>';
            html += '</div></div>';
        });

        // Add "View All" link
        html += '<div class="col-12 text-center mt-2">';
        html += '<a href="/Note" class="btn btn-outline-primary btn-sm">View All Notes <i class="bi bi-arrow-right"></i></a>';
        html += '</div>';

        container.innerHTML = html;
    }

    // ═══════════════════════════════════════════════════════════════
    //  COMPANY INFORMATION (Employee)
    // ═══════════════════════════════════════════════════════════════

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

    // ═══════════════════════════════════════════════════════════════
    //  SIDEBAR NAVIGATION
    // ═══════════════════════════════════════════════════════════════

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

        // Render accessible modules directly (no section headers for cleaner look)
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

    // ═══════════════════════════════════════════════════════════════
    //  USER MENU (Top Navbar)
    // ═══════════════════════════════════════════════════════════════

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

    // ═══════════════════════════════════════════════════════════════
    //  ACCESS CONTROL
    // ═══════════════════════════════════════════════════════════════

    function hasAccess(allowedRoles, userRole) {
        if (!userRole) return false;
        return allowedRoles.some(function (r) {
            return r.toLowerCase() === userRole.toLowerCase();
        });
    }

    // ═══════════════════════════════════════════════════════════════
    //  LOGOUT
    // ═══════════════════════════════════════════════════════════════

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

    // ═══════════════════════════════════════════════════════════════
    //  SIDEBAR TOGGLE
    // ═══════════════════════════════════════════════════════════════

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

    // ═══════════════════════════════════════════════════════════════
    //  UTILITY
    // ═══════════════════════════════════════════════════════════════

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

    // ═══════════════════════════════════════════════════════════════
    //  DOCUMENT READY
    // ═══════════════════════════════════════════════════════════════

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
