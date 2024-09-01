export function PopupNotification() {

    var notiBtn = $('#notiBtn');
    var notification_popup_container = $('#notification_popup_container');
    var overlay = $('#overlay');
    notiBtn.on('click', function (e) {
        e.preventDefault();
        overlay.toggleClass("appear");
        notification_popup_container.toggleClass("appear");
    })

    overlay.on('click', function () {
        overlay.removeClass("appear");
        notification_popup_container.removeClass("appear");
    })
}