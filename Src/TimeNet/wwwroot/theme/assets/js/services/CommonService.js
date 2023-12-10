appModule.service('CommonService', function ($http) {

    this.HttpHelper = function (url, data, method) {
        let content = {
            method: method,
            url: url,
        }
        if (data)
            content.data = data

        return $http(content).then(function mySuccess(response) {
            return response.data;
        }, function myError(response) {
            const error = response.data

            if(response.status !== 200)
                console.log('error response.status', response.status)
            try {
                console.log(error)
                if (error && error.errorCode && error.errorDetails) {
                    const errorKey = Object.keys(error.errorDetails)[0]
                    if (errorKey) {
                        console.log('errorKey', errorKey)

                        message.showErrorWithKey(errorKey)
                    }
                    else {
                        console.log('err', e)

                        message.error()
                    }

                }
            } catch (e) {
                console.log('err catch', e)
                message.error()
            }
            return null;
        });
    }
    this.Utils = {
        ConvertCurrency: (amount, currencyCode, countryCode) => {
            if (!countryCode)
                countryCode = "US"

            if (!currencyCode)
                currencyCode = "USD"

            if (currencyCode === "VND")
                countryCode = "VI"

            const formatCurrency = new Intl.NumberFormat(countryCode, {
                style: "currency",
                currency: currencyCode,
            });
            return formatCurrency.format(amount)
        },
        ConvertToLocalDate: (date) => {
            if (!date)
                return date
            return (new Date(date)).toLocaleString() 
        },
        Clear : () => {
            localStorage.clear()
        }
    }

    this.Http = {
        Post: (url, data) => {
            return this.HttpHelper(url, data, "POST")
        },
        Put: (url, data) => {
            return this.HttpHelper(url, data, "PUT")
        },
        Delete: (url, data) => {
            return this.HttpHelper(url, data, "Delete")
        },
        Get: (url, data) => {
            return this.HttpHelper(url, data, "Get")
        },
        Upload: (url, formData) => {
            return this.HttpUploadHelper(url, formData)
        }
    }

    this.Storage = {
        Get: (key) => localStorage.getItem(key),
        Set: (key, obj) => localStorage.setItem(key, typeof obj === 'string' ? obj : JSON.stringify(obj)),
        Delete: (key) => localStorage.removeItem(key),
        Clear: () => localStorage.clear(),
    }

    this.Constants = {
        cart_storage_key: 'cart_storage_key'
    }

    this.Cart = {
        GetCartId: () => {
            return this.Storage.Get(this.Constants.cart_storage_key)
        },
        Get: () => {
            const cartId = this.Storage.Get(this.Constants.cart_storage_key)
            if (!cartId)
                return {}
            return this.Http.Get(`/api/v1/shoppingCart/${cartId}`)
        },
        CreateEmptyCart: async () => {
            const data = await this.Http.Post(`/api/v1/shoppingCart/createEmptyCart`)
            return data
        },
        AddProductsToCart: async (cartItems) => {
            let cartId = this.Storage.Get(this.Constants.cart_storage_key)
            if (!cartId) {
                cartId = await this.Cart.CreateEmptyCart()
                this.Storage.Set(this.Constants.cart_storage_key, cartId)
            }
            const data = await this.Http.Post(`/api/v1/shoppingCart/${cartId}:addProductsToCart`, cartItems)
            return data
        },
        UpdateCartItems: async (cartItems) => {
            let cartId = this.Storage.Get(this.Constants.cart_storage_key)
            if (!cartId) {
                cartId = await this.Cart.CreateEmptyCart()
                this.Storage.Set(this.Constants.cart_storage_key, cartId)
            }
            const data = await this.Http.Put(`/api/v1/shoppingCart/${cartId}:updateCartItems`, cartItems)
            return data
        },
        RemoveCartItems: async (itemId) => {
            let cartId = this.Storage.Get(this.Constants.cart_storage_key)
            const data = await this.Http.Delete(`/api/v1/shoppingCart/${cartId}/item/${itemId}`)
            return data
        },
        UpdateCartUi: async (cart) => {
            if (!cart)
                cart = await this.Cart.Get()

            if (!cart.items || cart.items.length === 0) {
                $('#cart-count-badge').text('')
                $('#cart-count-badge').addClass('d-none')
                $('#cart-footer').hide()
            }
            else {
                $('#cart-count-badge').text(cart.items.length)
                $('#cart-count-badge').removeClass('d-none')
                $('#cart-footer').show()
            }

         
            $('#cart-item-list').html('')

            if (cart.items && cart.items.length > 0)
                cart.items.forEach(item => {
                    const att = item.attributes && item.attributes.length > 0 ? item.attributes.map(x => x.value).join('/') : null
                    const html = `
                    <li class="cart-item">
                            <div class="item-img">
                                <a href="single-product.html"><img src="/${item.product.image.path}" alt="Commodo Blown Lamp"></a>
                                <button onclick="deleteCartItems('${item.id}')" class="close-btn" aria-label="Delete cart item button"><i class="fas fa-times"></i></button>
                            </div>
                            <div class="item-content">
                                <h3 class="item-title"><a href="single-product-3.html">${item.product.name}</a></h3>
                                ${att ? `<p class="mb-0">${att}</p>` : ''}
                                <div class="item-price">${angular.element(document.getElementById("SiteController")).scope().convertCurrency(item.price)}</div>
                                <div class="pro-qty item-quantity">
                                <span class="dec qtybtn" onclick="updateCartItems('${item.id}', ${-1})">-</span>
                                    <input type="number" class="quantity-input" value="${item.quantity}" id="cart-item-qty-${item.id}" aria-label="Quantity">
                                <span class="inc qtybtn" onclick="updateCartItems('${item.id}',  ${1})">+</span>
                                </div>
                            </div>
                        </li>
                `
                    $('#cart-item-list').append(html);
                })
            else {
                let html = `<div class="row row-sm justify-content-center align-items-center">
                    <svg width="180" height="309.722" viewBox="0 0 180 309.722"><g id="no_cart_in_bag" data-name="no cart in bag" transform="translate(-988 -7673)"><g id="no_cart" data-name="no cart" transform="translate(988 7673)"><g id="Group_5970" data-name="Group 5970" transform="translate(0 0)"><g id="Group_5967" data-name="Group 5967" transform="translate(17.408 92.119)"><path id="Path_17743" data-name="Path 17743" d="M405.506,794.581l63.621-36.762L418.2,724.274Z" transform="translate(-323.428 -576.978)" fill="#ba8064"></path><path id="Path_17744" data-name="Path 17744" d="M135.711,140.727l32.918-.12,1.287-20.225,79.451,45.843-34.42,1.084v19.165Z" transform="translate(-118.988 -119.373)" fill="#dba480"></path><path id="Path_17745" data-name="Path 17745" d="M314.4,206.341,272,124.761l-2.279,22.008,1.4,59.572Z" transform="translate(-220.537 -122.691)" fill="#460100" opacity="0.1"></path><path id="Path_17746" data-name="Path 17746" d="M141.237,253.056l-10.26-47.388,34.59-.126v37.243Z" transform="translate(-115.402 -183.904)" fill="#995b47"></path><path id="Path_17747" data-name="Path 17747" d="M511.029,445.295l-49.136-22.179L460.8,385.489l1.089-72.515,35.954-1.133Z" transform="translate(-365.33 -264.454)" fill="#a96e56"></path><path id="Path_17748" data-name="Path 17748" d="M148.755,398.756l9.58-70.307,4.9-79.149L81.161,201.911,66.677,351.368Z" transform="translate(-66.677 -181.153)" fill="#dba480"></path><path id="Path_17749" data-name="Path 17749" d="M349.459,429.379c-.415,1.942-2.182,2.6-3.948,1.46a5.753,5.753,0,0,1-2.446-5.572c.414-1.942,2.182-2.6,3.948-1.46A5.753,5.753,0,0,1,349.459,429.379Z" transform="translate(-276.046 -348.874)" fill="#67251d"></path><path id="Path_17750" data-name="Path 17750" d="M209.819,348.753c-.415,1.942-2.182,2.6-3.948,1.46a5.753,5.753,0,0,1-2.446-5.572c.415-1.942,2.182-2.6,3.948-1.46A5.753,5.753,0,0,1,209.819,348.753Z" transform="translate(-170.233 -287.779)" fill="#67251d"></path><g id="Group_5965" data-name="Group 5965" transform="translate(31.503 60.166)" opacity="0.2"><path id="Path_17751" data-name="Path 17751" d="M219.35,441.507a16.861,16.861,0,0,1-8.439-2.272A28.35,28.35,0,0,1,196.858,412l4.383-45.226a2.414,2.414,0,0,1,4.806.467l-4.383,45.226a23.483,23.483,0,0,0,11.608,22.554,12.055,12.055,0,0,0,18.081-9.247l3.819-39.41a2.414,2.414,0,0,1,4.806.467l-3.819,39.41a16.912,16.912,0,0,1-16.809,15.266Z" transform="translate(-196.727 -364.591)" fill="#460100"></path></g><path id="Path_17752" data-name="Path 17752" d="M162.373,116.218,161.06,136.85l-34.59.126,82.078,47.388V164.738l35.954-1.132Zm44.968,47.351v18.7l-76.395-44.106,31.247-.113,1.261-19.819,76.774,44.3Z" transform="translate(-111.986 -116.218)" fill="#fcc89d"></path><g id="Group_5966" data-name="Group 5966" transform="translate(29.24 57.45)"><path id="Path_17753" data-name="Path 17753" d="M210.007,430.3a16.864,16.864,0,0,1-8.438-2.271,28.35,28.35,0,0,1-14.054-27.235l4.383-45.226a2.414,2.414,0,0,1,4.806.467l-4.383,45.226a23.483,23.483,0,0,0,11.608,22.554,12.055,12.055,0,0,0,18.081-9.247l3.819-39.41a2.414,2.414,0,0,1,4.806.467l-3.819,39.41A16.912,16.912,0,0,1,210.007,430.3Z" transform="translate(-187.384 -353.38)" fill="#995b47"></path></g><path id="Path_17754" data-name="Path 17754" d="M405.506,546.991,419.99,488.05V397.534Z" transform="translate(-323.428 -329.388)" fill="#995b47"></path></g><g id="Group_5968" data-name="Group 5968" transform="translate(0 0)"><path id="Path_17755" data-name="Path 17755" d="M394.573,120.6c-.142-.5.244-.855.5-1.088a1.4,1.4,0,0,0,.271-.293,0,0,0,0,0,0,0,1.39,1.39,0,0,0-.384-.107c-.34-.065-.853-.162-1-.665s.244-.855.5-1.088a1.39,1.39,0,0,0,.271-.293,0,0,0,0,0,0,0,1.4,1.4,0,0,0-.384-.107c-.34-.064-.853-.162-1-.664s.244-.855.5-1.088l.009-.008a.9.9,0,0,0,.259-.482.391.391,0,0,1,.276-.292.41.41,0,0,1,.5.316,1.122,1.122,0,0,1-.51,1.046,1.4,1.4,0,0,0-.271.292,0,0,0,0,0,0,0,1.4,1.4,0,0,0,.384.107c.34.065.853.162,1,.665s-.244.855-.5,1.088a1.4,1.4,0,0,0-.271.293,0,0,0,0,0,0,0,1.391,1.391,0,0,0,.384.107c.34.065.853.162,1,.665s-.244.855-.5,1.088a1.257,1.257,0,0,0-.273.3,0,0,0,0,0,0,0,1.641,1.641,0,0,0,.387.1c.331.063.826.157.983.625a.416.416,0,0,1-.21.507.392.392,0,0,1-.456-.109.789.789,0,0,0-.464-.253h0C395.229,121.2,394.716,121.1,394.573,120.6Z" transform="translate(-349.075 -37.518)" fill="#212121" opacity="0.3"></path><path id="Path_17765" data-name="Path 17765" d="M395.468,120.6c.142-.5-.244-.855-.5-1.088a1.4,1.4,0,0,1-.271-.293,0,0,0,0,1,0,0,1.39,1.39,0,0,1,.384-.107c.34-.065.853-.162,1-.665s-.244-.855-.5-1.088a1.389,1.389,0,0,1-.271-.293,0,0,0,0,1,0,0,1.4,1.4,0,0,1,.384-.107c.34-.064.853-.162,1-.664s-.244-.855-.5-1.088l-.009-.008a.9.9,0,0,1-.259-.482.391.391,0,0,0-.276-.292.41.41,0,0,0-.5.316,1.122,1.122,0,0,0,.51,1.046,1.4,1.4,0,0,1,.271.292,0,0,0,0,1,0,0,1.4,1.4,0,0,1-.384.107c-.34.065-.853.162-1,.664s.244.855.5,1.088a1.4,1.4,0,0,1,.271.293,0,0,0,0,1,0,0,1.39,1.39,0,0,1-.384.107c-.34.065-.853.162-1,.665s.244.855.5,1.088a1.257,1.257,0,0,1,.273.3,0,0,0,0,1,0,0,1.641,1.641,0,0,1-.387.1c-.331.063-.826.157-.983.625a.416.416,0,0,0,.21.507.392.392,0,0,0,.456-.109.789.789,0,0,1,.464-.253h0C394.812,121.2,395.326,121.1,395.468,120.6Z" transform="translate(-262.76 -23.736)" fill="#212121" opacity="0.3"></path><path id="Path_17756" data-name="Path 17756" d="M375.447,179.277a2.539,2.539,0,1,1,3.346,1.3A2.542,2.542,0,0,1,375.447,179.277Zm3.737-1.643a1.543,1.543,0,1,0-.791,2.034A1.545,1.545,0,0,0,379.184,177.634Z" transform="translate(-375.232 -52.408)" fill="#212121" opacity="0.3"></path><path id="Path_17764" data-name="Path 17764" d="M375.447,179.277a2.539,2.539,0,1,1,3.346,1.3A2.542,2.542,0,0,1,375.447,179.277Zm3.737-1.643a1.543,1.543,0,1,0-.791,2.034A1.545,1.545,0,0,0,379.184,177.634Z" transform="translate(-333.888 -175.716)" fill="#212121" opacity="0.3"></path><path id="Path_17757" data-name="Path 17757" d="M350.086,264.8a1.852,1.852,0,0,1-2.682-2.547l-.868-.823a3.047,3.047,0,0,0,4.417,4.194Z" transform="translate(-253.642 -206.302)" fill="#212121" opacity="0.3"></path><path id="Path_17766" data-name="Path 17766" d="M346.628,264.8a1.852,1.852,0,0,0,2.682-2.547l.867-.823a3.047,3.047,0,0,1-4.417,4.194Z" transform="translate(-170.953 -110.557)" fill="#212121" opacity="0.15"></path><path id="Union_11" data-name="Union 11" d="M2.059,6.97l.989-3.048L0,2.933.283,2.06l3.049.989L4.321,0,5.2.284l-.99,3.048,3.047.989L6.97,5.2l-3.048-.99-.99,3.049Z" transform="translate(6.528 48.598)" fill="#212121" stroke="rgba(0,0,0,0)" stroke-miterlimit="10" stroke-width="1" opacity="0.3"></path><path id="Union_13" data-name="Union 13" d="M2.059,6.97l.989-3.048L0,2.933.283,2.06l3.049.989L4.321,0,5.2.284l-.99,3.048,3.047.989L6.97,5.2l-3.048-.99-.99,3.049Z" transform="translate(94.294 121.132)" fill="#212121" stroke="rgba(0,0,0,0)" stroke-miterlimit="10" stroke-width="1" opacity="0.3"></path><path id="Union_12" data-name="Union 12" d="M1.235,4.182l.593-1.829L0,1.759l.17-.524L2,1.829,2.592,0l.525.17L2.523,2l1.828.594-.17.523L2.353,2.523,1.759,4.352Z" transform="translate(107.351 6.528)" fill="#212121" stroke="rgba(0,0,0,0)" stroke-miterlimit="10" stroke-width="1" opacity="0.3"></path></g></g></g></g></svg>
                </div>
                `
                $('#cart-item-list').append(html);
            }

            this.Cart.UpdateCartTotal(cart.subTotal)
        },
        UpdateCartTotal: (total) => {
            $('#cart-subtotal-amount').text(angular.element(document.getElementById("SiteController")).scope().convertCurrency(Number(total || 0)))
        }
    }

    this.Category = {
        GetTree: () => this.Http.Get(`/api/v1/categories?pageSize=1000`)
    }
    this.Brand = {
        Get: () => this.Http.Get(`/api/v1/brands?pageSize=1000`)
    }

    this.Product = {
        Get: (data) => this.Http.Post(`/api/v1/products`, data)
    }
    this.Attribute = {
        Get: (data) => this.Http.Get(`/api/v1/attributes?pageIndex=${data.pageIndex || 0}&pageSize=${data.pageSize || 10}`)
    }

    this.Order = {
        Checkout: (data) => this.Http.Post(`/api/v1/Checkout`, data),
        Get: (id) => this.Http.Get(`/api/v1/orders/${id}`)
    }

})
async function updateCartItems(id, qty) {
    const currentQty = $(`#cart-item-qty-${id}`).val()
    let newQty = Number(currentQty) + Number(qty)
    if (newQty < 1) {
        newQty = 0
        await deleteCartItems(id)
        return
    }
    $(`#cart-item-qty-${id}`).val(newQty)
    await angular.element(document.getElementById("SiteController")).scope().UpdateCartItems([{ id: id, quantity: newQty }])
}
async function deleteCartItems(itemId) {
    await angular.element(document.getElementById("SiteController")).scope().removeCartItems(itemId)
}