export function MarkAllAsRead() {
    var MarkBtn = $('#mark_read_btn');

    MarkBtn.on('click', function (e) {
        e.preventDefault();

        $.ajax({
            url: '/markAllAsRead',
            type: 'post',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function () {
                $('#admin_notis .notification_item_popup').each(function() {
                    var $notificationItem = $(this);
                    var $signUnread = $notificationItem.find('.notification_sign_unread');
                    var $overlay = $notificationItem.find('.notification_overlay');

                    if ($signUnread.hasClass('active')) {
                        $overlay.addClass('active');
                        $signUnread.removeClass('active');
                    }
                })
            },
            error: function () {
                alert('Cannot Mark All Notification as Read');
            }
        })
    })
}