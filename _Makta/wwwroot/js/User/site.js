$(".main-menu.menu-dark .navigation li a").on("click", function () {
    var href = $(this).attr('href');
    location.href = href;
})