// wwwroot/js/edit-profile.js
document.addEventListener('DOMContentLoaded', () => {
    // الحماية
    const user = getCurrentUser();
    if (!user) {
        showToast('Please log in to edit your profile', 'warning');
        setTimeout(() => window.location.href = '/Account/Login', 1500);
        return;
    }

    // تحميل البيانات الحالية
    document.getElementById('editName').value = user.name || '';
    document.getElementById('editEmail').value = user.email || '';
    document.getElementById('phone').value = localStorage.getItem('userPhone') || '';
    document.getElementById('location').value = localStorage.getItem('userLocation') || '';
    document.getElementById('bio').value = localStorage.getItem('userBio') || '';

    document.getElementById('profileNameHeader').textContent = user.name || 'User';
    document.getElementById('profileEmailHeader').textContent = user.email || '';

    // الصورة من localStorage أو placeholder
    const savedPhoto = localStorage.getItem('userPhoto');
    if (savedPhoto) {
        document.getElementById('profileAvatarImg').src = savedPhoto;
    }

    // تغيير الصورة
    document.getElementById('photoInput').addEventListener('change', (e) => {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = (ev) => {
                const dataUrl = ev.target.result;
                document.getElementById('profileAvatarImg').src = dataUrl;
                localStorage.setItem('userPhoto', dataUrl);
                showToast('Profile photo updated!', 'success');
            };
            reader.readAsDataURL(file);
        }
    });

    // حفظ التعديلات
    document.getElementById('editProfileForm').addEventListener('submit', (e) => {
        e.preventDefault();

        const name = document.getElementById('editName').value.trim();
        const email = document.getElementById('editEmail').value.trim();
        const phone = document.getElementById('phone').value.trim();
        const location = document.getElementById('location').value.trim();
        const bio = document.getElementById('bio').value.trim();
        const newPass = document.getElementById('newPassword').value;
        const confirmPass = document.getElementById('confirmPassword').value;

        if (!name || !email) {
            showToast('Name and email are required', 'error');
            return;
        }

        if (newPass && newPass.length < 6) {
            document.getElementById('passwordError').textContent = 'Password must be at least 6 characters';
            return;
        }
        if (newPass && newPass !== confirmPass) {
            document.getElementById('passwordError').textContent = 'Passwords do not match';
            return;
        }

        // حفظ البيانات
        localStorage.setItem('userName', name);
        localStorage.setItem('userEmail', email);
        localStorage.setItem('userPhone', phone);
        localStorage.setItem('userLocation', location);
        localStorage.setItem('userBio', bio);

        showToast('Profile updated successfully!', 'success');
        setTimeout(() => window.location.href = '/Account/Profile', 1500);
    });
});