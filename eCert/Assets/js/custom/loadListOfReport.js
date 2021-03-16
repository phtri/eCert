function getListOfReport(pageNumber) {
    var listReport = $(".listReport");
    $.ajax({
        type: "POST",
        url: '/Certificate/LoadListOfReport',
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