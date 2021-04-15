function getListOfAcaService(pageNumber) {
    var listAcaService = $(".listAcademicService");
    localStorage.setItem("pageNumber", pageNumber);
    $.ajax({
        type: "POST",
        url: '/Admin/LoadListOfAcademicService',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            listAcaService.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
function stopLoading() {
    $("#loading-overlay").hide();
}
function deactiveAcaService(userId, roleId) {
    let pageNumber = localStorage.getItem("pageNumber");
    $.ajax({
        type: "POST",
        url: '/Admin/DeactiveAcaService',
        context: document.body,
        data: { userId: userId, roleId: roleId },
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
                getListOfAcaService(pageNumber);
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
function activeAcaService(userId, roleId) {
    let pageNumber = localStorage.getItem("pageNumber");
    $.ajax({
        type: "POST",
        url: '/Admin/ActiveAcaService',
        context: document.body,
        data: { userId: userId, roleId: roleId },
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
                getListOfAcaService(pageNumber);
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
function handleDeleteAccount(title, msg, userId, campusId, roleId) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteAcademicService(userId, campusId, roleId);
    });
}

function deleteAcademicService(userId, campusId, roleId) {
    var firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/Admin/DeleteAcademicService',
        context: document.body,
        data: { userId: userId, campusId: campusId, roleId: roleId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            getListOfAcaService(firstPage);
            $(".modal-backdrop").remove();
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
