// 按tab - 隱藏底下商品列
let tabs = document.querySelectorAll('.nav-item');
let designerProducts = document.querySelector('.pf-products-row');
let pfContainer = document.querySelector('.pf-container');

tabs[0].addEventListener('click', () => {
    designerProducts.style.display = "flex";
    pfContainer.style.display = "block";
})

tabs[1].addEventListener('click', () => {
    designerProducts.style.display = "none";
    pfContainer.style.display = "none";
})

tabs[2].addEventListener('click', () => {
    designerProducts.style.display = "none";
    pfContainer.style.display = "none";
})





