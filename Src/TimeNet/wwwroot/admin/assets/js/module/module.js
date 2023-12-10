var adminApp = angular.module('TimeSolution', []);
if (location.pathname.toLowerCase().includes('add') || location.pathname.toLowerCase().includes('update'))
    adminApp = angular.module('TimeSolution', ['summernote']);

const loading = {
    show: () => $('#progress-bar').show(),
    hide: () => $('#progress-bar').hide()
}

var utils = {
    createGuid: () => {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r && 0x3 | 0x8);
            return v.toString(16);
        })
    },
    getCurrentLanguage: () => {
        try {
            return document.cookie.split(';').find(x => x.includes('.AspNetCore.Culture')).trim().endsWith('vi') ? 'vi' : 'en'
        } catch (e) {
            return 'en'
        }
    },
    string: {
        generateCode: () => {
            return Math.random().toString(36).slice(2).toUpperCase()
        }
    }
}

const resetToastPosition = function () {
    $('.jq-toast-wrap').removeClass('bottom-left bottom-right top-left top-right mid-center'); // to remove previous position class
    $(".jq-toast-wrap").css({
        "top": "",
        "left": "",
        "bottom": "",
        "right": ""
    }); //to remove previous position style
}
const showToast = function (notification) {
    'use strict';
    resetToastPosition();
    if (notification.type === 'success')
        $.toast({
            heading: notification.title,
            text: notification.content,
            showHideTransition: 'slide',
            icon: 'success',
            loaderBg: '#f96868',
            position: 'top-right'
        })
    else
        $.toast({
            heading: notification.title,
            text: notification.content,
            showHideTransition: 'slide',
            icon: 'error',
            loaderBg: '#f2a654',
            position: 'top-right'
        })
};

const loadSkeletonTable = (tableId) => {
    $(`#${tableId}_processing`).hide()
    var table = $(`#${tableId} tbody`);
    $(`#${tableId} tbody tr`).remove();
    const cols = 8
    const rows = 10
    for (var i = 0; i < rows; i++) {
        var row = $('<tr>');
        for (var j = 0; j < cols; j++) {
            var rowData = $('<td class="loading">');
            rowData.append('<div class="bar">');
            row.append(rowData);
        }
        table.append(row);
    }
}

const globalSearch = () => {
    const category = $('#global-search-category').val()
    const text = $('#global-search-text').val()
    switch (category) {
        case 'Categories':
            window.location.href = `/admin/category?searchText=${text}`
            break;
        case 'Products':
            window.location.href = `/admin/product?searchText=${text}`
            break;
        case 'Brands':
            window.location.href = `/admin/brand?searchText=${text}`
            break;
        case 'Attributes':
            window.location.href = `/admin/Attribute?searchText=${text}`
            break;
    }
}

//adminApp.directive("myElement", function () {
//    return {
//        link: postLink,
//    };
//    function postLink(scope, elem, attrs) {
//        console.log('post nhaaaaaaaaaaaa')
//        console.log(post)
//        console.log(scope)
//        var post = scope.$eval(attrs.post);


//        var html = `
//          <div id="objectMap-${post.id}">
//            <div id="BasemapToggle-${post.id}">
//            </div>
//          </div>
//        `;
//        elem.append(html);
//    }
//})