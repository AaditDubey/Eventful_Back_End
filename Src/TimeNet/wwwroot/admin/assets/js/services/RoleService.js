adminApp.service('RoleService', function ($http) {
    this.Get = function (data) {
        console.log('this.Get ', data)
        return $http({
            method: 'POST',
            url: `/admin/role/GetAll`,
            data: data
        }).then(function mySuccess(response) {
            console.log('response', response)
            return response.data;
        }, function myError(response) {
            console.log('myError', response)

            return null;
        });
    };

    this.GetRole = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/role/FindById?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddRole = function (role) {
        return $http({
            method: 'POST',
            url: '/admin/role/add',
            data: role
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdateRole = function (role) {
        return $http({
            method: 'PUT',
            url: '/role/Update',
            data: role
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
    this.DeleteMany = function (listIds) {
        return $http({
            method: 'POST',
            url: '/admin/role/DeleteMany',
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
            url: `/admin/role/AddImage?id=${id}`,
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
            url: `/admin/role/DeleteImage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})