// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function userScroll() {
    const navbar = document.querySelector(".navbar");
    const navbarBrand = document.querySelector(".navbar-brand");
    if (!navbar || !navbarBrand) return;
    window.addEventListener("scroll", () => {
        if (window.scrollY > 50) {
            
            navbar.classList.add("navbar-sticky");
            navbarBrand.classList.add("navbar-brand-opacity")
        } else {
            navbar.classList.add("navbar-brand-opacity");
            navbarBrand.classList.remove("navbar-brand-opacity")
            navbar.classList.remove("navbar-sticky");
        }
    });
}
document.addEventListener("DOMContentLoaded", userScroll);