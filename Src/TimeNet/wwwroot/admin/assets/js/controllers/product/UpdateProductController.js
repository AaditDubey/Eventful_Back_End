adminApp.controller('UpdateProductController',
    function (StorageService, ProductService, SpeakerService, CategoryService, ProductAttributeService, $timeout, $scope) {
        $scope.product = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
            speakerId: '',
            images: []
        }
        $scope.attributesTypes = [
            { id: "", name: "Choose value" },
            { id: "DropdownList", name: "Dropdown list" },
            { id: "RadioList", name: "Radio list" },
            { id: "ColorSquares", name: "Color squares" },
            { id: "MultilineTextbox", name: "Multiline text box" }
        ]
        $scope.categories = []
        $scope.productAttributes = [{}]
        $scope.isCheckAllCategories = false
        $scope.imageUpdate = {
            id: '',
            alt: '',
            displayOrder: 1
        }
       
        $scope.attributes = []
        $scope.currentAttributeId = ''
        $scope.getAllAttributes = function () {
            let data = {
                pageSize: 1000
            }
            ProductAttributeService.Get(data).then(function (data) {
                if (data) {
                    $scope.attributes = data.items;
                    $scope.currentAttributeId = $scope.attributes[0].id
                }
                else {
                    $scope.speakers.push({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                }
            });
        }
        $scope.init = function (id) {
            $scope.product.id = id
            $scope.GetProduct($scope.product.id)
            $scope.getSpeakers();
            $scope.getAllAttributes()
            console.log($scope.attributes)
            //loading.hide()
          
        }

        $scope.GetProduct = function (id) {
            ProductService.GetProduct(id).then(function (data) {
                if (data) {
                    $scope.product = data;
                    if ($scope.product.startDateUtc)
                        $scope.product.startDateUtc = new Date($scope.product.startDateUtc)
                    console.log($scope.product)
                    var list = $scope.product.productCategoryMapping.map(x => x.categoryId)
                    $scope.product.categories = list
                    CategoryService.GetAll().then(function (data) {
                        if (data) {
                            $scope.categories = data;
                            $(".select2").select2();
                        }
                    });
                }
            });
        }

        $scope.getSpeakers = function (name) {
            let data = {
                pageSize: 1000
            }
            SpeakerService.Get(data).then(function (data) {
                if (data) {
                    $scope.speakers = data.items;
                    $scope.speakers.unshift({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                }
            });
        }

        $scope.getAllCategories = function () {
            CategoryService.GetAll().then(function (data) {
                if (data) {
                    $scope.categories = data;
                }
            });
        }

        $scope.updateProduct = function (continueEdit) {
            if ($scope.updateProductform.$valid) {
                loading.show()
                if ($scope.product.startDateUtc)
                    $scope.product.startDateUtc = $scope.product.startDateUtc.toISOString()
                ProductService.UpdateProduct($scope.product).then(function (data) {
                    if (data) {
                        const link = continueEdit ? `/admin/event/update/${data.id}` : `/admin/event`
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

        $scope.deleteImage = function (imageId) {
            loading.show()
            const deleteIds = [imageId]
            StorageService.DeleteFiles(deleteIds).then(function (data) {
                let isSuccess = true
                if (data) {
                    ProductService.DeleteImage($scope.product.id, imageId).then(function (data) {
                        if (data)
                            $scope.GetProduct($scope.product.id)
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

        $scope.editImage = function (imageId) {
            $scope.imageUpdate.id = imageId
            $('#update-image-modal').modal('show')
        }

        //upload
        $scope.attribute = {
            required: true
        }
        $scope.addImage = function (event) {
            loading.show()
            var formData = new FormData();

            // Read selected files
            var totalfiles = document.getElementById('file-upload').files.length;
            for (var index = 0; index < totalfiles; index++) {
                formData.append("files", document.getElementById('file-upload').files[index]);
            }
            loading.show()
            StorageService.UploadFiles(formData).then(function (data) {
                let isSuccess = true
                loading.show()
                if (data) {
                    ProductService.AddImages($scope.product.id, data).then(function (data) {
                        if (data)
                            $scope.GetProduct($scope.product.id)
                        else
                            isSuccess = false
                    });
                }
                else {
                    isSuccess = false

                }
                if (!isSuccess)
                    message.error()
                loading.hide()
            });
        }

        $scope.note = '';
        $scope.addNote = function () {
            if ($scope.note) {
                $scope.product.notes.push($scope.note)
                $scope.note = '';
            }
        }
        $scope.deleteNote = function (note) {
            $scope.product.notes = $scope.product.notes.filter(n => n !== note)
        }
    })