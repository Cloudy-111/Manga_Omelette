export function LiveSearch() {
    var debounceTimer;
    var currentAJAXRequest;

    $('#searchHeader').on('keyup', function () {
        var term = $(this).val();

        if (term.length > 0) {
            $('.clear_input').show();
            $('.instruction_search').hide();
        } else {
            $('.clear_input').hide();
            $('.instruction_search').show();
        }

        clearTimeout(debounceTimer);

        debounceTimer = setTimeout(function () {
            if (term.length === 0) {
                $('.search_result').hide();
                return;
            }

            if (currentAJAXRequest && currentAJAXRequest.readyState !== 4) {
                currentAJAXRequest.abort();
            }

            currentAJAXRequest = $.ajax({
                url: '/Home/Search',
                type: 'get',
                data: { term: term },
                success: function (data) {
                    var results = $('.search_result ul');
                    results.empty();

                    if (data.length === 0) {
                        results.append('<p>No results found</p>');
                    } else {
                        $.each(data, function (i, item) {
                            results.append(`<li><a href="/titles/${item.id}">${item.title}</a></li>`);
                        });
                    }

                    $('.search_result').show();
                },
                error: function () {

                }
            })
        }, 300);

    });
    $('.clear_input').on('click', function () {
        $('#searchHeader').val('');
        $('#searchHeader').focus();
        $('.search_result ul').empty();
        $('.search_result').show();
        $('.instruction_search').show();
        $(this).hide();

        if (currentAJAXRequest && currentAJAXRequest.readyState !== 4) {
            currentAJAXRequest.abort();
        }
    });
}