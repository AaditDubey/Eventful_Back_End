adminApp.service('ReportService', function ($http) {
    this.Export = function (url, fileName) {
        loading.show()
        return $http({
            method: 'GET',
            responseType: 'blob',
            url: url,
        }).then(function mySuccess(response) {
            const blob = response.data
            if (blob) {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = url;
                // the filename you want
                a.download = fileName
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
                // Remove the element
                document.body.removeChild(a);
            }
            else
                message.error()
            loading.hide()
        }, function myError(response) {
            message.error()
            loading.hide()
        });
    };
})