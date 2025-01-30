export function CheckAndUncheck() {
    var list_selected = $('.search_box#search_author .list_selected')
    $(document).off('click', '.search_box#search_author .list_search ul li');
    $(document).on('click', '.search_box#search_author .list_search ul li', function (event) {
        var checkbox = $(this).find('input[type="checkbox"]');
        var isChecked = checkbox.prop('checked');
        checkbox.prop('checked', !isChecked);
        if ($(event.target).is(checkbox)) {
            // Nếu là input thì bỏ qua
            return;
        }
        var labelText = $(this).find('label').text();
        var authorId = $(this).find('input').val();

        var item_selected = document.createElement('div');
        item_selected.className = "items_selected";
        item_selected.id = authorId;

        var content_item_selected = document.createElement('div');
        content_item_selected.className = "content_item_selected";

        var cancel_icon = document.createElement('i');
        cancel_icon.className = "fa-solid fa-x";

        content_item_selected.textContent = labelText;
        content_item_selected.insertBefore(cancel_icon, content_item_selected.firstChild);

        item_selected.appendChild(content_item_selected);
        list_selected.append(item_selected);

        if (!checkbox.prop('checked')) {
            list_selected.find(`.content_item_selected:contains('${labelText}')`).parent().remove();
        }

        //console.log(labelText);
        //checkbox.prop('checked', !checkbox.prop('checked'));
    })

    $(document).on('click', '.search_box#search_author .list_selected .items_selected', function (event) {
        event.stopPropagation();
        var item_selected = $(this);
        var authorId = item_selected.attr('id');

        item_selected.remove();

        $(`#checkbox-${authorId}`).prop('checked', false);
    })

    //$(document).on('click', '.search_box#search_author .list_search ul li label', function (event) {
    //    event.stopPropagation(); // Ngăn sự kiện lan lên cha
    //})
}