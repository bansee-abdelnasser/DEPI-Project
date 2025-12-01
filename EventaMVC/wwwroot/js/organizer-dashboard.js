// wwwroot/js/organizer-dashboard.js
document.addEventListener('DOMContentLoaded', () => {
    // === الحماية: منظمين فقط ===
    if (getUserRole() !== 'organizer') {
        showToast('Access Denied - Organizers Only', 'error');
        setTimeout(() => window.location.href = '/', 1800);
        return;
    }

    const user = getCurrentUser();
    if (user && document.getElementById('userAvatar')) {
        document.getElementById('userAvatar').textContent = user.name.charAt(0).toUpperCase();
    }

    // جلب الأحداث من localStorage
    let events = [];
    try {
        const stored = localStorage.getItem('organizer_events');
        events = stored ? JSON.parse(stored) : [];
    } catch (e) {
        events = [];
    }

    const container = document.getElementById('organizerEvents');

    // تحديث الإحصائيات
    const totalEvents = events.length;
    const totalTickets = events.reduce((sum, e) => {
        return sum + (e.tickets?.reduce((s, t) => s + (t.quantityAvailable || 0), 0) || 0);
    }, 0);
    const totalRevenue = events.reduce((sum, e) => {
        return sum + (e.tickets?.reduce((s, t) => s + ((t.price || 0) * (t.quantityAvailable || 0)), 0) || 0);
    }, 0);

    document.getElementById('totalEvents').textContent = totalEvents;
    document.getElementById('ticketsSold').textContent = totalTickets.toLocaleString();
    document.getElementById('totalRevenue').textContent = '$' + totalRevenue.toLocaleString('en-US');

    // عرض الأحداث
    if (events.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2m-8 8h.01"/>
                </svg>
                <h3>No events created yet</h3>
                <p>Start by creating your first event!</p>
                <a href="/Events/Create" class="btn btn-primary" style="margin-top:1.5rem;">
                    Create Your First Event
                </a>
            </div>
        `;
        return;
    }

    container.innerHTML = '';
    events.reverse(); // الأحدث أولاً

    events.forEach(event => {
        const statusClass = event.status === 'active' ? 'status-live' : event.status === 'draft' ? 'status-draft' : 'status-ended';
        const statusText = event.status === 'active' ? 'Live' : event.status === 'draft' ? 'Draft' : 'Ended';

        const div = document.createElement('div');
        div.className = 'event-item';
        div.innerHTML = `
            <div class="event-info">
                <h3>${event.title}</h3>
                <p class="event-meta">
                    ${event.dateFrom} ${event.dateTo && event.dateTo !== event.dateFrom ? `→ ${event.dateTo}` : ''} 
                    • ${event.location}
                </p>
            </div>
            <div style="text-align:right;">
                <span class="event-status ${statusClass}">${statusText}</span>
                <div class="event-actions">
                    <button class="btn btn-outline" onclick="location.href='/Organizer/Analytics?id=${event.id}'">
                        Analytics
                    </button>
                    <button class="btn btn-primary" onclick="location.href='/Events/Create?edit=${event.id}'">
                        Edit
                    </button>
                    <button class="btn" style="background:#ef4444;color:white;" onclick="deleteEvent('${event.id}')">
                        Delete
                    </button>
                </div>
            </div>
        `;
        container.appendChild(div);
    });

    // دالة حذف الحدث
    window.deleteEvent = function (id) {
        if (confirm('Are you sure you want to delete this event? This cannot be undone.')) {
            let events = JSON.parse(localStorage.getItem('organizer_events') || '[]');
            events = events.filter(e => e.id !== id);
            localStorage.setItem('organizer_events', JSON.stringify(events));
            showToast('Event deleted successfully', 'success');
            setTimeout(() => location.reload(), 1000);
        }
    };
});