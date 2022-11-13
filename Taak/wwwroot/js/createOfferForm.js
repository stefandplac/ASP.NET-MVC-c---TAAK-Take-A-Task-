const handleChange = (form) => {
    var el = $('#btnSubmit');
    if ($(form).valid()) {
        el.attr('style', 'background-color:#92a8ad');
        el.prop('disabled', false);
    }
    else {
        el.attr('style', 'background-color:lightgray; border:1px solid white;');
        el.prop('disabled', true);
    }
    
}