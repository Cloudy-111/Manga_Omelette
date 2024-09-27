export function ClearButton() {
    $(document).on('keyup', '#search_bar', function () {
        var keyword = $(this).val();
        if (keyword.length > 0) {
            $('.clear_icon').show();
        } else {
            $('.clear_icon').hide();
        }
    })
    $(document).on('click', '.clear_icon', function () {
        $('#search_bar').val('');
    })
}