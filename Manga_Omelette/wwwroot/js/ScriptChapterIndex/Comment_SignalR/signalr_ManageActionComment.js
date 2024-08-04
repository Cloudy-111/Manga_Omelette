import { startConnection, connection } from './hubConnection.js';
import { setupReceiveComment } from './signalr_ReceiveComment.js';
import { setupReceiveReplyComment } from './signalr_ReceiveReplyComment.js';
import { setupReceiveDeleteComment } from './signalr_ReceiveDeleteComment.js';

$(document).ready(function () {
    setupReceiveComment();
    setupReceiveReplyComment();
    setupReceiveDeleteComment();
    startConnection();
})