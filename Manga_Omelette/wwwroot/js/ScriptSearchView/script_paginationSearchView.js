import sharedData from './shared_data.js'; // dùng export default thì không cần dùng {}

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

    var localRequestData = { ...sharedData.requestData };
    localRequestData.page = page;

    var filteredRequestData = Object.fromEntries( 
        Object.entries(localRequestData).filter(([key, value]) => { 
            if (Array.isArray(value)) {
                return value.length > 0;
            }
            return value;
        })
    );

    var queryParams = $.param(filteredRequestData, true);

    // Update URL on Browser
    if (queryParams) {
        var newUrl = `${window.location.pathname}?${queryParams}`;
        history.pushState({ filteredRequestData }, '', newUrl);
    }

    $.ajax({
        url: `/filter`,
        type: 'get',
        data: filteredRequestData,
        traditional: true,
        success: function (response) {
            $('.story_grid').html(response);
        },
        error: function () {
            alert("Error Search");
        }
    });
}