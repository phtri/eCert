$(document).ready(function() {
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    $(".dropdown-menu li").click(function () {
        var selText = $(this).text();
        $(this).find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
    });

    $('.main-org').on('change', function () {
        if ($(".main-org option:selected").text() === "Đại học FPT") {
            $('.sub-org').css('display', '');
        }
        else {
            $('.sub-org').css('display', 'none');
        }
    })
})

