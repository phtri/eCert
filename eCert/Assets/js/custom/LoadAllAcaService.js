function getAllAcaService(pageNumber) {
    var listAdmin = $("#admin");
    var listAcaService = $("#acaservice");
    localStorage.setItem("pageNumber", pageNumber);
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAcademicService',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            listAdmin.empty();
            listAcaService.empty();
            listAcaService.html(result);
            
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function handleDeleteAccountAcaService(title, msg, userId, campusId, roleId) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteAcaService(userId, campusId, roleId);
    });
    
}
function deactiveAcaService(userId, roleId, campusId) {
    let pageNumber = localStorage.getItem("pageNumber");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeactiveAcaService',
        context: document.body,
        data: { userId: userId, roleId: roleId, campusId: campusId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {

            if (result.IsSuccess == false) {
                $('#alertModal').modal('show');
                $('#confirmTitle').html('Alert');
                $('.modal-body').html(result.Message);
            } else {
                getAllAcaService(pageNumber);
            }
            $("#loading-overlay").hide();

        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
function activeAcaService(userId, roleId, campusId) {
    let pageNumber = localStorage.getItem("pageNumber");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/ActiveAcaService',
        context: document.body,
        data: { userId: userId, roleId: roleId, campusId: campusId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {

            if (result.IsSuccess == false) {
                $('#alertModal').modal('show');
                $('#confirmTitle').html('Alert');
                $('.modal-body').html(result.Message);
            } else {
                getAllAcaService(pageNumber);
            }
            $("#loading-overlay").hide();

        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
function deleteAcaService(userId, campusId, roleId) {
    var firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeleteAcademicService',
        context: document.body,
        data: { userId: userId, campusId: campusId, roleId: roleId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            if (result.IsSuccess) {
                $('#confirmModal').modal('hide');
                $("#loading-overlay").hide();
                $(".modal-backdrop").remove();
                getAllAcaService(firstPage);
                $.NotificationApp.send("Message", result.Message, "top-center", "Background color", "Icon");
            } else {
                alert("Error has occurred..");
            }
            
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
