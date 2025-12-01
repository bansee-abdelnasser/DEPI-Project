// wwwroot/js/login.js
document.addEventListener('DOMContentLoaded', function () {
    // لو اليوزر مسجل دخول خلاص → روح للصفحة الرئيسية
    if (getCurrentUser()) {
        showToast('You are already logged in!', 'info');
        setTimeout(function () {
            window.location.href = '/'; // أو '/Home/Index'
        }, 1200);
        return;
    }

    const form = document.getElementById('loginForm');
    const emailInput = document.getElementById('email');
    const passInput = document.getElementById('password');
    const organizerToggle = document.getElementById('loginAsOrganizer');

    const clearErrors = function () {
        document.querySelectorAll('.error-message').forEach(el => el.textContent = '');
        document.querySelectorAll('.form-control').forEach(el => el.classList.remove('error'));
    };

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        clearErrors();

        const email = emailInput.value.trim();
        const password = passInput.value;
        const isOrganizer = organizerToggle.checked;

        let valid = true;

        if (!email || !email.includes('@') || !email.includes('.')) {
            emailInput.classList.add('error');
            emailInput.nextElementSibling.textContent = 'Please enter a valid email address';
            valid = false;
        }

        if (password.length < 6) {
            passInput.classList.add('error');
            passInput.nextElementSibling.textContent = 'Password must be at least 6 characters';
            valid = false;
        }

        if (!valid) return;

        // Fake Login (هيتغير لـ API بعدين)
        localStorage.setItem('userLoggedIn', 'true');
        localStorage.setItem('userName', email.split('@')[0]);
        localStorage.setItem('userEmail', email);
        localStorage.setItem('userRole', isOrganizer ? 'organizer' : 'user');
        localStorage.setItem('userId', Date.now().toString());

        showToast(
            isOrganizer ? 'Welcome back, Organizer!' : 'Login successful!',
            'success'
        );

        addNotification('Logged In Successfully',
            isOrganizer ? 'You are now logged in as an event organizer' : 'Welcome back to Eventa!',
            'success'
        );

        // Redirect بعد 1.8 ثانية
        setTimeout(function () {
            window.location.href = isOrganizer ? '/Organizer' : '/';
        }, 1800);
    });
});