const CouponApp = new Vue({
    el: '#couponForm',
    data: {

        promoCode: '',
        promoCodeError: false,
        promoCodeWarn: '',
        condition: '',
        conditionError: false,
        conditionWarn: '',
        discount: '',
        startTime: '',
        endTime: '',
        startTimeCheck: false,
        endTimeCheck: false,
        checkWarn: '',
        checkpromoCodeWarn: '',
        editPromoCode: '',
        editCondition: '',
        editDiscount: 0,
        editStartTime: '',
        editEndTime: '',
        deletePromoCode: '',
        isEditStartTimeDisabled: false
    },
    watch: {
        promoCode: function (newVal) {
            let vm = this

            let r = /^[a-zA-Z0-9]{8,16}$/
            if (!r.test(this.promoCode)) {
                vm.promoCodeError = true;
                vm.promoCodeWarn = '請輸入8~16個字元，需為英文字母或數字'
            }
            else {
                vm.promoCodeError = false;
                vm.promoCodeWarn = ''
                $.ajax({
                    url: '/api/Seller/checkPromoCode',
                    type: 'POST', contentType: "application/json",
                    data: JSON.stringify({
                        promoCode: newVal
                    }),
                    success: function (response) {
                        if (response.result.hasPromoCode != '') {
                            vm.promoCodeError = true;
                            vm.promoCodeWarn = response.result.hasPromoCode;
                        } else {
                            vm.promoCodeError = false;
                            vm.promoCodeWarn = '';
                        }
                    }
                });
            }


        },
        condition: function () {
            let r = /^[1-9]\d*$/
            if (!r.test(this.condition)) {
                this.conditionError = true;
                this.conditionWarn = '請輸入大於0的整數'
            }
            else {
                this.conditionError = false;
                this.conditionWarn = ''
            }
        },
    },
    computed: {
        isVerify() {

            if (this.promoCode != '' && this.condition != '' && this.discount != '' &&
                this.startTime != '' && this.endTime != '' && this.conditionWarn == '' && this.promoCodeWarn == '') {
                this.checkWarn = ''
                return false

            }
            else { return true }
        }
    },
    methods: {
        createInfo: function () {
            let th = this
            if (new Date(th.startTime) < new Date(new Date().setHours(0, 0, 0, 0))) {
                th.checkWarn = '起始時間不可比今天還早'
            }
            else if (new Date(th.startTime) >= new Date(th.endTime)) {
                th.checkWarn = '結束時間不可比起始時間還早'
            }
            else {
                $.ajax({

                    url: '/api/Seller/CreateCoupon',
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify({
                        PromoCode: this.promoCode,
                        Condition: this.condition,
                        Discount: this.discount,
                        StartTime: this.startTime,
                        EndTime: this.endTime
                    }),
                    success: function (output) {

                        if (output.result.isCreate == "已新增優惠券") {
                            output.result.userList.forEach(x => {
                                connection.invoke('SendCouponMessage', x)
                            })
                            Toast.fire(output.result.isCreate);
                            setTimeout(function () {
                                window.location.reload()
                            }, 500);

                        }

                    }
                });
            }
        },
        getInfo: function () {
            let couponId = $(event.target).data("coupon-id") + "";
            let vm = this;
            $.ajax({
                url: '/api/Seller/GetCouponInfo',
                type: 'POST',
                contentType: "application/json",
                data: JSON.stringify({
                    PromoCode: couponId
                }),
                success: function (output) {
                    vm.editPromoCode = output.result.promoCode;
                    vm.editCondition = output.result.condition;
                    vm.editDiscount = output.result.discount;
                    vm.editStartTime = new Date(output.result.startTime + 'Z').toISOString().slice(0, 10);
                    vm.editEndTime = new Date(output.result.endTime + 'Z').toISOString().slice(0, 10);

                    if (new Date(vm.editStartTime) < new Date()) {
                        vm.isEditStartTimeDisabled = true;
                    } else {
                        vm.isEditStartTimeDisabled = false;
                    }

                    // 打開編輯 Modal
                    $('#editCouponModal').modal('show');
                }
            });

        },
        UpdateCoupon: function () {
            let th = this
            if (new Date(th.editStartTime) >= new Date(th.editEndTime)) {
                th.checkWarn = '結束時間不可比起始時間還早'
            }
            else {
                $.ajax({
                    url: '/api/Seller/UpdateCoupon',
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify({
                        PromoCode: this.editPromoCode,
                        Discount: this.editDiscount,
                        Condition: this.editCondition,
                        StartTime: this.editStartTime,
                        EndTime: this.editEndTime
                    }),
                    success: function (re) {
                        if (re.result.isUpdate == "已更新優惠券") {
                            Toast.fire(re.result.isUpdate);

                            setTimeout(function () {
                                window.location.reload()
                            }, 500);

                        }

                    }
                });
            }
        },
        deleteModal: function () {
            this.deletePromoCode = $(event.target).data("coupon-id") + "";

            $('#deleteCouponModal').modal('show');
        },
        deleteCoupon: function () {
            let PromoCode = this.deletePromoCode
            $.ajax({
                url: '/api/Seller/DeleteCoupon',
                type: 'POST',
                contentType: "application/json",
                data: JSON.stringify({
                    PromoCode: PromoCode

                }),
                success: function (re) {
                    $('#deleteCouponModal').modal('hide');
                    $("#coupon-" + PromoCode).remove();
                    if (re.result.isDelete == "已刪除優惠券") {

                        Toast.fire(re.result.isDelete);
                        setTimeout(function () {
                            window.location.reload()
                        }, 500);

                    }

                }
            });
        }
    }
})
