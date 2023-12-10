adminApp.service('VendorService', function (CommonService) {
    this.Get = (data) => CommonService.Http.Post(`/admin/vendor/GetAll`, data)

    this.GetVendor = (id) => CommonService.Http.Get(`/admin/vendor/FindById?id=${id}`)

    this.AddVendor = (vendor) => CommonService.Http.Post(`/admin/vendor/add`, vendor)

    this.UpdateVendor = (vendor) => CommonService.Http.Put(`/admin/vendor/update`, vendor)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/vendor/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/vendor/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/vendor/DeleteImage?id=${id}`)
})