function enableError(inputID, error) {
    var input = document.getElementById(inputID);
    var labelError = document.getElementById(inputID + "Error");
    input.classList.add('error');
    labelError.innerText = error;
}

function disableError(inputID) {
    var input = document.getElementById(inputID);
    input.classList.remove('error');
    labelError.innerText = null;
}

function enableOrDisableError(inputID, error) {
    var input = document.getElementById(inputID);

}

function showModal(id) {
    var modal = document.getElementById(id);
    modal.style.display = "block";
}