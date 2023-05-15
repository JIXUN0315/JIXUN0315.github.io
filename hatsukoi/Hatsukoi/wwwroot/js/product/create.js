let inputNumberOne = []
let inputNumberTwo = []



const app = new Vue({
    el: '#app',
    data: {
        nameVal: '',
        warnPassword: '',
        priceVal: '',
        checkNumber: '',
        subCategory: '選擇商品子分類',
        //規格部分
        //firstSpecification: '',
        //*secondSpecification: '',
        //firstSepcItem: '',
        //secondSepcItem: '',
        //unitPrice: '',
        //-----------------------
        description: '',
        editor: '',
        madeCountry: '產地',
        nameWarn: '',
        priceWarn: '',
        /*specificationWarn: ''*/
        numberVal: '',
        inputVal: '',
        warntext: '',
        //預覽照片
        preview_list: [],
        image_list: []
    },
    watch: {
        nameVal: {
            immediate: true,
            handler() {
                let r1 = /^[a-zA-Z0-9]*$/
                if (r1.test(this.nameVal)) {
                    this.warnPassword = '只能是中文'
                    this.nameVal = '' //輸入框不給他打
                } else {
                    this.warnPassword = ''
                }

                if (this.nameVal == 0) {
                    this.checkNumber = 0
                } else {
                    this.checkNumber = 60 - (this.nameVal.length * 2)
                }
            }
        },
        priceVal: function () {
            let r = /^[0-9]*$/
            if (!r.test(this.priceVal)) {
                this.priceVal = ''
            }
        },
    },
    methods: {
        addInputOne: function () {
            let btnOne = document.querySelector('.btnOne')
            let addOne = document.querySelector('.addOne')
            let input = document.createElement('input')
            input.setAttribute('class', 'form-control inputOne ')
            input.setAttribute('placeholder', '內容')
            input.setAttribute('v-model', 'firstSepcItem')
            addOne.appendChild(input)
            let inputDivOne = document.querySelector('.inputDivOne')
            let inputCountOne = inputDivOne.querySelectorAll('input')
            if (inputCountOne.length == 6) {
                btnOne.disabled = true
            }
        },
        addInputTwo: function () {
            let btnTwo = document.querySelector('.btnTwo')
            let addTwo = document.querySelector('.addTwo')
            let input = document.createElement('input')
            input.setAttribute('class', 'form-control inputTwo mx-2')
            input.setAttribute('placeholder', '內容')
            input.setAttribute('v-model', 'secondSepcItem')
            addTwo.appendChild(input)
            let inputDivTwo = document.querySelector('.inputDivTwo')
            let inputCountTwo = inputDivTwo.querySelectorAll('input')
            if (inputCountTwo.length == 6) {
                btnTwo.disabled = true
            }
        },
        SpecificationMix: function () {
            let mix = document.querySelector('.mix')
            let inputDivOne = document.querySelector('.inputDivOne')
            let inputDivTwo = document.querySelector('.inputDivTwo')
            let inputCountOne = inputDivOne.querySelectorAll('input')
            let inputCountTwo = inputDivTwo.querySelectorAll('input')
            inputNumberOne = []
            inputCountOne.forEach(x => {
                if (x.value != '') {
                    inputNumberOne.push(x.value)
                }
            })
            inputNumberTwo = []
            inputCountTwo.forEach(x => {
                if (x.value != '') {
                    inputNumberTwo.push(x.value)
                }
            })
            inputNumberOne.splice(0, 1)
            inputNumberTwo.splice(0, 1)
            mix.innerHTML = ''
            if (inputNumberOne.length > 0 && inputNumberTwo.length > 0) {
                inputNumberOne.forEach(x => {
                    inputNumberTwo.forEach(y => {
                        mix.innerHTML += `<div class="d-flex justify-content-around ppp align-items-center">
                                                                    <i class="fa-regular fa-trash-can dellIcon" ></i>
                                                                    <div class="SpecificationMixText"><span>${x}</span>  <span>+ ${y}</span></div>
                                                                        <div><span class="yyy text-danger d-flex justify-content-end"></span><input class="form-control SpecificationMixInput mt-4" placeholder="價格"  /></div>
                                                            </div>`
                    })
                })
            } else if (inputNumberOne.length > 0) {
                inputNumberOne.forEach(x => {
                    mix.innerHTML += `<div class="d-flex justify-content-around ppp align-items-center">
                                                                    <i class="fa-regular fa-trash-can dellIcon" ></i>
                                                                    <div class="SpecificationMixText"><span>${x}</span></div>
                                                                        <div><span class="yyy text-danger d-flex justify-content-end"></span><input class="form-control SpecificationMixInput mt-4" placeholder="價格"  /></div>
                                                            </div>`
                })
            } else if (inputNumberTwo.length > 0) {
                inputNumberTwo.forEach(y => {
                    mix.innerHTML += `<div class="d-flex justify-content-around ppp align-items-center">
                                                                    <i class="fa-regular fa-trash-can dellIcon" ></i>
                                                                    <div class="SpecificationMixText"><span>+ ${y}</span></div>
                                                                    <div><span class="yyy text-danger d-flex justify-content-end"></span><input class="form-control SpecificationMixInput mt-4" placeholder="價格"  /></div>
                                                            </div>`
                })
            }
            let del = document.querySelectorAll('.dellIcon')
            let ppp = document.querySelectorAll('.ppp')
            del.forEach((y, index) => {
                y.addEventListener('click', () => {
                    ppp[index].innerHTML = ''
                })
            });
            let SpecificationMixInput = document.querySelectorAll('.SpecificationMixInput')
            let yyy = document.querySelectorAll('.yyy')
            let r = /^[0-9]*$/
            SpecificationMixInput.forEach((x, index) => {
                x.addEventListener('change', function (e) {
                    if (!r.test(x.value)) {
                        //加紅框
                        x.value = ''
                    } else {
                        yyy[index].innerHTML = ''

                    }
                })
                x.addEventListener('click', function () {
                    yyy[index].innerHTML = '請輸入數字'
                    x.classList.add("mt-3")
                    x.classList.remove("mt-4")
                })
            })
        },
        previewImage: function (event) {
            let input = event.target  //輸入進去的照片
            let count = input.files.length  //輸入進去幾張照片
            let index = 0
            if (input.files) {
                while (count--) {
                    let reader = new FileReader(); //把檔案轉成base64
                    reader.onload = (e) => {
                        this.preview_list.push(e.target.result)
                    }
                    //顯示畫面
                    this.image_list.push(input.files[index]) //只要圖片 不需要這行
                    reader.readAsDataURL(input.files[index])
                    index++
                }
            }
        },
        changeCatefory: function () {
            let selectCategory = document.querySelector('#selectCategory')
            let subCategoryOption = document.querySelectorAll('.subCategoryOption')
            let selectSubCategory = document.querySelector('.sub-category')
            bigValue = selectCategory.options[selectCategory.selectedIndex].value
            subCategoryOption.forEach(y => {
                let a = y.getAttribute("data-num")
                if (a == bigValue) {
                    y.classList.remove("d-none")
                    selectSubCategory.disabled = false
                } else {
                    y.classList.add("d-none")
                }
            });
        },
        clearImg: function () {
            this.preview_list = ''
            let box = document.querySelector('.box')
            if (this.preview_list == '') {
                box.innerHTML = ''
            }
        },
        record: function () {
            let a = this
            $.ajax({
                url: '@Url.Action("Create","Product")',
                type: 'POST',
                data: {
                    ProductName: a.nameVal,
                    Price: a.priceVal,
                    SubCategoryName: a.subCategory,
                    //FirstSpecification: this.firstSpecification,
                    //SecondSpecification: this.secondSpecification,
                    //FirstSepcItem: this.firstSepcItem,
                    //SecondSepcItem: this.secondSepcItem,
                    /* UnitPrice: this.unitPrice,*/
                    Description: a.description,
                    MadeCountry: a.madeCountry,
                    Editor: a.editor
                },
                success: function (json) {
                    let result = json
                    if (result.isCreat == "") {
                        //window.location.href = "/Product/Preview"
                    } else {
                        warntext = result.isCreate
                        alert(warntext)
                    }
                }
            })
        }
    }

})