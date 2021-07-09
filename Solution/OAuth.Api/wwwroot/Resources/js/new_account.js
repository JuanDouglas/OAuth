function showOrHideModal() {
    var modal = document.getElementById('loadingModal');

    if (modal.style.display == "block") {
        modal.style.display = "none";
    } else {
        modal.style.display = "block";
    }
}