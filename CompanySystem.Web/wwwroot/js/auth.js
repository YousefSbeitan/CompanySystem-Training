// CompanySystem Authentication Helper
// Handles JWT storage, decoding, user state, and AJAX authorization.
// Supports both Cookie (MVC) and JWT Bearer (AJAX) authentication.

(function () {
    'use strict';

    // ── Token Storage ──────────────────────────────────────────────

    function saveTokens(accessToken, refreshToken) {
        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('refreshToken', refreshToken);
        // Set cookie for MVC navigation (composite auth)
        document.cookie = 'CompanySystem.Jwt=' + encodeURIComponent(accessToken) + '; path=/; secure; samesite=lax';
        document.cookie = 'CompanySystem.Auth=' + encodeURIComponent(accessToken) + '; path=/; secure; samesite=lax';
    }

    function getAccessToken() {
        return localStorage.getItem('accessToken');
    }

    function getRefreshToken() {
        return localStorage.getItem('refreshToken');
    }

    function removeTokens() {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        // Clear MVC navigation cookies
        document.cookie = 'CompanySystem.Jwt=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; secure; samesite=lax';
        document.cookie = 'CompanySystem.Auth=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; secure; samesite=lax';
    }

    // ── Cookie Helper ──────────────────────────────────────────────

    function getCookie(name) {
        var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        return match ? decodeURIComponent(match[2]) : null;
    }

    // ── JWT Decode ─────────────────────────────────────────────────

    function decodeJwt(token) {
        try {
            const payload = token.split('.')[1];
            const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
            return JSON.parse(decoded);
        } catch (e) {
            return null;
        }
    }

    function isTokenExpired(token) {
        const decoded = decodeJwt(token);
        if (!decoded || !decoded.exp) return true;
        const now = Math.floor(Date.now() / 1000);
        return decoded.exp < now;
    }

    // ── Permission Extraction ─────────────────────────────────────

    function extractPermissions(decoded) {
        if (!decoded) return [];
        var perm = decoded['Permission'];
        if (Array.isArray(perm)) return perm;
        if (typeof perm === 'string') return [perm];
        return [];
    }

    // ── User Info ──────────────────────────────────────────────────

    function getCurrentUser() {
        // Try to get user info from JWT in localStorage first
        var token = getAccessToken();

        // Fallback: try to read JWT from cookie (set by server on login)
        if (!token) {
            token = getCookie('CompanySystem.Jwt');
        }

        if (!token) return null;

        const decoded = decodeJwt(token);
        if (!decoded) return null;

        const permissions = extractPermissions(decoded);
        const role = decoded[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
            decoded.role || '';

        return {
            id: decoded[
                'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ||
                decoded.sub || '',
            username: decoded[
                'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
                decoded.unique_name || '',
            role: role,
            permissions: permissions
        };
    }

    // ── Expose Globals (for build-ui integration) ──────────────────

    function refreshUserGlobals() {
        const user = getCurrentUser();
        window.currentUser = user || { id: '', username: '', role: '', permissions: [] };
        window.currentUserRole = window.currentUser.role;
        window.currentUserPermissions = window.currentUser.permissions || [];
    }

    // ── Initial Load ───────────────────────────────────────────────

    refreshUserGlobals();

    // ── Show Notification (replaces alert()) ───────────────────────

    function showNotification(message, type) {
        type = type || 'error';
        // Dispatch a custom event that site.js listens to
        var event = new CustomEvent('companySystemNotification', {
            detail: {
                message: message,
                type: type
            }
        });
        document.dispatchEvent(event);
    }

    // ── AJAX Authorization Setup ───────────────────────────────────

    $.ajaxSetup({
        beforeSend: function (xhr) {
            var token = getAccessToken();
            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        },
        statusCode: {
            401: function () {
                // Unauthorized – redirect to login
                removeTokens();
                refreshUserGlobals();
                showNotification('Your session has expired. Please login again.', 'warning');
                setTimeout(function () {
                    window.location.href = '/Auth/Login';
                }, 500);
            },
            403: function () {
                // Forbidden – show notification instead of browser alert
                showNotification('You do not have permission to perform this action.', 'error');
            }
        }
    });

    // ── Public API ─────────────────────────────────────────────────

    window.auth = {
        saveTokens: saveTokens,
        getAccessToken: getAccessToken,
        getRefreshToken: getRefreshToken,
        removeTokens: removeTokens,
        decodeJwt: decodeJwt,
        isTokenExpired: isTokenExpired,
        getCurrentUser: getCurrentUser,
        refreshUserGlobals: refreshUserGlobals,
        getCookie: getCookie,
        showNotification: showNotification
    };

})();
