const api = {
    getUserName: '/Auth/GetUserName'
}

const apiCaller = {
    getUserName: () => httpGet(api.getUserName)
}

const authUserNameVue = new Vue({
    el: '#auth-username',
    data: {
        userName: ''
    },
    methods: {
        clickMe() {
            apiCaller.getUserName()
                .then((res) => {
                    this.userName = res.body
                    console.log(res)
                })
        }
    }
})