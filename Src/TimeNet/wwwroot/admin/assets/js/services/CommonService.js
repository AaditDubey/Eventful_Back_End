adminApp.service('CommonService', function ($http) {

    this.HttpHelper = function (url, data, method) {
        let content = {
            method: method,
            url: url,
        }
        if (data)
            content.data = data

        return $http(content).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            const error = response.data
            try {
                console.log(error)
                if (error && error.errorCode && error.errorDetails) {
                    const errorKey = Object.keys(error.errorDetails)[0]
                    if (errorKey) {
                        message.showErrorWithKey(errorKey)
                    }
                    else
                        message.error()

                }
            } catch (e) {
                console.log(e)
                message.error()
            }
            return null;
        });
    }
    this.HttpUploadHelper = function (url, formData) {
        return $http.post(url, formData, {
            headers: {
                'Content-Type': undefined
            },
            params: {
                formData
            },
        }).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            const error = response.data
            try {
                if (error && error.errorCode && error.errorDetails) {
                    const errorKey = Object.keys(error.errorDetails)[0]
                    if (errorKey) {
                        message.showErrorWithKey(errorKey)
                    }
                    else
                        message.error()

                }
            } catch (e) {
                console.log(e)
                message.error()
            }
            return null;
        });
    }
    this.Utils = {
        ConvertCurrency: (amount, currencyCode, countryCode) => {
            if (!countryCode)
                countryCode = "US"

            if (!currencyCode)
                currencyCode = "USD"

            if (currencyCode === "VND")
                countryCode = "VI"

            const formatCurrency = new Intl.NumberFormat(countryCode, {
                style: "currency",
                currency: currencyCode,
            });
            return formatCurrency.format(amount)
        },
        ConvertToLocalDate: (date) => {
            if (!date)
                return date
            return (new Date(date)).toLocaleString() 
        }
    }

    this.Http = {
        Post: (url, data) => {
            return this.HttpHelper(url, data, "POST")
        },
        Put: (url, data) => {
            return this.HttpHelper(url, data, "PUT")
        },
        Delete: (url, data) => {
            return this.HttpHelper(url, data, "Delete")
        },
        Get: (url, data) => {
            return this.HttpHelper(url, data, "Get")
        },
        Upload: (url, formData) => {
            return this.HttpUploadHelper(url, formData)
        }
    }
})