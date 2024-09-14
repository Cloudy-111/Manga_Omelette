export function HrefNotificationItem() {
    var notificationitem = $('.notification_item_popup');

    notificationitem.on("click", function (e) {
        e.preventDefault();

        var $notificationItem = $(this);
        var notificationId = $(this).find(".notification_id").text();

        $.ajax({
            url: '/markAsReadSingleNotification',
            data: { notificationId: notificationId },
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            type: 'post',
            success: function () {
                //Cannot use $(this) in callback function success, because $(this) is not reference an element HTML, it reference AJAX request
                var notification_storyId = $notificationItem.find('.notification_story_id').text();

                if ($notificationItem.closest('#system_notis').length > 0) {
                    var notificationId = $notificationItem.find('.notification_id').text();

                    var baseUrl = '/Notifications/Details';

                    window.location.href = baseUrl + '/' + notificationId;
                } else if ($notificationItem.closest('#admin_notis').length > 0) {
                    var baseUrl = '/titles';

                    window.location.href = baseUrl + '/' + notification_storyId;
                }
            },
            error: function () {
                console.log('Failed to mark notification as read');
            }
        })

        
    })
}