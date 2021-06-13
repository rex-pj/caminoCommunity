$(function () {
    $('.datetimepicker').datetimepicker({
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
        format: 'DD/MM/YYYY HH:mm'
    });
    $(".select2-remote-ajax").select2Ajax({ dropdownParent: $("#searchForm") });

    var articleDeactivateModal = document.getElementById('articleDeactivateModal');
    articleDeactivateModal.addEventListener('show.bs.modal', function (e) {
        var command = e.relatedTarget;
        this.getElementsByClassName('article-id')[0].value = command.dataset.id;
    });

    articleDeactivateModal.addEventListener('hide.bs.modal', function (e) {
        this.getElementsByClassName('article-id')[0].value = '';
    });

    var articleActivateModal = document.getElementById('articleActivateModal');
    articleActivateModal.addEventListener('show.bs.modal', function (e) {
        var command = e.relatedTarget;
        this.getElementsByClassName('article-id')[0].value = command.dataset.id;
    });

    articleActivateModal.addEventListener('hide.bs.modal', function (e) {
        this.getElementsByClassName('article-id')[0].value = '';
    });

    var articleRestoreModal = document.getElementById('articleRestoreModal');
    articleRestoreModal.addEventListener('show.bs.modal', function (e) {
        var command = e.relatedTarget;
        this.getElementsByClassName('article-id')[0].value = command.dataset.id;
    });

    articleRestoreModal.addEventListener('hide.bs.modal', function (e) {
        this.getElementsByClassName('article-id')[0].value = '';
    });

    var articlePermanentlyDeleteModal = document.getElementById('articlePermanentlyDeleteModal');
    articlePermanentlyDeleteModal.addEventListener('show.bs.modal', function (e) {
        var command = e.relatedTarget;
        this.getElementsByClassName('article-id')[0].value = command.dataset.id;
    });

    articlePermanentlyDeleteModal.addEventListener('hide.bs.modal', function (e) {
        this.getElementsByClassName('article-id')[0].value = '';
    });

    var articleTemporaryDeleteModal = document.getElementById('articleTemporaryDeleteModal');
    articleTemporaryDeleteModal.addEventListener('show.bs.modal', function (e) {
        var command = e.relatedTarget;
        this.getElementsByClassName('article-id')[0].value = command.dataset.id;
    });

    articleTemporaryDeleteModal.addEventListener('hide.bs.modal', function (e) {
        this.getElementsByClassName('article-id')[0].value = '';
    });
});