import { connection } from './Comment_SignalR/hubConnection.js';

export function postOriginComment() {
	var isPosting = false;
	$('#FormPostComment').submit(function (event) {
		event.preventDefault();

		if (isPosting) {
			alert("Please wait a moment before posting another comment.");
			return;
		}

		isPosting = true;

		var user = $('#usercomment').val();
		var content = $('#textInput').val().trim();
		var userId = $('#usercomment').val();


		$.ajax({
			url: '/postComment',
			type: 'post',
			data: $(this).serialize(),
			success: function (response) {
				if (response.success) {
					// $('#message').html('<div class="alert alert-success">Post Comment Successfully!</div>');
					$('#FormPostComment')[0].reset();
					addCommentInList(user, content, userId, response.commentId);
				} else {
					$('#message').html('<div class="alert alert-danger">Failed to Post Comment in success</div>');
				}
			},
			error: function () {
				$('#message').html('<div class="alert alert-danger">Failed to Post Comment</div>');
			},// Wait 5s for post next comment
			complete: function () {
				setTimeout(function () {
					isPosting = false;
				}, 5000);
			}
		})
	})
}

function addCommentInList(user, content, userId, commentId) {
	//Take 4 paragram: user, Content, userId, commentId
	connection.invoke("SendComment", user, content, userId, commentId).catch(function (err) {
		return console.error(err.toString());
	});
}