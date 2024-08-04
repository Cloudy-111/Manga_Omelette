import { deleteComment } from './delete_comment.js';
import { postOriginComment } from './post_origin_comment.js';
import { postReplyComment } from './post_reply_comment.js';
$(document).ready(function(){
	deleteComment();
	postOriginComment();
	postReplyComment();
})