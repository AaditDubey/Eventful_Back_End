adminApp.controller('DashboardController',
    function (OrderService, UserService, ProductService, $scope) {
        $scope.users = []
        $scope.orders = []
        $scope.products = []
        $scope.ordersSummary = {}
        $scope.init = function () {
            $scope.getUsers()
            $scope.getOrders()
            $scope.getProducts()
            $scope.getOrdersSummary()
        }
        $scope.getUsers = function () {
            UserService.Get({ pageSize: 6 }).then(function (result) {
                if (result.items) {
                    $scope.users = result.items
                    const bgs = ['bg-info', 'bg-danger', 'bg-pink', 'bg-success', 'bg-primary']
                    $scope.users.forEach(u => {
                            u.bg = `${bgs[Math.floor(Math.random() * 5)]}`
                    })
                }
            });
        }
        $scope.getOrders = function () {
            OrderService.Get({ pageSize: 10 }).then(function (result) {
                if (result.items) {
                    $scope.orders = result.items
                    $scope.orders.forEach(o => {
                        let cls = 'badge bg-pill '
                        const status = globalLocalizer.sales.orderStatuses.find(s => s.id === o.orderStatus).name
                        switch (o.orderStatus) {
                            case 'Complete':
                                cls += 'bg-primary-light'
                                break;
                            case 'Pending':
                                cls += 'bg-warning-light'
                                break;
                            case 'Cancelled':
                                cls += 'bg-danger-light'
                                break;
                            default:
                                cls += 'bg-success-light'
                                break;
                        }
                        o.class = cls
                        o.orderStatus = status
                    })
                }
            });
        }
        $scope.getOrdersSummary = function () {
            OrderService.GetOrdersSummary().then(function (result) {
                if (result) {
                    $scope.ordersSummary = result
                    $scope.ordersSummary.salePercent = 0
                    const percent = $scope.ordersSummary.todaySales * 100 / $scope.ordersSummary.lastDaySales
                    $scope.ordersSummary.salesPercent = Math.round(percent) - 100

                    $scope.ordersSummary.orderPercent = 0
                    const orderPercent = $scope.ordersSummary.todayOrders * 100 / $scope.ordersSummary.lastDayOrders
                    $scope.ordersSummary.orderPercent = Math.round(orderPercent) - 100
                }
            });
        }
        $scope.getProducts = function () {
            ProductService.FindProducts({ pageSize: 10 }).then(function (result) {
                if (result.items) {
                    $scope.products = result.items
                    console.log($scope.products)
                }
            });
        }
        $scope.goToDetailPage = function (type, id) {
            let url = '#'
            switch (type) {
                case 'user':
                    url = `/admin/user/update/${id}`
                    break;
                case 'product':
                    url = `/admin/event/update/${id}`
                    break;
                default:
                    url = '#'
                    break;
            }
            location.href = url
        }

        $scope.convertDate = function (date) {
            if (!date)
                return ''
            let d = (new Date(date)).toLocaleString();
            if (d.includes(','))
                d = d.split(',')[0]
            return d
        }

        $scope.convertUrlImage = function (url) {
            return url && url.path && url.path ? `/${url.path}` : '/admin/assets/img/no-image.svg'
        }
    })