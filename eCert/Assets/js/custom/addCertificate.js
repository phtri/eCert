$(document).ready(function(){
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
    alert("submit successfully");
    $(".addForm").submit();
}

function test() {
    alert("hello");
}