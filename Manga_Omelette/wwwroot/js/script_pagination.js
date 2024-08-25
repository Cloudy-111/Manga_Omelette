document.addEventListener("DOMContentLoaded", function () {
    var page_num = parseInt(document.getElementById("page_number").innerText);
    var totalPages = parseInt(document.getElementById("total_page").innerText);
    var currentPathPage = window.location.href;
    var pagination_item = document.getElementById(`pagination_${page_num}`);
    var page = "page=" + page_num.toString();

    var pageName = window.location.pathname.slice(1);

    if (currentPathPage.includes(page) || page_num === 1) {
        pagination_item.classList.add("active");
    } else {
        pagination_item.classList.remove("active");
    }
    var previousBtn = document.getElementById("previousBtn");
    var nextBtn = document.getElementById("nextBtn");
    if (page_num > 1) {
        previousBtn.querySelector("a").href = "/" + pageName + "?page=" + (page_num - 1);
    } else {
        previousBtn.classList.add("disabled");
    }

    if (page_num < totalPages) {
        nextBtn.querySelector("a").href = "/" + pageName + "?page=" + (page_num + 1);
    } else {
        nextBtn.classList.add("disabled");
    }
});