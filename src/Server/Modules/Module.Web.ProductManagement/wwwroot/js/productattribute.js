; (function (global) {
    'use strict';
    var features = {
        selectors: {
            deactivateModal: 'deactivateModal',
            activateModal: 'activateModal',
            permanentlyDeleteModal: 'permanentlyDeleteModal',
            modalEntityIdentifier: 'identifier',
            searchDropdownsParent: '#searchForm'
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
        features.loadModal(selectors.activateModal, selectors.modalEntityIdentifier);
        features.loadModal(selectors.deactivateModal, selectors.modalEntityIdentifier);
        features.loadModal(selectors.permanentlyDeleteModal, selectors.modalEntityIdentifier);
    };
})(window, document);

$(function () {
    window.onload = initialize;
});