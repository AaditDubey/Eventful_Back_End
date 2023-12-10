adminApp.controller('UpdateCategoryController',
    function (CategoryService, StorageService, $scope) {
        $scope.category = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
        }
        $scope.categories = []
        $scope.getAllCategories = function () {
            CategoryService.GetAll().then(function (data) {
                if (data) {
                    $scope.categories = data.filter(c => c.id !== $scope.category.id);
                    $scope.categories.push({ id: '', levelName: `-- ${globalLocalizer.common.select} --` })
                }
            });
        }
        $scope.init = function (id) {
            $scope.category.id = id
            $scope.GetCategory(id)
            $scope.getAllCategories()
        }
        $scope.GetCategory = function (id) {
            CategoryService.GetCategory(id).then(function (data) {
                if (data) {
                    $scope.category = data;
                    console.log($scope.category)
                }
            });
        }
        $scope.updateCategory = async function (continueEdit) {
            if ($scope.updateCategoryform.$valid) {
                loading.show()
                var files = document.getElementById("file-upload").files;

                if (files.length > 0) {
                    var formData = new FormData();
                    formData.append("file", files[0]);
                    const fileUploaded = await StorageService.UploadFile(formData)
                    if (fileUploaded)
                        $scope.category.image = fileUploaded
                    console.log($scope.category)
                }
                CategoryService.UpdateCategory($scope.category).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/category/update/${data.id}` : `/admin/category`
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
                        CategoryService.AddImage($scope.category.id, data).then(function (data) {
                            if (data)
                                $scope.GetCategory($scope.category.id)
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
                    CategoryService.DeleteImage($scope.category.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetCategory($scope.category.id)
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