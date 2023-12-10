adminApp.controller('ContactMessageController',
    function (ContactMessageService, ReportService, $scope, $timeout) {
        let contactMessage_table = null,
            getContactMessagesUrl = '/admin/contactMessage/get'
        $scope.deleteids_arr = []
        $scope.listActiveFilter = [
            {
                value: null,
                name: "All"
            },
            {
                value: 'Message',
                name: "Message"
            },
            {
                value: 'Subscribe',
                name: "Subscribe"
            }
        ]
        let firstLoading = false
        $scope.FilterModel = {
            Type: null,
            SearchText: searchText
        }
        $timeout(function () {
            contactMessage_table = $('#contactMessage_table').DataTable({
                "dom": '<"row"rt"<"col-sm-12 col-md-6 table-paging-customize"l><"col-sm-12 col-md-6 table-paging-customize"p>>',
                "language": globalLocalizer.datatableLanguageConfig,
                "processing": true,
                "serverSide": true,
                "ajax": {
                    "url": getContactMessagesUrl,
                    "type": "POST",
                    data: {
                        'FilterModel.SearchText': function () { return $scope.FilterModel.SearchText },
                        'FilterModel.Type': function () { return $scope.FilterModel.Type }
                    },
                    beforeSend: function () {
                        loading.show()
                        if (!firstLoading) {
                            loadSkeletonTable('contactMessage_table')
                        }
                    },
                    complete: function () {
                        loading.hide()
                        if (!firstLoading) firstLoading = true
                    },
                    error: function () {
                        message.error()
                        $(`#contactMessage_table tbody tr`).empty()
                        $('#contactMessage_table_processing').hide()
                    },
                },
                columnDefs: [
                    { orderable: false, targets: 0 },
                    { orderable: false, targets: 4 },
                    { orderable: false, targets: 5 },
                    { orderable: false, targets: 6 }
                ],
                order: [],
                "columns": [
                    {
                        data: 'Id',
                        render: function (col, type, row, meta) {
                            let html = '<td>'
                            html += `<label class="custom-control custom-checkbox">`
                            html += `<input type="checkbox" class="custom-control-input delete_check" value="${row.id}" onchange="angular.element(this).scope().selectedIdChange(this)" />`
                            html += `<span class="custom-control-label">&nbsp;</span>`
                            html += `</label>`
                            html += '</td>'

                            return html
                        }
                    },
                    {
                        data: 'Email',
                        render: function (col, type, row) {
                            return row.email;
                        }
                    },
                    {
                        data: 'FullName', render: (col, type, row) => row.fullName
                    },
                    
                    {
                        data: 'PhoneNumber', render: (col, type, row) => row.phoneNumber
                    },
                    {
                        data: 'Subject', render: (col, type, row) => row.subject
                    },
                    {
                        data: 'Content', render: (col, type, row) => row.content
                    },
                    {
                        data: 'Id',
                        render: function (col, type, row, meta) {
                            let html = `<a href="/admin/contactMessage/update/${row.id}" class="table-row-key"><i class="ti ti-pencil"></i></a>`
                            return html
                        }
                    },
                ]
            });
        }, 0);
        $scope.deleteIds = []
        $scope.categories = []

        $scope.isSelectedAll = false
        $scope.init = function () {
            $(".select2").select2();
            loading.hide()
        }

        $scope.getContactMessages = function () {
            contactMessage_table.ajax.reload();
        }
        $scope.setActive = function (val) {
            $scope.FilterModel.Type = val
            $scope.getContactMessages();
        }
        $scope.deleteContactMessages = function () {
            ContactMessageService.BulkDelete($scope.deleteids_arr).then(function (data) {
                if (data) {
                    message.deleteSuccess()
                    $scope.getContactMessages()
                    $scope.deleteids_arr = []
                    $scope.updateTableUi()
                    $scope.isSelectedAll = false
                }
                else
                    message.error()
                $('#modal-comfirm-delete').modal('hide')
            });
        }

        $scope.selectedAll = function () {
            if ($scope.isSelectedAll) {
                $scope.deleteids_arr = []
                $('#contactMessage_table input[type="checkbox"]').prop('checked', true);
                $('#contactMessage_table input[type="checkbox"].delete_check:checked').each(function () {
                    $scope.deleteids_arr.push($(this).val());
                });
            }
            else {
                $scope.deleteids_arr = []
                $('#contactMessage_table input[type="checkbox"]').prop('checked', false);
            }
            $scope.updateTableUi()
        }

        $scope.deleteSelected = function () {
            $scope.deleteContactMessages()
        }

        $scope.convertUrlImage = function (url) {
            return url && url.path && url.path ? `/${url.path}` : '/admin/assets/img/no-image.svg'
        }

        $scope.selectedIdChange = function (e) {
            var id = e.value
            var checked = e.checked
            if (checked && !$scope.deleteids_arr.find(i => i === id)) {
                $scope.deleteids_arr.push(id)
            }
            else if (!checked && $scope.deleteids_arr.find(i => i === id)) {
                $scope.deleteids_arr = $scope.deleteids_arr.filter(i => i !== id)
            }
            $scope.updateTableUi()

        }
        $scope.updateTableUi = function () {
            if ($scope.deleteids_arr.length > 0) {
                $('#delete-selected-button').show()
                $('#table-tab').hide()
            }
            else {
                $('#delete-selected-button').hide()
                $('#table-tab').show()
            }
        }

        $scope.export = async function () {
            const parms = contactMessage_table.ajax.params()
            const pageIndex = parms.start / parms.length + 1
            const pageSize = parms.length
            const published = $scope.FilterModel.Active
            const searchText = $scope.FilterModel.SearchText
            const language = utils.getCurrentLanguage()
            let url = `/admin/reports/ExportContactMessageReport?published=${published}&pageIndex=${pageIndex}&pageSize=${pageSize}&language=${language}`
            if (searchText)
                url += `&searchText=${searchText}`

            const order = parms.order
            if (order && order.length > 0) {
                const OrderBy = parms.columns[order[0].column].data
                const ascending = order[0].dir === 'asc'
                url += `&OrderBy=${OrderBy}&ascending=${ascending}`
            }
            const fileName = language === 'vi' ? 'thuong_hieu.xlsx' : 'contactMessages_data_export.xlsx';
            await ReportService.Export(url, fileName)
        }
    })