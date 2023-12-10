adminApp.service('BlogPostService', function (CommonService) {
    this.Get = (data) => CommonService.Http.Post(`/admin/blogPost/GetAll`, data)
  
    this.GetBlogPost = (id) => CommonService.Http.Get(`/admin/blogPost/FindById?id=${id}`)

    this.AddBlogPost = (blogPost) => CommonService.Http.Post(`/admin/blogPost/add`, blogPost)

    this.UpdateBlogPost = (blogPost) => CommonService.Http.Put(`/admin/blogPost/update`, blogPost)

    this.BulkDelete = (listIds) => CommonService.Http.Post(`/admin/blogPost/DeleteMany`, listIds)

    this.AddImage = (id, image) => CommonService.Http.Post(`/admin/blogPost/AddImage?id=${id}`, image)

    this.DeleteImage = (id) => CommonService.Http.Delete(`/admin/blogPost/DeleteImage?id=${id}`)
})