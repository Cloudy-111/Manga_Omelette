import { connection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js'

export function postCreateChapter() {
    $('#form_create_chapter').on('submit', function (e) {
        e.preventDefault();

        var token = $('input[name="__RequestVerificationToken"]').val();

        var formData = new FormData(this);
        formData.append('__RequestVerificationToken', token);

        var storyId = $('#hidden_attribute').data('story-id');

        $.ajax({
            url: `/admin/${storyId}/create_chapter`,
            data: formData,
            type: 'post',
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    connection.invoke("SendNotificationForFollow", response.notification, storyId).catch(function (err) {
                        return console.error(err.toString());
                    });
                    window.location.href = response.redirectUrl;
                }
            },
            error: function (err) {
                console.log("ERROR:" + err);
            }
        })

    })
}