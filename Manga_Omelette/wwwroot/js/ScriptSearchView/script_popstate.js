export function Popstate() {
    // Để có thể quay lại trang trước, dùng popstate 
    window.addEventListener('popstate', function (event) {
        if (event.state && event.state.filteredRequestData) {
            // Lấy lại requestData từ state
            const requestData = event.state.filteredRequestData;

            // Khôi phục nội dung
            $.ajax({
                url: '/filter',
                type: 'GET',
                data: requestData,
                traditional: true,
                success: function (response) {
                    $('.story_grid').html(response);
                },
                error: function () {
                    alert('Error fetching data.');
                }
            });
        }
    });

}