adminApp.controller('UpdateSpeakerController',
    function (SpeakerService, StorageService, $scope) {
        $scope.speaker = {
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
            $scope.GetSpeaker(id)
        }

        $scope.addInfo = function () {
            $scope.speaker.genericAttributes.push({ key: '', value: '' })
        }
        $scope.deleteInfor = function (key) {
            $scope.speaker.genericAttributes = $scope.speaker.genericAttributes.filter(d => d.key !== key)
        }
        $scope.GetSpeaker = function (id) {
            SpeakerService.GetSpeaker(id).then(function (data) {
                if (data) {
                    $scope.speaker = data;
                    console.log($scope.speaker)
                }
            });
        }
        $scope.updateSpeaker = async function (continueEdit) {
            if ($scope.updateSpeakerform.$valid) {
                loading.show()
                SpeakerService.UpdateSpeaker($scope.speaker).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/speaker/update/${data.id}` : `/admin/speaker`
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
                        SpeakerService.AddImage($scope.speaker.id, data).then(function (data) {
                            if (data)
                                $scope.GetSpeaker($scope.speaker.id)
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
                    SpeakerService.DeleteImage($scope.speaker.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetSpeaker($scope.speaker.id)
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