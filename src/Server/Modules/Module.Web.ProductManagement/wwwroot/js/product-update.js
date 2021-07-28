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
        attributeUpdateModal: 'productAttributeUpdateModal',
        attributeDeleteModal: 'productAttributeDeleteModal',
        attributeUpdateModalIdSelector: '#productAttributeUpdateModal #AttributeId',
        attributeUpdateModalControlTypeSelector: '#productAttributeUpdateModal #ControlTypeId',
        attributeUpdateModalDisplayOrderSelector: '#productAttributeUpdateModal #DisplayOrder',
        attributeUpdateModalRelationIdSelector: '#productAttributeUpdateModal #attributeRelationId',
        submitUpdateProductAttributeRelation: '#submitUpdateProductAttributeRelation',
        submitDeleteProductAttributeRelation: '#submitDeleteProductAttributeRelation',
        labelAttributeName: '.label-attribute-name',
        labelControlType: '.label-control-type',
        labelAttributeDisplayOrder: '.label-attribute-display-order',
        fieldAttributeId: '.field-attribute-id',
        fieldAttributeRelationId: '.field-attribute-relation-id',
        fieldControlTypeId: '.field-control-type-id',
        fieldAttributeDisplayOrder: '.field-attribute-display-order',
        fieldIsAttributeUpdated: '.is-attribute-updated',
        rowAttributeRelation: '.row-attribute-relation',
        productAttributeDeleteId: '#productAttributeDeleteId',
        formAttributeUpdateIdentifier: '#formAttributeUpdateIdentifier',
        formAttributeDeletionIdentifier: '#formAttributeDeletionIdentifier',
        listProductAttributes: '.list-product-attributes',
        attributeValueUpdateModal: 'productAttributeValueUpdateModal',
        attributeValueDeleteModal: 'productAttributeValueDeleteModal',
        attributeValueModalNameSelector: '#productAttributeValueUpdateModal #attributeValueName',
        attributeValueModalIdSelector: '#productAttributeValueUpdateModal #attributeValueId',
        attributeValueModalPriceAdjustmentSelector: '#productAttributeValueUpdateModal #PriceAdjustment',
        attributeValueModalPricePercentageAdjustmentSelector: '#productAttributeValueUpdateModal #PricePercentageAdjustment',
        attributeValueModalQuantitySelector: '#productAttributeValueUpdateModal #Quantity',
        attributeValueModalDisplayOrderSelector: '#productAttributeValueUpdateModal #attributeValueDisplayOrder',
        attributeValueModalIdOfAttributeRelation: '#productAttributeValueUpdateModal #idOfAttributeRelation',
        attributeValueModalIdOfAttribute: '#productAttributeValueUpdateModal #idOfAttribute',
        submitUpdateProductAttributeValue: '#productAttributeValueUpdateModal #submitUpdateProductAttributeValue',
        submitDeleteProductAttributeRelationValue: '#productAttributeValueDeleteModal #submitDeleteProductAttributeRelationValue',
        rowAttributeValue: '.row-attribute-value',
        fieldIsAttributeUpdated: '.is-attribute-value-updated',
        fieldAttributeValueName: '.field-attribute-value-name',
        fieldAttributeValueRelationId: '.field-attribute-value-relation-id',
        fieldAttributeValuePriceAdjustment: '.field-attribute-value-price-adjustment',
        fieldAttributeValuePricePercentageAdjustment: '.field-attribute-value-price-percentage-adjustment',
        fieldAttributeValueDisplayOrder: '.field-attribute-value-display-order',
        fieldAttributeValueQuantity: '.field-attribute-value-quantity',
        labelAttributeValueName: '.label-attribute-value-name',
        labelAttributeValuePriceAdjustment: '.label-attribute-value-price-adjustment',
        labelAttributeValuePricePercentageAdjustment: '.label-attribute-value-price-percentage-adjustment',
        labelAttributeValueQuantity: '.label-attribute-value-quantity',
        labelAttributeValueDisplayOrder: '.label-attribute-value-display-order',
        fieldAttributeValueAttributeId: '.field-attribute-value-attribute-id',
        listAttributeValues: '.list-attribute-{id}-values',
        productAttributeValueDeleteId: '#productAttributeValueDeleteId',
        formAttributeValueUpdateIdentifier: '#formAttributeValueUpdateIdentifier',
        formAttributeValueDeletionIdentifier: '#formAttributeValueDeletionIdentifier'
    };

    var features = {
        loadContentEditor: function (selector) {
            $(selector).tinymceEditor();
        },
        loadSelect2Ajax: function (selector, options) {
            if (options) {
                $(selector).select2Ajax(options);
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
                features.rerenderPicturesIndexes();
            });
        },
        rerenderPicturesIndexes: function () {
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
        // Product attribute relation
        initSubmitProductAttributeRelation: function (trigger) {
            $(trigger).on('click', function () {
                var attributeIdData = $(selectors.attributeUpdateModalIdSelector).select2('data');
                var controlTypeIdData = $(selectors.attributeUpdateModalControlTypeSelector).select2('data');
                var displayOrder = $(selectors.attributeUpdateModalDisplayOrderSelector).val();
                var attributeRelationId = $(selectors.attributeUpdateModalRelationIdSelector).val();
                // modify the existing attribute relation
                if (attributeRelationId) {
                    var formAttributeUpdateIdentifier = $(selectors.formAttributeUpdateIdentifier).val();
                    var attributeRow = $(generateAttributeRowSelector(formAttributeUpdateIdentifier));
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
                        $(selectors.listProductAttributes).prepend(response);
                        features.rerenderProductAttributeRelationRows();
                    });
                }

                features.hideModal('#' + selectors.attributeUpdateModal);
            });
        },
        initDeleteProductAttributeRelation: function (trigger) {
            $(trigger).on('click', function () {
                var attributeDeletionIdentifier = $(selectors.formAttributeDeletionIdentifier).val();
                var attributeRow = $(generateAttributeRowSelector(attributeDeletionIdentifier));
                attributeRow.remove();

                features.rerenderProductAttributeRelationRows();
                features.hideModal('#' + selectors.attributeDeleteModal);
            });
        },
        rerenderProductAttributeRelationRows: function () {
            $.each($(selectors.rowAttributeRelation), function (rowIndex, rowValue) {
                var attributeNameInputs = $(rowValue).find('input[name^="ProductAttributes"]');
                $.each(attributeNameInputs, function (index, value) {
                    var inputName = value.name.replace(/[\d\/]/, rowIndex);
                    value.name = inputName;
                });

                var attributeRelationIndexSelector = $(rowValue).find('data-attribute-relation-index');
                attributeRelationIndexSelector.data('attribute-relation-index', rowIndex);
                attributeRelationIndexSelector.attr('data-attribute-relation-index', rowIndex);
            });
        },
        rerenderProductAttributeValueRows: function () {
            var attributeValueRows = $(selectors.rowAttributeValue);
            $.each(attributeValueRows, function (rowIndex, rowValue) {
                var attributeValueNameInputs = $(rowValue).find('input[type="hidden"]');
                $.each(attributeValueNameInputs, function (index, value) {
                    var fieldName = value.name.split('.')[2];
                    if (fieldName) {
                        var inputName = 'ProductAttributes[0].AttributeRelationValues[' + rowIndex + '].' + fieldName;
                        value.name = inputName;
                    }
                });
            });
        },
        // Product attribute value
        initSubmitProductAttributeValue: function (trigger) {
            $(trigger).on('click', function () {
                var attributeValueName = $(selectors.attributeValueModalNameSelector).val();
                var attributeValueDisplayOrder = $(selectors.attributeValueModalDisplayOrderSelector).val();
                var attributeValuePriceAdjustment = $(selectors.attributeValueModalPriceAdjustmentSelector).val();
                var attributeValuePricePercentageAdjustment = $(selectors.attributeValueModalPricePercentageAdjustmentSelector).val();
                var attributeValueQuantity = $(selectors.attributeValueModalQuantitySelector).val();
                var idOfAttribute = $(selectors.attributeValueModalIdOfAttribute).val();
                var formAttributeValueUpdateIdentifier = $(selectors.formAttributeValueUpdateIdentifier).val();
                if (!attributeValueName) {
                    return;
                }

                if (attributeValuePriceAdjustment) {
                    attributeValuePricePercentageAdjustment = 0;
                }

                if (attributeValuePricePercentageAdjustment) {
                    attributeValuePriceAdjustment = 0;
                }

                var idOfAttributeRelation = $(selectors.attributeValueModalIdOfAttributeRelation).val();
                var attributeValueId = $(selectors.attributeValueModalIdSelector).val();
                // modify the existing attribute value
                if (attributeValueId) {
                    var attributeValueRow = $(generateAttributeValueRowSelector(formAttributeValueUpdateIdentifier));
                    attributeValueRow.find(selectors.labelAttributeValueName).text(attributeValueName);
                    attributeValueRow.find(selectors.labelAttributeValueDisplayOrder).text(attributeValueDisplayOrder);
                    attributeValueRow.find(selectors.labelAttributeValuePriceAdjustment).text(attributeValuePriceAdjustment);
                    attributeValueRow.find(selectors.labelAttributeValuePricePercentageAdjustment).text(attributeValuePricePercentageAdjustment);
                    attributeValueRow.find(selectors.labelAttributeValueQuantity).text(attributeValueQuantity);

                    attributeValueRow.find(selectors.fieldAttributeValueName).val(attributeValueName);
                    attributeValueRow.find(selectors.fieldAttributeValueDisplayOrder).val(attributeValueDisplayOrder);
                    attributeValueRow.find(selectors.fieldAttributeValuePriceAdjustment).val(attributeValuePriceAdjustment);
                    attributeValueRow.find(selectors.fieldAttributeValuePricePercentageAdjustment).val(attributeValuePricePercentageAdjustment);
                    attributeValueRow.find(selectors.fieldAttributeValueQuantity).val(attributeValueQuantity);
                    attributeValueRow.find(selectors.fieldAttributeValueRelationId).val(idOfAttributeRelation);
                    attributeValueRow.find(selectors.fieldAttributeValueAttributeId).val(idOfAttribute);
                }
                else {
                    $.post(this.dataset.url, {
                        name: attributeValueName,
                        priceAdjustment: attributeValuePriceAdjustment,
                        pricePercentageAdjustment: attributeValuePricePercentageAdjustment,
                        quantity: attributeValueQuantity,
                        displayOrder: attributeValueDisplayOrder,
                        attributeRelationId: idOfAttributeRelation,
                        attributeId: idOfAttribute
                    }, function (response) {
                        var listAttributeValues = generateListAttributeValuesSelector(formAttributeValueUpdateIdentifier);
                        $(listAttributeValues).prepend(response);
                        features.rerenderProductAttributeValueRows();
                    });
                }

                features.hideModal('#' + selectors.attributeValueUpdateModal);
            });
        },
        initDeleteProductAttributeValue: function (trigger) {
            $(trigger).on('click', function () {
                var attributeValueDeletionIdentifier = $(selectors.formAttributeValueDeletionIdentifier).val();
                var attributeValueRow = $(generateAttributeValueRowSelector(attributeValueDeletionIdentifier));
                attributeValueRow.remove();

                features.rerenderProductAttributeValueRows();
                features.hideModal('#' + selectors.attributeValueDeleteModal);
            });
        },
    };

    // Product attribute relation model
    var loadAttributeUpdateModel = function (trigger, modalSelector) {
        var attributeUpdateIdSelector = $(selectors.attributeUpdateModalIdSelector);
        var attributeUpdateControlTypeSelector = $(selectors.attributeUpdateModalControlTypeSelector);
        var attributeRow = $(trigger).parents(selectors.rowAttributeRelation);
        var isAttributeUpdated = attributeRow.find(selectors.fieldIsAttributeUpdated).val();

        $(selectors.formAttributeUpdateIdentifier).val(trigger.dataset.formidentifier);
        if (!trigger.dataset.id || trigger.dataset.id === '0' || isAttributeUpdated) {
            var labelAttributeName = attributeRow.find(selectors.labelAttributeName).text();
            var labelControlType = attributeRow.find(selectors.labelControlType).text();

            var fieldAttributeId = attributeRow.find(selectors.fieldAttributeId).val();

            var fieldControlTypeId = attributeRow.find(selectors.fieldControlTypeId).val();
            var displayOrder = attributeRow.find(selectors.fieldAttributeDisplayOrder).val();

            $(selectors.attributeUpdateModalDisplayOrderSelector).val(displayOrder);

            var fieldAttributeRelationId = trigger.dataset.id;
            if (fieldAttributeRelationId) {
                $(selectors.attributeUpdateModalRelationIdSelector).val(fieldAttributeRelationId);
            }
            else {
                $(selectors.attributeUpdateModalRelationIdSelector).val('');
                $(selectors.submitUpdateProductAttributeRelation).attr('data-url', trigger.dataset.url);
            }

            attributeUpdateIdSelector.select2Ajax("val", { id: fieldAttributeId, text: labelAttributeName });
            attributeUpdateControlTypeSelector.select2Ajax("val", { id: fieldControlTypeId, text: labelControlType });
        }
        else {
            $.get(trigger.dataset.url, { id: trigger.dataset.id }, function (response) {
                $(selectors.attributeUpdateModalDisplayOrderSelector).val(response.displayOrder);
                $(selectors.attributeUpdateModalRelationIdSelector).val(response.id);

                attributeUpdateIdSelector.select2Ajax("val", { id: response.attributeId, text: response.name });
                attributeUpdateControlTypeSelector.select2Ajax("val", { id: response.controlTypeId, text: response.controlTypeName });
            });
        }
    }

    var onAttributeUpdateModelShown = function (trigger, modalSelector) {
        var currentAttributeIdSelectors = $(selectors.fieldAttributeId);
        var currentAttributeIds = [];
        $.each(currentAttributeIdSelectors, function (index, value) {
            currentAttributeIds.push(parseInt(value.value))
        });
        features.loadSelect2Ajax(selectors.attributeUpdateModalIdSelector,
            {
                dropdownParent: $('#' + modalSelector),
                extendParams: {
                    excluded: currentAttributeIds.join(',')
                }
            });
        features.loadSelect2Ajax(selectors.attributeUpdateModalControlTypeSelector, { dropdownParent: $('#' + modalSelector) });
    }

    var clearAttributeUpdateModel = function () {
        $(selectors.attributeUpdateModalDisplayOrderSelector).val(0);
        $(selectors.attributeUpdateModalRelationIdSelector).val(0);

        $(selectors.attributeUpdateModalIdSelector).select2Ajax("remove");
        $(selectors.attributeUpdateModalControlTypeSelector).select2Ajax("remove");
        $(selectors.submitUpdateProductAttributeRelation).removeAttr('data-url');
    }

    var loadAttributeDeleteModel = function (trigger) {
        $(selectors.productAttributeDeleteId).val(trigger.dataset.id);
        $(selectors.formAttributeDeletionIdentifier).val(trigger.dataset.formidentifier);
    }

    var clearAttributeDeleteModel = function () {
        $(selectors.productAttributeDeleteId).val(0);
        $(selectors.formAttributeDeletionIdentifier).val('');
    }

    var generateAttributeRowSelector = function (attributeDeletionIdentifier) {
        return selectors.rowAttributeRelation + '-' + attributeDeletionIdentifier;
    };

    // Product attribute value model
    var loadAttributeValueUpdateModel = function (trigger, modalSelector) {
        var attributeValueRow = $(trigger).parents(selectors.rowAttributeValue);
        var isAttributeValueUpdated = attributeValueRow.find(selectors.fieldIsAttributeUpdated).val();
        var fieldAttributeId = $(trigger).parents(selectors.rowAttributeRelation)
            .find(selectors.fieldAttributeId).val();

        var idOfAttributeRelation = trigger.dataset.attributeid;
        if (!trigger.dataset.id || trigger.dataset.id === '0' || isAttributeValueUpdated) {
            var fieldAttributeValueName = attributeValueRow.find(selectors.fieldAttributeValueName).val();
            var fieldAttributeValuePriceAdjustment = attributeValueRow.find(selectors.fieldAttributeValuePriceAdjustment).val();
            var fieldAttributeValuePricePercentageAdjustment = attributeValueRow.find(selectors.fieldAttributeValuePricePercentageAdjustment).val();
            var fieldAttributeValueDisplayOrder = attributeValueRow.find(selectors.fieldAttributeValueDisplayOrder).val();
            var fieldAttributeValueQuantity = attributeValueRow.find(selectors.fieldAttributeValueQuantity).val();

            var fieldAttributeValueId = trigger.dataset.id;
            if (fieldAttributeValueId) {
                $(selectors.attributeValueModalIdSelector).val(fieldAttributeValueId);
            }
            else {
                $(selectors.attributeValueModalIdSelector).val('');
                $(selectors.submitUpdateProductAttributeValue).attr('data-url', trigger.dataset.url);
            }

            $(selectors.attributeValueModalDisplayOrderSelector).val(fieldAttributeValueDisplayOrder);
            $(selectors.attributeValueModalIdSelector).val(fieldAttributeValueId);
            $(selectors.attributeValueModalPriceAdjustmentSelector).val(fieldAttributeValuePriceAdjustment);
            $(selectors.attributeValueModalPricePercentageAdjustmentSelector).val(fieldAttributeValuePricePercentageAdjustment);
            $(selectors.attributeValueModalNameSelector).val(fieldAttributeValueName);
            $(selectors.attributeValueModalQuantitySelector).val(fieldAttributeValueQuantity);
            $(selectors.attributeValueModalIdOfAttributeRelation).val(idOfAttributeRelation);
            $(selectors.attributeValueModalIdOfAttribute).val(fieldAttributeId);
            $(selectors.formAttributeValueUpdateIdentifier).val(trigger.dataset.formidentifier);
        }
        else {
            $.get(trigger.dataset.url, { id: trigger.dataset.id }, function (response) {
                $(selectors.attributeValueModalDisplayOrderSelector).val(response.displayOrder);
                $(selectors.attributeValueModalIdSelector).val(response.id);
                $(selectors.attributeValueModalPriceAdjustmentSelector).val(response.priceAdjustment);
                $(selectors.attributeValueModalPricePercentageAdjustmentSelector).val(response.pricePercentageAdjustment);
                $(selectors.attributeValueModalNameSelector).val(response.name);
                $(selectors.attributeValueModalIdOfAttributeRelation).val(idOfAttributeRelation);
                $(selectors.attributeValueModalIdOfAttribute).val(fieldAttributeId);
                $(selectors.formAttributeValueUpdateIdentifier).val(trigger.dataset.formidentifier);
            });
        }
    }

    var onAttributeValueUpdateModelShown = function (trigger, modalSelector) {
        features.loadSelect2Ajax(selectors.attributeUpdateModalIdSelector, { dropdownParent: $('#' + modalSelector) });
        features.loadSelect2Ajax(selectors.attributeUpdateModalControlTypeSelector, { dropdownParent: $('#' + modalSelector) });
    }

    var clearAttributeValueUpdateModel = function () {
        $(selectors.attributeValueModalDisplayOrderSelector).val(0);
        $(selectors.attributeValueModalIdSelector).val(0);
        $(selectors.attributeValueModalPriceAdjustmentSelector).val(0);
        $(selectors.attributeValueModalPricePercentageAdjustmentSelector).val(0);
        $(selectors.attributeValueModalNameSelector).val('');
        $(selectors.attributeValueModalQuantitySelector).val(null);
        $(selectors.attributeValueModalIdOfAttributeRelation).val(0);
        $(selectors.attributeValueModalIdOfAttribute).val(0);
    }

    var loadAttributeValueDeleteModel = function (trigger) {
        $(selectors.productAttributeValueDeleteId).val(trigger.dataset.id);
        $(selectors.formAttributeValueDeletionIdentifier).val(trigger.dataset.formidentifier);
    }

    var clearAttributeValueDeleteModel = function () {
        $(selectors.productAttributeValueDeleteId).val(0);
        $(selectors.formAttributeValueDeletionIdentifier).val('');
    }

    var generateAttributeValueRowSelector = function (identifier) {
        return selectors.rowAttributeValue + '-' + identifier;
    };

    var generateListAttributeValuesSelector = function (formIdentifier) {
        var replacement = null;
        if (formIdentifier) {
            replacement = "-" + formIdentifier;
        }
        return selectors.listAttributeValues.replace('-{id}', replacement);
    };

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
        features.loadModal(selectors.attributeUpdateModal, loadAttributeUpdateModel, clearAttributeUpdateModel, onAttributeUpdateModelShown);
        features.loadModal(selectors.attributeDeleteModal, loadAttributeDeleteModel, clearAttributeDeleteModel);
        features.initSubmitProductAttributeRelation(selectors.submitUpdateProductAttributeRelation);
        features.initDeleteProductAttributeRelation(selectors.submitDeleteProductAttributeRelation);
        features.loadModal(selectors.attributeValueUpdateModal, loadAttributeValueUpdateModel, clearAttributeValueUpdateModel, onAttributeValueUpdateModelShown);
        features.initSubmitProductAttributeValue(selectors.submitUpdateProductAttributeValue);
        features.initDeleteProductAttributeValue(selectors.submitDeleteProductAttributeRelationValue);
        features.loadModal(selectors.attributeValueDeleteModal, loadAttributeValueDeleteModel, clearAttributeValueDeleteModel);
    };
})(window, document);

$(function () {
    window.onload = initialize;
});