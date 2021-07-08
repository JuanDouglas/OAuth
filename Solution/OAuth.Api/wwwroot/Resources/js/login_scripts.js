const host = 'https://nexus-oauth.azurewebsites.net'

function OnClickLogin() {
    var inputUser = document.getElementById('inputUser');
    var inputPas = document.getElementById('inputPassword');

    const URL = host + '/api/Login/FirstStep?web_page=false&user=' + inputUser.value;

    var firstStepKey;
    var firstStepID;
    var valid;

    var xhr = new XMLHttpRequest();
    xhr.open('GET', URL, true);
    xhr.mode = 'no-cors';
    xhr.orgin = 'Nexus Web Site';
    xhr.responseType = 'json';
    xhr.onload = function () {
        var status = xhr.status;
        if (status === 200) {
            firsStepKey = xhr.response.key;
            firstStepID = xhr.response.id;
            valid = xhr.response.valid;
        }
        else {
            valid = false;
            inputUser.classList.add('');
            inputUser.classList.remove('');
        }
    };
    xhr.send();
}


