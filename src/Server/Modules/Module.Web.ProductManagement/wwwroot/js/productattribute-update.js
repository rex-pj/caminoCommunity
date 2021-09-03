; (function (global) {
    'use strict';
    var features = {
        selectors: {
            select2Ajax: '.select2-remote-ajax'
        },
        loadSelect2Ajax: function (selector) {
            $(selector).select2Ajax();
        }
    };

    global.initialize = function () {
        var selectors = features.selectors;
        features.loadSelect2Ajax(selectors.select2Ajax);
    };
})(window, document);

$(function () {
    window.onload = initialize;
});