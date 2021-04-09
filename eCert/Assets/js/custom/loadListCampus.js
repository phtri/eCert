
function getListOfCampusByPage(pageNumber) {
    let listCampus = $(".listCampus");
    let eduSystemId = localStorage.getItem("eduSystemId");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadListCampusByEdu',
        context: document.body,
        data: { eduSystemId: eduSystemId, pageNumber: pageNumber},
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listCampus.html(result);
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function handleDeleteCampus(title, msg, campusId) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteCampus(campusId);
    });
}

function deleteCampus(campusId) {
    let firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeleteCampus',
        context: document.body,
        data: { campusId: campusId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            console.log(result);
            $('#confirmModal').modal('hide');
            $("#loading-overlay").hide();
            getListOfCampusByPage(firstPage);
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