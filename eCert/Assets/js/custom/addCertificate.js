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