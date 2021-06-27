; (function (global) {
    'use strict';
    var features = {
        selectors: {
            select2Ajax: '.select2-remote-ajax',
            searchDropdownsParent: '#searchForm'
        },
        loadSelect2Ajax: function (selector, dropdownParent) {
            $(selector).select2Ajax({ dropdownParent: $(dropdownParent) });
        }
    };

    global.initialize = function () {
        var selectors = features.selectors;
        features.loadSelect2Ajax(selectors.select2Ajax, selectors.searchDropdownsParent);
    };
})(window, document);

$(function () {
    window.onload = initialize;
});