// wwwroot/js/my-tickets.js
document.addEventListener('DOMContentLoaded', () => {
    const user = getCurrentUser();
    if (!user) {
        showToast('Please log in to view your tickets', 'warning');
        setTimeout(() => window.location.href = '/Account/Login', 1500);
        return;
    }

    const orders = JSON.parse(localStorage.getItem('purchased_tickets') || '[]');
    const container = document.getElementById('ticketsContainer');
    const emptyState = document.getElementById('emptyState');

    if (orders.length === 0) {
        emptyState.style.display = 'block';
        return;
    }

    orders.reverse();

    orders.forEach((order) => {
        order.items.forEach((item, idx) => {
            const ticketNumber = String(orders.indexOf(order) + 1).padStart(4, '0') +
                String(idx + 1).padStart(3, '0');
            const ticketId = `EVT-${order.orderId.slice(-8).toUpperCase()}-${ticketNumber}`;

            const qrData = JSON.stringify({
                ticketId,
                orderId: order.orderId,
                eventTitle: item.eventTitle,
                ticketType: item.ticketType,
                quantity: item.quantity,
                holderName: user.name,
                holderEmail: user.email,
                purchaseDate: order.date,
                totalPaid: order.total,
                valid: true
            });

            const ticketDiv = document.createElement('div');
            ticketDiv.className = 'ticket-card';
            ticketDiv.innerHTML = `
                <div class="ticket-status">Confirmed</div>
                <div class="ticket-header">
                    <h3>${item.eventTitle}</h3>
                    <div class="ticket-id">${ticketId}</div>
                </div>
                <div class="ticket-body">
                    <div class="ticket-info">
                        <div><strong>Ticket Holder</strong><br>${user.name}</div>
                        <div><strong>Type</strong><br>${item.ticketType.toUpperCase()}</div>
                        <div><strong>Quantity</strong><br>${item.quantity} ticket${item.quantity > 1 ? 's' : ''}</div>
                        <div><strong>Total Paid</strong><br>$${order.total.toFixed(2)}</div>
                        <div><strong>Purchase Date</strong><br>${new Date(order.date).toLocaleDateString('en-US', {
                weekday: 'short', month: 'short', day: 'numeric', year: 'numeric'
            })}</div>
                        <div><strong>Status</strong><br><span style="color:#10b981;font-weight:700;">Active & Valid</span></div>
                    </div>

                    <div class="qr-section">
                        <div id="qr-${order.orderId}-${idx}"></div>
                        <small class="qr-label">Scan at entrance • Valid for ${item.quantity} person${item.quantity > 1 ? 's' : ''}</small>
                    </div>

                    <div class="ticket-actions">
                        <button class="btn-print" onclick="window.print()">
                            Print Ticket
                        </button>
                        <button class="btn-download" onclick="showToast('PDF download coming soon!', 'info')">
                            Download PDF
                        </button>
                    </div>
                </div>
            `;

            container.appendChild(ticketDiv);

            setTimeout(() => {
                new QRCode(document.getElementById(`qr-${order.orderId}-${idx}`), {
                    text: qrData,
                    width: 220,
                    height: 220,
                    colorDark: "#1e293b",
                    colorLight: "#ffffff",
                    correctLevel: QRCode.CorrectLevel.H
                });
            }, 100);
        });
    });

    // رسالة نجاح لو جاي من الـ checkout
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get('from') === 'checkout') {
        showToast('Tickets added successfully!', 'success', 6000);
        history.replaceState({}, '', '/Tickets/MyTickets');
    }
});