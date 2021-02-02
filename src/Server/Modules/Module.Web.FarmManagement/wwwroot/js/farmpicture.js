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
});