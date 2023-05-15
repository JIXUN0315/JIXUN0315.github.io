
const app = new Vue({
    el: '#app',
    data: {
        //true代表在上架區那頁
        isChanged: true,
        allChecked: false,
        sortText: '',
        perPage: 6,
        currentPage: 1,
        shelveCurrentPage: 1,
        offShelveCurrentPage: 1,
        //後端傳來的資料
        products: [],
        //要傳回後端的資料
        productIdList: []
    },
    mounted() {
        //請求data
        this.getProducts();
    },
    computed: {
        shelveProductList() {
            return this.products.filter(p => p.productStatus === 2);
        },
        offShelveProductList() {
            return this.products.filter(p => p.productStatus === 1);
        },
        productsByStatus() {
            return {
                shelve: this.products.filter(p => p.productStatus === 2),
                offShelve: this.products.filter(p => p.productStatus === 1)
            };
        },
        paginatedData() {
            const currentList = this.isChanged ? 'shelveProductList' : 'offShelveProductList';
            this.currentPage = this.isChanged ? this.shelveCurrentPage : this.offShelveCurrentPage;
            const start = (this.currentPage - 1) * this.perPage;
            const end = start + this.perPage;
            return this[currentList].slice(start, end);
        }
    },
    methods: {
        //Tab切換分頁
        tabChange() {
            this.isChanged = !this.isChanged;
            this.allChecked = false;
        },
        //切換每區頁數
        updatePerPage() {
            this.shelveCurrentPage = 1;
            this.offShelveCurrentPage = 1;
            this.allChecked = false;
        },
        //獲取後端傳來的json資料
        getProducts() {
            $.ajax({
                url: '/api/product/GetProducts',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                success: (response) => {
                    if (response.status === 0) {
                        this.products = []
                        response.result.forEach(x => {
                            x.check = false
                            this.products.push(x)
                        })

                        //資料筆數為0時顯示圖
                        if (this.products.length === 0) {
                            const img = $('<img>').attr('src', '/img/product/img-tag-empty.png');
                            $('.product-list').append(img);
                        }
                    }
                    this.allChecked = false;
                }
            })
        },
        //批次處理 選項值
        batch(event) {
            const batchValue = event.target.value;
            if (batchValue === 'offShelve_multiple') {
                this.offShelveProducts();
            } else if (batchValue === 'delete_multiple') {
                this.deleteProducts();
            } else if (batchValue === 'shelve_multiple') {
                this.shelveProducts();
            }

            // 批次處理結束後切回預設值
            setTimeout(() => {
                event.target.value = 'batch_org';
            }, 1600);
        },
        //多選
        checkBoxAll() {
            const currentList = this.isChanged ? 'shelveProductList' : 'offShelveProductList';
            this.currentPage = this.isChanged ? this.shelveCurrentPage : this.offShelveCurrentPage;
            const start = (this.currentPage - 1) * this.perPage;
            const end = start + this.perPage;
            const productsOnPage = this[currentList].slice(start, end);

            if (this.allChecked) {
                productsOnPage.forEach(product => {
                    product.check = true;
                })
            } else {
                productsOnPage.forEach(product => {
                    product.check = false;
                })
            }
        },
        //排序 選項值
        sort(event) {
            const sortValue = event.target.value;

            if (sortValue === 'sort_org') {
                this.getProducts();
                sortText = '預設值';
            }
            else if (sortValue === 'time_asc') {
                this.sortByCreateTime('asc');
                sortText = '上架時間：舊 → 新';

            } else if (sortValue === 'time_desc') {
                this.sortByCreateTime('desc');
                sortText = '上架時間：新 → 舊';

            } else if (sortValue === 'name_asc') {
                this.sortByName('asc');
                sortText = '商品名稱：A → Z';

            } else if (sortValue === 'name_desc') {
                this.sortByName('desc');
                sortText = '商品名稱：Z → A';

            } else if (sortValue === 'price_asc') {
                this.sortByPrice('asc');
                sortText = '商品價格：低 → 高';

            } else if (sortValue === 'price_desc') {
                this.sortByPrice('desc');
                sortText = '商品價格：高 → 低';
            }
            const sortListStatus = document.querySelector('.sort-list-status');
            sortListStatus.textContent = '排序依據：' + sortText;
        },
        //排序 名稱
        sortByName(card) {
            //不區分大小寫和重音符號(中英混和比較)
            this.products.sort((a, b) => {
                let comparison = a.productName.localeCompare(b.productName, 'zh-TW', { sensitivity: 'accent' });
                return card === 'asc' ? comparison : -comparison;
            });
            Vue.set(this, 'products', this.products);
        },
        //排序 價格
        sortByPrice(card) {
            this.products.sort((a, b) => {
                let comparison = a.price - b.price;
                return card === 'asc' ? comparison : -comparison;
            });
            Vue.set(this, 'products', this.products);
        },
        //排序 上架時間(比較id大小)
        sortByCreateTime(card) {
            this.products.sort((a, b) => {
                let comparison = a.productId - b.productId;
                return card === 'asc' ? comparison : -comparison;
            });
            Vue.set(this, 'products', this.products);
        },
        //下架(單)
        offShelveOneProduct(productId) {
            $.ajax({
                url: '/api/product/offShelveProduct',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    productManageDtos: [{ productId: productId }]
                }),
                success: () => {
                    app.getProducts();
                }
            })
        },
        //下架(多)
        offShelveProducts() {
            //this.products.forEach((p) => {
            //    if (p.checked) {
            //        this.productIdList.push({ productId: p.productId });
            //        p.isVisible = !p.isVisible;
            //    }
            //})

            const selectedProducts = this.products.filter(p => p.check == true);
            if (selectedProducts.length > 0) {
                const productIdList = selectedProducts.map(p => ({ productId: p.productId }));
                $.ajax({
                    url: '/api/product/offShelveProduct',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        productManageDtos: productIdList
                    }),
                    success: () => {
                        app.getProducts();
                    }
                });
            }
        },
        //重新上架(單)
        shelveOneProduct(productId) {
            $.ajax({
                url: '/api/product/ShelveProduct',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    productManageDtos: [{ productId: productId }]
                }),
                success: () => {
                    app.getProducts();
                }
            })
        },
        //重新上架(多)
        shelveProducts() {
            const selectedProducts = this.products.filter(p => p.check == true);
            if (selectedProducts.length > 0) {
                const productIdList = selectedProducts.map(p => ({ productId: p.productId }));

                $.ajax({
                    url: '/api/product/ShelveProduct',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        productManageDtos: productIdList
                    }),
                    success: () => {
                        app.getProducts();
                    }
                })
            }
        },
        //刪除(單)
        deleteOneProduct(productId) {
            this.$bvModal.msgBoxConfirm('確定刪除資料 ?', {
                title: '警告',
                size: 'sm',
                okVariant: 'danger',
                okTitle: '確定',
                cancelVariant: 'outline-secondary',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: true,
                centered: true,
                headerBgVariant: 'danger',
                headerTextVariant: 'light',

            }).then(value => {
                if (value !== true) { return };

                $.ajax({
                    url: '/api/product/DeleteProduct',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        productManageDtos: [{ productId: productId }]
                    }),
                    success: () => {
                        app.getProducts();
                    }
                })

            })

        },
        //刪除(多)
        deleteProducts() {
            this.$bvModal.msgBoxConfirm('確定刪除資料 ?', {
                title: '警告',
                size: 'sm',
                okVariant: 'danger',
                okTitle: '確定',
                cancelVariant: 'outline-secondary',
                cancelTitle: '取消',
                footerClass: 'p-2',
                hideHeaderClose: true,
                centered: true,
                headerBgVariant: 'danger',
                headerTextVariant: 'light',

            }).then(value => {
                if (value !== true) { return };

                const selectedProducts = this.products.filter(p => p.check);
                if (selectedProducts.length > 0) {
                    const productIdList = selectedProducts.map(p => ({ productId: p.productId }));

                    $.ajax({
                        url: '/api/product/DeleteProduct',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify({
                            productManageDtos: productIdList
                        }),
                        success: () => {
                            app.getProducts();
                        }
                    })
                }

            })


        }
    }
});