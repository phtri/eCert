$(document).ready(function () {

    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    $(".dropdown-menu li").click(function () {
        var selText = $(this).text();
        $(this).find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
    });

    //$('.main-org').on('change', function () {
    //    if ($(".main-org option:selected").text() === "Đại học FPT") {
    //        $('.sub-org').css('display', '');
    //    }
    //    else {
    //        $('.sub-org').css('display', 'none');
    //    }
    //})
    getListOfEducationSystem();
    $('#edusystem').on('change', function (e) {
        var eduSystemId = $("option:selected", this).val();
        getListOfCampus(eduSystemId);
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
        success: function (result) {
            console.log(result);
            $('#edusystem').find('option').remove();
            $('#edusystem').append($('<option selected disabled>').text("Select Education System"));
            $.each(result, function (i, value) {
                $('#edusystem').append($('<option>').text(value.EducationName).attr('value', value.EducationSystemId));
            });
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
function getListOfCampus(eduSystemId) {
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/GetListCampus',
        context: document.body,
        data: { eduSystemId: eduSystemId },
        dataType: "json",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#CampusId').find('option').remove();
            $('#CampusId').append($('<option selected disabled>').text("Select Campus"));
            $.each(result, function (i, value) {
                $('#CampusId').append($('<option>').text(value.CampusName).attr('value', value.CampusId));
            });
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}



