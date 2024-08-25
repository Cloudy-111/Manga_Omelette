import { postRating } from './script_post_rating.js';
import { AddToFollowList, RemoveFollowList, IncreaseShare } from './scriptAddAndRemoveFollowList.js'
import { startConnection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js'
$(document).ready(function () {
    postRating();
    AddToFollowList();
    RemoveFollowList();
    IncreaseShare();
    startConnection();
})