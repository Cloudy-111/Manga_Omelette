export function postRating() {
    var rating_point = null;
    $('#form_rating button').on('click', function () {
        rating_point = $(this).val();
    })

    $('#form_rating').submit(function (event) {
        event.preventDefault();

        var userId = $('#hidden_attribute').data('user-id');
        var storyId = $('#hidden_attribute').data('story-id');
        var isRated = $('#hidden_attribute').data('rating');

        if (rating_point == -1) {
            $.ajax({
                url: '/delete_rate_story',
                type: 'post',
                data: { userId: userId, storyId: storyId },
                success: function() {
                    $('.form_rating').hide();
                },
                error: function (err) {

                }
            })
        } else {
            if (isRated != 0) {
                $.ajax({
                    url: '/edit_rate_story',
                    type: 'post',
                    data: { userId: userId, storyId: storyId, rating_point: rating_point },
                    success: function () {
                        $('.form_rating').hide();
                    },
                    error: function (err) {

                    }
                })
            } else {
                $.ajax({
                    url: '/rate_story',
                    type: 'post',
                    data: { userId: userId, storyId: storyId, rating_point: rating_point },
                    success: function () {
                        $('.form_rating').hide();
                    },
                    error: function (err) {

                    }
                })
            }
        }
        
    })
}