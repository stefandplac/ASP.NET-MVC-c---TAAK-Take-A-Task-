$(document).ready(() => {
    //populate date selector
    var date = new Date($('#taskEndDate').val().toString('yyyy-MM-dd'));
    var year = date.getFullYear();
    var day = date.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var month = date.getMonth()+1;
    $('#taskDate').val(year + '-' + month + '-' + day);
 
    //take dateOption data and displayed it to Ui
    const options = { weekday: 'long', year: 'numeric', month: 'short', day: 'numeric' };
    $('#taskEndDate').val(date.toISOString());
    $('.spanDate').text(date.toLocaleDateString('en-us', options));

    
    var dateOptionElem = $('.dateOption');
    for (const el of dateOptionElem) {
        var elValue = $(el).text();
        var valueToCheck = $('#dateOptionInput').val();
        if (elValue == valueToCheck) {
             $(el).parent().attr('style', 'color:white; background-color:#008fb4;');
        }
    }
   

    //take timeOptions data and display those in UI
    var timeOptionElem = $('.timeOptionChild');
    var timeOptionsSelected = $('#timeOptionsInput').val();
    for (const el of timeOptionElem) {
        var valueToCheck = $(el).text().trim();
        if (timeOptionsSelected.includes(valueToCheck)) {
            $(el).parent().data('selected', "isSelected");
            $(el).parent().attr('style', 'color:white; background-color:#008fb4;');

        }
    }
   

});

