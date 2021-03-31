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
            $('#filter1').find('option').remove();
            $('#filter1').append($('<option selected disabled>').text("Choose..."));
            $.each(result, function (i, value) {
                $('#filter1').append($('<option>').text(value.EducationName).attr('value', value.EducationSystemId));
            });

            $('#filter2').find('option').remove();
            $('#filter2').append($('<option selected disabled>').text("Choose..."));
            $.each(result, function (i, value) {
                $('#filter2').append($('<option>').text(value.EducationName).attr('value', value.EducationSystemId));
            });
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}