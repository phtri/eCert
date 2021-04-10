$(document).ready(function () {
    getListOfEducationSystem();
})
function getListOfEducationSystem() {
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/GetAllEducationSystem',
        context: document.body,
        //data: { eduSystemId: eduSystemId },
        dataType: "json",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            $('#EduSystemId').find('option').remove();
            $('#EduSystemId').append($('<option selected disabled>').text("Select Education System"));
            $.each(result, function (i, value) {
                $('#EduSystemId').append($('<option>').text(value.EducationName).attr('value', value.EducationSystemId));
            });
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}