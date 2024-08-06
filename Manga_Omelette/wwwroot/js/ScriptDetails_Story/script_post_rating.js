export function postRating() {
    var rating_point = null;
    $('#form_rating button').on('click', function () {
        rating_point = $(this).val();
    })

    $(document).on('click', '#remove_rate', function (event) {
        var userId = $('#hidden_attribute').data('user-id');
        var storyId = $('#hidden_attribute').data('story-id');
        event.preventDefault();
        $.ajax({
            url: `/api/delete_rate_story/${userId}/${storyId}`,
            type: 'delete',
            success: function () {
                $('.form_rating').hide();
                $('.rating_btn').removeClass('rated').html(`<i class="bi bi-star"></i>`);;
                $('#hidden_attribute').data('rating', 0);
                $('#remove_rate').remove();
            },
            error: function (err) {
                    
            }
        })
    })
    $('#form_rating').submit(function (event) {
        event.preventDefault();

        var userId = $('#hidden_attribute').data('user-id');
        var storyId = $('#hidden_attribute').data('story-id');
        var isRated = $('#hidden_attribute').data('rating');

        const url = isRated !== 0 ? '/edit_rate_story' : '/rate_story';
        $.ajax({
            url: url,
            type: 'post',
            data: { userId: userId, storyId: storyId, rating_point: rating_point },
            success: function () {
                $('.form_rating').hide();
                if (isRated === 0) {
                    $('.rating_btn').addClass('rated');
                    $('#form_rating').append('<button type="button" id="remove_rate">Remove Rate</button>');
                } 
                $('.rating_btn.rated').html(`<i class="bi bi-star"></i> ${rating_point}`);
                isRated = 1;
                $('#hidden_attribute').data('rating', 1);
            },
            error: function (err) {

            }
        })
    })
    
}