const selectDateOptions = (divParent, divSelected) => {
    $(divParent).children().attr('style', '');
    $(divSelected).attr('style', 'color:white; background-color:#008fb4;');
    
    
}

const selectTimeOption = (divSelected) => {
    var isSelect = $(divSelected).data('selected');
    //console.log(isSelect);
    //console.log(isSelect == "notSelected");
    if (isSelect == "notSelected") {
        $(divSelected).attr('style', 'color:white; background-color:#008fb4;');
        $(divSelected).data('selected', 'isSelected');
        return;
    }
    else if (isSelect == "isSelected") {
        $(divSelected).attr('style', '');
        $(divSelected).data('selected', 'notSelected');
        return;

    }

}

const activateDateOptions = (input) => {
    if (input.value =="") {
        $('#dateOptions').hide(300);
        $('#timeOptionsParent').children().attr('style', '');
        $('#timeOptionsParent').children().data('selected', 'notSelected');
        $('#activateTimeOpt').prop('checked', false);
        $('#timeOptions').hide(300);
        return;
    }
    $('#dateOptions').show(600);
    const options = { weekday: 'long', year: 'numeric', month: 'short', day: 'numeric' };
    var date = new Date(input.value);
    $('.spanDate').text(date.toLocaleDateString('en-us', options));
    
}

const activateTimeOptions = (checkbox) => {
    var timeOptions = $('#timeOptions');
    if (checkbox.checked == true) {
        timeOptions.show(600);
        $('.spanDate').attr('required');
        $(checkbox).attr('style', 'background-color:#91beca;');
       
    }
    else {
        timeOptions.hide(300);
        $('.spanDate').removeAttr('required');
        //when unchecked I want that all inserted data to be deleted and also restore default settings
        $('#timeOptionsParent').children().attr('style', '');
        $('#timeOptionsParent').children().data('selected','notSelected');
    }
}

const handleChange = (checkbox) => {
    if (checkbox.checked == true) {
        $('#specialinput').show(600);
        $(checkbox).attr('style', 'background-color:#91beca;');
    }
    else {
        $('#specialinput').hide(300);
    }
}


