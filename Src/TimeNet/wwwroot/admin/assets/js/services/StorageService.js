adminApp.service('StorageService', function (CommonService) {
    this.UploadFile = function (formData) {
        return CommonService.Http.Upload(`/admin/storage/uploadFile`, formData)
    };
    this.UploadFiles = function (formData) {
        return CommonService.Http.Upload(`/admin/storage/uploadFiles`, formData)
    };

    this.DeleteFiles = function (paths) {
        return CommonService.Http.Post(`/admin/storage/DeleteFiles`, paths)
    };
})