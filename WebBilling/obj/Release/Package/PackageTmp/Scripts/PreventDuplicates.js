$(document).on('invalid-form.validate', 'form', function () {
    var button = $(this).find('input[type="submit"]');
    setTimeout(function () {
        button.removeAttr('disabled');
    }, 1);
});
$(document).on('submit', 'form', function () {
    var button = $(this).find('input[type="submit"]');
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);
});