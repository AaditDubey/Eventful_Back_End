adminApp.controller('UpdateContactMessageController',
    function (ContactMessageService, $scope) {
        $scope.contactMessage = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
            roles: []
        }
        $scope.contactMessages = []
        $scope.init = async function (id) {
            await $scope.GetContactMessage(id)
        }
        $scope.GetContactMessage = function (id) {
            ContactMessageService.GetContactMessage(id).then(function (data) {
                if (data) {
                    $scope.contactMessage = data;
                    console.log(data)
                }
            });
        }
      
        $scope.updateContactMessage = async function (continueEdit) {
            if ($scope.updateContactMessageform.$valid) {
                loading.show()
                const contactMessage = await ContactMessageService.UpdateContactMessage($scope.contactMessage)
                if (contactMessage)
                    window.location.href = continueEdit ? `/admin/contactMessage/update/${data.id}` : `/admin/contactMessage`

                loading.hide()
            } else {
                return false;
            }
        };
    })