appModule.controller('CheckoutController',
    function ($scope, CommonService) {
        $scope.siteController = angular.element(document.getElementById("SiteController")).scope()
        $scope.countries = [
            {
                id: 'VN',
                name: 'Viet Nam',
            },
            {
                id: 'UK',
                name: 'United Kindom (UK)',
            },
            {
                id: 'USA',
                name: 'United States (USA)',
            },
            {
                id: 'Australia',
                name: 'Australia',
            },
            {
                id: 'England',
                name: 'England',
            },
            {
                id: 'Switzerland',
                name: 'Switzerland',
            },
          
        ]
        $scope.order = {
            paymentMethod: "COD",
            shippingAddress: {
                countryId: $scope.countries[0].id
            },
            billingAddress: {
                countryId: $scope.countries[0].id
            }
        }
        $scope.init = function () {
            CommonService.Cart.Get().then(function (data) {
                if (data)
                    $scope.cart = data
            })
        }

        $scope.string = ''
        $scope.checkout = async function () {
            if ($scope.checkoutform.$valid) {
                var useDifferentAddress = $('#useDifferentAddress').is(':checked');
                if (!useDifferentAddress)
                    $scope.order.billingAddress = $scope.order.shippingAddress

                $scope.order = {
                    ...$scope.order,
                    cartId: CommonService.Cart.GetCartId(),
                    customerEmail: $scope.order.shippingAddress.email,
                    firstName: $scope.order.shippingAddress.firstName,
                    lastName: $scope.order.shippingAddress.lastName
                }

                var order = await CommonService.Order.Checkout($scope.order)
                if (order)
                {
                    CommonService.Utils.Clear()
                    window.location.href = `/success?order=${order.id}`
                }
                else
                    alert('Some things went wrong. Try again, please!')

            } else {
                return false;
            }
        };
    })
