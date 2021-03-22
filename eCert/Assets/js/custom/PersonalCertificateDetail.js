function handleDeleteAccount(title, msg, id) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        $.ajax({
            type: "POST",
            url: '/Certificate/Delete',
            context: document.body,
            data: { certId: id },
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                window.location.replace('/Certificate/Index');
            },
            error: function (req, err) {
                //debugger;  
                console.log(err);
                alert("Error has occurred..");
            }
        });
    });
}