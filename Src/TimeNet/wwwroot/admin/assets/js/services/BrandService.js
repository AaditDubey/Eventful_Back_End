adminApp.service('BrandService', function (CommonService) {
    this.Get = (data) => CommonService.Http.Post(`/admin/brand/GetAll`, data)
  
    this.GetBrand = (id) => CommonService.Http.Get(`/admin/brand/FindById?id=${id}`)

    this.AddBrand = (brand) => CommonService.Http.Post(`/admin/brand/add`, brand)

    this.UpdateBrand = (brand) => CommonService.Http.Put(`/admin/brand/update`, brand)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/brand/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/brand/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/brand/DeleteImage?id=${id}`)
})