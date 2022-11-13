$(document).ready(() => {
    $('#dateOptionInput').val($('#dateOptionDefault').text());    
});

const setCounty = (input) => {
    
}


const selectDateOptions = (divParent, divSelected) => {
    $(divParent).children().attr('style', '');
    $(divSelected).attr('style', 'color:white; background-color:#008fb4;');
    $('#dateOptionInput').val($(divSelected).first().text().trim());
    
    
}

const selectTimeOption = (divSelected) => {
    var el = $(divSelected);
    var isSelect = el.data('selected');
    if (isSelect == "notSelected") {
       el.attr('style', 'color:white; background-color:#008fb4;');
       el.data('selected', "isSelected");

    }
    else if (isSelect == "isSelected") {
        el.attr('style',"");
        el.data('selected', "notSelected");
    }
    var timeOptionsResult ="";
    
    var elements = $('.timeOptionChild');
    for (const el of elements) {
        //console.log($(el).parent());
        //console.log($(el).parent().data('selected'));
        var parentIsSelected = $(el).parent().data('selected');
        if (parentIsSelected == "isSelected") {
            timeOptionsResult +=$(el).text().trim()+',';
        }
        
    }
    //console.log(timeOptionsResult);
    $('#timeOptionsInput').val(timeOptionsResult);
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
    $('#taskEndDate').val(date.toISOString());    
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


