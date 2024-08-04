function loadReplies(event) {
	var button = $(event.target);
	let parentCommentId = button.data('parent-id');
	let lastReplyId = button.data('last-reply-id');
	let amountReplies = 5;
	$.ajax({
		url: `/chapter/${chapterId}/comment_replies`,
		data: { parentCommentId: parentCommentId, lastReplyId: lastReplyId, amount: amountReplies },
		success: function (data) {
			if (data.length > 0) {
				data.forEach(sub_comment => {
					var subComment = $('<div>').addClass('sub-comment').attr({ id: `sub-comment-${sub_comment.id}` });

					var comment_content = $('<div>').addClass("comment_content");
					var nameDisplay = $('<p>').addClass("nameDisplay").text(sub_comment.userNameDisplay);
					var create_date = $('<p>').addClass('create_date').text(new Date(sub_comment.createdDate).toLocaleString());
					var content = $('<p>').text(sub_comment.comment_content).css({
						'white-space': 'pre-wrap',
						'text-align': 'justify'
					});

					var optionBtn = $('<div>').addClass('option_btn');

					var userId = $('#hidden_attribute').data('user-id');

					//FIX THIS
					if (sub_comment.userId === userId) { // Assuming currentUserId is available
						optionBtn.append('<a class="option_btn_comment" id="btn_deletecmt" data-comment-id="' + sub_comment.id + '"><i class="bi bi-trash3"></i>Delete</a>');
						optionBtn.append('<a class="option_btn_comment" id="btn_editcmt" data-comment-id="' + sub_comment.id + '"><i class="bi bi-pencil-square"></i>Edit</a>');
					}

					comment_content.append(nameDisplay).append(create_date).append(content);
					subComment.append(comment_content);
					subComment.append(optionBtn);

					$('#comment_replies' + parentCommentId).append(subComment);
				});
				lastReplyId = data[data.length - 1].id;
				button.data('last-reply-id', lastReplyId);
			} else {
				$('#load-more-replies-comment' + parentCommentId).hide();
			}
		},
		error: function (error) {
			console.error("Error loading comments:", error);
		}
	})
}