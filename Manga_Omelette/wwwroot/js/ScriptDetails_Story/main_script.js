import { postRating } from './script_post_rating.js';
import { AddToFollowList, RemoveFollowList } from './scriptAddAndRemoveFollowList.js'
$(document).ready(function () {
    postRating();
    AddToFollowList();
    RemoveFollowList();
})