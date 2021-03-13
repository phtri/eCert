$(document).ready(function () {
    var firstPage = 1;
    getListOfAcaService(firstPage);
});

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