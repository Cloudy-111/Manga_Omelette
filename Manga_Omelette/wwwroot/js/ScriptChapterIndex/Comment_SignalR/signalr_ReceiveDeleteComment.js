import { connection } from './hubConnection.js';

export function setupReceiveDeleteComment() {
	//Connection for all Client receive Delete comment from 1 Client
	connection.on("ReceiveDeletedComment", function (commentId) {
		var commentItem = $(`#comment_item_${commentId}`);
		if (commentItem.length > 0) {
			var replies = commentItem.find('.comment_replies .sub-comment');
			if (replies.length > 0) {
				commentItem.find('.comment_origin .content').html('<p style="white-space: pre-wrap; text-align: justify;">This Comment has been Deleted!</p>')
				commentItem.find('.comment_origin .option_btn #btn_deletecmt').remove();
				commentItem.find('.comment_origin .option_btn #btn_editcmt').remove();
			} else {
				commentItem.remove();
			}
		} else {
			var subComment = $(`#sub-comment-${commentId}`);
			subComment.remove();
		}
	});
}