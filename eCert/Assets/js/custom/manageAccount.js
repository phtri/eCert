$(document).ready(function () {
    var firstPage = 1;
    //getListOfEducationSystem();
    //getListOfAdmin(firstPage);
    getListOfAcaService();
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

function getListOfAdmin(pageNumber) {
    var listAdmin = $(".contentAdmin");
    var listAcaService = $(".contentAcaService");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAdmin',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAcaService.empty();
            listAdmin.empty();
            listAdmin.html(result);
            //alert('Loading done load admin');
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function getListOfAcaService(pageNumber) {
    var listAcaService = $(".contentAcaService");
    //var listAdmin = $(".contentAdmin");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAcademicService',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAcaService.empty();
            //listAdmin.empty();
            listAcaService.html(result);
            alert('Loading done load aca');
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

