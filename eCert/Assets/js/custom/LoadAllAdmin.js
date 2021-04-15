function getListOfAdmin(pageNumber) {
    console.log(pageNumber);
    var listAdmin = $("#admin");
    var listAcaService = $("#acaservice");
    localStorage.setItem("pageNumber", pageNumber);
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAdmin',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAdmin.empty();
            listAcaService.empty();
            listAdmin.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
function deactiveAcc(userId, roleId) {
    let pageNumber = localStorage.getItem("pageNumber");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeactiveAdmin',
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
                getListOfAdmin(pageNumber);
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
function activeAcc(userId, roleId) {
    let pageNumber = localStorage.getItem("pageNumber");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/ActiveAdmin',
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
                getListOfAdmin(pageNumber);
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
function handleDeleteAccountAdmin(title, msg, userId, campusId, roleId) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteAdmin(userId, campusId, roleId);
    });
}

function deleteAdmin(userId, campusId, roleId) {
    let firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeleteAdmin',
        context: document.body,
        data: { userId: userId, campusId: campusId, roleId: roleId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            $('#confirmModal').modal('hide');
            $("#loading-overlay").hide();
            getListOfAdmin(firstPage);
            $(".modal-backdrop").remove();
            
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
