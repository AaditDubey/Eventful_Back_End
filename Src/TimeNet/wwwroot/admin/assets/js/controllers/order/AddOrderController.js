adminApp.controller('AddOrderController',
    function (OrderService, UserService, ProductService, $scope, $timeout) {
        $scope.shippingStatuses = globalLocalizer.sales.shippingStatuses
        $scope.paymentStatuses = globalLocalizer.sales.paymentStatuses
        $scope.order = {
            id: '',
            orderItems: [],
            shippingStatus: $scope.shippingStatuses[0].id,
            paymentStatus: $scope.paymentStatuses[0].id
        }
        $scope.editNote = false
        $scope.init = function () { }

        $scope.addOrder = async function (continueEdit) {
            if ($scope.addOrderform.$valid) {
                loading.show()
                if ($scope.order.customer) {
                    $scope.order.customerId = $scope.order.customer.id
                    $scope.order.customerEmail = $scope.order.customer.email
                    $scope.order.firstName = $scope.order.customer.firstName
                    $scope.order.lastName = $scope.order.customer.lastName
                }
                const order = await OrderService.AddOrder($scope.order)
                if (order) {
                    const link = continueEdit ? `/admin/order/update/${data.id}` : `/admin/order`
                    window.location.href = link
                }
                loading.hide()
            } else {
                return false;
            }
        };

       //Customer
        var timeout = $timeout(function () { });
        $scope.customerSearchText = ''
        $scope.customers = []

        $scope.clearCustomerSearching = function () {
            $scope.customerSearchText = ''
            $scope.customers = []
        }

        $scope.addCustomer = function () {
            $('#modal-customer').modal('show')
            $scope.clearCustomerSearching()
        }
        $scope.createNewCustomer = function () {
            window.open("/admin/user/add", "_blank", "width=640,height=480");
        }

        $scope.searchOnChange = function () {
            $timeout.cancel(timeout); //cancel the last timeout
            $scope.customers = []
            timeout = $timeout(function () {
                const query = { searchText: $scope.customerSearchText, pageSize: 20 }
                UserService.Get(query).then(function (result) {
                    if (result && result.items) {
                        result.items.forEach(i => {
                            $scope.customers.push(i)
                        })
                    }
                });
                 
            }, 300);
        }

        $scope.selectCustomer = function (id) {
            $scope.order.customer = $scope.customers.find(c => c.id === id)
            $('#modal-customer').modal('hide')
        }

        //Address
        $scope.addOrUpdateAddress = { }
        $scope.openAddOrUpdateAddressModal = function (type) {
            if (type === 'shippingAddress') {
                $scope.addOrUpdateAddress = { ...angular.copy($scope.order.shippingAddress) , type: type}
            }
            else if (type === 'billingAddress') {
                $scope.addOrUpdateAddress = { ...angular.copy($scope.order.billingAddress), type: type }//angular.copy($scope.order.billingAddress)
            }
            $('#modal-add-or-update-address').modal('show')
        }
        $scope.saveAddress = function () {
            if ($scope.addOrUpdateAddress.type === 'shippingAddress') {
                $scope.order.shippingAddress = $scope.addOrUpdateAddress
                if ($scope.order.useShippingAddress)
                    $scope.order.billingAddress = $scope.addOrUpdateAddress
            }
            else if ($scope.addOrUpdateAddress.type === 'billingAddress') {
                $scope.order.billingAddress = $scope.addOrUpdateAddress
            }
        }

        $scope.useShippingAddressOnChange = function () {
            if ($scope.order.useShippingAddress)
                $scope.order.billingAddress = angular.copy($scope.order.shippingAddress)
            else
                $scope.order.billingAddress = { id: '' }
        }

        //addProductItem
        $scope.productSearchText = ''
        $scope.products = []
        $scope.currentItem = {}
        $scope.addOrderItems = function () {
            $scope.productSearchText = ''
            $scope.products = []
            $scope.currentItem = {}
            $('#modal-add-order-items').modal('show')
        }
       
        $scope.productSearchTextOnChange = function () {
            $timeout.cancel(timeout); //cancel the last timeout
            $scope.currentItem = {}
            $scope.products = []
            timeout = $timeout(function () {
                const query = { searchText: $scope.productSearchText, pageSize: 20 }
                ProductService.FindProducts(query).then(function (result) {
                    if (result && result.items) {
                        result.items.forEach(i => {
                            $scope.products.push(i)
                        })
                    }
                });

            }, 300);
        }

        $scope.convertUrlImage = function (url) {
            return url && url.path && url.path ? `/${url.path}` : '/admin/assets/img/no-image.svg'
        }
        $scope.convertToUrlImage = function (url) {
            return url ? `/${url}` : '/admin/assets/img/no-image.svg'
        }
        $scope.addOrderItem = function (id) {
      
            const product = $scope.products.find(p => p.id === id)
            $scope.currentItem = 
            {
                quantity: 1,
                productId: product.id,
                productName: product.name,
                pictureThumbnailUrl: product.images && product.images[0] ? product.images[0].path : '',
                attributes: product.attributes,
                variants: product.variants,
                priceInclTax: product.price,
                priceExclTax: product.price,
            }

            $scope.currentItem.attributes.forEach(a => {
                $scope.currentItem[a.name] = a.values.length > 0 ? a.values[0] : ''
            })

            $scope.productSearchText = ''
            $scope.products = []
        }

        $scope.saveOrderItem = function () {
            let atts = []
            $scope.currentItem.attributes.forEach(a => {
                atts.push({
                    key: a.name,
                    value: $scope.currentItem[a.name]
                })
            })
            $scope.currentItem.attributes = atts

            for (let i = 0; i < $scope.currentItem.variants.length; i++) {
                const v = $scope.currentItem.variants[i]
                const attributes = v.attributes
                if ($scope.currentItem.attributes.length === attributes.length) {
                    const result = $scope.currentItem.attributes.every((v, i) => v.key === attributes[i].key && v.value === attributes[i].value);
                    if (result) {
                        $scope.currentItem.priceInclTax = v.price
                        $scope.currentItem.priceExclTax = v.price
                        break
                    }
                }
            }
            $scope.order.orderItems.push({ ...$scope.currentItem, id: utils.createGuid() })
            $('#modal-add-order-items').modal('hide')
            $scope.calculateOrderTotal()
        }

        $scope.getVariant = function (attributes) {
            return attributes && attributes.length > 0 ? attributes.map(x => x.value).join('/') : '--'
        }

        $scope.deleteOrderItem = function (id) {
            $scope.order.orderItems = $scope.order.orderItems.filter(i => i.id !== id)
            $scope.calculateOrderTotal()
        }

        $scope.calculateOrderTotal = function () {
            const total = $scope.order.orderItems.reduce((sum, i) => { return sum + ((i.quantity || 1) * (i.priceExclTax || 0)) }, 0)
            $scope.order.orderSubtotalInclTax = total
        }
    })
