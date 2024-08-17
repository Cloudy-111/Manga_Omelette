import { ReceiveNotification } from './signalr_ReceiveNotification.js';
import { startConnection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js';
$(document).ready(function () {
    ReceiveNotification();
    startConnection();
})