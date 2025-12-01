// wwwroot/js/create-event.js
document.addEventListener('DOMContentLoaded', () => {
    // الحماية: منظمين فقط
    if (getUserRole() !== 'organizer') {
        showToast('Only organizers can create events', 'error');
        setTimeout(() => window.location.href = '/', 1800);
        return;
    }

    const urlParams = new URLSearchParams(window.location.search);
    const editId = urlParams.get('edit');

    let isEditMode = false;
    let editingEvent = null;

    // وضع التعديل
    if (editId) {
        isEditMode = true;
        document.getElementById('mainTitle').textContent = 'Edit Event';
        document.getElementById('formTitle').textContent = 'Edit Your Event';
        document.querySelector('#submitBtn').textContent = 'Update Event';

        const events = JSON.parse(localStorage.getItem('organizer_events') || '[]');
        editingEvent = events.find(e => e.id === editId);

        if (editingEvent) {
            document.getElementById('eventTitle').value = editingEvent.title || '';
            document.getElementById('eventCategory').value = editingEvent.category || '';
            document.getElementById('eventDescription').value = editingEvent.description || '';
            document.getElementById('dateFrom').value = editingEvent.dateFrom || '';
            document.getElementById('dateTo').value = editingEvent.dateTo || '';
            document.getElementById('eventTime').value = editingEvent.time || '';
            document.getElementById('eventLocation').value = editingEvent.location || '';
            document.getElementById('eventCapacity').value = editingEvent.capacity || '';
            document.getElementById('eventImage').value = editingEvent.image || '';

            // إعادة ملء أنواع التذاكر
            const container = document.getElementById('ticketTypesContainer');
            container.innerHTML = '';
            editingEvent.tickets.forEach(t => addTicketType(t.name, t.price, t.quantityAvailable));
        }
    }

    // إضافة نوع تذكرة جديد
    document.getElementById('addTicketBtn').addEventListener('click', () => {
        addTicketType();
    });

    function addTicketType(name = '', price = '', qty = '') {
        const container = document.getElementById('ticketTypesContainer');
        const div = document.createElement('div');
        div.className = 'ticket-type';
        div.innerHTML = `
            <div class="ticket-grid">
                <div class="form-group">
                    <label>Ticket Name <span>*</span></label>
                    <input type="text" class="ticket-name form-control" value="${name}" placeholder="e.g. VIP" required>
                </div>
                <div class="form-group">
                    <label>Price ($)<span>*</span></label>
                    <input type="number" step="0.01" class="ticket-price form-control" value="${price}" min="0" required>
                </div>
                <div class="form-group">
                    <label>Available Quantity <span>*</span></label>
                    <input type="number" class="ticket-quantity form-control" value="${qty}" min="1" required>
                </div>
                <div class="form-group">
                    <button type="button" class="remove-ticket-btn" onclick="this.closest('.ticket-type').remove()">
                        Remove
                    </button>
                </div>
            </div>
        `;
        container.appendChild(div);
    }

    // إرسال النموذج
    document.getElementById('createEventForm').addEventListener('submit', e => {
        e.preventDefault();

        const title = document.getElementById('eventTitle').value.trim();
        const category = document.getElementById('eventCategory').value;
        const description = document.getElementById('eventDescription').value.trim();
        const dateFrom = document.getElementById('dateFrom').value;
        const dateTo = document.getElementById('dateTo').value || dateFrom;
        const time = document.getElementById('eventTime').value;
        const location = document.getElementById('eventLocation').value.trim();
        const capacity = document.getElementById('eventCapacity').value || null;
        const image = document.getElementById('eventImage').value || `https://via.placeholder.com/1200x600/6366f1/ffffff?text=${encodeURIComponent(title)}`;

        if (!title || !category || !description || !dateFrom || !time || !location) {
            showToast('Please fill all required fields', 'error');
            return;
        }

        const tickets = [];
        document.querySelectorAll('.ticket-type').forEach(t => {
            const name = t.querySelector('.ticket-name').value.trim();
            const price = parseFloat(t.querySelector('.ticket-price').value);
            const qty = parseInt(t.querySelector('.ticket-quantity').value);

            if (name && price >= 0 && qty > 0) {
                tickets.push({ name, price, quantityAvailable: qty });
            }
        });

        if (tickets.length === 0) {
            showToast('Add at least one ticket type', 'error');
            return;
        }

        const eventData = {
            id: isEditMode ? editId : Date.now().toString(),
            title,
            category,
            description,
            dateFrom,
            dateTo,
            time,
            location,
            capacity: capacity ? parseInt(capacity) : null,
            image,
            tickets,
            status: 'active',
            createdAt: new Date().toISOString(),
            organizerId: getCurrentUser().id
        };

        let events = JSON.parse(localStorage.getItem('organizer_events') || '[]');
        if (isEditMode) {
            events = events.map(e => e.id === editId ? eventData : e);
            showToast('Event updated successfully!', 'success');
        } else {
            events.push(eventData);
            showToast('Event created successfully!', 'success');
        }
        localStorage.setItem('organizer_events', JSON.stringify(events));

        setTimeout(() => {
            window.location.href = '/Organizer';
        }, 1500);
    });
});