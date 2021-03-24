
$(document).ready(function () {
    $("#top-search").unbind('keypress').bind('keypress', function (e) {
        if (e.keycode === 13 || e.which === 13) {
            getListOfCert();
        }
    });
    localStorage.setItem("searchKeyword", "");
    //$("#top-search").keyup(function (event) {
    //    if (event.keyCode === 13 || event.which === 13) {
    //        getListOfCert();
    //    }
    //});

    //auto check radio certificate link
    $("#customRadio1").prop("checked", true)

    //default show input certificate link
    //$(".certificate_file").hide();
    //$(".certificate_link").show();
    
    //Hide error message element by class
    hideElementByClass('.cert_name');
    hideElementByClass('.cert_des');
    hideElementByClass('.cert_file');
    hideElementByClass('.issuer_name');
    hideElementByClass('.cert_file_extension');
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
function hideElementFile() {
    hideElementByClass('.cert_file');
    hideElementByClass('.cert_file_extension');
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
    //let resultIssuerName = validateIssuerName();
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
    if ($("#Links").val() == "" && $("#CertificateFile").val() == "") {
        $(".cert_file").show();
        return false;
    }
    else if ($("#Links").val() != "" && $("#CertificateFile").val() == "") {
        return true;
    }
    else if ($("#Links").val() == "" && $("#CertificateFile").val() != "") {
        if (validateExtensionFile()) {
            return true;
        } else {
            return false;
        }
    }
    return true;
}
function validateExtensionFile() {
    var _validFileExtensions = ["jpg", "jpeg", "pdf", "png"];
    var filePath = $("#CertificateFile").val();
    var filename = filePath.split('\\').pop();
    var extension = filename.split('.').pop();
    var flag = false;
    for (var i = 0; i < _validFileExtensions.length; i++) {
        if (extension == _validFileExtensions[i]) {
            flag = true;
            break;
        }
    }
    if (!flag) {
        $(".cert_file_extension").show();
    }
    return flag;
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
function validateIssuerName() {
    if ($("#IssuerName").val() == "") {
        $(".issuer_name").show();
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
    if ($("#Links").val() == "") {
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

function getListOfCert() {
    var listCert = $(".listCertificate");  
    var keyword = $("#top-search").val();
    
    $.ajax({
        type: "POST",
        url: '/Certificate/LoadListOfCert',
        context: document.body,
        data: { keyword: keyword },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listCert.html(result);
            localStorage.setItem("searchKeyword", keyword);
        },
        error: function (req, err) {
            //debugger;  
            
            console.log(err);
            alert("Error has occurred..");
        }
    });
}

function downloadSearchCert() {
    var keyword = localStorage.getItem("searchKeyword");
    $.ajax({
        type: "POST",
        url: '/Certificate/DownloadSearchedCertificate',
        context: document.body,
        data: { keyword: keyword },
        dataType: "html",
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            //listCert.html(result);
            window.location = '/Certificate/DownloadSearchedCertificate?keyword=' + keyword;
        },
        error: function (req, err) {
            //debugger;  

            console.log(err);
            alert("Error has occurred..");
        }
    });
}