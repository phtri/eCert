$(document).ready(function () {
    var firstPage = 1;
    getListOfSignature(firstPage);
});

function getListOfSignature(pageNumber) {
    var listSignature = $(".listSignature");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadListOfSignature',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            listSignature.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}