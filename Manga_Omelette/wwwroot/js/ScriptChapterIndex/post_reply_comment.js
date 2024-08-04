import { connection } from './Comment_SignalR/hubConnection.js';
export function postReplyComment() {
	//Ajax for each Reply Comment
	$(document).on('submit', '.FormPostCommentReply', function (event) {
		event.preventDefault();

		var parentCommentId = $(this).data('parent-id');
		var user = $(`#usercommentReply_${parentCommentId}`).val();
		var content = $(`#textInputReply_${parentCommentId}`).val().trim();
		var userId = $(`#usercommentReply_${parentCommentId}`).val();

		var serializedData = $(this).serialize();
		var token = $('input[name="__RequestVerificationToken"]').val();
		//Need  Token CSRF to POST Form (SUPER IMPORTANT)

		$.ajax({
			url: '/postComment',
			type: 'post',
			data: serializedData + "&__RequestVerificationToken=" + token,
			success: function (response) {
				if (response.success) {
					$(`#FormPostCommentReply_${parentCommentId}`)[0].reset();
					$(`#FormPostCommentReply_${parentCommentId}`).hide();
					connection.invoke("SendReplyComment", user, content, userId, response.commentId, parentCommentId).catch(function (err) {
						return console.error(err.toString());
					})
				} else {
					$('#message').html('<div class="alert alert-danger">Failed to Post Comment in success</div>');
				}
			},
			error: function (xhr, status, error) {
				$('#message').html(`<div class="alert alert-danger">Failed to Post Comment ERROR: ${xhr.responseText}</div>`);
			}
		})
	})
}