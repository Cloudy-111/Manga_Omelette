import { connection } from './hubConnection.js';

export function setupReceiveComment() {
	//Connection for all Client receive message from 1 Client
	//Ajax is for take UserNameDisplay by GET, because element of ASP.NET core Blazor not available in Javascript
	connection.on("ReceiveComment", async function (user, message, userId, commentId) {
		$.ajax({
			url: '/getUserName',
			type: 'get',
			data: { userId: userId },
			success: function (response) {
				var userNameDisplay = response.userName;

				var comment_item = $('<div>').addClass('comment_item').attr({ id: `comment_item_${commentId}` });
				var comment_origin = $('<div>').addClass('comment_origin');

				var comment_content = $('<div>').addClass('comment_content');
				var nameDisplay = $('<p>').addClass('nameDisplay').text(userNameDisplay);
				var create_date = $('<p>').addClass('create_date').text(new Date().toLocaleString());
				var content = $('<p>').text(message).css({
					'white-space': 'pre-wrap',
					'text-align': 'justify'
				});

				var this_userId = $('#hidden_attribute').data('user-id');
				var storyId = $('#hidden_attribute').data('story-id');
				var chapterId = $('#hidden_attribute').data('chapter-id');

				var optionBtn = $('<div>').addClass('option_btn');

				if (this_userId === userId) {
					optionBtn.append('<a class="option_btn_comment" id="btn_deletecmt" data-comment-id="' + commentId + '"><i class="bi bi-trash3"></i>Delete</a>');
					optionBtn.append('<a class="option_btn_comment" id="btn_editcmt" data-comment-id="' + commentId + '"><i class="bi bi-pencil-square"></i>Edit</a>');
				}

				if (userId) { // Assuming isSignedIn is available
					optionBtn.append('<button class="option_btn_comment reply_btn"><i class="bi bi-reply"></i>Reply</button>');
					var replyForm = $('<form>')
						.addClass('FormPostCommentReply')
						.attr('id', 'FormPostCommentReply_' + commentId)
						.attr('method', 'post')
						.css('display', 'none')
						.attr('data-parent-id', commentId);
					replyForm.append(`<textarea name="Content" placeholder="Comment Here..." id="textInputReply_${commentId}"></textarea>`);
					replyForm.append(`<input type="hidden" name="ChapterId" value="${chapterId}" />`);
					replyForm.append(`<input type="hidden" name="StoryId" value="${storyId}" />`);
					replyForm.append(`<input type="hidden" name="UserId" value="${userId}" id="usercommentReply_${commentId}" />`);
					replyForm.append(`<input type="hidden" name="ParentCommentId" value=${commentId} id="parentCommentId_${commentId}" />`);
					replyForm.append('<button type="submit">Post your Reply</button>');
					optionBtn.append(replyForm);
				}

				comment_content.append(nameDisplay).append(create_date).append(content);
				comment_origin.append(comment_content);
				comment_origin.append(optionBtn);
				comment_item.append(comment_origin);

				$('#comment_list').append(comment_item);

			}
		})
	});
}