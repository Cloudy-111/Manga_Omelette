import { connection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js';

export function AddToFollowList() {
	$("#FormAddToFollowList").submit(function (event) {
		event.preventDefault();

		var stotyId = $('#hidden_attribute').data('story-id');

		$.ajax({
			url: '/addToFollowList',
			type: 'POST',
			data: $(this).serialize(),
			success: function (response) {
				showMessage(response.message, response.success ? "alert-success" : "alert-danger");

				connection.invoke("AddToGroupFollow", stotyId).catch(function (err) {
					console.log(err.toString());
				});

				if (response.success) {
					$('#FormAddToFollowListContainer').hide();
					$('#FormRemoveFromListContainer').show();
				}
			},
			error: function () {
				$('#message').html('<div class="alert alert-danger">Failed to Add Story to Library!</div>');
			}
		})
	})
}

export function RemoveFollowList() {
	$('#FormRemoveFromList').submit(function (event) {
		event.preventDefault();

		$.ajax({
			url: '/removeFromFollowList',
			type: 'POST',
			data: $(this).serialize(),
			success: function (response) {
				showMessage(response.message, response.success ? "alert-success" : "alert-danger");

				connection.invoke("RemoveFromGroupFollow", $('#hidden_attribute').data('story-id')).catch(function (err) {
					return console.error(err.toString());
				});

				if (response.success) {
					$('#FormRemoveFromListContainer').hide();
					$('#FormAddToFollowListContainer').show();
				}
			},
			error: function () {
				$('#message').html('<div class="alert alert-danger">Failed to Remove Story from Library!</div>');
			}
		});
	});
}

export function IncreaseShare() {
	$('.share_btn').on('click', function (e) {
		var storyId = $('#hidden_attribute').data('story-id');
		const link = window.location.href;
		navigator.clipboard.writeText(link).catch(function (error) {
			$('#message').html('<div class="alert alert-danger">Failed to Copy to Clipboard!</div>');
		})
		$.ajax({
			url: `/share/${storyId}`,
			type: 'post',
			success: function (response) {
				showMessage(response.message, response.success ? "alert-success" : "alert-danger");
			},
			error: function () {

			}
		})
	})
}

function showMessage(message, alertType) {
	$('#message').removeClass().addClass('alert ' + alertType).text(message).fadeIn();

	setTimeout(function () {
		$('#message').fadeOut();
	}, 2000);
}