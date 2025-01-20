import sharedData from './shared_data.js';
export function FilterSearch() {

    const urlParams = new URLSearchParams(window.location.search);
    const keyword = urlParams.get('keyword');

    if (keyword) {
        $('#search_bar').val(keyword); 
        performSearch(keyword); 
    }

    clickSelectBox();

    const includeGenres = [];
    const excludeGenres = [];

    selectGenre(includeGenres, excludeGenres);

    var requestData = {
        keyword: keyword,
        IncludeGenres: includeGenres,
        ExcludeGenres: excludeGenres,
    }


    $(document).off('click', '#search_button').on('click', '#search_button', function (e) {
        requestData.keyword = $('#search_bar').val(); 
        sharedData.requestData = requestData;
        handleSearch(e, requestData);
    });

    $(document).off('submit', '#filter_search').on('submit', '#filter_search', function (e) {
        requestData.keyword = $('#search_bar').val();
        sharedData.requestData = requestData;
        handleSearch(e, requestDate);
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

function handleSearch(e, requestData) {
    e.preventDefault();

    var filteredRequestData = Object.fromEntries( // Object.fromEntries() chuyển kết quả lọc ngược lại thành object.
        Object.entries(requestData).filter(([key, value]) => { //Chuyển requestData thành mảng các cặp [key,value], filter() loại bỏ các mục có giá trị null, undefined, chuỗi rỗng hoặc mảng rỗng.
            if (Array.isArray(value)) {
                return value.length > 0; // Chỉ giữ nếu mảng có phần tử
            }
            return value; // Loại bỏ nếu null, undefined, hoặc chuỗi rỗng
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

function clickSelectBox() {
    const box_select_genre = $('.box');
    box_select_genre.on("click", function (e) {
        e.stopPropagation();

        const parent = $(this).closest('.select_box');
        const list = parent.find('.box_list_select');

        const box_list_select = $('.box_list_select');
        box_list_select.not(list).removeClass('appear');

        list.toggleClass("appear");
    })

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.select_box').length) {
            $('.box_list_select').removeClass('appear');
        }
    });
}

function selectGenre(includeGenres, excludeGenres) {
    const genreList = $('.list_all_genres .genre_item');
    const clickCount = {};


    genreList.on('click', function () {
        const genreItem = $(this);
        const id = genreItem.attr('id');

        if (!clickCount[id]) clickCount[id] = 0;

        clickCount[id] = (clickCount[id] + 1) % 3;
        if (clickCount[id] === 0) {
            genreItem.removeClass('include notInclude');
            excludeGenres.pop();
        } else if (clickCount[id] === 1) {
            genreItem.addClass('include').removeClass('notInclude');
            includeGenres.push(parseInt(id));
        } else if (clickCount[id] === 2) {
            genreItem.removeClass('include').addClass('notInclude');
            includeGenres.pop();
            excludeGenres.push(parseInt(id));
        }
    })
}