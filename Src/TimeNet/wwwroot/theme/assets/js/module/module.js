var appModule = angular.module('TimeCommerceModule', []);

var UPDATE_STATE_DELAY = 10;

appModule.factory('StateService', ['$timeout',
    function ($timeout) {
        var service = { state: { cart: null, data: null  } };
        service.updateState = function (key, value) {
            if(key === 'cart')
                $timeout(function () {
                    if (value)
                        service.state.cart = value
                    else
                        service.state.cart = {};
                }, UPDATE_STATE_DELAY);
        };
        return service;
    }]);