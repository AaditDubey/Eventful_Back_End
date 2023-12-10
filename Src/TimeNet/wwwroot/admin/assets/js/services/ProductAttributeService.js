adminApp.service('ProductAttributeService', function ($http) {
    this.Get = function (data) {
        return $http({
            method: 'POST',
            url: `/admin/attribute/GetAll`,
            data: data
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.GetProductAttribute = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/attribute/FindById?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddProductAttribute = function (productAttribute) {
        return $http({
            method: 'POST',
            url: '/admin/attribute/add',
            data: productAttribute
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdateProductAttribute = function (productAttribute) {
        return $http({
            method: 'PUT',
            url: '/admin/attribute/Update',
            data: productAttribute
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteMany = function (listIds) {
        return $http({
            method: 'POST',
            url: '/admin/attribute/DeleteMany',
            data: listIds
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})