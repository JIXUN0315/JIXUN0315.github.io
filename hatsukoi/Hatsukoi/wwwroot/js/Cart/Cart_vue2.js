
const app = new Vue({
    el: '#app',
    data: {
        url: window.location.origin,
        shops: [
            //"id": 4,
            //"shopName": "Decole Concombre 生活雜貨",
            //"cartItems": [
            //    {
            //        "cartId": 87,
            //        "productId": 5,
            //        "itemName": "日本Decole Concombre小福兔過新年",
            //        "quantity": 1,
            //        "unitPrice": 250,
            //        "itemImg": "https://hackmd.io/_uploads/S1-qeQ1xh.png",
            //        "description": null,
            //        "specID": 0,
            //        "specName": ""
            //    }
            //]
            //    }],
            //    "checked": false }
        ],
        /*selectedShop: 0,// 選擇的商店*/
        selectedItems: [],// 選擇的商品 
        selectedShopId: 0, // 選擇的商店 ID
        
        /*cookieKey: "shopId",*/
        coupon: [
            //{
            //    couponId: ,
            //    condition:22,
            //    couponNumber:"16515615616",
            //    discount:0.85,
            //    discountStr:85,
            //    endTime:"2023-05-05T00:00:00",
            //    img:"https://picsum.photos/200/200?grayscale"
            //    sellerId:4,
            //    sellerName:"Decole Concombre 生活雜貨",
            //    startTime:"2023-04-10T00:00:00"
            //    StartTimeStr
            //},
        ],
        selectedCoupon:'',
        useDiscount: 1,//打折數 預設1
        selectedCouponId:0,
        totalPrice: 0, //未打折金額
        discountAmount: 0 ,//已折抵金額
        finalAmount: 0 //最終金額
    },
    created() {
        this.initData()
        
    },
    computed: {
        cartlistCount() {
            return this.shops.length
        },
        canCheckout() {
            return this.shops.filter(x=>x.checked).length == 1;
        },
        selectedShopTotal() {
            let total = 0;
            for (var item of this.shops.filter(x => x.checked)) {
               /* console.log(item)*/
                for (var product of item.cartItems) {
                    total += product.quantity * product.unitPrice 
                }
                this.selectedShopId = item.id
            }
            this.totalPrice = total

            return total;
            
        },
        priceDiscount() {
            //優惠券有金額限制要大於限制金額才能折抵
            if (this.selectedCoupon !== '' && this.totalPrice > this.selectedCoupon.condition) {

                this.selectedCouponId = this.selectedCoupon.couponId
                console.log(this.selectedCouponId)
                //折數替換成優惠券折數
                this.useDiscount = this.selectedCoupon.discount
                //已折抵金額
                this.discountAmount = Math.round(this.totalPrice * (1 - this.useDiscount))
               
            }
            
            return this.discountAmount
        }
        
       

    },
    filters: {
        currency(val) {
            return ` NT$ ${val.toLocaleString('en-us')} `
        }
    },
    methods: {
        initData() {
            //this.$cookies.remove(this.cookieKey)
            console.log(this.selectedCouponId)
            axios.get('/api/Cart/Get').then(res => {

                this.shops = res.data.result.map(item => ({
                    ...item,
                    checked: false
                }))
               /* console.log(JSON.stringify(this.shops))*/
            }
               
            )
        },
        
        //加減鍵綁同一個方法
        changeQuantity(operation, shop, product) {
            if (operation === 'increment') {
                // 增加商品數量
                product.quantity++;
                this.updateQuantity(product)

            } else if (operation === 'decrement') {
                // 減少商品數量
                if (product.quantity > 1) {
                    product.quantity--;
                    this.updateQuantity(product)
                }
            }

        },

        //更新資料庫產品數量
        updateQuantity(product) {
            const request = {
                cartId: product.cartId,
                quantity: product.quantity
            }
            axios.post('/api/Cart/UpdateQuantity', request)
                .then(res => {
                    this.initData()
                    //console.log('更新成功')
                })
                .catch(err => {
                    console.log(err)
                })
        },

        //刪除商品(帶入購物車Id)
        removeProduct(id) {

            axios.delete(`${this.url}/Cart/DeleteCartProduct/${id}`)
                .then(res => {
                    console.log(id)
                    //this.shops = res.data
                    //this.shop.cartItems.splice(this.index,1) 
                    this.initData()
                })
                .catch(err => {
                    console.log(err)
                })


        },
        removeShop(id) {

            axios.delete(`${this.url}/Cart/DeleteCartShop/${id}`)

                .then(res => {
                    console.log(id)
                    this.initData()
                })
                .catch(err => {

                    console.log(err)
                })

        },

        getCoupon(sellerId) {
            
            const request = {
                SellerId: sellerId, 
            }
            //console.log(request)
            
            
            axios.post('/api/Cart/GetUserCoupon', request)
            
                .then(res => {
                    
                    this.coupon = res.data.result.map(item => ({
                        ...item,
                        checked: false
                    }))
                    //console.log(this.coupon)
                    
                })
        },
       
        toCheck() {
           
            //console.log(this.$cookies)
            //this.$cookies.set(this.cookieKey, shop.id)
            /*location.href = `/Cart/CartToCheck/`*/

            //將shop.id傳給webApi
            //const formData = new FormData();
            //formData.append('shopId', shop.id);
            //axios.post('/Cart/GetCheckShop', formData)
                //.then(response => {
                //    //console.log(response.data);
                //    //location.href = `/Cart/CartToCheck/`
                //})
                //.catch(error => {
                //    console.error(error);
                //});

        },

        // 更新商品的單價
        //updateSpecPrice(product) {
        //    if (!product.selectedSpec) {
        //        // 如果沒有選擇規格，則使用商品原本的單價
        //        product.unitPrice = product.defaultPrice;
        //    } else {
        //        // 取得所選的規格單價，並更新商品的單價
        //        product.unitPrice = product.selectedSpec;
        //    }
        //},
        


    }
    



})