export function listImageUpload() {
    $('#fileUpload').change(function (event) {
        var files = this.files;
        var listImage = $('#list_image_upload');
        for (let i = 0; i < files.length; i++) {
            var file = files[i];

            var reader = new FileReader();
            reader.onload = function (e) {
                var imgItem = $('<div>').attr({

                }).addClass("imageItem");
                var img = $('<img>').attr({
                    'src': e.target.result,
                    id: `image${i}`,
                });
                imgItem.append(img);

                listImage.append(imgItem);
            }
            reader.readAsDataURL(file);
        }
    })

    $('.list_image_upload').on('click', 'i.bi-x-circle-fill', function () {
        $(this).closest('.imageItem').remove();
    })
}