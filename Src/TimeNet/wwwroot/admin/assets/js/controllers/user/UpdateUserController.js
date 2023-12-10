adminApp.controller('UpdateUserController',
    function (UserService, RoleService, VendorService, $scope) {
        $scope.user = {
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
        $scope.users = []
        $scope.roles = []
        $scope.vendors = []
        $scope.init = async function (id) {
            await $scope.GetUser(id)

            await $scope.getRoles()
            await $scope.getVendors()
        }
        $scope.GetUser = function (id) {
            UserService.GetUser(id).then(function (data) {
                if (data) {
                    $scope.user = data;
                    $scope.user.roles = $scope.user.roles.map(u => u.id)
                    console.log($scope.user)
                }
            });
        }
        $scope.getRoles = function () {
            let data = {
                pageSize: 1000
            }
            RoleService.Get(data).then(function (data) {
                if (data && data.items) {
                    $scope.roles = data.items
                    console.log($scope.roles)
                }
                else
                    $scope.roles.push({ id: '', name: `-- ${globalLocalizer.common.select} --` })
            });
        };

        $scope.getVendors = function () {
            let data = {
                pageSize: 1000
            }
            VendorService.Get(data).then(function (data) {
                if (data && data.items) {
                    $scope.vendors = data.items
                    $scope.vendors.unshift({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                }
                else
                    $scope.vendors.push({ id: '', name: `-- ${globalLocalizer.common.select} --` })
            });
        };
        $scope.updateUser = async function (continueEdit) {
            if ($scope.updateUserform.$valid) {
                loading.show()
                const user = await UserService.UpdateUser($scope.user)
                if (user)
                    window.location.href = continueEdit ? `/admin/user/update/${data.id}` : `/admin/user`

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