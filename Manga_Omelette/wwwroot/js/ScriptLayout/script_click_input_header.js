export function ClickInputHeader() {
    var inputHeader = $('#searchHeader');
    var overlay = $('#overlay');
    var search_result = $('.search_result');
    inputHeader.on('focus', function () {
        inputHeader.addClass('active');
        overlay.show();
        search_result.show();
    });

    $(document).on('click', function (e) {
        var $searchField = $('.searchField');
        if (!$searchField.is(e.target) && $searchField.has(e.target).length === 0) {
            $('.search_result').hide();
            $('.clear-input').hide();
        }
    });

    inputHeader.on('blur', function () {
        inputHeader.removeClass('active');
        overlay.hide();
    });

}