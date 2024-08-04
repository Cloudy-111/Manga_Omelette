//Script Click load more comment, load 10 comments each click
let chapterId = $('#hidden_attribute').data('chapter-id');
let lastCommentId = 0;
let amount = 10;

function loadMoreComments() {
	$.ajax({
		url: `/chapter/${chapterId}/comments`, //Configure URL in Program
		data: { chapterId: chapterId, lastCommentId: lastCommentId, amount: amount },
		success: function (data) {
			if (data.length > 0) {
				data.forEach(comment => {
					//Config this Comment Item

					var comment_item = $('<div>').addClass("comment_item").attr({ id: `comment_item_${comment.id}` });
					var comment_origin = $('<div>').addClass("comment_origin");

					var comment_content = $('<div>').addClass("comment_content");
					var nameDisplay = $('<p>').addClass("nameDisplay").text(comment.userNameDisplay);
					var create_date = $('<p>').addClass('create_date').text(new Date(comment.createdDate).toLocaleString());
					var content = $('<p>').text(comment.comment_content).css({
						'white-space': 'pre-wrap',
						'text-align': 'justify'
					});

					var userId = $('#hidden_attribute').data('user-id');
					var storyId = $('#hidden_attribute').data('story-id');
					var chapterId = $('#hidden_attribute').data('chapter-id');

					// Option buttons logic
					var optionBtn = $('<div>').addClass('option_btn');

					//FIX THIS
					if (comment.userId === userId) { // Assuming currentUserId is available
						optionBtn.append('<a class="option_btn_comment" id="btn_deletecmt" data-comment-id="' + comment.id + '"><i class="bi bi-trash3"></i>Delete</a>');
						optionBtn.append('<a class="option_btn_comment" id="btn_editcmt" data-comment-id="' + comment.id + '"><i class="bi bi-pencil-square"></i>Edit</a>');
					}

					if (userId) { // Assuming isSignedIn is available
						optionBtn.append('<button class="option_btn_comment reply_btn"><i class="bi bi-reply"></i>Reply</button>');
						var replyForm = $('<form>')
							.addClass('FormPostCommentReply')
							.attr('id', 'FormPostCommentReply_' + comment.id)
							.attr('method', 'post')
							.css('display', 'none')
							.attr('data-parent-id', comment.id);
						replyForm.append(`<textarea name="Content" placeholder="Comment Here..." id="textInputReply_${comment.id}"></textarea>`);
						replyForm.append(`<input type="hidden" name="ChapterId" value="${chapterId}" />`);
						replyForm.append(`<input type="hidden" name="StoryId" value="${storyId}" />`);
						replyForm.append(`<input type="hidden" name="UserId" value="${userId}" id="usercommentReply_${comment.id}" />`);
						replyForm.append(`<input type="hidden" name="ParentCommentId" value=${comment.id} id="parentCommentId_${comment.id}" />`);
						replyForm.append('<button type="submit">Post your Reply</button>');
						optionBtn.append(replyForm);
					}

					var comment_replies = $('<div>').addClass('comment_replies').attr({ id: 'comment_replies' + comment.id });

					var btn_load_more_replies = $('<button>')
						.addClass('load-more-replies-comment')
						.attr({ id: 'load-more-replies-comment' + comment.id, 'data-parent-id': comment.id, 'data-last-reply-id': comment.id })
						.text('Load More Replies')
						.click(loadReplies);

					comment_content.append(nameDisplay).append(create_date).append(content);
					comment_origin.append(comment_content);
					comment_origin.append(optionBtn);
					comment_item.append(comment_origin);
					comment_item.append(comment_replies);
					if (comment.reply_amount > 0) comment_item.append(btn_load_more_replies);

					$('#comment_list').append(comment_item);
				});
				//Change lastCommentId to not Mutiple comment
				lastCommentId = data[data.length - 1].id;
			} else {
				$('#load-more-comments').hide();
			}
		},
		error: function (error) {
			console.error("Error loading comments:", error);
		}
	})
}