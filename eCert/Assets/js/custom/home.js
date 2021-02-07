$(document).ready(function () {
    //auto check radio certificate link
    $("#customRadio1").prop("checked", true)

    //default show input certificate link
    $(".certificate_file").hide();
    $(".certificate_link").show();
    
    //Hide error message element by class
    hideElementByClass('.cert_name');
    hideElementByClass('.cert_link');
    hideElementByClass('.cert_file');
    hideElementByClass('.cert_des');

});

function hideElementByClass(className) {
    $(className).hide();
}

function handleClickRadio(myRadio) {
    if (myRadio.value == 1){
        $(".certificate_file").hide();
        $(".certificate_link").show();
    } else if (myRadio.value == 2){
        $(".certificate_link").hide();
        $(".certificate_file").show();
    }
}
function validateAddcertificate() { 
    let resultcertname = validateCertName();
    let resultDes = validateDescription();
    if ($('input[name="customRadio"]:checked').val() == 1) {
        let resutcertlink = validateCertLink();
        if (resultcertname && resultDes && resutcertlink) {
            return true;
        } else {
            return false;
        }
    } else if ($('input[name="customRadio"]:checked').val() == 2) {
        let resutcertfile = validateCertFile();
        if (resultcertname && resultDes && resutcertfile) {
            return true;
        } else {
            return false;
        }
    }
}
function validateCertName() {
    if ($("#CertificateName").val() == "") {
        $(".cert_name").show();
        return false;
    }
    return true;
}
function validateDescription() {
    if ($("#Description").val() == "") {
        $(".cert_des").show();
        return false;
    }
    return true;
}
function validateCertLink() {
    if ($("#Content").val() == "") {
        $(".cert_link").show();
        return false;
    }
    return true;
}
function validateCertFile() {
    if ($("#CertificateFile").val() == "") {
        $(".cert_file").show();
        return false;
    }
    return true;
}
function submitCertificate() {
    if (validateAddcertificate()) {
        $(".addForm").submit();
       
    } else {
        return false;
    }
  
}

function showFormModal(headerText, submitButtonText) {
    $('#compose-modal').modal('show');
    $('.title-add-form').html(headerText);
    $('#submitButton').html(submitButtonText)
}

function UpdateUserDetail() {
 
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/JsonDemo/UpdateUsersDetail',
        context: document.body,
        data: getReportColumnsParams,
        success: function (result) {
            alert(result);
        },
        error: function (xhr) {
            //debugger;  
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}  