$(document).ready(function () {
	$(".custom-file-input").on("change", function () {
		var fileName = $(this).val().split("\\").pop();
		$(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });
    getListOfEducationSystem();
})

function getListOfEducationSystem() {
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/GetAllEducationSystem',
        context: document.body,
        dataType: "json",
        success: function (result) {
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