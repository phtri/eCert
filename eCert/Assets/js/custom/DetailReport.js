$(document).ready(function () {
    //getStatusByReportId(getUrlParameter('reportId'));
});

function getStatusByReportId(reportId) {
    $.ajax({
        type: "POST",
        url: '/AcademicService/GetStatusByReportId',
        context: document.body,
        data: { reportId: reportId },
        dataType: "json",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result == "Pending") {
                $("#pending").prop("checked", true);
            } else if (result == "Updated") {
                $("#updated").prop("checked", true);
            } else if (result == "Rejected") {
                $("#rejected").prop("checked", true);
            }
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
function sendForm() {
    $("#updateStatus").submit();
}
function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return typeof sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};