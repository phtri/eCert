// cache dom
var $shareBtn = $('.share-btn');
var $shareUrl = $('.share-url');
var $shareContainer = $('.share-container');
var $notificationButton = $('.notification-button');

// set data
var $url = 'https://selfnote.io/read/AHSUF8D93K76D0XJ';
var $shared = false;

/**
 *
 */
function shareLink(e) {

    // set active class
    $shareBtn.toggleClass('active');
    $shareUrl.toggleClass('active');
    $shareContainer.toggleClass('active');

    if ($shared === false) {

        // trigger notification alert
        $notificationButton.toggleClass('active');
        $shared = true;
        $shareBtn.text('Unshare');
        $shareUrl.text($url);

        var range = document.createRange();
        range.selectNode($(this).next()[0]);
        window.getSelection().addRange(range);

        try {
            // Now that we've selected the anchor text, execute the copy command
            var successful = document.execCommand('copy');
            var msg = successful ? 'successful' : 'unsuccessful';
            console.log('Copy email command was ' + msg);

        } catch (err) {

            console.log('Oops, unable to copy');

        }

        // Remove the selections - NOTE: Should use
        // removeRange(range) when it is supported
        window.getSelection().removeAllRanges();


    } else {
        $shared = false;
        $shareBtn.text('Share');
    }
}


function fadeOutNotification() {
    setTimeout(function () {
        $notificationButton.removeClass('active');
    }, 2000);
}

$(document).ready(function () {
    // bind events
    $shareBtn.on('click', shareLink);
    $notificationButton.on('transitionend', fadeOutNotification);
})
