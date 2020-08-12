$(function () {
    $('.datepicker').datepicker();

    $(".select2-remote-ajax").select2Ajax({ dropdownParent: $("#searchForm") });
});