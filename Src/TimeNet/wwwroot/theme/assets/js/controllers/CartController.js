appModule.controller('CartController',
    function ($scope, CommonService, StateService) {
        $scope.cart = {}
        $scope.siteController = angular.element(document.getElementById("SiteController")).scope()
        $scope.$watch(
            function () { return StateService.state.cart; },
            function (newValue, oldValue) {
                if (newValue !== oldValue)
                    $scope.cart  = newValue
            }
        );
        $scope.init = function () {
        }
        
        $scope.setQty = function (id, qty) {
        
            let item = $scope.cart.items.find(i => i.id === id)
            item.quantity = item.quantity + qty
            if (item.quantity < 1)
                $scope.cart.items = $scope.cart.items.filter(i => i.id !== id)

            const subTotal = $scope.cart.items.reduce((sum, i) => { return sum + ((i.quantity || 1) * (i.price || 0)) }, 0)
            $scope.cart.subTotal = subTotal
        }

        $scope.updateCart =async function () {
            const items = $scope.cart.items.map(i => ({ id: i.id, quantity: i.quantity }))
            await $scope.siteController.UpdateCartItems(items)
        }

        $scope.deleteCartItem = async function (itemId) {
            await $scope.siteController.removeCartItems(itemId)
        }
    })
