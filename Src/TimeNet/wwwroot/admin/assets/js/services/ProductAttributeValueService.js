adminApp.service('ProductAttributeValueService', function ($http) {
    this.GetProductAttributeValue = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/productAttributeValue/GetProductAttributeValue?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddOrUpdateProductAttributeValue = function (productAttributeValue) {
        let method = 'POST', url = 'AddProductAttributeValue'
        if (productAttributeValue.id) {
            method = 'PUT'
            url = 'UpdateProductAttributeValue'
        }
        return $http({
            method: method,
            url: `/admin/productAttributeValue/${url}`,
            data: productAttributeValue
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteProductAttributeValues = function (ids) {
        return $http({
            method: 'POST',
            url: `/admin/productAttributeValue/DeleteProductAttributeValues`,
            data: ids
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddOrUpdateProductProductAttributeValue = function (productAttributeValue) {
        let method = 'POST', url = 'AddProductAttributeValue'
        if (productAttributeValue.id) {
            method = 'PUT'
            url = 'UpdateProductAttributeValue'
        }
        return $http({
            method: method,
            url: `/admin/productAttributeValue/${url}`,
            data: productAttributeValue
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddImage = function (id, formData) {
        return $http.post(`/admin/productAttributeValue/addImage?id=${id}`, formData, {
            headers: {
                'Content-Type': undefined
            },
            params: {
                formData
            },
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteImage = function (id) {
        return $http({
            method: 'DELETE',
            url: `/admin/productAttributeValue/DeleteImage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})