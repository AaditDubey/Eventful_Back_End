appModule.controller('SuccessController',
    function ($scope, CommonService) {
        $scope.siteController = angular.element(document.getElementById("SiteController")).scope()
        $scope.order = {}
        $scope.init = function (orderNo) {
            CommonService.Order.Get(orderNo).then(function (data) {
                if (data)
                    $scope.order = data
                console.log($scope.order)
            })
        }
    })
