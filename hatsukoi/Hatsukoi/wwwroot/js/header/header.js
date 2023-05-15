const Toast = Swal.mixin({
    toast: true,
    position: 'bottom-end',
    showConfirmButton: false,
    timer: 1800,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
})

const searchPcApp = new Vue({
    el: '#search-pc',
    data: {
        isVisible: false,
        searchInput: '',
        records: [],
        keywords: ['日本', '手作', '環保杯', '辦公室', '手錶'],
    },
    methods: {
        visibleHandle() {
            this.isVisible = !this.isVisible
        },
        search() {
            if (this.searchInput.trim() !== '') {
                const input = this.searchInput.trim();
                this.records.unshift(input);
                this.searchInput = '';
                this.save();
                window.location.href = `/Search?keyword=${input}`;
            }
            
        },
        save() {
            localStorage.setItem('search-records', JSON.stringify(this.records))
            
        },
        remove(index) {
            this.records.splice(index, 1)
            this.save()
        }
    },
    computed: {
        filteredRecs() {
            return this.records.slice(0, 6)
        }
    },
    mounted() {
        const records = localStorage.getItem('search-records')
        if (records) {
            this.records = JSON.parse(records)
            this.records = this.records.slice(0, 6) // 取出最新的6筆搜尋紀錄
        }
    }
})

const searchPhoneApp = new Vue({
    el: '#search-phone',
    data: {
        isVisible: false,
        searchInput: '',
        records: [],
        keywords: ['日本', '手作', '環保杯', '辦公室', '手錶']
    },
    methods: {
        visibleHandle() {
            this.isVisible = !this.isVisible
        },
        search() {
            if (this.searchInput.trim() !== '') {
                const input = this.searchInput.trim();
                this.records.unshift(input);
                this.searchInput = '';
                this.save();
                window.location.href = `/Search?keyword=${input}`;
            }
        },
        save() {
            localStorage.setItem('search-records', JSON.stringify(this.records))
        },
        remove(index) {
            this.records.splice(index, 1)
            this.save()
        }
    },
    computed: {
        filteredRecs() {
            return this.records.slice(0, 6)
        }
    },
    mounted() {
        const records = localStorage.getItem('search-records')
        if (records) {
            this.records = JSON.parse(records)
            this.records = this.records.slice(0, 6) // 取出最新的6筆搜尋紀錄
        }
    }
})