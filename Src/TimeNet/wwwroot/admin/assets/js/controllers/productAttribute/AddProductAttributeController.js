adminApp.controller('AddProductAttributeController',
    function (ProductAttributeService, $scope, $timeout) {
        $scope.productAttribute = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
            parentId: '',
            optionValues: []
        }
        $scope.productAttributes = []

        $scope.init = function () {
            loading.hide()
        }

        //new
        $scope.addOptionValues = function () {
            $scope.productAttribute.optionValues.push({
                id: utils.createGuid(),
                name: ''
            })
         
            $scope.setPickerColor()
        }
        $scope.deleteOptionValue = function (id) {
            $scope.productAttribute.optionValues = $scope.productAttribute.optionValues.filter(o => o.id !== id)
        }
        
        $scope.onColorAttributeChange = function () {
            $scope.setPickerColor()
        }

        $scope.setPickerColor = function () {
            $timeout(function () {
                $('.showAlpha').spectrum({
                    preferredFormat: "hex",
                    //color: '#000000',
                    showInput: true,
                });
            }, 50)
        }


        $scope.addProductAttribute = function (continueEdit) {
            if ($scope.addProductAttributeform.$valid) {
                loading.show()
                ProductAttributeService.AddProductAttribute($scope.productAttribute).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/attribute/update/${data.id}` : `/admin/attribute`
                        window.location.href = link
                    }
                    else {
                        message.error()
                    }
                    loading.show()
                });
            } else {
                return false;
            }
        }
    })