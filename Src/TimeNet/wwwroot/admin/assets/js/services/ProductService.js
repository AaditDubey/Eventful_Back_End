adminApp.service('ProductService', function (CommonService) {
  
    this.GetProduct = (id) => CommonService.Http.Get(`/admin/product/FindById?id=${id}`)

    this.FindProducts = (data) => CommonService.Http.Post(`/admin/product/FindProducts`, data)

    this.AddProduct = (product) => CommonService.Http.Post(`/admin/product/add`, product)

    this.UpdateProduct = (product) => CommonService.Http.Put(`/admin/product/update`, product)

    this.AddImages = (id, images) => CommonService.Http.Post(`/admin/product/AddImages?id=${id}`, images)

    this.DeleteImage = (id, imageId) => CommonService.Http.Delete(`/admin/product/DeleteImage?id=${id}&imageId=${imageId}`)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/product/DeleteMany`, listIds)

    this.GetAttributeControlType = () => CommonService.Http.Get(`/admin/product/GetAttributeControlType`)

    this.AddAttribute = (productId, attribute) => CommonService.Http.Post(`/admin/product/AddAttribute?id=${productId}`, attribute)

    this.UpdateAttribute = (productId, attribute) => CommonService.Http.Put(`/admin/product/UpdateAttribute?id=${productId}`, attribute)

    this.DeleteAttribute = (productId, attributeId) => CommonService.Http.Delete(`/admin/product/DeleteAttribute?id=${productId}&attributeId=${attributeId}`)

    this.AddVariant = (productId, variant) => CommonService.Http.Post(`/admin/product/AddVariant?id=${productId}`, variant)

    this.UpdateVariant = (productId, variant) => CommonService.Http.Put(`/admin/product/UpdateVariant?id=${productId}`, variant)

    this.DeleteVariant = (productId, variantId) => CommonService.Http.Delete(`/admin/product/DeleteVariant?id=${productId}&variantId=${variantId}`)

    this.ApplyVariants = (productId, variants) => CommonService.Http.Put(`/admin/product/ApplyVariants?id=${productId}`, variants)
})