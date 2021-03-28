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

function handleDeleteAccount(title, msg, userId, campusId) {
    $('#confirmModal').modal('show');
    $('#confirmTitle').html(title);
    $('.modal-body').html(msg);
    $('#confirmModal').on('click', '.btn-yes', function (e) {
        deleteAcademicService(userId, campusId);
    });
}

function deleteAcademicService(userId, campusId) {
    var firstPage = 1;
    $.ajax({
        type: "POST",
        url: '/Admin/DeleteAcademicService',
        context: document.body,
        data: { userId: userId, campusId: campusId },
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
