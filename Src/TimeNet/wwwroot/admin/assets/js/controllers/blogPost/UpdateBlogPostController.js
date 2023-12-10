adminApp.controller('UpdateBlogPostController',
    function (BlogPostService, StorageService, $scope, $timeout) {
        $scope.blogPost = {
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
            $scope.GetBlogPost(id)
            $timeout(function () {
                $('#tags').tagsinput('items')
            }, 100)
        }
        $scope.GetBlogPost = function (id) {
            BlogPostService.GetBlogPost(id).then(function (data) {
                if (data) {
                    $scope.blogPost = data;
                    console.log($scope.blogPost)
                }
            });
        }
        $scope.updateBlogPost = async function (continueEdit) {
            if ($scope.updateBlogPostform.$valid) {
                loading.show()
                var tags = angular.copy($scope.blogPost.tags)
                if (tags && typeof (tags) === 'string' && tags.length > 0)
                    $scope.blogPost.tags = tags.split(',')
                else
                    $scope.blogPost.tags = []
          
                BlogPostService.UpdateBlogPost($scope.blogPost).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/blogPost/update/${data.id}` : `/admin/blogPost`
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
                        BlogPostService.AddImage($scope.blogPost.id, data).then(function (data) {
                            if (data)
                                $scope.GetBlogPost($scope.blogPost.id)
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
                    BlogPostService.DeleteImage($scope.blogPost.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetBlogPost($scope.blogPost.id)
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