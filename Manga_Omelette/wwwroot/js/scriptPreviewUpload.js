export function previewUpload() {
    $('#fileUpload').change(function (event) {
        var input = this;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#ImagePreview').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    })
}