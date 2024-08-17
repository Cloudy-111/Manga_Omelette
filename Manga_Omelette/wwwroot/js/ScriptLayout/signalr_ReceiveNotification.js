import { connection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js';

export function ReceiveNotification() {
    connection.on("ReceiveNotification", async function (notification) {
        console.log(notification);
        var notification_section = $('.notification_section');

        var notificationItem = $('<div>').addClass('notification_item');

        var senderId = $('<p>').text(notification.senderId);
        var title = $('<p>').text(notification.title);

        notificationItem.append(senderId).append(title);

        notification_section.append(notificationItem);
    })
}