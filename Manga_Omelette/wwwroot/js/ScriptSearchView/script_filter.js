export function FilterSearch() {

    const urlParams = new URLSearchParams(window.location.search);
    const keyword = urlParams.get('keyword');

    if (keyword) {
        $('#search_bar').val(keyword); 
        performSearch(keyword); 
    }

    $(document).off('submit', '#filter_search').on('submit', '#filter_search', function (e) {
        e.preventDefault();

        var keyword = $('#search_bar').val();

        $.ajax({
            url: `/filter`,
            type: 'get',
            data: { keyword: keyword },
            success: function (response) {
                $('.story_grid').html(response);
                window.history.replaceState({ keyword: keyword }, '', '/titles?keyword=' + encodeURIComponent(keyword));
            },
            error: function () {
                alert("Error Search");
            }
        });
    });
}

function performSearch(keyword) {
    $.ajax({
        url: `/filter?keyword=${keyword}`,
        type: 'get',
        success: function (response) {
            $('.story_grid').html(response);
            window.history.replaceState({ keyword: keyword }, '', '/titles?keyword=' + encodeURIComponent(keyword));
        },
        error: function () {
            alert("Error Search");
        }
    });
}