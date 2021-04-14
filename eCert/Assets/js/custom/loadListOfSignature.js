function getListOfSignature(pageNumber) {
    var listSignature = $(".listSignature");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadListOfSignature',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            listSignature.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function handleDeleteSignature(title, msg, signatureId, eduSystemId) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteSignature(signatureId, eduSystemId);
    });
}

function deleteSignature(signatureId, eduSystemId) {
    let firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeleteSignature',
        context: document.body,
        data: { signatureId: signatureId, eduSystemId: eduSystemId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            console.log(result);
            $('#confirmModal').modal('hide');
            $("#loading-overlay").hide();
            $(".modal-backdrop").remove();
            if (result.IsSuccess == false) {
                $('#alertModal').modal('show');
                $('#confirmTitle').html('Alert');
                $('.modal-body').html(result.Message);
            } else {
                $.NotificationApp.send("Message", result.Message, "top-center", "Background color", "Icon");
                getListOfSignature(firstPage);
            }
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}