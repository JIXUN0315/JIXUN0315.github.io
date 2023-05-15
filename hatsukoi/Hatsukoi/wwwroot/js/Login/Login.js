
mainLoginModal = new bootstrap.Modal(document.getElementById('login-modal'), true)
applyModal = new bootstrap.Modal(document.getElementById('apply-modal'), true)
normalLoginModal = new bootstrap.Modal(document.getElementById('normal-login-modal'), true)
setPasswordModal = new bootstrap.Modal(document.getElementById('set-password-modal'), true)
resendModal = new bootstrap.Modal(document.getElementById('resend-modal'), true)
createNewUserModal = new bootstrap.Modal(document.getElementById('create-new-user-modal'), true)
infoModal = new bootstrap.Modal(document.getElementById('info-modal'), true)

emailInput = document.querySelector('#email');
accountInput = document.querySelector('#account');
passwordInput = document.querySelector('#password');
sendPasswordInput = document.querySelector('#send-password')
resendPasswordInput = document.querySelector('#resend-password')
account1Input = document.querySelector('#account1')
password1Input = document.querySelector('#password1')
email1Input = document.querySelector('#email1')

applyBtn = document.querySelector('#applyBtn');
loginBtn = document.querySelector('#loginBtn');
sendvalidateBtn = document.querySelector('#sendvalidateBtn');
normalLoginBtn = document.querySelector('#normal-login-btn');
seePasswordBtn = document.querySelector('#seepassword-btn');
setPasswordSendBtn = document.querySelector('#set-password-send')
resetPasswordBtn = document.querySelector('#reset-password-btn')
setPasswordResendBtn = document.querySelector('#set-password-resend')
resetEmailBtn = document.querySelector('#reset-email-btn')
createNewUserBtn = document.querySelector('#create-new-user-btn')
seepasswordBtn2 = document.querySelector('#seepassword-btn2')
myInputArr = document.querySelectorAll('.my-input')
infoBtn = document.querySelector('#info-btn')

Year = document.querySelector('#Year')
Month = document.querySelector('#Month')
Day = document.querySelector('#Day')
boy = document.querySelector('#flexRadioDefault2')
girl = document.querySelector('#flexRadioDefault1')
gender = true
showEmail = document.querySelector('#showEmail')

$(document).ready(function () {
    mainLoginModal.show();
})

seepasswordBtn2.addEventListener('click', function () {
    if (password1Input.type == 'password') {
        seepasswordBtn2.innerHTML = '<i class="fa-regular fa-eye"></i>'
        password1Input.type = 'text'
    }
    else {
        seepasswordBtn2.innerHTML = '<i class="fa-regular fa-eye-slash"></i>'
        password1Input.type = 'password'
    }
})
seePasswordBtn.addEventListener('click', function () {
    if (passwordInput.type == 'password') {
        seePasswordBtn.innerHTML = '<i class="fa-regular fa-eye"></i>'
        passwordInput.type = 'text'
    }
    else {
        seePasswordBtn.innerHTML = '<i class="fa-regular fa-eye-slash"></i>'
        passwordInput.type = 'password'
    }
})

resetPasswordBtn.addEventListener('click', () => {
    normalLoginModal.hide();
    setPasswordModal.show();
})
resetEmailBtn.addEventListener('click', () => {
    normalLoginModal.hide();
    resendModal.show();
})
function openFirstModal() {
    mainLoginModal.show();
}
function resendCheck() {
    setPasswordResendBtn.disabled = !resendPasswordInput.value ? true : false;
}
function apply() {
    mainLoginModal.hide();
    normalLoginModal.hide();
    createNewUserModal.show();
}
function login() {
    createNewUserModal.hide();
    mainLoginModal.hide();
    applyModal.hide();
    normalLoginModal.show();
}
function turnBack() {
    createNewUserModal.hide();
    applyModal.hide();
    normalLoginModal.hide();
    setPasswordModal.hide();
    resendModal.hide();
    mainLoginModal.show();
}

function inputCheck() {
    if (accountInput.value && passwordInput.value) {
        normalLoginBtn.disabled = false
    }
    else {
        normalLoginBtn.disabled = true
    }
}
function CloseModal() {
    applyModal.hide()
}
