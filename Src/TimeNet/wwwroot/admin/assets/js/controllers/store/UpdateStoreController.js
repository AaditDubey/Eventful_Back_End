adminApp.controller('UpdateStoreController',
    function (StoreService, StorageService, $scope, LocaleService) {
        $scope.store = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
        }
        $scope.currencies = []
        $scope.init = function (id) {
            $scope.GetStore(id)
            $scope.getCurrencies()
        }
        $scope.GetStore = function (id) {
            StoreService.GetStore(id).then(function (data) {
                if (data) {
                    $scope.store = data;
                }
            });
        }
        $scope.templateResult = function (currency) {
            if (!currency.id) { return currency.text; }
            const country = $scope.currencies.find(x => x.code === currency.id)
            const countryName = country?.countryCode.toLowerCase()
            const name = country?.name
            const imageUrl = `${location.origin}/images/flags/${countryName}.svg`
            //if (countryName === 'undefined')
            //    return currency.text;
            var $currency = $(`<img class="flag-currency" src="${imageUrl}" />` + '<span>' + name + '</span>');
            return $currency;
        };
        $scope.getCurrencies = function () {
            LocaleService.GetCurrencies().then(function (data) {
                if (data) {
                    $scope.currencies = data
                    $("#currencies-select").select2({
                        templateResult: $scope.templateResult,
                        templateSelection: $scope.templateResult
                    });
                }
                else
                    message.error()
            });
        }
        $scope.updateStore = async function (continueEdit) {
            if ($scope.updateStoreform.$valid) {
                loading.show()
                var files = document.getElementById("file-upload").files;

                if (files.length > 0) {
                    var formData = new FormData();
                    formData.append("file", files[0]);
                    const fileUploaded = await StorageService.UploadFile(formData)
                    if (fileUploaded)
                        $scope.store.image = fileUploaded
                    console.log($scope.store)
                }
                StoreService.UpdateStore($scope.store).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/store/update/${data.id}` : `/admin/store`
                        window.location.href = link
                    }
                    else
                        message.error()
                });
                loading.hide()
            } else {
                return false;
            }
        };

        //image
        $scope.addImage = function (event) {
            var file = document.getElementById('file-upload').files[0];
            if (file) {
                var formData = new FormData();
                formData.append("file", file);
                loading.show()
                StorageService.UploadFile(formData).then(function (data) {
                    let isSuccess = true
                    loading.show()
                    if (data) {
                        StoreService.AddImage($scope.store.id, data).then(function (data) {
                            if (data)
                                $scope.GetStore($scope.store.id)
                            else
                                isSuccess = false
                        });
                    }
                    else
                        isSuccess = false

                    if (!isSuccess)
                        message.error()
                    loading.hide()
                });
            }
       
        }
        $scope.deleteImage = function (imageId) {
            loading.show()
            const deleteIds = [imageId]
            StorageService.DeleteFiles(deleteIds).then(function (data) {
                let isSuccess = true
                if (data) {
                    StoreService.DeleteImage($scope.store.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetStore($scope.store.id)
                        else
                            isSuccess = false
                    });

                }
                else
                    isSuccess = false

                if (!isSuccess)
                    message.error()
            });
            loading.hide()
        }

        $scope.convertUrlImage = function (url) {
            return url && url.path && url.path ? `/${url.path}` : '/admin/assets/img/no-image.svg'
        }
    })