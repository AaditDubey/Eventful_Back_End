appModule.controller('SiteController',
    function ($scope, CommonService, $timeout, StateService) {
        $scope.storeSettings = {}
        $scope.cart = {}
        $scope.searchText = ''
        $scope.searchResults = {}

        var timeout = $timeout(function () { });
        $scope.init = async function (data) {
            $scope.storeSettings = {
                currency: data.storeSettings.currency
            }
            const cart = await $scope.getCart()
            if (cart && cart.id) {
                StateService.updateState('cart', cart);
                await CommonService.Cart.UpdateCartUi(cart)
            }
        }
        $scope.convertCurrency = function (number, currency) {
            if (!currency)
                currency = $scope.storeSettings.currency
            const country = currency === 'VND' ? 'vi-VN' : 'en-US'
            let fm = new Intl.NumberFormat(country, {
                style: 'currency',
                currency: currency,
            });
            fm.format(number)
            return fm.format(number)
        }
        $scope.convertUrlImage = function (url) {
            return url && url.path && url.path ? `/${url.path}` : '/admin/assets/img/no-image.svg'
        }
        //CART
        $scope.getCart = async () => await CommonService.Cart.Get()
        $scope.addProductsToCart = async (cartItems) => {
            const cart = await CommonService.Cart.AddProductsToCart(cartItems)
            await CommonService.Cart.UpdateCartUi(cart)
            CommonService.Cart.UpdateCartTotal(cart.subTotal)
        }
        $scope.UpdateCartItems = async (cartItems) => {
            const cart = await CommonService.Cart.UpdateCartItems(cartItems)
            await CommonService.Cart.UpdateCartUi(cart)
            CommonService.Cart.UpdateCartTotal(cart.subTotal)
            StateService.updateState('cart', cart);
        }
        $scope.removeCartItems = async (itemId) => {
            const cart = await CommonService.Cart.RemoveCartItems(itemId)
            await CommonService.Cart.UpdateCartUi(cart)
            CommonService.Cart.UpdateCartTotal(cart.subTotal)
            StateService.updateState('cart', cart);
        }
  
        $scope.onSearchTextChange = () => {
            $timeout.cancel(timeout); //cancel the last timeout
            $scope.searchResults = {}
            timeout = $timeout(function () {
                console.log($scope.searchText)

                const query = { searchText: $scope.searchText, pageSize: 20 }
                 CommonService.Product.Get(query).then(function (result) {
                    if (result && result.items) {
                        $scope.searchResults = result
                    }
                });

            }, 300);
        }

        $scope.goto = function (url) {
            location.href = url
        }
    })

