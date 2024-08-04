import { connection } from './hubConnection.js';

export function setupReceiveReplyComment() {
	//SignalR for Receive Reply Comment
	connection.on("ReceiveReplyComment", async function (user, content, userId, commentId, parentId) {
		$.ajax({
			url: '/getUserName',
			type: 'get',
			data: { userId: userId },
			success: function (response) {
				var userNameDisplay = response.userName;

				var subComment = $('<div>').addClass('sub-comment');
				var comment_content = $('<div>').addClass('comment_content');
				var nameDisplay = $('<p>').addClass('nameDisplay').text(userNameDisplay);
				var create_date = $('<p>').addClass('create_date').text(new Date().toLocaleString());
				var subComment_content = $('<p>').text(content).css({
					'white-space': 'pre-wrap',
					'text-align': 'justify'
				});

				var optionBtn = $('<div>').addClass('option_btn');

				var this_userId = $('#hidden_attribute').data('user-id');

				if (this_userId === userId) { // Assuming currentUserId is available
					optionBtn.append('<a class="option_btn_comment" id="btn_deletecmt" data-comment-id="' + commentId + '"><i class="bi bi-trash3"></i>Delete</a>');
					optionBtn.append('<a class="option_btn_comment" id="btn_editcmt" data-comment-id="' + commentId + '"><i class="bi bi-pencil-square"></i>Edit</a>');
				}

				comment_content.append(nameDisplay).append(create_date).append(subComment_content);
				subComment.append(comment_content);
				subComment.append(optionBtn);

				$(`.comment_item#comment_item_${parentId} .comment_replies`).append(subComment);
			}
		})
	});
}