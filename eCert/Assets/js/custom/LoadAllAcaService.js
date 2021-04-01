function getAllAcaService(pageNumber) {
    var listAdmin = $("#admin");
    var listAcaService = $("#acaservice");
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

function deleteAcaService(userId, campusId, roleId) {
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
            $('#confirmModal').modal('hide');
            $("#loading-overlay").hide();
            $(".modal-backdrop").remove();
            getAllAcaService(firstPage);
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred1");
        }
    });
}
