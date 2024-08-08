//Selectize to select and create item not appear in dropdown
export function innitializeSelectize(selector, placeholder) {
    $(selector).selectize({
        create: true,
        placeholder: placeholder,
    });
}

export function innitializeSelect2(selector, placeholder) {
    $(selector).select2({
        placeholder: placeholder,
        allowClear: true,
        multiple: false,
    });
}

export function handleSelectionChange(previewList, seletor, itemtype) {
    var list = $(previewList);
    $(seletor).change(function () {
        var selectOption = $(this).find('option:selected');
        var itemId = selectOption.val().replace(/\s+/g, '-');
        var itemName = selectOption.text().replace(/\s+/g, '-');
        if ($(`${previewList} #${itemName}`).length === 0) {
            var item = $('<div>');
            item.attr({
                id: itemName,
                'data-id': selectOption.val(),
                'data-name': selectOption.text(),
            }).addClass(`${itemtype}_item`);

            var item_name = $('<div>');
            item_name.addClass(`${itemtype}_name`).text(itemName);

            var item_delete_btn = $('<i>');
            item_delete_btn.addClass("bi bi-x-circle-fill");

            item.append(item_name).append(item_delete_btn);

            list.append(item);
        }
    })
}

export function handleItemRemoval(previewList, itemtype) {
    $(previewList).on('click', 'i.bi-x-circle-fill', function () {
        $(this).closest(`.${itemtype}_item`).remove();
    })
}