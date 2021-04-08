$(document).ready(function () {
	
	var max_fields = 10; //maximum input boxes allowed
	var wrapper = $(".input_fields_wrap"); //Fields wrapper
	var add_button = $(".add_field_button"); //Add button ID
	var new_form = '<div class="form-group"><div class="d-flex align-items-center"><input type="text" name="CampusNames" class="form-control"><button type="button" class="btn btn-danger ml-2 remove_field">-</a></div></div>';

	var x = 1; //initlal text box count
	$(add_button).click(function (e) { //on add input button click
		e.preventDefault();
		if (x < max_fields) { //max input box allowed
			x++; //text box increment
			$(wrapper).append(new_form); //add input box
		}
	});
	
	$(wrapper).on("click", ".remove_field", function (e) { //user click on remove button
		e.preventDefault();
		$(this).parent('div').remove();
		x--;
	})

	$(".custom-file-input").on("change", function () {
		var fileName = $(this).val().split("\\").pop();
		$(this).siblings(".custom-file-label").addClass("selected").html(fileName);
	});
});