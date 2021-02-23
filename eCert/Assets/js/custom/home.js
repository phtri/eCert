$(document).ready(function () {
    //auto check radio certificate link
    $("#customRadio1").prop("checked", true)

    //default show input certificate link
    //$(".certificate_file").hide();
    //$(".certificate_link").show();
    
    //Hide error message element by class
    hideElementByClass('.cert_name');
    hideElementByClass('.cert_des');
    hideElementByClass('.cert_file');
    hideDateMsg();

    configDatePicker();

});
function hideDateMsg() {
    hideElementByClass('.cert_date');
    hideElementByClass('.cert_date_require');
}
function hideElementByClass(className) {
    $(className).hide();
}
function configDatePicker() {
    $('.issue-date').val(null);
    $('.expiry-date').val(null);
    var dtToday = new Date();

    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate();
    var year = dtToday.getFullYear();

    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var maxDate = year + '-' + month + '-' + day;
    $('.issue-date').attr('max', maxDate);
}

//function handleClickRadio(myRadio) {
//    if (myRadio.value == 1){
//        $(".certificate_file").hide();
//        $(".certificate_link").show();
//    } else if (myRadio.value == 2){
//        $(".certificate_link").hide();
//        $(".certificate_file").show();
//    }
//}
function validateAddcertificate() { 
    let resultcertname = validateCertName();
    let resultDes = validateDescription();
    let validateDate = validateDateIssueAndExpiry();
    let resutcertlinkorFile = validateCertLinkOrFile();
    
    //let resutcertfile = validateCertFile();
    if (resultcertname && resultDes && validateDate && resutcertlinkorFile) {
        return true;
    } else {
        return false;
    }

    //if ($('input[name="customRadio"]:checked').val() == 1) {
    //    let resutcertlink = validateCertLink();
    //    if (resultcertname && resultDes && resutcertlink) {
    //        return true;
    //    } else {
    //        return false;
    //    }
    //} else if ($('input[name="customRadio"]:checked').val() == 2) {
    //    let resutcertfile = validateCertFile();
    //    if (resultcertname && resultDes && resutcertfile) {
    //        return true;
    //    } else {
    //        return false;
    //    }
    //}
}
function validateCertLinkOrFile() {
    if ($("#Content").val() == "" && $("#CertificateFile").val() == "") {
        $(".cert_file").show();
        return false;
    }
    return true;
}
function validateDateIssueAndExpiry() {
    var issueDate = new Date($('.issue-date').val());
    var expiryDate = new Date($('.expiry-date').val());
    if (($('.issue-date').val() == "" && $('.expiry-date').val() != "") || ($('.issue-date').val() != "" && $('.expiry-date').val() == "")) {
        $(".cert_date_require").show();
        return false;
    }
    if ($('.issue-date').val() != "" || $('.expiry-date').val() != "") {
        if (issueDate < expiryDate) {
            return true;
        } else {
            $(".cert_date").show();
            return false;
        }
    } 
    return true;
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

function loadDataEdit(certId) {
    //$('#compose-modal').modal('show');
    //$('.title-add-form').html("Edit a certificate");
    //$('#submitButton').html("Edit");
    //$.ajax({
    //    type: "POST",
    //    url: '/home/EditCertificate',
    //    context: document.body,
    //    data: { certId: certId },
    //    dataType: "json",
    //    contentType: 'application/json; charset=utf-8',
    //    success: function (result) {
    //        console.log(result.CertificateName);
    //        $('#CertificateName').val(result.CertificateName);
    //        $('#Description').val(result.Description);
    //        $('#Content').val(result.Content);
    //        if (result.Format === 'LINK') {
    //            $('#customRadio1').prop('checked', true);
    //            $('#customRadio2').prop('checked', false);
    //        }
    //        else {
    //            $('#customRadio1').prop('checked', false);
    //            $('#customRadio2').prop('checked', true);
    //            $('.certificate_link').css('display', 'none');
    //            $('.certificate_file').css('display', '');
    //        }
    //    },
    //    error: function (req, err) {
    //        //debugger;  
    //        console.log(err);
    //        alert("Error has occurred..");
    //    }
    //});
}  

function loadListCert() {
    var listCert = $(".listCertificate");  
    $.ajax({
        type: "POST",
        url: '/home/LoadListOfCert',
        context: document.body,
        //data: { certId: certId },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listCert.html(result);
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}
function test() {
    alert("aa");
}