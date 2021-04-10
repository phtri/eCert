$(document).ready(function () {
    getListOfEducationSystem();
    $('#EduSystemId').on('change', function (e) {
        var eduSystemId = $("option:selected", this).val();
        getListOfCampus(eduSystemId, 1);
        localStorage.setItem("eduSystemId", eduSystemId);
    });
})

function getListOfEducationSystem(eduSystemId) {
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/GetAllEducationSystem',
        context: document.body,
        //data: { eduSystemId: eduSystemId },
        dataType: "json",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            $('#EduSystemId').find('option').remove();
            $('#EduSystemId').append($('<option selected disabled>').text("Select Education System"));
            $.each(result, function (i, value) {
                $('#EduSystemId').append($('<option>').text(value.EducationName).attr('value', value.EducationSystemId));
            });
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function getListOfCampus(eduSystemId, pageNumber) {
    console.log(eduSystemId);
    var listCampus = $(".listCampus");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadListCampusByEdu',
        context: document.body,
        data: { eduSystemId: eduSystemId, pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        success: function (result) {
            //console.log(result);
            listCampus.empty();
            listCampus.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

