window.addEventListener('load', function () {
    //let btn = document.getElementById("backto-top");
    //if (btn) {
    //    btn.addEventListener('click', (e) => {
    //        e.preventDefault();
    //        $('html, body').animate({
    //            scrollTop: 0
    //        }, '300');
    //    });
    //}
  
})
window.addEventListener("scroll", function (event) {
    let btn = document.getElementById("backto-top");
    var top = this.scrollY
    if (top > 300) {
        btn.classList.add("show");
    } else {
        btn.classList.remove("show");
    }
}, false);

(function (window, document, $) {
    'use strict';

    var axilInit = {
        i: function(e) {
            axilInit.s();
            axilInit.methods();
        },

        s: function(e) {
            this._window = $(window),
                this._document = $(document),
                this._body = $('body'),
                this._html = $('html')
        },

        methods: function(e) {
            axilInit.w();
            //axilInit.axilBackToTop();
            axilInit.shopFilterWidget();
            axilInit.mobileMenuActivation();
            axilInit.menuLinkActive();
            axilInit.headerIconToggle();
            axilInit.sideOffcanvasToggle('.cart-dropdown-btn', '#cart-dropdown');
            axilInit.sideOffcanvasToggle('.mobile-nav-toggler', '.header-main-nav');
            axilInit.sideOffcanvasToggle('.department-side-menu', '.department-nav-menu');
            axilInit.sideOffcanvasToggle('.filter-toggle', '.axil-shop-sidebar');
            axilInit.sideOffcanvasToggle('.axil-search', '#header-search-modal');
            axilInit.sideOffcanvasToggle('.popup-close, .closeMask', "#offer-popup-modal");
            axilInit.stickyHeaderMenu();
            axilInit.colorVariantActive();
            axilInit.headerCampaignRemove();
            axilInit.scrollSmoth();
            axilInit.onLoadClassAdd();
            axilInit.dropdownMenuSlide();
        },

        w: function(e) {
            this._window.on('load', axilInit.l).on('scroll', axilInit.res)
        },

        scrollSmoth: function (e) {
            $(document).on('click', '.smoth-animation', function (event) {
                event.preventDefault();
                $('html, body').animate({
                    scrollTop: $($.attr(this, 'href')).offset().top
                }, 200);
            });
        },

        //axilBackToTop: function() {
            //var btn = $('#backto-top');
            //$(window).scroll(function() {
            //    if ($(window).scrollTop() > 300) {
            //        btn.addClass('show');
            //    } else {
            //        btn.removeClass('show');
            //    }
            //});
            //btn.on('click', function(e) {
            //    e.preventDefault();
            //    $('html, body').animate({
            //        scrollTop: 0
            //    }, '300');
            //});
            //var btn = $('#backto-top');
            //$(window).scroll(function() {
            //    if ($(window).scrollTop() > 300) {
            //        btn.addClass('show');
            //    } else {
            //        btn.removeClass('show');
            //    }
            //});
            //btn.on('click', function(e) {
            //    e.preventDefault();
            //    $('html, body').animate({
            //        scrollTop: 0
            //    }, '300');
            //});
        //},

        shopFilterWidget: function() {
            $('.toggle-list > .title').on('click', function(e) {

                var target = $(this).parent().children('.shop-submenu');
                var target2 = $(this).parent();
                $(target).slideToggle();
                $(target2).toggleClass('active');
            });

            $('.toggle-btn').on('click', function(e) {

                var target = $(this).parent().siblings('.toggle-open');
                var target2 = $(this).parent();
                $(target).slideToggle();
                $(target2).toggleClass('active');
            });
        },

        mobileMenuActivation: function(e) {
            
            $('.menu-item-has-children > a').on('click', function(e) {

                var targetParent = $(this).parents('.header-main-nav');
                var target = $(this).siblings('.axil-submenu');

                if (targetParent.hasClass('open')) {
                    $(target).slideToggle(400);
                    $(this).parent('.menu-item-has-children').toggleClass('open');
                }

            });

            $('.nav-link.has-megamenu').on('click', function(e) {

                var $this = $(this),
                targetElm = $this.siblings('.megamenu-mobile-toggle');
                targetElm.slideToggle(500);
            });

            // Mobile Sidemenu Class Add
            function resizeClassAdd() {
                if (window.matchMedia('(max-width: 1199px)').matches) {
                    $('.department-title').addClass('department-side-menu');
                    $('.department-megamenu').addClass('megamenu-mobile-toggle');
                } else {
                    $('.department-title').removeClass('department-side-menuu');
                    $('.department-megamenu').removeClass('megamenu-mobile-toggle').removeAttr('style');
                }
            }

            $(window).resize(function() {
                resizeClassAdd();
            });

            resizeClassAdd();
        },

        menuLinkActive: function () {
            var currentPage = location.pathname.split("/"),
                current = currentPage[currentPage.length-1];
            $('.mainmenu li a, .main-navigation li a').each(function(){
                var $this = $(this);
                if($this.attr('href') === current){
                    $this.addClass('active');
                    $this.parents('.menu-item-has-children').addClass('menu-item-open')
                }
            });
        },

        headerIconToggle: function() {

            $('.my-account > a').on('click', function(e) {
                $(this).toggleClass('open').siblings().toggleClass('open');
            })
        },

        sideOffcanvasToggle: function(selectbtn, openElement) {

            $('body').on('click', selectbtn, function(e) {
                e.preventDefault();

                var $this = $(this),
                    wrapp = $this.parents('body'),
                    wrapMask = $('<div / >').addClass('closeMask'),
                    cartDropdown = $(openElement);

                if (!(cartDropdown).hasClass('open')) {
                    wrapp.addClass('open');
                    cartDropdown.addClass('open');
                    cartDropdown.parent().append(wrapMask);
                    wrapp.css({
                        'overflow': 'hidden'

                    });

                } else {
                    removeSideMenu();
                }

                function removeSideMenu() {
                    wrapp.removeAttr('style');
                    wrapp.removeClass('open').find('.closeMask').remove();
                    cartDropdown.removeClass('open');
                }

                $('.sidebar-close, .closeMask').on('click', function() {
                    removeSideMenu();
                });

            });
        },

        stickyHeaderMenu: function() {

            $(window).on('scroll', function() {
                // Sticky Class Add
                if ($('body').hasClass('sticky-header')) {
                    var stickyPlaceHolder = $('#axil-sticky-placeholder'),
                        menu = $('.axil-mainmenu'),
                        menuH = menu.outerHeight(),
                        topHeaderH = $('.axil-header-top').outerHeight() || 0,
                        headerCampaign = $('.header-top-campaign').outerHeight() || 0,
                        targrtScroll = topHeaderH + headerCampaign;
                    if ($(window).scrollTop() > targrtScroll) {
                        menu.addClass('axil-sticky');
                        stickyPlaceHolder.height(menuH);
                    } else {
                        menu.removeClass('axil-sticky');
                        stickyPlaceHolder.height(0);
                    }
                }
            });
        },

        colorVariantActive: function() {
            $('.color-variant > li').on('click', function(e) {
                $(this).addClass('active').siblings().removeClass('active');
            })
        },

        headerCampaignRemove: function() {
           $('.remove-campaign').on('click', function() {
                var targetElem = $('.header-top-campaign');
                targetElem.slideUp(function() {
                    $(this).remove();
                });
           });
        },

        offerPopupActivation: function() {
            if ($('body').hasClass('newsletter-popup-modal')) {
                setTimeout(function(){ 
                    $('body').addClass('open');
                    $('#offer-popup-modal').addClass('open');
                }, 1000);
            }
        },

        onLoadClassAdd: function () {
            this._window.on( "load", function() {
                setTimeout(function() {
                    $('.main-slider-style-4').addClass('animation-init');
                }, 500);
            });

        },

        dropdownMenuSlide: function () {
            if (window.matchMedia('(max-width: 991px)').matches) {
                $('#dropdown-header-menu').removeAttr('data-bs-toggle');
                $('#dropdown-header-menu').on('click', function() {
                   $(this).siblings('.dropdown-menu').slideToggle();
                    // console.log(this)
                })
            }

        },

    }
    axilInit.i();

})(window, document, jQuery);