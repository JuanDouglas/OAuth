function enableError(inputID, error) {
    var input = document.getElementById(inputID);
    var labelError = document.getElementById(inputID + "Error");
    input.classList.add('error');
    labelError.innerText = error;
}

function disableError(inputID) {
    var input = document.getElementById(inputID);
    var labelError = document.getElementById(inputID + "Error");
    input.classList.remove('error');
    labelError.innerText = null;
}

function showModal(id) {
    var modal = document.getElementById(id);
    modal.style.display = "block";
}

function showOrHidePassword(idPasswordInput, idCheckbox) {
    var inputPassword = document.getElementById(idPasswordInput)
    var checkbox = document.getElementById(idCheckbox);
    if (inputPassword.type == "password") {
        inputPassword.type = "text";
        checkbox.innerText = 'Hide password';
    } else {
        inputPassword.type = "password";
        checkbox.innerText = 'Show password';
    }
}