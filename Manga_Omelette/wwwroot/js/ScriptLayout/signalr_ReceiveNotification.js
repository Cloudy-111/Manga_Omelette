﻿import { connection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js';
import { formatDate } from '../Ultilities/DateTimeConvert.js';


export function ReceiveNotification() {
    connection.on("ReceiveNotification", async function (notification) {

        const senderId = notification.senderId;
        const notificationId = notification.id;

        let senderName = senderId;
        let notificationType = notificationId;

        //Ajax to call NameDisplay of Sender
        let getUserNamePromise = $.ajax({
            url: '/getUserName',
            type: 'get',
            data: { userId: senderId },
        });

        //Ajax to call NotificationType
        let getNotificationTypePromise = $.ajax({
            url: `/getTypeNotification/${notificationId}`,
            type: 'get',
            data: { notificationId: notificationId }
        });


        Promise.all([getUserNamePromise, getNotificationTypePromise]).then(function (response) {
            let userNameResponse = response[0];
            let notificationTypeResponse = response[1];

            senderName = userNameResponse.userName;
            notificationType = notificationTypeResponse.type;

            var notification_section = $('.notification_section');
      
            // Check the number of notifications, remove the oldest if there are already 2
            if (notification_section.children('.notification_item').length >= 2) {
                notification_section.children('.notification_item').last().addClass('hidden');
                setTimeout(function () {
                    notification_section.children('.notification_item').last().remove();
                }, 500);
            }

            var notificationItem = $('<div>').addClass('notification_item');

            var notificationHeader = $('<div>').addClass('notification_header');
            var notificationTitle = $('<div>').addClass('notification_title');
            var title = $('<p>').text(notification.title).addClass('notification_type');
            notificationTitle.append(title);

            var name_and_type = $('<div>').addClass('name_and_type');
            var from = $('<p>').text(`from ${senderName} - ${notificationType} Notification`);
            name_and_type.append(from);

            var notificationContent = $('<div>').addClass('notification_content');
            var content = $('<p>').text(notification.content).addClass('notification_content');
            notificationContent.append(content);

            notificationHeader.append(notificationTitle).append(name_and_type);
            notificationItem.append(notificationHeader).append(content);

            notification_section.prepend(notificationItem);

            var signalNotiBtn = $('#notiBtn .noti-dot');
            signalNotiBtn.addClass('active');

            addNotificationToList(notification, $('#new_notis'));

            setTimeout(function () {
                notificationItem.addClass('hidden');
                setTimeout(function () {
                    notificationItem.remove();
                }, 500);
            }, 5000);
        })
        
    })
}

function addNotificationToList(notifications, container) {
    let noti_item = $('<div>').addClass("notification_item_popup");
    let noti_item_col = $('<div>').addClass("notification_item_col");
    let noti_header = $('<div>').addClass("notification_header_popup");
    let noti_title = $('<div>').addClass("notification_title_popup").text(notifications.title);
    let noti_id = $('<div>').addClass("notification_id").text(notifications.id).hide();
    let noti_story_id = $('<div>').addClass("notification_story_id").text(notifications.storyId).hide();

    let noti_content = $('<div>').addClass("notification_content_popup").text(notifications.content);
    let noti_datetime = $('<div>').addClass("notification_time_popup").text(formatDate(notifications.createDate));

    noti_header.append(noti_title).append(noti_id).append(noti_story_id);
    noti_item_col.append(noti_header).append(noti_content).append(noti_datetime);
    noti_item.append(noti_item_col);

    container.append(noti_item);
}