$.fn.select2Ajax = function (options) {
    var defaults = {
        minimumInputLength: 0
    };

    var settings = $.extend({}, defaults, options);
    var select2s = $(this);
    if (select2s.length === 0) {
        return;
    }

    $.each(select2s, function (key, value) {
        var select2 = $(value);
        var method = select2.attr("method") ? select2.attr("method") : "get";
        var url = select2.data("url");

        select2.select2({
            allowClear: true,
            placeholder: 'select..',
            ajax: {
                url: url,
                type: method,
                dataType: 'json',
                processResults: function (data) {
                    if (data === null || data === undefined) {
                        return {
                            results: []
                        };
                    }

                    return {
                        results: data
                    };
                },
            },
            minimumInputLength: settings.minimumInputLength,
            dropdownParent: settings.dropdownParent
        });
    });
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

$(function () {
    $(".common-tooltip").tooltip();
    $('.filter-form').filterDataTable();
});