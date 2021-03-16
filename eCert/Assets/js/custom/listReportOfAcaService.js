$(document).ready(function () {
    var firstPage = 1;
    getListOfReport(firstPage);
});

function getListOfReport(pageNumber) {
    var listReport = $(".listAllReport");
    $.ajax({
        type: "POST",
        url: '/AcademicService/LoadAllReport',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listReport.html(result);
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}