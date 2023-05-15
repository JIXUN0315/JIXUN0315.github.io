const evaluateApp = new Vue({
    el: '#evaluate-app',
    data: {
        EvaluateList: [],
        filteredEvaluateList: [],
        pagedList: [],
        Evaluate: 0,
        listText: "綜合評價",
        filteredEvaluateListCurrentPage: 1,
        perPage: 5,
    },
    created() {
        this.getEvaluateList()
    },
    computed: {
        averageScore() {
            if (this.EvaluateList === undefined || this.EvaluateList.length === 0) {
                return 0;
            }

            let sum = 0;
            for (let i = 0; i < this.EvaluateList.length; i++) {
                sum += this.EvaluateList[i].evaluate;
            }
            let avg = sum / this.EvaluateList.length
            return avg.toFixed(1);
        },
        evaluateListCount() {
            return this.EvaluateList == undefined ? 0 : this.EvaluateList.length
        },
        oneStarCount() {
            return this.EvaluateList.filter((o) => o.evaluate == 1).length
        },
        twoStarCount() {
            return this.EvaluateList.filter((o) => o.evaluate == 2).length
        },
        threeStarCount() {
            return this.EvaluateList.filter((o) => o.evaluate == 3).length
        },
        fourStarCount() {
            return this.EvaluateList.filter((o) => o.evaluate == 4).length
        },
        fiveStarCount() {
            return this.EvaluateList.filter((o) => o.evaluate == 5).length
        },

    },
    methods: {
        goTop() {
            this.$nextTick(() => {
                window.scrollTo(0, 0);
                this.pagination();
            });

        },
        getEvaluateList() {
            axios.get('/api/Order/GetEvaluateList').then((res) => {
                console.log(res);
                this.EvaluateList = res.data.result;
                this.Evaluate = 0
                this.showEvaluateList(this.Evaluate)
            });
        },
        pagination() {
            const startIndex = (this.filteredEvaluateListCurrentPage - 1) * this.perPage;
            const endIndex = startIndex + this.perPage;
            this.pagedList = this.filteredEvaluateList.slice(startIndex, endIndex);
        },
        showEvaluateList(evaluate) {
            this.Evaluate = evaluate;
            this.filteredEvaluateListCurrentPage = 1;
            if (this.Evaluate == 0) {
                this.filteredEvaluateList = this.EvaluateList;
                this.listText = "綜合評價";
                this.pagination();
            }
            else {
                this.filteredEvaluateList = this.EvaluateList.filter((o) => o.evaluate == this.Evaluate);
                //根據分頁設置，截取對應的數據
                this.pagination();
                switch (this.Evaluate) {
                    case 1:
                        this.listText = "一星評價";
                        break;
                    case 2:
                        this.listText = "二星評價";
                        break;
                    case 3:
                        this.listText = "三星評價";
                        break;
                    case 4:
                        this.listText = "四星評價";
                        break;
                    case 5:
                        this.listText = "五星評價";
                        break;
                    default:
                        break;
                }
            }


        },

    },

});