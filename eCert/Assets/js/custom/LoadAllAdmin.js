$(document).ready(function () {
    var firstPage = 1;
    getListOfAdmin(firstPage);
})
function getListOfAdmin(pageNumber) {
    var listAdmin = $(".contentAdmin");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAdmin',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAdmin.html(result);
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
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/DeleteAdmin',
        context: document.body,
        data: { userId: userId, campusId: campusId, roleId: roleId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            //listAcaService.html(result);
            getListOfAcaService(firstPage);
            $('#confirmModal').modal('hide');
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
