function loadListCert(pageNumber) {
    var listCert = $(".listCertificate");
    $.ajax({
        type: "POST",
        url: '/Certificate/LoadListOfCert',
        context: document.body,
        data: { pageNumber: pageNumber },
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