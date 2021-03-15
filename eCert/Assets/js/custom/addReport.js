function displayMessage(title, msg) {
    $('#notificationModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        alert('Done');
    });
}

function handleSendReport() {
    alert('Send report');
}