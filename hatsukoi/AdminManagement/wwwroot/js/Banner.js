let bannerApp = new Vue({
    el: '#banner-app',
    data: {
        imgList: [],
        dragOptions: {
            animation: 200,
            group: 'description',
            disabled: false,
            ghostClass: 'ghost'
        },
        drag: false,
        edit: false,
        deleteId: '',
        isbusy: true,
    },
    created() {
        this.loadImages();
    },
    methods: {
        loadImages() {
            this.isbusy = true;
            axios.get('/api/Banner/GetBanner')
                .then(res => {
                    this.isbusy = false;
                    this.imgList = res.data.body;
                })
                .catch(error => {
                    console.log(error);
                });
        },
        createImg: function (event) {
            this.isbusy = true;
            var input = event.target;
            var sortNum = this.imgList.length
            if (input.files) {
                var reader = new FileReader();
                reader.onload = (e) => {
                    var base64Img = e.target.result;
                    axios.post('/api/Banner/CreateBanner', { "BannerImg": base64Img, "SortNum": sortNum })
                        .then(res => {
                            this.loadImages();
                            this.isbusy = false;
                        })
                        .catch(error => {
                            console.log(error);
                        });
                }
                reader.readAsDataURL(input.files[0]);
            }


        },
        updateSort() {
            this.edit = false
            this.isbusy = true;
            this.imgList.forEach((img, index) => img.sort = index + 1);
            axios.post('/api/Banner/UpdateBanner', { "imgList": this.imgList })
                .then(res => {
                    this.loadImages();
                })
                .catch(error => {
                    console.log(error);
                });
        },
        toggleDrag() {
            this.drag = !this.drag;
            this.edit = true
        },
        showModal() {
            this.deleteId = $(event.target).data("banner-id") + "";

            $('#deleteModal').modal('show');
        },
        deleteBanner() {
            this.isbusy = true;
            $('#deleteModal').modal('hide');
            console.log(this.imgList)
            const index = this.imgList.findIndex(img => img.id == this.deleteId)
            this.imgList.splice(index, 1)

            axios.post('/api/Banner/DeleteBanner', { "Id": this.deleteId })
                .then(res => {
                    this.updateSort();
                })
                .catch(error => {
                    console.log(error);
                });
        },
        closeModal() {
            $('#deleteModal').modal('hide');
        }
    }
})
