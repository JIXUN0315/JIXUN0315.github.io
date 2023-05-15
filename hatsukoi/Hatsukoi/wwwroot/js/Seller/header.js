const headerApp = new Vue({
    el: '#header-app',
    data: {
        searchText: '',
    },
  
    methods: {
        searchOrders() {
            window.location.href = '/OrderSearch?q=' + this.searchText;
        }
    }
})
const Toast = Swal.mixin({
    toast: true,
    position: 'bottom-end',
    showConfirmButton: false,
    timer: 800,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
})