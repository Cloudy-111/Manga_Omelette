
//Scroll thì hiện background header
var layoutHeader = document.getElementById("layoutHeader");
var main_container = document.querySelector(".main_container");

main_container.addEventListener("scroll", function () {
    if (main_container.scrollTop === 0) {
        layoutHeader.classList.remove("scrolled");
    } else {
        layoutHeader.classList.add("scrolled");
    }
})

//Khi ở trang nào thì phần tử link trang đó sẽ active, thực hiện bằng cách kiểm tra Path và ID phần tử đó
document.addEventListener("DOMContentLoaded", function () {
    var currentPath = window.location.pathname;
    var menuItems = document.querySelectorAll(".title-sidebar");
    menuItems.forEach(item => {
        if (currentPath === `/${item.id}` && currentPath !== "/") {
            item.classList.add("active");
        } else if (currentPath === "/" && item.id === "home") {
            item.classList.add("active");
        } else {
            item.classList.remove("active");
        }
    })
})



// Lưu trạng thái checkbox khi thay đổi
document.getElementById('checkToggle').addEventListener('change', function () {
    localStorage.setItem('checkboxState', this.checked);
});

// Khôi phục trạng thái checkbox khi tải trang
window.addEventListener('load', function () {
    var checkboxState = localStorage.getItem('checkboxState');
    if (checkboxState !== null) {
        document.getElementById('checkbox').checked = (checkboxState === 'true');
    }
});
