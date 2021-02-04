const certLinkOption = 1;
const certFileOption = 2;

$(document).ready(function () {
    //auto check radio certificate link
    $("#customRadio1").prop("checked", true)

    //default show input certificate link
    $(".certificate_file").hide();
    $(".certificate_link").show();

    //hide warining message
    clearWarningMsg();
});
function clearMsgCertName() {
    $(".cert_name").hide();
}
function clearMsgCertLink() {
    $(".cert_link").hide();
}
function clearMsgCertFile() {
    $(".cert_file").hide();
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
function clearWarningMsg() {
    clearMsgCertName();
    clearMsgCertLink();
    clearMsgCertFile(); 
}
function validateAddcertificate() { 
    let resultcertname = validateCertName();
    if ($('input[name="customRadio"]:checked').val() == 1) {
        let resutcertlink = validateCertLink();
        if (resultcertname && resutcertlink) {
            return true;
        } else {
            return false;
        }
    } else if ($('input[name="customRadio"]:checked').val() == 2) {
        let resutcertfile = validateCertFile();
        if (resultcertname && resutcertfile) {
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
    //$.NotificationApp.send("Success", "Add your certificate successfully.", "top-center", "Background color", "Icon")
    
    //$(".addForm").submit(function (e) {
    //    e.preventDefault();
    //});
    
   
}

function test1() {
    alert("hi");
}

