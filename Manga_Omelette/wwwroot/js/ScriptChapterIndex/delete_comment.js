import { connection } from './Comment_SignalR/hubConnection.js';

export function deleteComment() {
	//Button Delete Comment of each user
	var commentId;
	$(document).on('click', '#btn_deletecmt', function () {
		$('#delete_cmt_popup').show();
		commentId = $(this).data('comment-id');
	})

	$('#confirm-yes').on('click', function () {
		// var commentId = $(this).data('comment-id');

		$.ajax({
			url: '/deleteComment',
			type: 'post',
			data: { commentId: commentId },
			success: function (response) {
				if (response.success) {
					connection.invoke("DeleteComment", commentId).catch(function (err) {
						return console.error(err.toString());
					});
				} else {
					alert(response.message);
				}
			}
		})
		$('#delete_cmt_popup').hide();
	})

	$('#confirm-no').on('click', function () {
		$('#delete_cmt_popup').hide();
	})
}