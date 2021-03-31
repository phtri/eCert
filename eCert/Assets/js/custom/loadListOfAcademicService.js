function getListOfAcaService(pageNumber) {
    var listAcaService = $(".listAcademicService");
    $.ajax({
        type: "POST",
        url: '/Admin/LoadListOfAcademicService',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAcaService.html(result);
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function handleDeleteAccount(title, msg, userId, campusId, roleId) {
    alert("Role Id 1: " + roleId);
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        alert("Role Id 2: " + this.roleId);
        
    });
}

function deleteAcademicService(userId, campusId, roleId) {
    var firstPage = 1;
    alert("Role Id 3: " + roleId);
    $.ajax({
        type: "POST",
        url: '/Admin/DeleteAcademicService',
        context: document.body,
        data: { userId: userId, campusId: campusId, roleId: roleId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
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
