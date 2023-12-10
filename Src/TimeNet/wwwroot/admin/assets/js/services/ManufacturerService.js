adminApp.service('ManufacturerService', function ($http) {
    this.GetManufacturer = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/manufacturer/GetManufacturer?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
    
    this.GetAllManufacturersByName = function (manufacturerName) {
        return $http({
            method: 'GET',
            url: `/admin/manufacturer/GetAllManufacturersByName?manufacturerName=${manufacturerName}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddManufacturer = function (manufacturer) {
        return $http({
            method: 'POST',
            url: '/admin/manufacturer/AddManufacturer',
            data: manufacturer
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdateManufacturer = function (manufacturer) {
        return $http({
            method: 'PUT',
            url: '/admin/manufacturer/UpdateManufacturer',
            data: manufacturer
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteManufacturers = function (listIds) {
        return $http({
            method: 'POST',
            url: '/admin/manufacturer/DeleteManufacturers',
            data: listIds
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddImage = function (id, formData) {
        return $http.post(`/admin/manufacturer/addImage?id=${id}`, formData, {
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
            url: `/admin/manufacturer/DeleteImage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.ImportExcelManufacturer = function (formData) {
        return $http.post(`/admin/manufacturer/ImportExcelManufacturer`, formData, {
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
})