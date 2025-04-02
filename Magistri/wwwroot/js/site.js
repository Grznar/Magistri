// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function userScroll() {
    const navbar = document.querySelector(".navbar");

    window.addEventListener("scroll", () => {
        if (window.scrollY > 50) {
            navbar.classList.add("cyan");
            navbar.classList.add("navbar-sticky");
        } else {
            navbar.classList.remove("cyan");
            navbar.classList.remove("navbar-sticky");
        }
    });
}
document.addEventListener("DOMContentLoaded", userScroll);