document.addEventListener("DOMContentLoaded", function () {
    var inputHeader = document.getElementById("searchHeader");
    var overlay = document.getElementById("overlay");
    inputHeader.addEventListener('focus', function () {
        inputHeader.classList.add('active');
        overlay.classList.add("appear");
    });

    inputHeader.addEventListener('blur', function () {
        inputHeader.classList.remove('active');
        overlay.classList.remove("appear");
    });
})


//Scroll thì hiện background header
var backgroundHeader = document.getElementById("backgroundHeader");
var main_container = document.querySelector(".main_container");

main_container.addEventListener("scroll", function () {
    if (main_container.scrollTop === 0) {
        backgroundHeader.classList.remove("scrolled");
    } else {
        backgroundHeader.classList.add("scrolled");
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

document.addEventListener("DOMContentLoaded", function () {
    var userBtn = document.getElementById("userBtn");
    var overlay = document.getElementById("overlay");
    var profile_container = document.getElementById("profile_container");
    userBtn.addEventListener("click", function () {
        overlay.classList.toggle("appear");
        profile_container.classList.toggle("appear");
    });

    overlay.addEventListener("click", function () {
        overlay.classList.remove("appear");
        profile_container.classList.remove("appear");
    })
});

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
