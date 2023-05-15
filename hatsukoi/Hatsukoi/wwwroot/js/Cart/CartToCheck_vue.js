
const app = new Vue({
    el: '#app',
    data: {
        shop: {
            id: 0,
            shopName: "",
            cartItems: [
            ]
        },
        /*shopId: this.$cookies.get('shopId'),*/

        RecipientName: '',// 收件人姓名
        RecipientPhone: '',// 收件人電話
        RecipientAddress: '',// 收件地址
        RecipientCity: '',// 收件人的城市
        RecipientPostCode: '',// 收件人的郵遞區號
        Recipient: '請填寫收件人與購買人資料',
        TotalPrice: '',//最終金額 (已減折扣)
        Payment: '',
        Status: 1,
        orderId: '',
        shopId: this.shop.id,
        inputDataCheck: {
            //有錯//錯誤訊息
            RecipientNameError: false,
            RecipientNameErrorMsg: '',

            RecipientPhoneError: false,
            RecipientPhoneMsg: '',

            RecipientAddressError: false,
            RecipientAddressErrorMsg: '',

            RecipientCityError: false,
            RecipientCityErrorMsg: '',

            RecipientPostCodeError: false,
            RecipientPostCodeErrorMsg: '',
        },
        couponId: 0,
        discountAmount:0

    },
    created() {
        this.shop = shop
        this.couponId = couponId
        this.discountAmount = discountAmount
        console.log(this.shop)
        console.log(this.couponId)
        console.log(this.discountAmount)
    },
    mounted() {
        //this.shops = shops

    },
    computed: {
        selectedShopTotal() {
            let total = 0;
            /* console.log(this.shop)*/
            this.shop.cartItems.forEach(product => {
                total += product.quantity * product.unitPrice
            })

            this.TotalPrice = total - this.shop.discountAmount

            return total;
        },
        getRecipient() {
            //console.log(this.Recipient)
            if (this.RecipientName !== "" && this.RecipientCity !== "" && this.RecipientAddress !== "") {
                this.Recipient = `${this.RecipientName}<br/> ${this.RecipientPhone}<br/>${this.RecipientCity} ${this.RecipientAddress}`
            }

        },
        canCheckout() {

            return this.Payment !== '';
        },
        //驗證收件人資訊
        isVerify() {
            for (let prop in this.inputDataCheck) {
                if (this.inputDataCheck[prop] == true) {

                    return false
                }
            }
            return true
        }
    },
    filters: {
        currency(val) {
            return ` NT$ ${val.toLocaleString('en-us')} `
        }
    },
    methods: {
        toCheck() {
            //if (this.canCheckout) {

            //    let shopId = this.shop.id

            //$.ajax({
            //    url: '/Cart/CreateOrder',
            //    type: 'POST',
            //    data: {
            //        Status: 1,
            //        RecipientName: this.RecipientName,
            //        RecipientPhone: this.RecipientPhone,
            //        RecipientCity: this.RecipientCity,
            //        RecipientAddress: this.RecipientAddress,
            //        RecipientPostCode: this.RecipientPostCode,
            //        TotalPrice: this.selectedShopTotal,
            //        SellerId: shopId,
            //        Payment: this.Payment

            //    },
            //    success: function (json) {



            //        debugger
            //        let result = json
            //        console.log(result)
            //        //console.log(orderId)

            //        if (result.isCreate == "") {

            //            //要新增一頁結帳成功跳轉?直接導到綠界付款??付款完??
            //            //$.ajax({
            //            //    url: '/Payment/CheckOut',
            //            //    type: 'POST',
            //            //    data: {
            //            //        OrderId:orderId
            //            //    },
            //            //    success: function () {
            //            //        console.log("sucess")
            //            //    }

            //            //})


            //            return
            //            this.orderId = result.orderId
            //            let formData = new FormData().append(orderId) 
            //            //$.ajax({
            //            //    url: '/Payment/CheckOut',
            //            //    type: "POST",
            //            //    data: formData,
            //            //    contentType: false,
            //            //    cache: false,
            //            //    processData: false,
            //            //    success: function (data) {
            //            //        console.log(data);
            //            //    }, error: function (data) {
            //            //        console.log('無法送出');
            //            //    }
            //            //})


            //            /*location.href = `/Cart/Index/`*/
            //        } else {
            //            warntext = result.isCreate
            //            console.log(warntext)
            //        }
            //    }
            //})
        }

    },
    //initData() {

    //    //axios.post('/Cart/CartToCheck')
    //    //    .then(res => {
    //    //        debugger
    //    //        console.log(res)
    //    //    this.shops = res.data.result

    //    //    console.log(this.shops)
    //    //})

    //    //axios.post('/Cart/GetCheckShop')
    //    //    .then(response => {
    //    //        debugger
    //    //        console.log(response.data);

    //    //    })
    //    //    .catch(error => {
    //    //        console.error(error);
    //    //    });

    //},
    //getCookies() {
    //    this.$cookies.get(this.cookieKey)
    //    this.shopId = this.$cookies
    //    console.log(this.shopId)
    //}



    watch: {
        //收件人姓名驗證
        RecipientName: {
            //一載入馬上檢查
            immediate: true,
            handler() {
                if (this.RecipientName == '') {
                    this.inputDataCheck.RecipientNameError = true
                    this.inputDataCheck.RecipientNameMsg = '收件人不得為空'
                }
                else if (this.RecipientName.length <= 1) {
                    this.inputDataCheck.RecipientNameError = true
                    this.inputDataCheck.RecipientNameErrorMsg = '收件人名字長度不可小於2個字'
                }
                else {
                    this.inputDataCheck.RecipientNameError = false
                    this.inputDataCheck.RecipientNameErrorMsg = ''
                }
            },

        },
        //收件人電話驗證
        RecipientPhone: {
            //一載入馬上檢查
            immediate: true,
            handler() {
                let phoneRegexp = /^[0-9]*$/
                if (this.RecipientPhone == '') {
                    this.inputDataCheck.RecipientPhoneError = true
                    this.inputDataCheck.RecipientPhoneMsg = '收件人電話不得為空'
                }
                else if (!phoneRegexp.test(this.RecipientPhone)) {
                    this.inputDataCheck.RecipientPhoneError = true
                    this.inputDataCheck.RecipientPhoneErrorMsg = '收件人電話必須為數字'
                }
                else {
                    this.inputDataCheck.RecipientPhoneError = false
                    this.inputDataCheck.RecipientPhoneErrorMsg = ''
                }
            }

        },
        //收件人地址驗證
        RecipientAddress: {
            //一載入馬上檢查
            immediate: true,
            handler() {
                let zipcode = /^(?:\d{5}|\d{3})$/
                let addRegexp =/^([\u4e00-\u9fa5]+(?:市區|鎮區|鎮市|[鄉鎮市區]).+)[\d-]*$/;
                if (this.RecipientAddress == '') {
                    this.inputDataCheck.RecipientAddressError = true
                    this.inputDataCheck.RecipientAddressErrorMsg = '收件人地址不得為空'
                }
                else if (!addRegexp.test(this.RecipientAddress)) {
                    this.inputDataCheck.RecipientAddressError = true
                    this.inputDataCheck.RecipientAddressErrorMsg = '請輸入正確地址格式'
                }
                else {
                    this.inputDataCheck.RecipientAddressError = false
                    this.inputDataCheck.RecipientAddressErrorMsg = ''
                }
            },

        },
        //收件人郵遞區號3或5碼或6碼驗證
        RecipientPostCode: {
            //一載入馬上檢查
            immediate: true,
            handler() {
                let zipcodeRegexp = /^(?:\d{5}|\d{3}|\d{6})$/
                
                if (this.RecipientPostCode == '') {
                    this.inputDataCheck.RecipientPostCodeError = true
                    this.inputDataCheck.RecipientPostCodeErrorMsg = '收件人郵遞區號不得為空'
                }
                else if (!zipcodeRegexp.test(this.RecipientPostCode)) {
                    this.inputDataCheck.RecipientPostCodeError = true
                    this.inputDataCheck.RecipientPostCodeErrorMsg = '請輸入正確郵遞區號'
                }
                else {
                    this.inputDataCheck.RecipientPostCodeError = false
                    this.inputDataCheck.RecipientPostCodeErrorMsg = ''
                }
            },

        },

    }

})