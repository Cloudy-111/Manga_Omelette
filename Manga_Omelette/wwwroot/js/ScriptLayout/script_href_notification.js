export function HrefNotificationItem() {
    var notificationitem = $('.notification_item_popup');

    notificationitem.on("click", function () {
        var notification_storyId = $(this).find('.notification_story_id').text();

        if ($(this).closest('#system_notis').length > 0) {
            var notificationId = $(this).find('.notification_id').text();

            var baseUrl = '/Notifications/Details';

            window.location.href = baseUrl + '/' + notificationId;
        } else if ($(this).closest('#admin_notis').length > 0) {
            var baseUrl = '/titles';

            window.location.href = baseUrl + '/' + notification_storyId;
        }
    })
}