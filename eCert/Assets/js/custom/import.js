$(document).ready(function() {
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

    $('#edusystem').on('change', function (e) {
        var eduSystemId = $("option:selected", this).val();
        getListOfCampus(eduSystemId);
   
    });
})

function getListOfCampus(eduSystemId) {
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/GetListCampus',
        context: document.body,
        data: { eduSystemId: eduSystemId },
        dataType: "json",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            $('#campusSelect').find('option').remove();
            $.each(result, function (i, value) {
                $('#campusSelect').append($('<option>').text(value.CampusName).attr('value', value.CampusId));
            });
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

