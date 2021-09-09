var host = window.location.protocol + "//" + window.location.host + window.location.port+"/api";
const origin = 'Nexus Web Site'

function OnClickLogin() {
    var inputUser = document.getElementById('inputUser');
    var modal = document.getElementById('loadingModal');
    var URL = host + '/api/Login/FirstStep?user=' + inputUser.value;
    modal.style.display = "block";

    var firstStepKey;
    var firstStepID;
    var valid;

    var xhr = new XMLHttpRequest();
    xhr.open('GET', URL, true);
    xhr.orgin = origin;
    xhr.withCredentials = true;
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
    var loadingModal = document.getElementById('loadingModal');
    var confirmModal = document.getElementById('confirmModal');

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
        URL = host + '/api/Login/SecondStep?set_cookie=true&key=' + firstStepKey + '&fs_id=' + firstStepID + '&pwd=' + inputPas.value;
        xhr.open('GET', URL, true);
        xhr.onload = function () {
            status = xhr.status;
            if (status == 200) {
                if (!xhr.response.validedAccount) {
                    confirmModal.style.display = "block";

                } else {
                    window.location = '../index.html'
                }
            } else {
                showOrHiddenPasswordError(true);
            }
            loadingModal.style.display = "none";
        };
        xhr.send();
    } else {
        loadingModal.style.display = "none";
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


function ComeOnClick() {
    var URL = host + '/api/Account/SendConfirmation';
    var xhr = new XMLHttpRequest();
    var confirmModal = document.getElementById('confirmModal');

    xhr.open('POST', URL, true);
    xhr.orgin = origin;
    xhr.withCredentials = true;
    xhr.responseType = 'json';
    xhr.onload = function () {
        confirmModal.style.display = 'none';
    }
    xhr.send();
}