function getAllReport(pageNumber) {
    var listReport = $(".listAllReport");
    $.ajax({
        type: "POST",
        url: '/AcademicService/LoadAllReport',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listReport.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}