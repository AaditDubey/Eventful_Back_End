adminApp.controller('AddProductController',
    function (ProductService, SpeakerService, CategoryService, StorageService, ProductAttributeService, $scope, $timeout) {
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
            attributes: [],
            variants: [],
            notes: []
        }
        $scope.products = []
        $scope.speakers = []
        $scope.categories = []
        $scope.attributes = []
        $scope.currentAttributeId = ''
        $scope.init = function () {
            $scope.getSpeakers();
            $scope.getAllCategories()
            $scope.getAllAttributes()
            $(".select2").select2();
            console.log('okokok')
        }

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

        $scope.getSpeakers = function (name) {
            let data = {
                pageSize: 1000
            }
            SpeakerService.Get(data).then(function (data) {
                if (data) {
                    $scope.speakers = data.items;
                    $scope.speakers.unshift({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                }
                else {
                    $scope.speakers.push({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                }
            });
        }
        $scope.getAllCategories = function () {
            CategoryService.GetAll().then(function (data) {
                if (data) {
                    $scope.categories = data;
                    $scope.categories.unshift({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                }
                else {
                    $scope.categories.push({ id: '', name: `-- ${globalLocalizer.common.select} --` })
                }
            });
        }

        $scope.addProduct = async function (continueEdit) {
            if ($scope.addProductform.$valid) {
                loading.show()
                var files = document.getElementById("file-upload").files;

                if (files.length > 0) {
                    var formData = new FormData();
                    for (var index = 0; index < files.length; index++) {
                        formData.append("files", files[index]);
                    }
                    $scope.product.images = await StorageService.UploadFiles(formData)
                }
                if ($scope.product.startDateUtc)
                    $scope.product.startDateUtc = $scope.product.startDateUtc.toISOString()

                ProductService.AddProduct($scope.product).then(function (data) {
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

        //Attributes

        $scope.addAttributes = function () {
            let id = $scope.currentAttributeId
            var ids = $scope.product.attributes.map(x => x.attributeId)
            if (ids.length > 0 && ids.includes(id)) {
                alert('exist')
            }
            else {
                const att = $scope.attributes.find(a => a.id === id)
                $scope.product.attributes.push({
                    attributeId: att.id,
                    name: att.name,
                    values: []
                })
            }
            $timeout(function () {
                $('.attributes-tag').tagsinput('items')
            }, 100)
            $scope.updateVariants()
        }

        $scope.deleteAttribute = function (attId) {
            $scope.product.attributes = $scope.product.attributes.filter(a => a.attributeId !== attId)
            $scope.updateVariants()
        }

        //variants
        $scope.updateVariants = function () {
            var attributes = angular.copy($scope.product.attributes) 
            attributes.forEach(x => {
                if (x.values && typeof (x.values) === 'string' && x.values.length > 0) {
                    x.values = x.values.split(',')
                }
            })
            if (attributes.length > 0) {
                let dictionary = Object.assign({}, ...attributes.map((x) => ({ [x.name || '']: x.values })));
                let attrs = [];

                for (const [attr, values] of Object.entries(dictionary)) {
                    if (values) {
                        attrs.push(values.map((v) => ({ [attr]: v })));
                    }
                }
                if (attrs.length > 0)
                    attrs = attrs.reduce((a,b) => a.flatMap((d) => b.map((e) => ({ ...d, ...e }))));
                    const map = attrs.map((att) => {
                    let variant = { id: utils.createGuid(), sku: '', price: 0 }
                    variant.attributes = []
                    for (const [key, value] of Object.entries(att)) {
                        variant.attributes.push({
                            key: key,
                            value: value
                        })
                    }
                    return variant
                })
                $scope.product.variants = map
                $scope.product.variants.forEach(v => {
                    v.name = v.attributes?.map((a, i) => `${i > 0 ? '/ ' : ''}${a.value}`).join(' ')
                })
            }
        }

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