; (function (global) {
    'use strict';
    var features = {
        selectors: {
            datetimepicker: '.datetimepicker',
            select2Ajax: '.select2-remote-ajax',
            deactivateModal: 'deactivateModal',
            activateModal: 'activateModal',
            permanentlyDeleteModal: 'permanentlyDeleteModal',
            modalEntityIdentifier: 'identifier',
            searchDropdownsParent: '#searchForm'
        },
        settings: {
            dateFormat: 'DD/MM/YYYY HH:mm'
        },
        loadDateTimePicker: function (selector, format) {
            $(selector).datetimepicker({
                icons: {
                    time: 'far fa-clock',
                    date: 'far fa-calendar',
                    up: 'fas fa-arrow-up',
                    down: 'fas fa-arrow-down',
                    previous: 'fas fa-chevron-left',
                    next: 'fas fa-chevron-right',
                    today: 'far fa-calendar-check',
                    clear: 'far fa-trash-alt',
                    close: 'fas fa-times'
                },
                format: format
            });
        },
        loadSelect2Ajax: function (selector, dropdownParent) {
            $(selector).select2Ajax({ dropdownParent: $(dropdownParent) });
        },
        loadModal: function (selector, identifier) {
            var modal = document.getElementById(selector);
            modal.addEventListener('show.bs.modal', function (e) {
                var trigger = e.relatedTarget;
                this.getElementsByClassName(identifier)[0].value = trigger.dataset.id;
            });

            modal.addEventListener('hide.bs.modal', function (e) {
                this.getElementsByClassName(identifier)[0].value = '';
            });
        }
    };

    global.initialize = function () {
        var selectors = features.selectors;
        var settings = features.settings;
        features.loadDateTimePicker(selectors.datetimepicker, settings.dateFormat);
        features.loadSelect2Ajax(selectors.select2Ajax, selectors.searchDropdownsParent);
        features.loadModal(selectors.activateModal, selectors.modalEntityIdentifier);
        features.loadModal(selectors.deactivateModal, selectors.modalEntityIdentifier);
        features.loadModal(selectors.permanentlyDeleteModal, selectors.modalEntityIdentifier);
    };
})(window, document);

$(function () {
    window.onload = initialize;
});