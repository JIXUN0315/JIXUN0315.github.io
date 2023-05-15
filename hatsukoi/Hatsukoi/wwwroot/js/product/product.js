/*< !--Initialize Swiper-- >*/

//第一組Swiper
var swiper = new Swiper(".mySwiper", {
    loop: true, //無限循環
    spaceBetween: 10, //圖片間的距離
    slidesPerView: 4, //圖片數量
});

//第二組Swiper
var swiper2 = new Swiper(".mySwiper2", {
    loop: true,
    spaceBetween: 10,
    navigation: {
        //設定前進和後退按鈕
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    thumbs: {
        swiper: swiper,
        /*設定對應的主輪播swiper，
        實現輪播切換時縮圖和主圖同步顯示。*/
    },
});

//價格
const productPriceLabel = document.querySelector('.product-price')
const specSelect = document.querySelector('#spec-select')

specSelect.onchange = function () {
    productPriceLabel.innerText = `NT$ ${specSelect.selectedOptions[0].getAttributeNode('ha-price').value}`
}


