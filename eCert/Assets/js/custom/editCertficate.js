$(document).ready(function () {
    var rowIndex = Number.parseInt($('.table-link tr').length) - 1;
    $('.add-more-row').click(function () {
        $('.table-link tbody').append('<tr id="'+ rowIndex++ +'"><td class="row-index"><input type="text"></td><td style="padding-top: 5px"><button type="button" class="btn delete-row"><i class="dripicons-trash"></i></button></td></tr>');
    });
    $('#tbody').on('click', '.delete-row', function () {

        // Getting all the rows next to the row 
        // containing the clicked button 
        var child = $(this).closest('tr').nextAll();

        // Iterating across all the rows  
        // obtained to change the index 
        child.each(function () {

            // Getting <tr> id. 
            var id = $(this).attr('id');

            // Gets the row number from <tr> id. 
            var dig = parseInt(id.substring(1));

            // Modifying row id. 
            $(this).attr('id', `R${dig - 1}`);
        });

        // Removing the current row. 
        $(this).closest('tr').remove();

        // Decreasing total number of rows by 1. 
        rowIndex--;
    });
});

