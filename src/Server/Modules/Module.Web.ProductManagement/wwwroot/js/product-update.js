; (function (global) {
    'use strict';
    var selectors = {
        contentEditor: 'textarea#Description',
        select2Ajax: '.select2-remote-ajax',
        fileUploadSelector: '#pictureUpload',
        uploadProgressingBar: '#uploadProgress',
        productPicturesBar: '#productPictures',
        pictureItemSelector: 'picture-item',
        removePictureSelector: '.remove-picture',
        productAttributeUpdateModal: 'productAttributeUpdateModal',
        productAttributeDeleteModal: 'productAttributeDeleteModal',
        attributeUpdateIdSelector: '#productAttributeUpdateModal #AttributeId',
        attributeUpdateControlTypeIdSelector: '#productAttributeUpdateModal #ControlTypeId',
        attributeUpdateDisplayOrderSelector: '#productAttributeUpdateModal #DisplayOrder',
        attributeRelationIdSelector: '#productAttributeUpdateModal #attributeRelationId',
        submitUpdateProductAttributeRelation: '#submitUpdateProductAttributeRelation',
        submitDeleteProductAttributeRelation: '#submitDeleteProductAttributeRelation',
        labelAttributeName: '.label-attribute-name',
        labelControlType: '.label-control-type',
        labelAttributeDisplayOrder: '.label-attribute-display-order',
        fieldAttributeId: '.field-attribute-id',
        fieldControlTypeId: '.field-control-type-id',
        fieldAttributeDisplayOrder: '.field-attribute-display-order',
        fieldIsAttributeUpdated: '.is-attribute-updated',
        rowAttributeRelation: '.row-attribute-relation',
        productAttributeDeleteId: '#productAttributeDeleteId',
        listProductAttributes: '.list-product-attributes'
    };

    var features = {
        loadContentEditor: function (selector) {
            $(selector).tinymceEditor();
        },
        loadSelect2Ajax: function (selector, dropdownParent) {
            if (dropdownParent) {
                $(selector).select2Ajax({ dropdownParent: $(dropdownParent) });
            }
            else {
                $(selector).select2Ajax();
            }
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
            var removePictureSelector = selectors.removePictureSelector;
            $(selectors.productPicturesBar).on('click', removePictureSelector, function () {
                var pictureItemSelector = '.' + selectors.pictureItemSelector;
                $(this).parent(pictureItemSelector).remove();
                rerenderPicturesIndexes();
            });
        },
        loadModal: function (modalSelector, onShow, onClose, onShown) {
            var modal = document.getElementById(modalSelector);
            modal.addEventListener('show.bs.modal', function (e) {
                if (onShow) {
                    var trigger = e.relatedTarget;
                    onShow(trigger, modalSelector);
                }
            });

            modal.addEventListener('shown.bs.modal', function (e) {
                if (onShown) {
                    var trigger = e.relatedTarget;
                    onShown(trigger, modalSelector);
                }
            });

            modal.addEventListener('hide.bs.modal', function (e) {
                if (onClose) {
                    onClose();
                }
            });
        },
        hideModal: function (selector) {
            var myModalEl = document.querySelector(selector);
            var myModal = bootstrap.Modal.getInstance(myModalEl);
            myModal.hide();
        },
        initSubmitProductAttributeRelation: function (selector) {
            $(selector).on('click', function () {
                var attributeIdData = $(selectors.attributeUpdateIdSelector).select2('data');
                var controlTypeIdData = $(selectors.attributeUpdateControlTypeIdSelector).select2('data');
                var displayOrder = $(selectors.attributeUpdateDisplayOrderSelector).val();
                var attributeRelationId = $(selectors.attributeRelationIdSelector).val();

                // modify the existing attribute relation
                if (attributeRelationId) {
                    var attributeRow = $(selectors.rowAttributeRelation + '-' + attributeRelationId);
                    attributeRow.find(selectors.labelAttributeName).text(attributeIdData[0].text);
                    attributeRow.find(selectors.labelControlType).text(controlTypeIdData[0].text);
                    attributeRow.find(selectors.labelAttributeDisplayOrder).text(displayOrder);

                    attributeRow.find(selectors.fieldAttributeId).val(attributeIdData[0].id);
                    attributeRow.find(selectors.fieldControlTypeId).val(controlTypeIdData[0].id);
                    attributeRow.find(selectors.fieldAttributeDisplayOrder).val(displayOrder);
                    attributeRow.find(selectors.fieldIsAttributeUpdated).val(true);
                }
                else {
                    $.post(this.dataset.url, {
                        attributeId: attributeIdData[0].id,
                        name: attributeIdData[0].text,
                        controlTypeId: controlTypeIdData[0].id,
                        controlTypeName: controlTypeIdData[0].text,
                        displayOrder: displayOrder
                    }, function (response) {
                        $(selectors.listProductAttributes).append(response);
                    });
                }

                features.hideModal('#' + selectors.productAttributeUpdateModal);
            });
        },
        initDeleteProductAttributeRelation: function (selector) {
            $(selector).on('click', function () {
                var attributeRelationId = $(selectors.productAttributeDeleteId).val();
                var attributeRow = $(selectors.rowAttributeRelation + '-' + attributeRelationId);
                attributeRow.remove();

                features.hideModal('#' + selectors.productAttributeDeleteModal);
            });
        }
    };

    var loadAttributeUpdateModel = function (trigger, modalSelector) {
        var attributeUpdateIdSelector = $(selectors.attributeUpdateIdSelector);
        var attributeUpdateControlTypeIdSelector = $(selectors.attributeUpdateControlTypeIdSelector);
        var attributeRow = $(trigger).parents(selectors.rowAttributeRelation);
        var isAttributeUpdated = attributeRow.find(selectors.fieldIsAttributeUpdated).val();
        if (!trigger.dataset.id || trigger.dataset.id === '0' || isAttributeUpdated) {
            var labelAttributeName = attributeRow.find(selectors.labelAttributeName).text();
            var labelControlType = attributeRow.find(selectors.labelControlType).text();

            var fieldAttributeId = attributeRow.find(selectors.fieldAttributeId).val();

            var fieldControlTypeId = attributeRow.find(selectors.fieldControlTypeId).val();
            var displayOrder = attributeRow.find(selectors.fieldAttributeDisplayOrder).val();

            $(selectors.attributeUpdateDisplayOrderSelector).val(displayOrder);

            var fieldAttributeRelationId = trigger.dataset.id;
            if (fieldAttributeRelationId) {
                $(selectors.attributeRelationIdSelector).val(fieldAttributeRelationId);
            }
            else {
                $(selectors.attributeRelationIdSelector).val('');
                $(selectors.submitUpdateProductAttributeRelation).attr('data-url', trigger.dataset.url);
            }

            attributeUpdateIdSelector.select2Ajax("val", { id: fieldAttributeId, text: labelAttributeName });
            attributeUpdateControlTypeIdSelector.select2Ajax("val", { id: fieldControlTypeId, text: labelControlType });
        }
        else {
            $.get(trigger.dataset.url, { id: trigger.dataset.id }, function (response) {
                $(selectors.attributeUpdateDisplayOrderSelector).val(response.displayOrder);
                $(selectors.attributeRelationIdSelector).val(response.id);

                attributeUpdateIdSelector.select2Ajax("val", { id: response.attributeId, text: response.name });
                attributeUpdateControlTypeIdSelector.select2Ajax("val", { id: response.controlTypeId, text: response.controlTypeName });
            });
        }
    }

    var shownAttributeUpdateModel = function (trigger, modalSelector) {
        features.loadSelect2Ajax(selectors.attributeUpdateIdSelector, '#' + modalSelector);
        features.loadSelect2Ajax(selectors.attributeUpdateControlTypeIdSelector, '#' + modalSelector);
    }

    var clearAttributeUpdateModel = function () {
        $(selectors.attributeUpdateDisplayOrderSelector).val(0);
        $(selectors.attributeRelationIdSelector).val(0);

        $(selectors.attributeUpdateIdSelector).select2Ajax("remove");
        $(selectors.attributeUpdateControlTypeIdSelector).select2Ajax("remove");
        $(selectors.submitUpdateProductAttributeRelation).removeAttr('data-url');
    }

    var loadAttributeDeleteModel = function (trigger) {
        $(selectors.productAttributeDeleteId).val(trigger.dataset.id);
    }

    var clearAttributeDeleteModel = function () {
        $(selectors.productAttributeDeleteId).val(0);
    }

    var rerenderPicturesIndexes = function () {
        var pictureItemSelector = '.' + selectors.pictureItemSelector;
        var pictureItems = $(selectors.productPictures).find(pictureItemSelector);
        $.each(pictureItems, function (index, value) {
            var pictureItem = $(value);
            var hiddenPicturess = pictureItem.find('input[type=hidden]');
            $.each(hiddenPicturess, function (hIndex, hValue) {
                var inputSubfix = hValue.name.split('.')[1];
                hValue.name = 'Pictures[' + index + '].' + inputSubfix;
            });
        });
    }

    var fileUploadCallback = function (data) {
        var blobImage = base64toBlob(data.result.url, data.result.contentType);
        var blobUrl = URL.createObjectURL(blobImage);
        var productPictures = $(selectors.productPicturesBar);
        var pictureItemSelector = '.' + selectors.pictureItemSelector;
        var pictureItems = productPictures.find(pictureItemSelector);
        var appendedIndex = pictureItems.length;

        var base64Data = 'data:' + data.result.contentType + ';base64,' + data.result.url;
        var appendedHtml = '<div class="col-6 col-sm-6 col-md-4 col-lg-3 ' + selectors.pictureItemSelector + '">';
        appendedHtml += '<div class="mb-3">';
        appendedHtml += '<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">';
        appendedHtml += '</div>';
        appendedHtml += '<input type="hidden" name="Picturess[' + appendedIndex + '].Base64Data" value="' + base64Data + '" />';
        appendedHtml += '<input type="hidden" name="Picturess[' + appendedIndex + '].FileName" value="' + data.result.name + '" />';
        appendedHtml += '<input type="hidden" name="Picturess[' + appendedIndex + '].ContentType" value="' + data.result.contentType + '" />';
        appendedHtml += '<span class="remove-picture"><i class="fa fa-times"></i></span>';
        appendedHtml += '</div>';
        productPictures.append(appendedHtml);
    };

    global.initialize = function () {
        features.loadContentEditor(selectors.contentEditor);
        features.loadSelect2Ajax(selectors.select2Ajax);
        features.loadFileUploader(selectors.fileUploadSelector, selectors.uploadProgressingBar, fileUploadCallback);
        features.initPictureRemoving();
        features.loadModal(selectors.productAttributeUpdateModal, loadAttributeUpdateModel, clearAttributeUpdateModel, shownAttributeUpdateModel);
        features.initSubmitProductAttributeRelation(selectors.submitUpdateProductAttributeRelation);
        features.initDeleteProductAttributeRelation(selectors.submitDeleteProductAttributeRelation);
        features.loadModal(selectors.productAttributeDeleteModal, loadAttributeDeleteModel, clearAttributeDeleteModel);
    };
})(window, document);

$(function () {
    window.onload = initialize;
});