adminApp.controller('UpdateBrandController',
    function (BrandService, StorageService, $scope, $timeout) {
        $scope.brand = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
        }
        $scope.init = function (id) {
            $scope.GetBrand(id)
        }
        $scope.GetBrand = function (id) {
            BrandService.GetBrand(id).then(function (data) {
                if (data) {
                    $scope.brand = data;
                    console.log($scope.brand)
                }
            });
        }
        $scope.updateBrand = async function (continueEdit) {
            if ($scope.updateBrandform.$valid) {
                loading.show()
                BrandService.UpdateBrand($scope.brand).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/brand/update/${data.id}` : `/admin/brand`
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
                        BrandService.AddImage($scope.brand.id, data).then(function (data) {
                            if (data)
                                $scope.GetBrand($scope.brand.id)
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
                    BrandService.DeleteImage($scope.brand.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetBrand($scope.brand.id)
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