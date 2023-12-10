adminApp.controller('AddVendorController',
    function (VendorService, StorageService, $scope) {
        $scope.vendor = {
            id: '',
            name: '',
            displayOrder: 0,
            active: true,
            metaKeywords: '',
            metaDescription: '',
            metaTitle: '',
            description: '',
        }
        $scope.vendors = []
        $scope.roles = []
        $scope.init = function () {
        }
        
        $scope.addVendor = async function (continueEdit) {
            if ($scope.addVendorform.$valid) {
                loading.show()
                var files = document.getElementById("file-upload").files;

                if (files.length > 0) {
                    var formData = new FormData();
                    formData.append("file", files[0]);
                    const fileUploaded = await StorageService.UploadFile(formData)
                    if (fileUploaded)
                        $scope.vendor.image = fileUploaded
                    console.log($scope.vendor)
                }
                const vendor = await VendorService.AddVendor($scope.vendor)
                if (vendor) {
                    const link = continueEdit ? `/admin/vendor/update/${data.id}` : `/admin/vendor`
                    window.location.href = link
                }
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