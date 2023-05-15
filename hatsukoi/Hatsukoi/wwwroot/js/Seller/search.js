const searchApp = new Vue({
    el: '#search-app',
    data: {
        orderList: [],
        q: '',
        ListCurrentPage: 1,
        perPage: 5,
        sort: "newest",
    },
    created() {
        this.getOrderList()
        const searchParams = new URLSearchParams(window.location.search);
        this.q = searchParams.get('q');
    },
    computed: {
        filteredOrders() {
            const keyword = this.q.toLowerCase().trim();
            let list = this.orderList.filter(order =>
                order.orderNumber.toLowerCase().includes(keyword) ||
                order.name.toLowerCase().includes(keyword) ||
                order.recipientPhone.includes(keyword) ||
                order.recipientName.toLowerCase().includes(keyword) ||
                order.recipientAddress.includes(keyword) 
            );
            if (this.sort === 'newest') {
                list.sort((a, b) => new Date(b.createTime) - new Date(a.createTime));
            } else if (this.sort === 'oldest') {
                list.sort((a, b) => new Date(a.createTime) - new Date(b.createTime));
            }
            return list
        },
        pagedList() {
            const start = (this.ListCurrentPage - 1) * this.perPage
            const end = start + this.perPage
            return this.filteredOrders.slice(start, end)
        }
    },
    methods: {
        getOrderList() {
            axios.get('/api/order/getorderlist')
                .then(res => {
                    console.log(res)
                    this.orderList = res.data.result
                })
        },
        goTop() {
            window.scrollTo(0, 0);
        },
        highlight(str) {
            if (!this.q) {
                return str;
            }
            const re = new RegExp(this.q, 'gi');
            return str.replace(re, '<mark>$&</mark>');
        },
        
    },
   
})