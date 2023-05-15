
//let sortSelect = document.querySelector("#sort")
//let orderListStatus = document.querySelector(".order-list-status")

//let notPayList = document.querySelector("#pills-notPay .order-list")
//let tobeshippedList = document.querySelector("#pills-tobeshipped .order-list")
//let shippedList = document.querySelector("#pills-shipped .order-list")
//let completedList = document.querySelector("#pills-completed .order-list")
//let cancelList = document.querySelector("#pills-cancel .order-list")

//let TimeSort = "new";

//orderListStatus.innerHTML = sortSelect.options[1].innerText

//sortSelect.onchange = function () {
//    let value = document.querySelector("#sort").value;
//    orderListStatus.innerHTML = sortSelect.options[2].innerText;


//    let notPayOrders = document.querySelectorAll("#pills-notPay .order");
//    let tobeshippedOrders = document.querySelectorAll("#pills-tobeshipped .order");
//    let shippedOrders = document.querySelectorAll("#pills-shipped .order");
//    let completedOrders = document.querySelectorAll("#pills-completed .order");
//    let cancelOrders = document.querySelectorAll("#pills-cancel .order");

//    if (value == "oldest" && TimeSort == "new") {
//        notPayList.innerHTML = "";
//        tobeshippedList.innerHTML = "";
//        shippedList.innerHTML = "";
//        completedList.innerHTML = "";
//        cancelList.innerHTML = "";

//        let notPayCount = notPayOrders.length;
//        let tobeshippedCount = tobeshippedOrders.length;
//        let shippedCount = shippedOrders.length;
//        let completedCount = completedOrders.length;
//        let cancelCount = cancelOrders.length;

//        for (let i = notPayCount - 1; i >= 0; i--) {
//            notPayList.appendChild(notPayOrders[i])
//        }
//        for (let i = tobeshippedCount - 1; i >= 0; i--) {
//            tobeshippedList.appendChild(tobeshippedOrders[i])
//        }
//        for (let i = shippedCount - 1; i >= 0; i--) {
//            shippedList.appendChild(shippedOrders[i])
//        }
//        for (let i = completedCount - 1; i >= 0; i--) {
//            completedList.appendChild(completedOrders[i])
//        }
//        for (let i = cancelCount - 1; i >= 0; i--) {
//            cancelList.appendChild(cancelOrders[i])
//        }

//        TimeSort = "old"
//    }
//    else if (value == "newest" && TimeSort == "old") {
//        orderListStatus.innerHTML = sortSelect.options[1].innerText
//        notPayList.innerHTML = "";
//        tobeshippedList.innerHTML = "";
//        shippedList.innerHTML = "";
//        completedList.innerHTML = "";
//        cancelList.innerHTML = "";

//        let notPayCount = notPayOrders.length;
//        let tobeshippedCount = tobeshippedOrders.length;
//        let shippedCount = shippedOrders.length;
//        let completedCount = completedOrders.length;
//        let cancelCount = cancelOrders.length;

//        for (let i = notPayCount - 1; i >= 0; i--) {
//            notPayList.appendChild(notPayOrders[i])
//        }
//        for (let i = tobeshippedCount - 1; i >= 0; i--) {
//            tobeshippedList.appendChild(tobeshippedOrders[i])
//        }
//        for (let i = shippedCount - 1; i >= 0; i--) {
//            shippedList.appendChild(shippedOrders[i])
//        }
//        for (let i = completedCount - 1; i >= 0; i--) {
//            completedList.appendChild(completedOrders[i])
//        }
//        for (let i = cancelCount - 1; i >= 0; i--) {
//            cancelList.appendChild(cancelOrders[i])
//        }
//        TimeSort = "new"
//    }

//}

//let tobeshippedTab = document.querySelector('#tobeshipped')
//let leftSideTool = document.querySelector('.left-side')


//tobeshippedTab.addEventListener('click', event => {
//    leftSideTool.classList.remove('dp-none')
//})

//function dpNoneShipTab() {
//    leftSideTool.classList.add('dp-none')
//}


////單筆訂單出貨
//function shipOrder(orderNumber) {
//    console.log(orderNumber)
//    $.ajax({
//        url: '/api/Order/ShipOrder',
//        type: 'POST',
//        contentType: "application/json",
//        data: JSON.stringify({

//            orderNum: orderNumber

//        }),
//        success: function (data) {
//            window.location.href = "/Order/Management?status=tobeshipped"
//        }
//    });
//}

//let $selectAll = $('#select-all');
//let $orderInputs = $('.order input[type="checkbox"]');
//let selectNum = document.querySelector('.selectNum')
//let $orderInputscheck = $('.order input[type="checkbox"]');
//$selectAll.change(function () {
//    console.log('change')
//    $orderInputs.prop('checked', $selectAll.prop('checked'));
//    let $orderInputsChecked = $orderInputs.filter(':checked').length;
//    if ($orderInputsChecked > 0) {
//        selectNum.innerText = $orderInputsChecked;
//    } else {
//        selectNum.innerText = "選取";
//    }
//});

//// 訂單的checkbox
//$orderInputs.change(function () {
//    let $orderInputsChecked = $orderInputs.filter(':checked').length;
//    if ($orderInputsChecked === $orderInputs.length) {
//        $selectAll.prop('checked', true);
//    } else {
//        $selectAll.prop('checked', false);
//    }

//    if ($orderInputsChecked > 0) {
//        selectNum.innerText = $orderInputsChecked;
//    } else {
//        selectNum.innerText = "選取";
//    }
//});

////多筆訂單出貨
//$('.ship-btn').click(function () {
//    var orderNumbers = [];

//    $('.order input[type="checkbox"]:checked').each(function () {
//        var orderNumber = $(this).closest('.order-info').find('.order-id').attr('id');
//        orderNumbers.push(orderNumber);
//    });
//    console.log(orderNumbers)
//    $.ajax({
//        url: '/api/Order/ShipOrders',
//        type: 'POST',
//        contentType: "application/json",
//        data: JSON.stringify({
//            orderNums: orderNumbers

//        }),
//        success: function (data) {
//            window.location.href = "/Order/Management?status=tobeshipped"
//        }
//    });

//});


//切換到指定狀態的tab
function getUrlParameter(name) {
    const params = new URLSearchParams(window.location.search);
    return params.get(name) || '';
}

//獲取當前網頁 URL 中指定名稱的查詢status參數的值
let orderStatus = getUrlParameter('status');
if (orderStatus != '') {
    $('#notPay').removeClass('active');
    $('#pills-notPay').removeClass('active');
    $('.nav-link[data-status="' + orderStatus + '"]').addClass('active');
    $('.tab-pane[data-status="' + orderStatus + '"]').addClass('active show');


}


const orderManagementApp = new Vue({
    el: '#order-management-app',
    data: {
        orderList: [],
        selectAll: false,
        isShippingTab: false,
        notPayListCurrentPage: 1,
        tobeshippedListCurrentPage: 1,
        toBeReceivedListCurrentPage: 1,
        completedListCurrentPage: 1,
        cancelListCurrentPage: 1,
        perPage: 5,
        checkedCount: 0,
        sort: "newest",
        ordersToShip: [],
    },
    created() {
        this.getOrderList(),
            this.checkStatus()
    },
    computed: {
        notPayList() {
            let list = this.orderList.filter(o => o.status == 1)
            if (this.sort === 'newest') {
                list.sort((a, b) => new Date(b.createTime) - new Date(a.createTime));
            } else if (this.sort === 'oldest') {
                list.sort((a, b) => new Date(a.createTime) - new Date(b.createTime));
            }
            return list
        },
        notPayListCount() {
            return this.notPayList == undefined ? 0 : this.notPayList.length
        },
        tobeshippedList() {
            let list = this.orderList.filter(o => o.status == 2)
            if (this.sort === 'newest') {
                list.sort((a, b) => new Date(b.createTime) - new Date(a.createTime));
            } else if (this.sort === 'oldest') {
                list.sort((a, b) => new Date(a.createTime)- new Date(b.createTime));
            }
            return list
        },
        tobeshippedListCount() {
            return this.tobeshippedList == undefined ? 0 : this.tobeshippedList.length
        },
        toBeReceivedList() {
            let list = this.orderList.filter(o => o.status == 3)
            if (this.sort === 'newest') {
                list.sort((a, b) => new Date(b.createTime) - new Date(a.createTime) );
            } else if (this.sort === 'oldest') {
                list.sort((a, b) => new Date(a.createTime) - new Date(b.createTime) );
            }
            return list
        },
        toBeReceivedListCount() {
            return this.toBeReceivedList == undefined ? 0 : this.toBeReceivedList.length
        },
        completedList() {
            let list = this.orderList.filter(o => o.status == 4)
            if (this.sort === 'newest') {
                list.sort((a, b) => new Date(b.createTime) - new Date(a.createTime));
            } else if (this.sort === 'oldest') {
                list.sort((a, b) => new Date(a.createTime) - new Date(b.createTime));
            }
            return list
        },
        completedListCount() {
            return this.completedList == undefined ? 0 : this.completedList.length
        },
        cancelList() {
            let list = this.orderList.filter(o => o.status == 5)
            if (this.sort === 'newest') {
                list.sort((a, b) => new Date(b.createTime) - new Date(a.createTime));
            } else if (this.sort === 'oldest') {
                list.sort((a, b) => new Date(a.createTime) - new Date(b.createTime));
            }
            return list
        },
        cancelListCount() {
            return this.cancelList == undefined ? 0 : this.cancelList.length
        },

        // 分頁後的List
        pagedNotPayList() {
            const start = (this.notPayListCurrentPage - 1) * this.perPage
            const end = start + this.perPage
            return this.notPayList.slice(start, end)
        },
        pagedTobeshippedList() {
            const start = (this.tobeshippedListCurrentPage - 1) * this.perPage
            const end = start + this.perPage
            return this.tobeshippedList.slice(start, end)
            
        },
        pagedToBeReceivedList() {
            const start = (this.toBeReceivedListCurrentPage - 1) * this.perPage
            const end = start + this.perPage
            return this.toBeReceivedList.slice(start, end)
        },
        pagedCompletedList() {
            const start = (this.completedListCurrentPage - 1) * this.perPage
            const end = start + this.perPage
            return this.completedList.slice(start, end)
        },
        pagedCancelList() {
            const start = (this.cancelListCurrentPage - 1) * this.perPage
            const end = start + this.perPage
            return this.cancelList.slice(start, end)
        },
    },
    methods: {
        goTop() {
            window.scrollTo(0, 0);
            this.selectAll = false;
            this.orderList.forEach((order) => {
                order.checked = false;
            });
            this.updateCheckedCount()
        },
        updateCheckedCount() {
            this.checkedCount = this.tobeshippedList.filter((order) => order.checked).length;
        },
        getOrderList() {
            axios.get('/api/order/getorderlist')
                .then(res => {
                    console.log(res)
                    this.orderList = res.data.result
                })
        },
        // 全選 checkbox
        selectAllChanged(event) {
            const checked = event.target.checked;
            this.pagedTobeshippedList.forEach((order) => {
                order.checked = checked
            });
            this.updateCheckedCount();
        },
        // 單一訂單 checkbox
        orderCheckedChanged(event) {
            this.selectAll = this.tobeshippedList.every((order) => order.checked);
            this.updateCheckedCount();
        },
        checkShip() {
            $('#shipOrderModal').modal('show');
            this.ordersToShip = this.tobeshippedList.filter((order) => order.checked).map((order) => order.orderNumber);

        },
        // 多筆出貨按鈕
        shipOrders() {
            let th=this
            $.ajax({
                url: '/api/Order/ShipOrders',
                type: 'POST',
                contentType: "application/json",
                data: JSON.stringify({
                    orderNums: th.ordersToShip
                }),
                success: function (data) {
                    //Tom 通知中心的發送
                    th.ordersToShip.forEach(x => {
                        connection.invoke('SendOrderMessage', x)
                    })

                    if (data.result.isSuccess == "已成功更新") {
                        Toast.fire(data.result.isSuccess);
                        setTimeout(function () {
                            window.location.href = "/Order/Management?status=tobeshipped";
                        }, 500);

                    }
                    th.ordersToShip=[]

                    
                }
            });
        },
        shipOrder(orderNumber) {
            console.log(`出貨訂單: ${orderNumber}`);
            $.ajax({
                url: '/api/Order/ShipOrder',
                type: 'POST',
                contentType: "application/json",
                data: JSON.stringify({
                    orderNum: orderNumber

                }),
                success: function (data) {
                    //Tom 通知中心的發送
                    connection.invoke('SendOrderMessage', orderNumber)

                    if (data.result.isSuccess == "已成功更新") {
                        Toast.fire(data.result.isSuccess);
                        setTimeout(function () {
                            window.location.href = "/Order/Management?status=tobeshipped";
                        }, 500);

                    }
                }
            });
        },
        getUrlParameter(name) {
            const params = new URLSearchParams(window.location.search);
            return params.get(name) || '';
        },
        checkStatus() {
            let orderStatus = getUrlParameter('status');
            if (orderStatus == "tobeshipped") {
                this.isShippingTab = true;
            }
        },
        setNotPayTab(tab) {
            this.isShippingTab = false;
            this.notPayListCurrentPage = 1;
            history.replaceState(null, '', `/Order/Management?status=${tab}`);
        },
        setTobeshippedTab(tab) {
            this.isShippingTab = true;
            this.tobeshippedListCurrentPage = 1;
            history.replaceState(null, '', `/Order/Management?status=${tab}`);
        },
        setToBeReceivedTab(tab) {
            this.isShippingTab = false;
            this.toBeReceivedListCurrentPage = 1;
            history.replaceState(null, '', `/Order/Management?status=${tab}`);
        },
        setCompletedTab(tab) {
            this.isShippingTab = false;
            this.completedListCurrentPage = 1;
            history.replaceState(null, '', `/Order/Management?status=${tab}`);
        },
        setCancelTab(tab) {
            this.isShippingTab = false;
            this.cancelListCurrentPage = 1;
            history.replaceState(null, '', `/Order/Management?status=${tab}`);
        },

    },

})