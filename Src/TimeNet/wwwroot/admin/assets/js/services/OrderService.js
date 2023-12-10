adminApp.service('OrderService', function (CommonService, $http) {
    this.GetOrderStatuses = function () {
        return CommonService.Http.Get(`/admin/order/getOrderStatuses`)
    };

    this.GetOrdersSummary = function () {
        return CommonService.Http.Get(`/admin/order/getOrdersSummary`)
    };

    this.AddOrder = function (order) {
        return CommonService.Http.Post(`/admin/order/add`, order)
    };

    this.UpdateOrder = function (order) {
        return CommonService.Http.Put(`/admin/order/update`, order)
    };

    this.BulkDelete = function (listIds) {
        return CommonService.Http.Post('/admin/order/DeleteMany', listIds)
    };

    this.GetOrder = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/order/FindById?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
    this.Get = (data) => CommonService.Http.Post(`/admin/order/GetAll`, data)
})