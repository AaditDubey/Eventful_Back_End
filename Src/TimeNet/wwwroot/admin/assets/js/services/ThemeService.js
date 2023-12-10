adminApp.service('ThemeService', function ($http) {
    this.Get = function (data) {
        return $http({
            method: 'POST',
            url: `/theme/GetAll`,
            data: data
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.GetTheme = function (id) {
        return $http({
            method: 'GET',
            url: `/theme/FindById?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.GetThemeByStoreId = function (storeId) {
        return $http({
            method: 'GET',
            url: `/theme/GetThemeByStoreId?storeId=${storeId}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.GetBaseCurrencies = function () {
        return $http({
            method: 'GET',
            url: `/api/v1/locale/getCurrencies`,
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.GetCountries = function () {
        return $http({
            method: 'GET',
            url: `/api/v1/locale/getCountries`,
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddTheme = function (theme) {
        return $http({
            method: 'POST',
            url: '/theme/add',
            data: theme
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdateTheme = function (theme) {
        return $http({
            method: 'PUT',
            url: '/theme/Update',
            data: theme
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteMany = function (listIds) {
        return $http({
            method: 'POST',
            url: '/theme/DeleteMany',
            data: listIds
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddImage = function (id, image) {
        return $http({
            method: 'POST',
            url: `/theme/AddImage?id=${id}`,
            data: image
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteImage = function (id) {
        return $http({
            method: 'DELETE',
            url: `/theme/DeleteImage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})