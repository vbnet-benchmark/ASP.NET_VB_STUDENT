document.addEventListener('DOMContentLoaded', function () {
    /* global RvVanillaModal */
    'use strict';
    var modal = new RvVanillaModal({
        //showOverlay: true
    });

    // each method
    modal.each(function (element) {
        var target = element.getAttribute('data-rv-vanilla-modal');
        var targetElement = document.querySelector(target);
        var closeBtn = targetElement.querySelector(modal.settings.closeSelector);

        // close click listerner
        closeBtn.addEventListener('click', function (event) {
            event.preventDefault();
            modal.close(targetElement);
        });

        closeBtn.addEventListener('OK', function (event) {
            //event.preventDefault();
            alert("You clicked OK");
        });

        // open click listerner
        element.addEventListener('click', function (event) {
            event.preventDefault();
            modal.open(targetElement);
        });
    });
}, false);