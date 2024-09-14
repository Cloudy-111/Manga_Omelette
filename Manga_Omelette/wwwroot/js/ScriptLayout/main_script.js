﻿import { ReceiveNotification } from './signalr_ReceiveNotification.js';
import { startConnection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js';
import { PopupNotification } from './script_popup_notification_on_icon.js';
import { MarkAllAsRead } from './script_mark_all_read.js'
$(document).ready(function () {
    ReceiveNotification();
    startConnection();
    PopupNotification();
    MarkAllAsRead();
})