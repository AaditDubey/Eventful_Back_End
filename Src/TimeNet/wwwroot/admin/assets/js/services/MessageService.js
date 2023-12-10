adminApp.service('MessageService', function ($http) {
    this.Get = function (data) {
        return $http({
            method: 'POST',
            url: `/message/GetAll`,
            data: data
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.GetMessage = function (id) {
        return $http({
            method: 'GET',
            url: `/message/FindById?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddMessage = function (message) {
        return $http({
            method: 'POST',
            url: '/message/add',
            data: message
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdateMessage = function (message) {
        return $http({
            method: 'PUT',
            url: '/message/Update',
            data: message
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteMany = function (listIds) {
        return $http({
            method: 'POST',
            url: '/message/DeleteMany',
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
            url: `/message/AddImage?id=${id}`,
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
            url: `/message/DeleteImage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})