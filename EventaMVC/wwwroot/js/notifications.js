// wwwroot/js/notifications.js
document.addEventListener('DOMContentLoaded', () => {
    // الحماية
    if (!getCurrentUser()) {
        showToast('Please log in to view notifications', 'warning');
        setTimeout(() => window.location.href = '/Account/Login', 1500);
        return;
    }

    const container = document.getElementById('notificationsList');
    const emptyState = document.getElementById('emptyState');
    const unreadCount = document.querySelector('.notification-count');

    let notifications = JSON.parse(localStorage.getItem('eventa_notifications') || '[]');

    // تحديث عدد الإشعارات غير المقروءة
    const updateUnreadCount = () => {
        const unread = notifications.filter(n => !n.read).length;
        if (unreadCount) {
            unreadCount.textContent = unread > 99 ? '99+' : unread;
            unreadCount.style.display = unread > 0 ? 'flex' : 'none';
        }
    };

    // عرض الإشعارات
    const renderNotifications = () => {
        if (notifications.length === 0) {
            emptyState.style.display = 'block';
            container.innerHTML = '';
            return;
        }

        emptyState.style.display = 'none';
        container.innerHTML = '';

        notifications.reverse().forEach(notif => {
            const div = document.createElement('div');
            div.className = `notification-item ${notif.read ? '' : 'unread'}`;
            div.innerHTML = `
                <div class="notification-icon ${notif.type}">
                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        ${notif.type === 'success' ? '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"/>' :
                    notif.type === 'error' ? '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>' :
                        notif.type === 'warning' ? '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 4c-.77 1.333.192 3 1.732 3z"/>' :
                            '<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>'}
                    </svg>
                </div>
                <div class="notification-content">
                    <h4>${notif.title}</h4>
                    <p>${notif.message}</p>
                    <small>${new Date(notif.timestamp).toLocaleString()}</small>
                </div>
                <button class="notification-delete" onclick="deleteNotification('${notif.id}', this)">
                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
                    </svg>
                </button>
            `;

            // تعليم كمقروءة عند الضغط
            if (!notif.read) {
                div.addEventListener('click', () => {
                    if (!div.classList.contains('unread')) return;
                    notif.read = true;
                    localStorage.setItem('eventa_notifications', JSON.stringify(notifications));
                    div.classList.remove('unread');
                    updateUnreadCount();
                });
            }

            container.appendChild(div);
        });
    };

    // حذف إشعار
    window.deleteNotification = (id, button) => {
        notifications = notifications.filter(n => n.id !== id);
        localStorage.setItem('eventa_notifications', JSON.stringify(notifications));
        button.closest('.notification-item').remove();
        updateUnreadCount();
        if (notifications.length === 0) emptyState.style.display = 'block';
    };

    // زر حذف الكل
    document.getElementById('clearAllBtn').addEventListener('click', () => {
        if (confirm('Delete all notifications?')) {
            notifications = [];
            localStorage.setItem('eventa_notifications', JSON.stringify(notifications));
            container.innerHTML = '';
            emptyState.style.display = 'block';
            updateUnreadCount();
        }
    });

    // بدء العرض
    renderNotifications();
    updateUnreadCount();
});