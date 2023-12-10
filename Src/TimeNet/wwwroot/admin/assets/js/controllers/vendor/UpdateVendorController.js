adminApp.controller('UpdateVendorController',
    function (VendorService, StorageService, $scope) {
        $scope.vendor = {
            id: '',
            name: '',
            displayOrder: 0,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
            roles: []
        }
        $scope.vendors = []
        $scope.roles = []
        $scope.init = async function (id) {
            await $scope.GetVendor(id)
        }
        $scope.GetVendor = function (id) {
            VendorService.GetVendor(id).then(function (data) {
                if (data) {
                    $scope.vendor = data;
                    console.log($scope.vendor)
                }
            });
        }
        $scope.updateVendor = async function (continueEdit) {
            if ($scope.updateVendorform.$valid) {
                loading.show()
                const vendor = await VendorService.UpdateVendor($scope.vendor)
                if (vendor) {
                    const link = continueEdit ? `/admin/vendor/update/${data.id}` : `/admin/vendor`
                    window.location.href = link
                }
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
                        VendorService.AddImage($scope.vendor.id, data).then(function (data) {
                            if (data)
                                $scope.GetVendor($scope.vendor.id)
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
                    VendorService.DeleteImage($scope.vendor.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetVendor($scope.vendor.id)
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