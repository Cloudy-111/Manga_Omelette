export function ClickUserBtn() {
    var userBtn = $('#userBtn');
    var overlay = $('#overlay_user');
    var profile_container = $('#profile_container');
    userBtn.on("click", function () {
        overlay.toggleClass("appear");
        profile_container.toggleClass("appear");
    });

    overlay.on("click", function () {
        overlay.removeClass("appear");
        profile_container.removeClass("appear");
    })
}