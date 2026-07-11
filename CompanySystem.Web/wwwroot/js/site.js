// ── CompanySystem – Site-wide JavaScript ──────────────────────────
// Shared utilities, notifications, collapse handlers, password toggles

(function () {
    'use strict';

    // ── Toast Notification System ──────────────────────────────────
    // Replaces browser alert() with Bootstrap-style toasts

    function ensureToastContainer() {
        var container = document.getElementById('toastContainer');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toastContainer';
            container.className = 'toast-container-custom';
            document.body.appendChild(container);
        }
        return container;
    }

    function showToast(message, type, duration) {
        type = type || 'info';
        duration = duration || 5000;

        var container = ensureToastContainer();

        var icons = {
            success: 'bi-check-circle-fill',
            error: 'bi-exclamation-circle-fill',
            warning: 'bi-exclamation-triangle-fill',
            info: 'bi-info-circle-fill'
        };

        var icon = icons[type] || icons.info;

        var toast = document.createElement('div');
        toast.className = 'toast-custom toast-' + type;
        toast.innerHTML =
            '<span class="toast-icon"><i class="bi ' + icon + '"></i></span>' +
            '<span class="toast-body">' + escapeHtml(message) + '</span>' +
            '<button class="toast-close" aria-label="Close">&times;</button>';

        container.appendChild(toast);

        // Close button handler
        toast.querySelector('.toast-close').addEventListener('click', function () {
            removeToast(toast);
        });

        // Auto-remove after duration
        if (duration > 0) {
            setTimeout(function () {
                removeToast(toast);
            }, duration);
        }

        return toast;
    }

    function removeToast(toast) {
        if (!toast || !toast.parentNode) return;
        toast.style.animation = 'toastSlideOut 0.3s ease forwards';
        setTimeout(function () {
            if (toast.parentNode) {
                toast.parentNode.removeChild(toast);
            }
        }, 300);
    }

    function escapeHtml(str) {
        if (!str) return '';
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(str));
        return div.innerHTML;
    }

    // ── Listen for global notifications from auth.js ───────────────
    document.addEventListener('companySystemNotification', function (e) {
        var detail = e.detail || {};
        showToast(detail.message || 'Unknown notification', detail.type || 'info');
    });

    // ── Bootstrap Collapse Arrow Rotation ──────────────────────────
    // Syncs arrow icons with collapse state

    function initCollapseToggles() {
        // Handle all Bootstrap collapses with arrow icons
        document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(function (toggle) {
            var icon = toggle.querySelector('.collapse-arrow, .bi-chevron-down, .bi-chevron-right');
            if (!icon) return;

            var targetId = toggle.getAttribute('data-bs-target') || toggle.getAttribute('href');
            if (!targetId) return;

            var target = document.querySelector(targetId);
            if (!target) return;

            // Set initial state
            var isShowing = target.classList.contains('show');
            updateCollapseIcon(icon, isShowing);

            // Listen for show/hide events on the target
            target.addEventListener('show.bs.collapse', function () {
                updateCollapseIcon(icon, true);
            });

            target.addEventListener('hide.bs.collapse', function () {
                updateCollapseIcon(icon, false);
            });
        });
    }

    function updateCollapseIcon(icon, isOpen) {
        if (!icon) return;
        if (icon.classList.contains('bi-chevron-down') || icon.classList.contains('bi-chevron-right')) {
            icon.classList.remove('bi-chevron-down', 'bi-chevron-right');
            icon.classList.add(isOpen ? 'bi-chevron-down' : 'bi-chevron-right');
        }
        if (icon.classList.contains('collapse-arrow')) {
            icon.style.transform = isOpen ? 'rotate(180deg)' : 'rotate(0deg)';
        }
    }

    // ── Password Show/Hide Toggle (shared helper) ──────────────────
    // Used by Login, Register, and any other password fields

    function initPasswordToggles() {
        document.querySelectorAll('.password-toggle-btn').forEach(function (btn) {
            // Avoid double initialization
            if (btn.dataset.passwordToggleInit) return;
            btn.dataset.passwordToggleInit = 'true';

            btn.addEventListener('click', function () {
                var container = this.closest('.input-group, .password-toggle-group');
                if (!container) return;

                var input = container.querySelector('input[type="password"], input[type="text"]');
                if (!input) return;

                var icon = this.querySelector('.bi-eye, .bi-eye-slash');
                var isPassword = input.type === 'password';

                input.type = isPassword ? 'text' : 'password';

                if (icon) {
                    icon.classList.remove('bi-eye', 'bi-eye-slash');
                    icon.classList.add(isPassword ? 'bi-eye-slash' : 'bi-eye');
                }

                this.setAttribute('title', isPassword ? 'Hide password' : 'Show password');
                this.setAttribute('aria-label', isPassword ? 'Hide password' : 'Show password');
            });
        });
    }

    // ── Document Ready ─────────────────────────────────────────────
    $(document).ready(function () {
        initCollapseToggles();
        initPasswordToggles();

        // Re-initialize when new content is loaded via AJAX
        $(document).ajaxComplete(function () {
            initPasswordToggles();
            initCollapseToggles();
        });
    });

})();
