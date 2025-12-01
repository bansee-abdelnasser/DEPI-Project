// wwwroot/js/register.js
document.addEventListener('DOMContentLoaded', () => {
    // لو مسجل دخول خلاص → روح للرئيسية
    if (getCurrentUser()) {
        showToast('You are already registered and logged in!', 'info');
        setTimeout(() => window.location.href = '/', 1200);
        return;
    }

    const form = document.getElementById('registerForm');

    const showError = (fieldId, message) => {
        const input = document.getElementById(fieldId);
        input.classList.add('error');
        input.nextElementSibling.textContent = message;
    };

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        // مسح الأخطاء القديمة
        document.querySelectorAll('.error-message').forEach(el => el.textContent = '');
        document.querySelectorAll('.form-control').forEach(el => el.classList.remove('error'));

        const name = document.getElementById('name').value.trim();
        const email = document.getElementById('email').value.trim();
        const password = document.getElementById('password').value;
        const confirmPassword = document.getElementById('confirmPassword').value;
        const isOrganizer = document.getElementById('isOrganizer').checked;
        const terms = document.getElementById('terms').checked;

        let valid = true;

        if (!name) { showError('name', 'Full name is required'); valid = false; }
        if (!email || !email.includes('@') || !email.includes('.')) { showError('email', 'Please enter a valid email address'); valid = false; }
        if (password.length < 6) { showError('password', 'Password must be at least 6 characters'); valid = false; }
        if (password !== confirmPassword) { showError('confirmPassword', 'Passwords do not match'); valid = false; }
        if (!terms) { showToast('You must agree to the Terms & Conditions', 'warning'); valid = false; }

        if (!valid) return;

        // حفظ اليوزر (Fake Registration)
        localStorage.setItem('userLoggedIn', 'true');
        localStorage.setItem('userName', name);
        localStorage.setItem('userEmail', email);
        localStorage.setItem('userRole', isOrganizer ? 'organizer' : 'user');
        localStorage.setItem('userId', Date.now().toString());

        showToast(
            isOrganizer
                ? 'Welcome aboard, Organizer! Your dashboard is ready'
                : 'Welcome to Eventa! Account created successfully',
            'success',
            5000
        );

        addNotification(
            'Account Created!',
            isOrganizer
                ? 'You are now an event organizer. Start creating events!'
                : 'Welcome! Start exploring events now',
            'success'
        );

        setTimeout(() => {
            window.location.href = isOrganizer ? '/Organizer' : '/';
        }, 1800);
    });
});