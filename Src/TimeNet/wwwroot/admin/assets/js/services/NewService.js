adminApp.service('NewService', function ($http) {
    this.GetNew = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/new/GetNew?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
    
    this.GetAllNewsByName = function (newName) {
        return $http({
            method: 'GET',
            url: `/admin/new/GetAllNewsByName?newName=${newName}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddNew = function (news) {
        return $http({
            method: 'POST',
            url: '/admin/new/AddNew',
            data: news
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdateNew = function (news) {
        return $http({
            method: 'PUT',
            url: '/admin/new/UpdateNew',
            data: news
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteNews = function (listIds) {
        return $http({
            method: 'POST',
            url: '/admin/new/DeleteNews',
            data: listIds
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddImage = function (id, formData) {
        return $http.post(`/admin/new/addImage?id=${id}`, formData, {
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
            url: `/admin/new/DeleteImage?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})