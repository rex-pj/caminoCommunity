$.fn.select2Ajax = function (options) {
    var defaults = {
        minimumInputLength: 0
    };

    var settings = $.extend({}, defaults, options);
    var select2 = $(this);
    select2.select2({
        ajax: {
            url: select2.data("url"),
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
}

$(document).ready(function () {
    $(".common-tooltip").tooltip();
    $(".select2-remote-ajax").select2Ajax({ dropdownParent: $("#grantRoleModal") });
});