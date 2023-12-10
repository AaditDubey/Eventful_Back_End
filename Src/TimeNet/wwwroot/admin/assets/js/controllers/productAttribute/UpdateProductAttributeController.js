adminApp.controller('UpdateProductAttributeController',
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
     
        $scope.init = function (id) {
            $scope.getProductAttribute (id)
        }

        $scope.getProductAttribute = function (id) {
            ProductAttributeService.GetProductAttribute(id).then(function (data) {
                loading.hide()
                console.log(data)
                if (data) {
                    $scope.productAttribute = data
                    $scope.setPickerColor()
                }
                else {
                    message.error()
                }
            });
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

        $scope.updateAttribute = function (continueEdit) {
            if ($scope.updateAttributeform.$valid) {
                loading.show()
                console.log($scope.productAttribute)
                ProductAttributeService.UpdateProductAttribute($scope.productAttribute).then(function (data) {
              
                    console.log(data)
                    if (data) {
                        const link = continueEdit ? `/admin/attribute/update/${data.id}` : `/admin/attribute`
                        window.location.href = link
                    }
                    else {
                        message.error()
                    }
                    loading.hide()
                });
            } else {
                return false;
            }
        }
    })