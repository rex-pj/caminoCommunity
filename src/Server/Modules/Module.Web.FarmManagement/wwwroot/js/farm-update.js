; (function (global) {
    'use strict';
    var features = {
        selectors: {
            contentEditor: 'textarea#Description',
            select2Ajax: '.select2-remote-ajax',
            searchDropdownsParent: '#searchForm',
            fileUploadSelector: '#pictureUpload',
            uploadProgressingBar: '#uploadProgress',
            farmPicturesBar: '#farmPictures',
            pictureItemSelector: 'picture-item',
            removePictureSelector: '.remove-picture'
        },
        loadContentEditor: function (selector) {
            $(selector).tinymceEditor();
        },
        loadSelect2Ajax: function (selector, dropdownParent) {
            $(selector).select2Ajax({ dropdownParent: $(dropdownParent) });
        },
        loadFileUploader: function (selector, progressingBar, callback) {
            $(selector).fileupload({
                dataType: "json",
                progress: function (e, data) {
                    $(progressingBar).removeClass('d-none');
                },
                done: function (e, data) {
                    $(progressingBar).addClass('d-none');
                    if (callback) {
                        callback(data);
                    }
                }
            });
        },
        initPictureRemoving: function () {
            var selectors = features.selectors;
            var removePictureSelector = '.' + selectors.removePictureSelector;
            $(selectors.farmPictures).on('click', removePictureSelector, function () {
                var pictureItemSelector = '.' + selectors.pictureItemSelector;
                $(this).parent(pictureItemSelector).remove();
                rerenderPicturesIndexes();
            });
        }
    };

    var rerenderPicturesIndexes = function () {
        var selectors = features.selectors;
        var pictureItemSelector = '.' + selectors.pictureItemSelector;
        var pictureItems = $(selectors.farmPicturesBar).find(pictureItemSelector);
        $.each(pictureItems, function (index, value) {
            var pictureItem = $(value);
            var hiddenPictures = pictureItem.find('input[type=hidden]');
            $.each(hiddenPictures, function (hIndex, hValue) {
                var inputSubfix = hValue.name.split('.')[1];
                hValue.name = 'Pictures[' + index + '].' + inputSubfix;
            });
        });
    }

    var fileUploadCallback = function (data) {
        var selectors = features.selectors;
        var blobImage = base64toBlob(data.result.url, data.result.contentType);
        var blobUrl = URL.createObjectURL(blobImage);
        var farmPictures = $(selectors.farmPicturesBar);
        var pictureItemSelector = '.' + selectors.pictureItemSelector;
        var pictureItems = farmPictures.find(pictureItemSelector);
        var appendedIndex = pictureItems.length;

        var base64Data = 'data:' + data.result.contentType + ';base64,' + data.result.url;
        var appendedHtml = '<div class="col-6 col-sm-6 col-md-4 col-lg-3 ' + selectors.pictureItemSelector + '">';
        appendedHtml += '<div class="mb-3">';
        appendedHtml += '<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">';
        appendedHtml += '</div>';
        appendedHtml += '<input type="hidden" name="Pictures[' + appendedIndex + '].Base64Data" value="' + base64Data + '" />';
        appendedHtml += '<input type="hidden" name="Pictures[' + appendedIndex + '].FileName" value="' + data.result.name + '" />';
        appendedHtml += '<input type="hidden" name="Pictures[' + appendedIndex + '].ContentType" value="' + data.result.contentType + '" />';
        appendedHtml += '<span class="remove-picture">X</span>';
        appendedHtml += '</div>';
        farmPictures.append(appendedHtml);
    };

    global.initialize = function () {
        var selectors = features.selectors;
        features.loadContentEditor(selectors.contentEditor);
        features.loadSelect2Ajax(selectors.select2Ajax, selectors.searchDropdownsParent);
        features.loadFileUploader(selectors.fileUploadSelector, selectors.uploadProgressingBar, fileUploadCallback);
        selectors.initPictureRemoving();
    };
})(window, document);

$(function () {
    window.onload = initialize;
});