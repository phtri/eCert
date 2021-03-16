$(function () {
    $('#dropArea').filedrop({
        url: '@Url.Action("UploadFiles")',
        allowedfiletypes: ['image/jpeg', 'image/png', 'image/gif'],
        allowedfileextensions: ['.jpg', '.jpeg', '.png', '.gif'],
        paramname: 'files',
        maxfiles: 5,
        maxfilesize: 5, // in MB
        dragOver: function () {
            $('#dropArea').addClass('active-drop');
        },
        dragLeave: function () {
            $('#dropArea').removeClass('active-drop');
        },
        drop: function () {
            $('#dropArea').removeClass('active-drop');
        },
        afterAll: function (e) {
            $('#dropArea').html('file(s) uploaded successfully');
        },
        uploadFinished: function (i, file, response, time) {
            $('#uploadList').append('<li class="list-group-item">' + file.name + '</li>')
        }
    })
})