// wwwroot/js/checkout.js
document.addEventListener('DOMContentLoaded', () => {
    // تحقق من تسجيل الدخول
    if (!getCurrentUser()) {
        showToast('You must log in to complete the purchase.', 'warning');
        setTimeout(() => window.location.href = '/Account/Login', 1800);
        return;
    }

    const cart = getCart();
    const itemsContainer = document.getElementById('cartItemsList');
    const wrapper = document.getElementById('checkoutWrapper');
    const emptyState = document.getElementById('emptyCart');
    const checkoutBtn = document.getElementById('completeCheckout');

    if (!cart || cart.length === 0) {
        wrapper.style.display = 'none';
        emptyState.style.display = 'block';
        return;
    }

    wrapper.style.display = 'grid';
    emptyState.style.display = 'none';

    // ملء بيانات اليوزر تلقائيًا
    const user = getCurrentUser();
    document.getElementById('buyerName').value = user.name || '';
    document.getElementById('buyerEmail').value = user.email || '';

    // عرض العناصر في السلة
    let subtotal = 0;
    itemsContainer.innerHTML = '';

    cart.forEach(item => {
        const itemTotal = item.price * item.quantity;
        subtotal += itemTotal;

        const div = document.createElement('div');
        div.className = 'cart-item';
        div.innerHTML = `
            <img src="~/uploads/eventa1.jpg" alt="${item.eventTitle}" onerror="this.src='https://via.placeholder.com/110?text=Event'">
            <div class="item-details">
                <h4>${item.eventTitle}</h4>
                <div class="item-meta">${item.quantity} × ${item.ticketType} Ticket</div>
            </div>
            <div class="item-price">$${itemTotal.toFixed(2)}</div>
        `;
        itemsContainer.appendChild(div);
    });

    const fee = subtotal * 0.10;
    const total = subtotal + fee;

    document.getElementById('subtotal').textContent = `$${subtotal.toFixed(2)}`;
    document.getElementById('fee').textContent = `$${fee.toFixed(2)}`;
    document.getElementById('totalPrice').textContent = `$${total.toFixed(2)}`;

    // زر إتمام الشراء
    checkoutBtn.addEventListener('click', function () {
        const name = document.getElementById('buyerName').value.trim();
        const email = document.getElementById('buyerEmail').value.trim();

        if (!name || !email) {
            showToast('Please fill in both name and email.', 'error');
            return;
        }

        if (this.classList.contains('loading')) return;

        this.classList.add('loading');
        this.innerHTML = 'Processing Payment...';

        setTimeout(() => {
            const orderId = 'ORD-' + Date.now().toString().slice(-9);

            const order = {
                orderId,
                buyerName: name,
                buyerEmail: email,
                items: cart,
                subtotal,
                fee,
                total,
                date: new Date().toISOString(),
                status: 'paid'
            };

            // حفظ الطلب
            let purchased = JSON.parse(localStorage.getItem('purchased_tickets') || '[]');
            purchased.unshift(order);
            localStorage.setItem('purchased_tickets', JSON.stringify(purchased));

            // تفريغ السلة
            clearCart();

            addNotification('Payment Successful!', `Your order #${orderId} has been confirmed`, 'success');
            showToast('Payment successful! Redirecting...', 'success');

            setTimeout(() => {
                window.location.href = '/Tickets/Success?order=' + orderId;
            }, 2200);
        }, 2200);
    });
});