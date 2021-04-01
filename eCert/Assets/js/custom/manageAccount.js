﻿$(document).ready(function () {
    var firstPage = 1;
    //getListOfEducationSystem();
    getListAdmin(firstPage);
    //getListOfAcaService();
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

function getListAdmin(pageNumber) {
    console.log('ad');
    var listAdmin = $("#admin");
    var listAcaService = $("#acaservice");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAdmin',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAdmin.empty();
            listAcaService.empty();
            listAdmin.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function getListOfAcaService(pageNumber) {
    console.log('ac');
    var listAdmin = $("#admin");
    var listAcaService = $("#acaservice");
    $.ajax({
        type: "POST",
        url: '/SuperAdmin/LoadAllAcademicService',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listAdmin.empty();
            listAcaService.empty();
            listAcaService.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

