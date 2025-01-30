export function LiveSearchAuthor(){
    var debounceTimer;
    var currentAJAXRequest;

    var search_author = $('.search_box#search_author')
    search_author.find('input').on('keyup', function () {
        var keyword = $(this).val();
        var list_selected = search_author.find('.list_selected');
        var list_search = search_author.find('.list_search');

        if (keyword.length > 0) {
            list_selected.show();
            list_search.show();
        } else {
            list_search.find('ul')[0].innerHTML = "";
            list_search.hide();
        }

        clearTimeout(debounceTimer);

        debounceTimer = setTimeout(function () {
            if (keyword.length === 0) {
                list_search.find('ul').empty();
                list_search.hide();
            }

            if (currentAJAXRequest && currentAJAXRequest.readyState != 4) {
                currentAJAXRequest.abort();
            }

            if (keyword.length > 0) {
                currentAJAXRequest = $.ajax({
                    url: '/api/AuthorApi',
                    type: 'GET',
                    data: { name: keyword },
                    success: function (data) {
                        //console.log(data);
                        var result = list_search.find('ul')[0];
                        result.innerHTML = "";

                        if (data.length == 0) {
                            var noti_no_result = document.createElement("p");
                            noti_no_result.textContent = "No results found!";
                            result.appendChild(noti_no_result);
                        } else {
                            const idAuthor = new Set();
                            $(".items_selected").each(function () {
                                const id = $(this).attr("id");
                                if (id) idAuthor.add(id);
                            })
                            $.each(data, function (i, item) {
                                var li = document.createElement("li");
                                var checkbox = document.createElement("input");
                                var custom_checkbox = document.createElement("span");

                                custom_checkbox.className = "custom_checkbox";

                                checkbox.type = "checkbox";
                                checkbox.value = item.id;
                                checkbox.id = `checkbox-${item.id}`;

                                // check items in set
                                if (idAuthor.has(String(item.id))) {
                                    checkbox.checked = true;
                                }

                                var label = document.createElement("label");
                                label.textContent = item.name;

                                label.insertBefore(custom_checkbox, label.firstChild);
                                label.insertBefore(checkbox, custom_checkbox);

                                li.appendChild(label);

                                result.appendChild(li);
                            })
                        }

                    },
                    error: function () {

                    }
                })
            }
        }, 300)

        
    })
}