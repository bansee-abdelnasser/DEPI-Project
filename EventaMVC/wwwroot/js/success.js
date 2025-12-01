// wwwroot/js/success.js
document.addEventListener('DOMContentLoaded', () => {
    const urlParams = new URLSearchParams(window.location.search);
    const orderIdFromUrl = urlParams.get('order');

    let orders = JSON.parse(localStorage.getItem('purchased_tickets') || '[]');
    let currentOrder = null;

    // جلب الطلب من الـ URL أولاً
    if (orderIdFromUrl && orders.length > 0) {
        currentOrder = orders.find(o => o.orderId === orderIdFromUrl);
    }

    // لو مفيش → جيب آخر طلب
    if (!currentOrder && orders.length > 0) {
        currentOrder = orders[0]; // الأحدث (لأننا استخدمنا unshift)
    }

    // عرض البيانات
    if (currentOrder) {
        document.getElementById('orderId').textContent = currentOrder.orderId;
        document.getElementById('totalAmount').textContent = '$' + currentOrder.total.toFixed(2);
        document.getElementById('buyerEmail').textContent = currentOrder.buyerEmail || 'your email';
    } else {
        document.getElementById('orderId').textContent = 'ORD-UNKNOWN';
        document.getElementById('totalAmount').textContent = '$0.00';
        document.getElementById('buyerEmail').textContent = 'your email';
    }

    // إشعار ترحيب
    addNotification('Order Confirmed!', 'Your tickets are ready in your account', 'success');
    showToast('Welcome to your tickets!', 'success', 6000);
});