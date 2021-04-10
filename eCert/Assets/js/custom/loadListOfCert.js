function loadListCert(pageNumber) {
    var listCert = $(".listCertificate");
    $.ajax({
        type: "POST",
        url: '/Certificate/LoadListOfCert',
        context: document.body,
        data: { pageNumber: pageNumber },
        dataType: "html",
        beforeSend: function () {
            $("#loading-overlay").show();
        },
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //console.log(result);
            listCert.html(result);
            $("#loading-overlay").hide();
        },
        error: function (req, err) {
            //debugger;  
            console.log(err);
            alert("Error has occurred..");
        }
    });
}