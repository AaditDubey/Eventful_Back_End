adminApp.service('CouponService', function (CommonService) {
    this.Get = (data) => CommonService.Http.Post(`/admin/coupon/GetAll`, data)
  
    this.GetCoupon = (id) => CommonService.Http.Get(`/admin/coupon/FindById?id=${id}`)

    this.AddCoupon = (coupon) => CommonService.Http.Post(`/admin/coupon/add`, coupon)

    this.UpdateCoupon = (coupon) => CommonService.Http.Put(`/admin/coupon/update`, coupon)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/coupon/DeleteMany`, listIds)
})