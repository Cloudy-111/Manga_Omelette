export function FilterSearch() {

    const urlParams = new URLSearchParams(window.location.search);
    const keyword = urlParams.get('keyword');

    if (keyword) {
        $('#search_bar').val(keyword); 
        performSearch(keyword); 
    }

    $(document).on('submit', '#filter_search', function (e) {
        e.preventDefault();

        var keyword = $('#search_bar').val();

        $.ajax({
            url: `/filter`,
            type: 'get',
            data: { keyword: keyword },
            success: function (response) {
                $('body').html(response);
                window.history.pushState({ keyword: keyword }, '', '/titles?keyword=' + encodeURIComponent(keyword));
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
            $('body').html(response);
            window.history.pushState({ keyword: keyword }, '', '/titles?keyword=' + encodeURIComponent(keyword));
        },
        error: function () {
            alert("Error Search");
        }
    });
}