adminApp.controller('AddContactMessageController',
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
        }
        $scope.contactMessages = []

        $scope.init = function () {
        }
    
        $scope.addContactMessage = async function (continueEdit) {
            if ($scope.addContactMessageform.$valid) {
                loading.show()
                $scope.contactMessage.type = 'Message'
                const contactMessage = await ContactMessageService.AddContactMessage($scope.contactMessage)
                if (contactMessage) {
                    const link = continueEdit ? `/admin/contactMessage/update/${data.id}` : `/admin/contactMessage`
                    window.location.href = link
                }
                loading.hide()
            } else {
                return false;
            }
        };
    })