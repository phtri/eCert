$(document).ready(function () {
    $(".edu").hide();
    $(".campus").hide();
    getListOfEducationSystem();
    $('#EducationSystemId').on('change', function (e) {
        var eduSystemId = $("option:selected", this).val();
        getListOfCampus(eduSystemId);
    });
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
        url: '/SuperAdmin/GetListCampus',
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
function getListTranscript() {
    $(".edu").hide();
    $(".campus").hide();
    if (validate()) {
        var campusId = $("#CampusId option:selected").val();
        var listTranscript = $(".listTranscript");
        $.ajax({
            type: "POST",
            url: '/Transcript/LoadListTranscript',
            context: document.body,
            data: { campusId: campusId },
            dataType: "html",
            beforeSend: function () {
                $("#loading-overlay").show();
            },
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                listTranscript.html(result);
                $("#loading-overlay").hide();
            },
            error: function (req, err) {
                //debugger;  
                console.log(err);
                alert("Error has occurred..");
            }
        });
    }
    
}
function validate() {
    let resultEdu = validateEdu();
    let resultCampus = validateCampus();

    if (resultEdu && resultCampus) {
        return true;
    } else {
        return false;
    }
}
function validateEdu() {
    console.log($("#EduSystemId").val());
    if ($("#EduSystemId").val() == null) {
        $(".edu").show();
        return false;
    }
    return true;
}
function validateCampus() {
    if ($("#CampusId").val() == null) {
        $(".campus").show();
        return false;
    }
    return true;
}

function submitData(semester, subjectCode, name, mark) {
    submitRowData(semester, subjectCode, name, mark);
}

function submitRowData(semester, subjectCode, name, mark) {
    //var listCert = $(".listCertificate");
    alert('aaa');
        //$.ajax({
        //    type: "POST",
        //    url: '/Transcript/GenerateCertificate',
        //    context: document.body,
        //    data: { semester: semester, subjectCode: subjectCode, name: name, mark: mark},
        //    //dataType: "html",
        //    //contentType: 'application/json; charset=utf-8',
        //    success: function (result) {
        //        //console.log(result);
        //        //listCert.html(result);
        //    },
        //    error: function (req, err) {
        //        //debugger;  
        //        console.log(err);
        //        alert("Error has occurred..");
        //    }
        //});
}