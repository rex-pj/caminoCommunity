; (function (global) {
    'use strict';
    var features = {
        selectors: {
            contentEditor: 'textarea#Content',
            select2Ajax: '.select2-remote-ajax',
            searchDropdownsParent: '#searchForm',
            fileUploadSelector: '#pictureUpload',
            uploadProgressingBar: '#uploadProgress',
            pictureSelector: '#picture',
            pictureFileTypeSelector: '#pictureFileType',
            pictureFileNameSelector: '#pictureFileName',
            articlePictureSelector: '#articlePicture'
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
        }
    };

    var fileUploadCallback = function (data) {
        var selectors = features.selectors;
        var blobImage = base64toBlob(data.result.url, data.result.contentType);
        var blobUrl = URL.createObjectURL(blobImage);
        $(selectors.pictureSelector).val(data.result.url);
        $(selectors.pictureFileTypeSelector).val(data.result.contentType);
        $(selectors.pictureFileNameSelector).val(data.result.name);
        $(selectors.articlePictureSelector).html('<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">');
    };

    global.initialize = function () {
        var selectors = features.selectors;
        features.loadContentEditor(selectors.contentEditor);
        features.loadSelect2Ajax(selectors.select2Ajax, selectors.searchDropdownsParent);
        features.loadFileUploader(selectors.fileUploadSelector, selectors.uploadProgressingBar, fileUploadCallback);
    };
})(window, document);

$(function () {
    window.onload = initialize;
});