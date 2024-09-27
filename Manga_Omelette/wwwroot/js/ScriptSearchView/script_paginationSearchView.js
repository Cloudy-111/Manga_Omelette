export function Pagination() {
    $(document).on('click', '.pagination_page', function (e) {
        e.preventDefault();
        ClickPage("page", $(this));
    })
    $(document).on('click', '#nextBtn', function (e) {
        e.preventDefault();
        ClickPage("page_next", $(this));
    })
    $(document).on('click', '#previousBtn', function (e) {
        e.preventDefault();
        ClickPage("page_previous", $(this));
    })
};

function ClickPage(typePage, element) {
    var page;
    var URLParams = new URLSearchParams(window.location.search);
    var totalPages = parseInt($('#total_page').text());
    var currentPage = parseInt(URLParams.get('page')) || 1;
    if (typePage === "page") {
        page = parseInt(element.attr('id').slice(5));
    } else if (typePage === "page_next") {
        page = currentPage >= totalPages ? currentPage : currentPage + 1;
    } else {
        page = currentPage <= 1 ? currentPage : currentPage - 1;
    };

    if (page === currentPage) return;

    //console.log(page);
    var filterData = $('#filter_search').serializeArray()
        .filter(function (item) {
            return item.value.trim() !== "";
        })
        .map(function (item) {
            return `${encodeURIComponent(item.name)}=${encodeURIComponent(item.value)}`;
        }).join('&');

    var requestData = filterData ? filterData + `&page=${page}` : `page=${page}`;

    $.ajax({
        url: '/filter',
        data: requestData,
        type: 'get',
        success: function (response) {
            console.log(requestData);
            $('.story_grid').html(response);
            var newUrl = filterData ? `/titles?page=${page}&${filterData}` : `/titles?page=${page}`;
            window.history.replaceState({ page: page }, '', newUrl);
        },
        error: function (err) {
            alert("Error Search");
        }
    })
}