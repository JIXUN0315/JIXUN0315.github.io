let orderNumText = document.querySelector('#orderNum').innerText;
let cancelBtn = document.querySelector('.cancelBtn');
let cancelReason = document.querySelector('.reason');
function shipOrder() {
    $.ajax({
        url: '/api/Order/ShipOrder',
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify({
            orderNum: orderNumText
        }),
        success: function (data) {
            if (data.result.isSuccess == "已成功更新") {
                Toast.fire(data.result.isSuccess);
                //Tom 通知中心的發送
                connection.invoke('SendOrderMessage', orderNumText)
                setTimeout(function () {
                    window.location.href = "/OrderDetails?orderNum=" + orderNumText;
                }, 500);

            }
        }
    })

};


function CancelOrder() {

    let cancelReasonTxt = cancelReason.value;

    $.ajax({
        url: '/api/Order/CancelOrder',
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify({

            orderNum: orderNumText,
            cancelReason: cancelReasonTxt
        }),
        success: function (data) {
            if (data.result.isSuccess == "已成功更新") {
                Toast.fire(data.result.isSuccess);
                setTimeout(function () {
                    window.location.href = "/OrderDetails?orderNum=" + orderNumText;
                }, 500);

            }
        }
    });
}


let stepShip = document.querySelector('.step-ship')
let stepDone = document.querySelector('.step-done')
let stepPay = document.querySelector('.step-pay')
let statusText = document.querySelector('.order-status-label').innerText
let status
switch (statusText) {
    case "待付款":
        status = "notPay"
        break;
    case "待出貨":
        stepPay.classList.add('step-finish');
        status = "tobeshipped"
        break;
    case "已出貨":
        stepPay.classList.add('step-finish');
        stepShip.classList.add('step-finish');
        status = "shipped"
        break;
    case "已完成":
        stepPay.classList.add('step-finish');
        stepShip.classList.add('step-finish');
        stepDone.classList.add('step-finish');
        status = "completed"
        break;
    case "已取消":
        stepPay.classList.add('dp-none');
        stepShip.classList.add('dp-none');
        stepDone.classList.add('dp-none');
        status = "cancel"
        break;
    default:
        break;
}
$('.breadcrumb-item > a').on('click', function (event) {
    event.preventDefault(); // 防止預設行為
    window.location.href = '@Url.Action("Management", "Order")' + '?status=' + status; // 跳轉到帶有狀態錨點的 URL
});
if (status == "notPay") {
    cancelReason.addEventListener("input", () => {
        if (cancelReason.value.trim().length > 0) {
            cancelBtn.disabled = false;
        }
        else {
            cancelBtn.disabled = true;
        }
    })
}