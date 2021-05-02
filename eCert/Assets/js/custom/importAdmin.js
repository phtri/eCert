$(document).ready(function () {
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    $(".dropdown-menu li").click(function () {
        var selText = $(this).text();
        $(this).find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
    });

    getListOfEducationSystem();
    $('#EducationSystemId').on('change', function (e) {
        var eduSystemId = $("option:selected", this).val();
        getListOfCampus(eduSystemId);
        getListOfSignature(eduSystemId);
    });
})
function sendForm() {
    $("#loading-overlay").show();
    $("#importForm").submit();
}
function getListOfEducationSystem() {
    $.ajax({
        type: "POST",
        url: '/Admin/GetEducationSystem',
        context: document.body,
        //data: { eduSystemId: eduSystemId },
        dataType: "json",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            $('#EducationSystemId').find('option').remove();
            $('#EducationSystemId').append($('<option selected disabled>').text("Select Education System"));
            $.each(result, function (i, value) {
                $('#EducationSystemId').append($('<option>').text(value.EducationName).attr('value', value.EducationSystemId));
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
        url: '/Admin/GetcampusByUserId',
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
function getListOfSignature(eduSystemId) {
    $.ajax({
        type: "POST",
        url: '/Admin/GetSignatureByEduid',
        context: document.body,
        data: { eduSystemId: eduSystemId },
        dataType: "json",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#SignatureId').find('option').remove();
            $('#SignatureId').append($('<option selected disabled>').text("Select Signature"));
            $.each(result, function (i, value) {
                $('#SignatureId').append($('<option>').text(value.FullName).attr('value', value.SignatureId));
            });
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}


