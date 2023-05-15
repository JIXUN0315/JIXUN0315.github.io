
let categoryApp = new Vue({
    el: '#category-app',
    data: {
        salesData: [],
        currentMonth: 0,
    },
    created() {
        this.getMonth()
        this.getData()
    },
    methods: {
        getMonth() {
            const Month = new Date().getMonth() + 1
            this.currentMonth = Month
        },
        getData() {
            axios.post('/api/DashBoard/GetSalseData', {
                "month": this.currentMonth,
            })
                .then(response => {
                    this.salesData = response.data.body;
                    console.log(this.salesData)
                    this.drawChart();
                })
                .catch(error => {
                    console.log(error);
                });
        },
        drawChart() {
            const chartData = {
                labels: this.salesData.map(s => s.categoryName),
                datasets: [
                    {
                        data: this.salesData.map(s => s.salesData),
                        backgroundColor: [
                            '#80D9C5',
                            '#C8DD9F',
                            '#F4D58F',
                            '#EF8476',
                            '#C3A5D0',
                            '#EAC5BD',
                            '#DE9190',
                            '#F5C186',
                            '#F2AB80',
                            '#A6BF5E',
                            '#8FC2D3',
                            '#7E6E9A',
                        ],
                    },
                ],
            };

            const ctx = document.getElementById('category-chart');
            new Chart(ctx, {
                type: 'pie',
                data: chartData,
            });
        }
    },
})
