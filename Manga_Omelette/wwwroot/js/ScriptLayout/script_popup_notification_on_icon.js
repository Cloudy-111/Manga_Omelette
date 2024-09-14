import { formatDate } from '../Ultilities/DateTimeConvert.js';
import { HrefNotificationItem } from './script_href_notification.js';

export function PopupNotification() {

    var notiBtn = $('#notiBtn');
    var notiBtnDot = $('#notiBtn .noti-dot');
    var notification_popup_container = $('#notification_popup_container');
    var overlay = $('#overlay');
    var loadNoti = {};
    notiBtn.on('click', function (e) {
        e.preventDefault();

        notiBtnDot.removeClass('active');
        if (loadNoti["System"] || loadNoti["Admin"]) {
            $("#system_notis").html(loadNoti["System"]);
            $("#admin_notis").html(loadNoti["Admin"]);
            overlay.toggleClass("appear");
            notification_popup_container.toggleClass("appear");
            return;
        }
        
        $.ajax({
            url: '/getNotificationHeader',
            type: 'get',
            success: function (data) {
                var system_notis = $('#system_notis');
                var system_notiHTML = '';
                addNotificationToList(data.system_notification, system_notis);
                system_notiHTML = system_notis.prop("outerHTML");
                loadNoti["System"] = system_notiHTML;

                var admin_notis = $('#admin_notis');
                var admin_notiHTML = '';
                addNotificationToList(data.admin_notification, admin_notis);
                admin_notiHTML = admin_notis.prop("outerHTML");
                loadNoti["Admin"] = admin_notiHTML;

                HrefNotificationItem();

                overlay.toggleClass("appear");
                notification_popup_container.toggleClass("appear");
            },
            error: function (err) {
                alert("Failed to Get Notifications");
            }
        })
    })

    overlay.on('click', function () {
        overlay.removeClass("appear");
        notification_popup_container.removeClass("appear");
    })
}

function addNotificationToList(notifications, container) {
    notifications.forEach(noti => {
        let noti_item = $('<div>').addClass("notification_item_popup");
        let noti_item_col = $('<div>').addClass("notification_item_col");
        let noti_header = $('<div>').addClass("notification_header_popup");
        let noti_title = $('<div>').addClass("notification_title_popup").text(noti.title);
        let noti_id = $('<div>').addClass("notification_id").text(noti.id).hide();
        let noti_story_id = $('<div>').addClass("notification_story_id").text(noti.storyId).hide();

        let noti_content = $('<div>').addClass("notification_content_popup").text(noti.content);
        let noti_datetime = $('<div>').addClass("notification_time_popup").text(formatDate(noti.createDate));

        noti_header.append(noti_title).append(noti_id).append(noti_story_id);
        noti_item_col.append(noti_header).append(noti_content).append(noti_datetime);
        noti_item.append(noti_item_col);

        let isRead = noti.isRead;

        let noti_sign_unread = $('<i>').addClass("bi bi-dot notification_sign_unread");
        let noti_overlay = $('<div>').addClass("notification_overlay");

        if (isRead === false) {
            noti_sign_unread.addClass('active');
        } else if (isRead === true) {
            noti_overlay.addClass('active');
        }

        noti_item.append(noti_sign_unread);
        noti_item.append(noti_overlay);

        container.append(noti_item);
    });
}