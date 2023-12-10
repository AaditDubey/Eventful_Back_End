adminApp.service('PageService', function ($http) {
    this.GetPage = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/page/GetPage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
    
    this.GetAllPagesByName = function (pageName) {
        return $http({
            method: 'GET',
            url: `/admin/page/GetAllPagesByName?pageName=${pageName}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddPage = function (pages) {
        return $http({
            method: 'POST',
            url: '/admin/page/AddPage',
            data: pages
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdatePage = function (pages) {
        return $http({
            method: 'PUT',
            url: '/admin/page/UpdatePage',
            data: pages
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeletePages = function (listIds) {
        return $http({
            method: 'POST',
            url: '/admin/page/DeletePages',
            data: listIds
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddImage = function (id, formData) {
        return $http.post(`/admin/page/addImage?id=${id}`, formData, {
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
            url: `/admin/page/DeleteImage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})