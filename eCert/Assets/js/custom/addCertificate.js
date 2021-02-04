$(document).ready(function () {
    $("#customRadio1").prop("checked", true)
    $(".certificate_file").hide();
    $(".certificate_link").show();
});
function handleClickRadio(myRadio) {
    if(myRadio.value == 1){
        $(".certificate_file").hide();
        $(".certificate_link").show();
    }else if(myRadio.value == 2){
        $(".certificate_link").hide();
        $(".certificate_file").show();
    }
}

function submitCertificate() {
    if ($('input[name="customRadio"]:checked').val() == 1 && !$("#Content").val()) {
        alert("ecert link is empty");
        return false;
    }
    if ($('input[name="customRadio"]:checked').val() == 2 && $("#CertificateFile").val() == "") {
        alert("empty file");
        return false;
    }
    $.NotificationApp.send("Success", "Add your certificate successfully.", "top-center", "Background color", "Icon")
    
    //$(".addForm").submit(function (e) {
    //    e.preventDefault();
    //});
    
    $('#compose-modal').modal('hide');
    $(".addForm").submit();
}

function test1() {
    $(".certificate_link").prop("checked", true)
}