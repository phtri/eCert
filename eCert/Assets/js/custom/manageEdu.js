$(document).ready(function () {
    var firstPage = 1;
    getListOfEduSystem(firstPage);
});

function getListOfEduSystem(pageNumber) {
    var listEdu = $(".listEdu");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadListOfEducationSystem',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            listEdu.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}