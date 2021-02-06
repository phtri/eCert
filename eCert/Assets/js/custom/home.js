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
        
        //$('#compose-modal').modal('hide');
        //$(".addForm").submit(function (e) {
    //    e.preventDefault();
    //});

        $(".addForm").submit();
        //$.NotificationApp.send("Success", "Add your certificate successfully.", "top-center", "Background color", "Icon")

        //$(".addForm").on("submit", function (e) {
        //    var dataString = $(this).serialize();
        //    alert(dataString);
        //    $.ajax({
        //        type: "POST",
        //        url: "bin/process.php",
        //        data: dataString,
        //        success: function () {
        //            // Display message back to the user here
        //        }
        //    });

        //    e.preventDefault();
        //});
    } else {
        return false;
    }
    
    
    //$(".addForm").submit(function (e) {
    //    e.preventDefault();
    //});
    
   
}

function showFormModal(headerText, submitButtonText) {
    $('#compose-modal').modal('show');
    $('.title-add-form').html(headerText);
    $('#submitButton').html(submitButtonText)
}

function UpdateUserDetail(certId) {
    $('#compose-modal').modal('show');
    $('.title-add-form').html("Edit a certificate");
    $('#submitButton').html("Edit");
    $.ajax({
        type: "GET",
        traditional: true,
        async: false,
        cache: false,
        url: '/home/EditCertificate',
        context: document.body,
        data: { certId: certId },
        success: function (result) {
            console.log(result);
            
        },
        error: function (xhr) {
            //debugger;  
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}  