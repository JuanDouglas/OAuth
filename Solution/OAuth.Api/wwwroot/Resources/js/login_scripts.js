const host = 'https://nexus-oauth.azurewebsites.net'


function OnClickLogin() {
    var inputUser = document.getElementById('inputUser');
    var modal = document.getElementById('loadingModal');
    var URL = host + '/api/Login/FirstStep?web_page=false&user=' + inputUser.value;
    modal.style.display = "block"; 

    var firstStepKey;
    var firstStepID;
    var valid;

    var xhr = new XMLHttpRequest();
    xhr.open('GET', URL, true);
    xhr.mode = 'no-cors';
    xhr.orgin = 'Nexus Web Site';
    xhr.responseType = 'json';
    xhr.onload = function () {
        VerifyFirstStepResponse(xhr);
    };

    showOrHiddenLoginError(false);
    showOrHiddenPasswordError(false);
    xhr.send();
}

function VerifyFirstStepResponse(xhr) {
    var status = xhr.status;
    var inputPas = document.getElementById('inputPassword');
    var modal = document.getElementById('loadingModal');

    if (status === 200) {
        firstStepKey = xhr.response.key;
        firstStepID = xhr.response.id;
        valid = xhr.response.valid;
    }
    else {
        valid = false;
        showOrHiddenLoginError(true);
    }

    if (valid) {
        URL = host + '/api/Login/SecondStep?web_page=false&key=' + firstStepKey + '&fs_id=' + firstStepID + '&pwd=' + inputPas.value;
        xhr.open('GET', URL, true);
        xhr.onload = function () {
            status = xhr.status;
            if (status == 200) {
                window.location = '../index.html'
            } else {
                showOrHiddenPasswordError(true);
            }
            modal.style.display = "none";
        };
        xhr.send();
    } else {
        modal.style.display = "none";
    }


};

function showOrHiddenLoginError(show) {
    var inputUser = document.getElementById('inputUser')
    var userError = document.getElementById('userError')

    if (show) {
        inputUser.classList.add('error');
        userError.classList.add('form-error-visible');
    } else {
        inputUser.classList.remove('error');
        userError.classList.remove('form-error-visible');
    }
}

function showOrHiddenPasswordError(show) {
    var inputPassword = document.getElementById('inputPassword')
    var userError = document.getElementById('passwordError')

    if (show) {
        inputPassword.classList.add('error');
        userError.classList.add('form-error-visible');
    } else {
        inputPassword.classList.remove('error');
        userError.classList.remove('form-error-visible');
    }
}

function showOrHidePassword() {
    var inputPassword = document.getElementById('inputPassword')
    var checkbox = document.getElementById('lblCBSPas');
    if (inputPassword.type == "password") {
        inputPassword.type = "text";
        checkbox.innerText = 'Hide password';
    } else {
        inputPassword.type = "password";
        checkbox.innerText = 'Show password';
    }
}