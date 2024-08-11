import { postRating } from './script_post_rating.js';
import { AddToFollowList, RemoveFollowList, IncreaseShare } from './scriptAddAndRemoveFollowList.js'
$(document).ready(function () {
    postRating();
    AddToFollowList();
    RemoveFollowList();
    IncreaseShare();
})