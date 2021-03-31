function getAllAcaService(pageNumber) {
    var listAcaService = $(".contentAcaService");
    var listAdmin = $(".contentAdmin");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAcademicService',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAcaService.empty();
            listAdmin.empty();
            listAcaService.html(result);
            //alert('Loading done get list');
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function handleDeleteAccountAcaService(title, msg, userId, campusId, roleId) {
    alert(userId);
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteAcaService(userId, campusId, roleId);
    });
}

function deleteAcaService(userId, campusId, roleId) {
    var firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/Admin/DeleteAcademicService',
        context: document.body,
        data: { userId: userId, campusId: campusId, roleId: roleId },
        //dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            //alert('Loading done delete');
            getAllAcaService(firstPage);
            $('#confirmModal').modal('hide');
            
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred1");
        }
    });
}
