adminApp.controller('UpdateContentController',
    function (ContentService, StorageService, $scope, $timeout) {
        $scope.content = {
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
            $scope.GetContent(id)
            $timeout(function () {
                $('#tags').tagsinput('items')
            }, 100)
        }
        $scope.GetContent = function (id) {
            ContentService.GetContent(id).then(function (data) {
                if (data) {
                    $scope.content = data;
                    console.log($scope.content)
                }
            });
        }
        $scope.updateContent = async function (continueEdit) {
            if ($scope.updateContentform.$valid) {
                loading.show()
                var tags = angular.copy($scope.content.tags)
                if (tags && typeof (tags) === 'string' && tags.length > 0)
                    $scope.content.tags = tags.split(',')
                else
                    $scope.content.tags = []
          
                ContentService.UpdateContent($scope.content).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/content/update/${data.id}` : `/admin/content`
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

        //summernote
        $scope.imageUpload = async function (files) {
            console.log('statr imageUpload')

            let formData = new FormData();
            formData.append("file", files[0]);
            const fileUploaded = await StorageService.UploadFile(formData)
            if (fileUploaded) {
                console.log('upload success', fileUploaded)
                $('#summernote').summernote('insertImage', `${location.origin}/${fileUploaded.path}`);
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
                        ContentService.AddImage($scope.content.id, data).then(function (data) {
                            if (data)
                                $scope.GetContent($scope.content.id)
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
                    ContentService.DeleteImage($scope.content.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetContent($scope.content.id)
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