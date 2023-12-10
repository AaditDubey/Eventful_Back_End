adminApp.service('NotificationService', function ($http) {
    this.GetNotification = function (id) {
        return $http({
            method: 'GET',
            url: `/admin/notification/GetNotification?id=${id}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
    
    this.GetAllNotificationsByName = function (notificationName) {
        return $http({
            method: 'GET',
            url: `/admin/notification/GetAllNotificationsByName?notificationName=${notificationName}`
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.AddNotification = function (notifications) {
        return $http({
            method: 'POST',
            url: '/admin/notification/AddNotification',
            data: notifications
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.UpdateNotification = function (notifications) {
        return $http({
            method: 'PUT',
            url: '/admin/notification/UpdateNotification',
            data: notifications
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };

    this.DeleteNotifications = function (listIds) {
        return $http({
            method: 'POST',
            url: '/admin/notification/DeleteNotifications',
            data: listIds
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            return null;
        });
    };
})