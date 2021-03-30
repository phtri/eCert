$(document).ready(function () {
    getListOfEducationSystem();
})

function getListOfEducationSystem() {
    $.ajax({
        type: "POST",
        url: '/Admin/GetEducationSystem',
        context: document.body,
        dataType: "json",
        success: function (result) {
            console.log(result);
            $('#filter').find('option').remove();
            $('#filter').append($('<option selected disabled>').text("Choose..."));
            $.each(result, function (i, value) {
                $('#filter').append($('<option>').text(value.EducationName).attr('value', value.EducationSystemId));
            });
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}