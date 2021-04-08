
function getListOfCampus(eduSystemId, pageNumber) {
    var listCampus = $(".listCampus");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/GetListCampusByEdu',
        context: document.body,
        data: { eduSystemId: eduSystemId, pageNumber: pageNumber},
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listCampus.html(result);
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}