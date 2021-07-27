const origin = 'Nexus Web Site'
function CreateAccount() {
    var xhr = new XMLHttpRequest();
    var form = document.getElementById('CreateAccount');
    xhr.open('PUT', form.action, true);
    xhr.setRequestHeader('Content-Type', 'application/json; charset=UTF8;');
    xhr.origin = origin;
    xhr.responseType = 'json';
    var json = {
        "password": form.Password.value,
        "confirmPassword": form.ConfirmPassword.value,
        "email": form.Email.value,
        "phonenumber": form.PhoneNumber.value,
        "username": form.Username.value,
        "acceptterms": form.AcceptTerms.checked,
        "iscompany": form.IsCompany.checked,
        "zipcode": form.ZipCode.value,
        "name": form.Name.value
    };
    xhr.onload = function () {
        var status = xhr.status;
        if (status == 400) {
            var errors = xhr.response.errors;
            Object.entries(errors).forEach(entry => {
                const [key, value] = entry;
                enableError(key, value);
                console.log(key, value);
            });

        } else if (status == 200) {
            show('welcomeModal');
        } else if (status == 500) {
            showError('CreateAccount');
        }
        hide('loadingModal');
    };
    show('loadingModal');
    xhr.send(JSON.stringify(json));
}