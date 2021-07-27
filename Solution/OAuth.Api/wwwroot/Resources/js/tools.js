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

function show(id) {
    var modal = document.getElementById(id);
    modal.style.display = "block";
}

function hide(id) {
    var modal = document.getElementById(id);
    modal.style.display = "none";
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

function phoneMask(input) {
    const phone = clean(input.value);
    let text = phone;

    if (phone.length > 5 && phone.length < 9) {
        const part1 = phone.slice(0, 5);
        const part2 = phone.slice(5, phone.length);
        text = `${part1}-${part2}`
    }

    if (phone.length > 8 && phone.length < 10) {
        const p1 = phone.slice(0, 1);
        const p2 = phone.slice(1, 5);
        const p3 = phone.slice(5, 9);
        text = `${p1} ${p2}-${p3}`
    }

    if (phone.length == 11) {
        const p1 = phone.slice(0, 2);
        const p2 = phone.slice(2, 3);
        const p3 = phone.slice(3, 7);
        const p4 = phone.slice(7, 11);
        text = `(${p1}) ${p2} ${p3}-${p4}`
    }
    input.value = text;
}

function zipCodeMask(input) {
    const zipCode = clean(input.value);
    let text = zipCode;
    if (zipCode.length > 5) {
        const part1 = zipCode.slice(0, 5);
        const part2 = zipCode.slice(5, zipCode.length);
        text = `${part1}-${part2}`
    }
    input.value = text;
}

function clean(str) {
    return str.replace(/[^0-9]/g, "")
}

function showError(fatter) {
    var element = document.getElementById(fatter);
    element.innerHTML = element.innerHTML + '<div class=\"notification-bottom\" id=\"card-error\"><div class=\"card\"><h2 class="error">Error</h2><p class="error-text">&emsp;An error occurred while performing this action, our developers are already being alerted.</p><p class="error-p-button"><button class="normal" onclick="hideError()">Ook!</button></p></div></div>';
}

function hideError() {
    var error = document.getElementById('card-error');
    error.style.display = 'none';
}