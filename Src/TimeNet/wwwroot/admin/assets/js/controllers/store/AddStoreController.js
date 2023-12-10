adminApp.controller('AddStoreController',
    function (StoreService, StorageService, LocaleService, $scope) {
        $scope.store = {
            id: '',
            name: '',
            displayOrder: 0,
            published: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
            defaultCurrency: 'USD'
        }
        $scope.currencies = []
        $scope.init = function () {
            $scope.getCurrencies()
        }
        $scope.templateResult = function (currency) {
            if (!currency.id) { return currency.text; }
            const country = $scope.currencies.find(x => x.code === currency.id)
            const countryName = country?.countryCode.toLowerCase()
            const name = country?.name
            const imageUrl = `${location.origin}/images/flags/${countryName}.svg`
            //if (countryName === 'undefined')
            //    return currency.text;
            var $currency = $(`<img class="flag-currency" src="${imageUrl}" />` + '<span>' + name + '</span>');
            return $currency;
        };
        $scope.getCurrencies = function () {
            LocaleService.GetCurrencies().then(function (data) {
                if (data) {
                    $scope.currencies = data
                    $("#currencies-select").select2({
                        templateResult: $scope.templateResult,
                        templateSelection: $scope.templateResult
                    });
                }
                else
                    message.error()
            });
        }

        $scope.addStore = async function (continueEdit) {
            if ($scope.addStoreform.$valid) {
                loading.show()
                var files = document.getElementById("file-upload").files;

                if (files.length > 0) {
                    var formData = new FormData();
                    formData.append("file", files[0]);
                    const fileUploaded = await StorageService.UploadFile(formData)
                    if (fileUploaded)
                        $scope.store.logo = fileUploaded
                    console.log($scope.store)
                }
                const store = await StoreService.AddStore($scope.store)
                if (store) {
                    const link = continueEdit ? `/admin/store/update/${data.id}` : `/admin/store`
                    window.location.href = link
                }
                else
                    message.error()
                loading.hide()
            } else {
                return false;
            }
        };


        //upload
        var fileArr = [];
        $('body').on('click', '#action-icon', function (evt) {
            var divName = this.value;
            var fileName = $(this).attr('role');
            $(`#${divName}`).remove();

            for (var i = 0; i < fileArr.length; i++) {
                if (fileArr[i].name === fileName) {
                    fileArr.splice(i, 1);
                }
            }
            document.getElementById('file-upload').files = FileListItem(fileArr);
            evt.preventDefault();
        });
        function FileListItem(file) {
            file = [].slice.call(Array.isArray(file) ? file : arguments)
            for (var c, b = c = file.length, d = !0; b-- && d;) d = file[b] instanceof File
            if (!d) throw new TypeError("expected argument to FileList is File or array of File objects")
            for (b = (new ClipboardEvent("")).clipboardData || new DataTransfer; c--;) b.items.add(file[c])
            return b.files
        }

        $scope.addImage = function (event) {
            if (fileArr.length > 0) fileArr = [];
            $('#image_preview').html("");
            var total_file = document.getElementById("file-upload").files;
            console.log(total_file)
            if (!total_file.length) return;
            for (var i = 0; i < total_file.length; i++) {
                if (total_file[i].size > 1048576) {
                    return false;
                } else {
                    fileArr.push(total_file[i]);
                    $('#image_preview').append("<div class='img-div' id='img-div" + i + "'><img src='" + URL.createObjectURL(total_file[i]) + "' class='img-responsive image img-thumbnail' title='" + total_file[i].name + "'><div class='middle'><button id='action-icon' value='img-div" + i + "' class='btn btn-danger' role='" + total_file[i].name + "'><i class='fa fa-trash'></i></button></div></div>");
                }
            }
        }
    })