adminApp.controller('AddProductByAiController',
    function (ProductService, BrandService, CategoryService, StorageService, $scope) {
        $scope.product = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
            brandId: '',
            createShortDescription: false,
            createMetaKeywords: false,
            createMetaTitle: false,
            createMetaDescription: false,
            removeBackground: false,
            resize: false,
            suggestCategory: false,
            optimiseDescription: false,
            images: []
        }
        $scope.products = []
        $scope.brands = []
        $scope.categories = []
        $scope.isCheckAllCategories = false

        $scope.init = function () {
            console.log("AddProductByAiController")
            $(".select2").select2();
            //loading.hide()
            $('#review-modal').modal()
        }

        $scope.addProduct = function () {
            if ($scope.addProductform.$valid) {
                $('#proccessing-modal').show()
                var files = $scope.imagesReview
                if (files.length > 0) {
                    var formData = new FormData();
                    for (var index = 0; index < files.length; index++) {
                        formData.append("files", files[index]);
                    }
                    StorageService.UploadFilesWithAI(formData).then(function (data) {
                        if (data) {
                            console.log("uploadImages result")
                            console.log(data)
                            //$scope.imagesEdited = data
                            $scope.product.images = data
                            $scope.chatGpt()
                        }
                        else
                            message.error()
                    });
                }
                else
                    $scope.chatGpt()
            } else {
                return false;
            }
        };

        $scope.editProduct = function () {
            location.href = `/product/update/${$scope.product.id}`
        }


        $scope.chatGpt = function () {
            $scope.processingTask = "Data processing..."
            ProductService.AddProductByAi($scope.product).then(function (data) {
                //loading.hide()
                if (data) {
                    console.log('AddProductByAi')
                    console.log(data)
                    $scope.product = data
                }
                else {
                    message.error()
                }
                $('#proccessing-modal').hide()
                $('#success-modal').modal()

            });
        }


        $scope.imagesReview = []
        $scope.testValue = "value"

        $scope.addImage = async function (event) {
            console.log('changed addImage')
            const xx = document.getElementById("images-review")

            //loading.show()
            var formData = new FormData();
            $scope.testValue = "changed"
            console.log($scope.testValue)

            // Read selected files
            var totalfiles = document.getElementById('file-upload').files.length;
            for (var index = 0; index < totalfiles; index++) {

                console.log('file' + index)
                console.log(document.getElementById('file-upload').files[index])
                const file = document.getElementById('file-upload').files[index]
                var img = document.createElement("img");

                const url = await readURL(file);
                img.className = 'col-3'
                img.src = url;

                let html = '<div class="card d-flex flex-row mb-3">'
                html += `<img src="${url}" class="list-thumbnail responsive border-0">`
                html += '</div>'
                var str = '<p>Just some <span>text</span> here</p>';
                var temp = document.createElement('div');
                temp.className = 'card d-flex flex-row mb-3'
                temp.innerHTML = img;
                xx.appendChild(img);


                $scope.imagesReview.push(file)
                console.log($scope.imagesReview.length)

                formData.append("files", document.getElementById('file-upload').files[index]);
            }
            //loading.show()
            //StorageService.UploadFiles(formData).then(function (data) {
            //    let isSuccess = true
            //    loading.show()
            //    if (data) {
            //        ProductService.AddImages($scope.product.id, data).then(function (data) {
            //            if (data)
            //                $scope.GetProduct($scope.product.id)
            //            else
            //                isSuccess = false
            //        });
            //    }
            //    else {
            //        isSuccess = false

            //    }
            //    if (!isSuccess)
            //        message.error()
            //    loading.hide()
            //});
        }
        $scope.processingTask = "Image editor processing..."
        //$scope.imagesEdited = []
        $scope.uploadImages = function () {
            console.log('$scope.uploadImages')
            //loading.show()
            //// Read selected files
            var files = $scope.imagesReview
            if (files.length > 0) {
                var formData = new FormData();
                for (var index = 0; index < files.length; index++) {
                    formData.append("files", files[index]);
                }
                StorageService.UploadFilesWithAI(formData).then(function (data) {
                    let isSuccess = true
                    if (data) {
                        console.log("uploadImages result")
                        console.log(data)
                        $scope.imagesEdited = data
                        return true
                    }
                    else {
                        return false
                    }
                });
            }

            //var totalfiles = document.getElementById('file-upload').files.length;
            //for (var index = 0; index < totalfiles; index++) {
            //    formData.append("files", document.getElementById('file-upload').files[index]);
            //}
            //loading.show()
            //StorageService.UploadFiles(formData).then(function (data) {
            //    let isSuccess = true
            //    loading.show()
            //    if (data) {
            //        ProductService.AddImages($scope.product.id, data).then(function (data) {
            //            if (data)
            //                $scope.GetProduct($scope.product.id)
            //            else
            //                isSuccess = false
            //        });
            //    }
            //    else {
            //        isSuccess = false

            //    }
            //    if (!isSuccess)
            //        message.error()
            //    loading.hide()
            //});
        }
    })

const readURL = file => {
    return new Promise((res, rej) => {
        const reader = new FileReader();
        reader.onload = e => res(e.target.result);
        reader.onerror = e => rej(e);
        reader.readAsDataURL(file);
    });
};