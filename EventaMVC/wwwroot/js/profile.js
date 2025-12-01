// wwwroot/js/profile.js
document.addEventListener('DOMContentLoaded', () => {
    const user = getCurrentUser();
    if (!user) {
        showToast('Please log in to view your profile', 'warning');
        setTimeout(() => window.location.href = '/Account/Login', 1500);
        return;
    }

    // تحديث بيانات الملف الشخصي
    document.getElementById('profileName').textContent = user.name || 'User';
    document.getElementById('profileEmail').textContent = user.email || 'user@example.com';

    const avatar = document.getElementById('profileAvatar');
    const savedPhoto = localStorage.getItem('userPhoto');
    if (savedPhoto) avatar.src = savedPhoto;

    const avatarNav = document.getElementById('userAvatar');
    if (avatarNav) avatarNav.textContent = (user.name || 'U').charAt(0).toUpperCase();

    // Tabs
    document.querySelectorAll('.profile-tab').forEach(tab => {
        tab.addEventListener('click', () => {
            document.querySelectorAll('.profile-tab').forEach(t => t.classList.remove('active'));
            document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
            tab.classList.add('active');
            document.getElementById(tab.dataset.tab).classList.add('active');
        });
    });

    // تحميل التذاكر المشتراة
    const orders = JSON.parse(localStorage.getItem('purchased_tickets') || '[]');
    const ticketsList = document.getElementById('ticketsList');

    if (orders.length === 0) {
        ticketsList.innerHTML = `
            <div class="empty-state">
                <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z"/>
                </svg>
                <h3>No tickets yet</h3>
                <p>Book your first event and your tickets will appear here!</p>
                <a href="/Events/Category" class="btn btn-primary" style="margin-top:1.5rem;">
                    Browse Events
                </a>
            </div>
        `;
        return;
    }

    ticketsList.innerHTML = '';
    orders.reverse();

    orders.forEach(order => {
        order.items.forEach(item => {
            const div = document.createElement('div');
            div.className = 'ticket-item';
            div.innerHTML = `
                <div class="ticket-banner">
                    <h4>${item.eventTitle}</h4>
                    <p style="margin:0;font-size:0.95rem;opacity:0.9;">Order #${order.orderId}</p>
                </div>
                <div class="ticket-body">
                    <div class="ticket-info">
                        <div><strong>Type</strong>${item.ticketType.toUpperCase()}</div>
                        <div><strong>Quantity</strong>${item.quantity}</div>
                        <div><strong>Date</strong>${new Date(order.date).toLocaleDateString()}</div>
                        <div><strong>Total</strong>$${order.total.toFixed(2)}</div>
                    </div>
                    <div style="text-align:center;margin-top:1.5rem;">
                        <a href="/Tickets/MyTickets" class="btn btn-primary">
                            View Ticket Details
                        </a>
                    </div>
                </div>
            `;
            ticketsList.appendChild(div);
        });
    });
});