$.fn.select2Ajax = function (options, selectedData) {
    var select2s = $(this);
    if (typeof (options) === "string" && options === "val") {
        $.each(select2s, function (key, selection) {
            var select2 = $(selection);
            setDefaultSelect2Ajax(select2, selectedData);
        });

        return;
    }

    if (typeof (options) === "string" && options === "remove") {
        $.each(select2s, function (key, selection) {
            var select2 = $(selection);
            removeSelectedSelect2Ajax(select2);
        });

        return;
    }

    var defaults = {
        minimumInputLength: 0
    };

    var settings = $.extend({}, defaults, options);
    if (select2s.length === 0) {
        return;
    }

    $.each(select2s, function (key, selection) {
        var select2 = $(selection);
        loadSelect2Ajax(select2);

        select2.on('change', function (e, value) {
            setTimeout(function () {
                loadSelect2Ajax(select2);
            }, 100);
            clearTimeout();
        });

        select2.on('select2:unselect', function (e) {
            setTimeout(function () {
                loadSelect2Ajax(select2);
            }, 100);
            clearTimeout();
        })
    });

    function setDefaultSelect2Ajax(selector, selected) {
        if (!selected.id) {
            return;
        }

        if (!selector.find("option[value='" + selected.id + "']").length) {
            selector.append(new Option(selected.text, selected.id, true, true));
        }

        selector.value = selected.id;
        selector.trigger('change');
    }

    function removeSelectedSelect2Ajax(selector) {
        selector.val(null);
        selector.html('');
        selector.select2('destroy');
    }

    function loadSelect2Ajax(select2) {
        var method = select2.attr("method") ? select2.attr("method") : "get";
        var url = select2.data("url");
        var selected = select2.val();
        if (selected && typeof (selected) !== 'string') {
            selected = selected.join(',');
        }

        var placeholder = select2.attr('placeholder');

        select2.select2({
            allowClear: true,
            placeholder: placeholder ? placeholder : 'select..',
            ajax: {
                url: url,
                type: method,
                data: function (params) {
                    var query = $.extend({}, {
                        q: params.term,
                        currentId: selected
                    }, settings.extendParams);

                    return query;
                },
                dataType: 'json',
                processResults: function (response) {
                    if (response === null || response === undefined) {
                        return {
                            results: []
                        };
                    }

                    return {
                        results: response
                    };
                },
            },
            minimumInputLength: settings.minimumInputLength,
            dropdownParent: settings.dropdownParent,
        });
    }
}

$.fn.filterDataTable = function () {
    $(this).on('submit', function (e) {
        e.preventDefault();
        var filterForm = $(this);
        var target = $(filterForm.data('target'));
        var loading = target.find('.loading').clone();
        loading.removeClass('d-none');

        target.html(loading);
        var data = filterForm.serializeArray();
        var url = filterForm.attr('action');
        $.get(url, data, function (response) {
            loading.addClass('d-none');
            target.append(response);
            $(".common-tooltip").tooltip();
        });
    });
}

$.fn.checkboxGroup = function () {
    $(this).on('focusin', function (e) {
        var checkbox = $(this);
        var isChecked = checkbox.is(':checked');
        checkbox.data('ischecked', isChecked);
    }).on('change', function (e) {
        var checkbox = $(this);
        var checkboxGroup = checkbox.attr('checkbox-group');
        if (checkboxGroup) {
            $('[checkbox-group="' + checkboxGroup + '"]').prop("checked", '');
            var isChecked = checkbox.data('ischecked');
            if (!isChecked) {
                checkbox.prop("checked", 'checked');
            }
        }
    });
}

$.fn.tinymceEditor = function () {
    var editor = $(this);
    var id = editor[0].id;
    tinymce.init({
        selector: '#' + id,
        height: 300,
        menubar: false,
        plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks code fullscreen',
            'insertdatetime media table contextmenu paste code imagetools '
        ],
        toolbar: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        file_picker_types: 'image',
        file_picker_callback: function (callback, value, meta) {
            if (meta.filetype == 'image') {
                // creating input on-the-fly
                var input = $(document.createElement('input'));
                input.attr("type", "file");
                input.trigger('click'); // opening dialog
                $(input).on('change', function () {
                    var file = this.files[0];
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        callback(e.target.result, {
                            alt: ''
                        });
                    };
                    reader.readAsDataURL(file);
                });
            }
        },
        branding: false
    });
}

function base64toBlob(dataURI, mineType) {
    var urlSplitted = dataURI.split(',');
    var byteString = null;
    if (urlSplitted.length > 1) {
        byteString = atob(dataURI.split(',')[1]);
    }
    else {
        byteString = atob(dataURI);
    }

    var buffers = new ArrayBuffer(byteString.length);
    var unitBuffers = new Uint8Array(buffers);

    for (var i = 0; i < byteString.length; i++) {
        unitBuffers[i] = byteString.charCodeAt(i);
    }

    mineType = mineType ? mineType : 'image/jpeg';
    return new Blob([buffers], { type: mineType });
}

$(function () {
    $(".common-tooltip").tooltip();
    $('.filter-form').filterDataTable();
});