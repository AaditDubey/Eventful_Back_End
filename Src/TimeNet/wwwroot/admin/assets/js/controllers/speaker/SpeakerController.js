adminApp.controller('SpeakerController',
    function (SpeakerService, ReportService, $scope, $timeout, $http) {
        let speaker_table = null,
            getSpeakersUrl = '/admin/speaker/get'
        $scope.deleteids_arr = []
        $scope.listActiveFilter = globalLocalizer.activeTabValues
        let firstLoading = false
        $scope.FilterModel = {
            Published: null,
            SearchText: searchText
        }
        $timeout(function () {
            speaker_table = $('#speaker_table').DataTable({
                "dom": '<"row"rt"<"col-sm-12 col-md-6 table-paging-customize"l><"col-sm-12 col-md-6 table-paging-customize"p>>',
                "language": globalLocalizer.datatableLanguageConfig,
                "processing": true,
                "serverSide": true,
                "ajax": {
                    "url": getSpeakersUrl,
                    "type": "POST",
                    data: {
                        'FilterModel.SearchText': function () { return $scope.FilterModel.SearchText },
                        'FilterModel.Published': function () { return $scope.FilterModel.Published }
                    },
                    beforeSend: function () {
                        loading.show()
                        if (!firstLoading) {
                            loadSkeletonTable('speaker_table')
                        }
                    },
                    complete: function () {
                        loading.hide()
                        if (!firstLoading) firstLoading = true
                    },
                    error: function () {
                        message.error()
                        $(`#speaker_table tbody tr`).empty()
                        $('#speaker_table_processing').hide()
                    },
                },
                columnDefs: [
                    { orderable: false, targets: 0 },
                    { orderable: false, targets: 1 },
                    { orderable: false, targets: 3 }
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
                        data: 'Images',
                        render: function (col, type, row, meta) {
                            let html = '<td>'
                            html += `<img src=${$scope.convertUrlImage(row.image)} style="height: 50px;" />`
                            html += '</td>'
                            $scope.convertUrlImage(row.images)
                            return html
                        }
                    },
                    {
                        data: 'Name',
                        render: function (col, type, row) {
                            let html = `<a href="/admin/speaker/update/${row.id}" class="table-row-key">${row.name}</a>`
                            return html;
                        }
                    },
                    {
                        data: 'Published',
                        render: function (col, type, row) {
                            return row.description
                        }
                    },
                    {
                        data: 'CreatedOn',
                        render: function (col, type, row) {
                            return row.createdOn ? (new Date(row.createdOn)).toLocaleString() : '';
                        }
                    },
                    {
                        data: 'UpdatedOn',
                        render: function (col, type, row) {
                            return row.updatedOn ? (new Date(row.updatedOn)).toLocaleString() : '';
                        }
                    }
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

        $scope.getSpeakers = function () {
            speaker_table.ajax.reload();
        }
        $scope.setActive = function (val) {
            $scope.FilterModel.Published = val
            $scope.getSpeakers();
        }
        $scope.deleteSpeakers = function () {
            SpeakerService.BulkDelete($scope.deleteids_arr).then(function (data) {
                if (data) {
                    message.deleteSuccess()
                    $scope.getSpeakers()
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
                $('#speaker_table input[type="checkbox"]').prop('checked', true);
                $('#speaker_table input[type="checkbox"].delete_check:checked').each(function () {
                    $scope.deleteids_arr.push($(this).val());
                });
            }
            else {
                $scope.deleteids_arr = []
                $('#speaker_table input[type="checkbox"]').prop('checked', false);
            }
            $scope.updateTableUi()
        }

        $scope.deleteSelected = function () {
            $scope.deleteSpeakers()
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
            const parms = speaker_table.ajax.params()
            const pageIndex = parms.start / parms.length + 1
            const pageSize = parms.length
            const published = $scope.FilterModel.Published
            const searchText = $scope.FilterModel.SearchText
            const language = utils.getCurrentLanguage()
            let url = `/admin/reports/ExportSpeakerReport?published=${published}&pageIndex=${pageIndex}&pageSize=${pageSize}&language=${language}`
            if (searchText)
                url += `&searchText=${searchText}`

            const order = parms.order
            if (order && order.length > 0) {
                const OrderBy = parms.columns[order[0].column].data
                const ascending = order[0].dir === 'asc'
                url += `&OrderBy=${OrderBy}&ascending=${ascending}`
            }
            const fileName = language === 'vi' ? 'thuong_hieu.xlsx' : 'speakers_data_export.xlsx';
            await ReportService.Export(url, fileName)
        }
    })