function GetListEduSystem(pageNumber) {
    var listEdu = $(".listEdu");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadListOfEducationSystem',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            listEdu.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function handleDeleteEdu(title, msg, eduSystemId) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteEducation(eduSystemId);
    });
}

function deleteEducation(eduSystemId) {
    let firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeleteEducation',
        context: document.body,
        data: { eduSystemId: eduSystemId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            console.log(result);
            $('#confirmModal').modal('hide');
            $("#loading-overlay").hide();
            GetListEduSystem(firstPage);
            $(".modal-backdrop").remove();
            if (result.IsSuccess == false) {
                $('#alertModal').modal('show');
                $('#confirmTitle').html('Alert');
                $('.modal-body').html(result.Message);
            } else {
                $.NotificationApp.send("Message", result.Message, "top-center", "Background color", "Icon");
            }
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}