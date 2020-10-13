function CalltoApiController(url, model) {    
    var result;
    $.ajax({
        url: url.replace("%2F", "/"),
        method: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(model),
        async: false,
        success: function (data) {
            result = data;
        },
    });
    return result;
}

function ShowMessage(msgid, functionname) {
    var Mmodel = {
        MessageID: msgid,
    };

    var data = CalltoApiController($("#MessageURL").val(), Mmodel)

    var msgdata = JSON.parse(data);

    Swal.fire({
        icon: msgdata[0].MessageIcon,
        title: msgdata[0].MessageID,
        text: msgdata[0].MessageText,
    }).then(function () {
        if (functionname) {
            var fn = window[functionname];
            fn();
        }
    })
}

function RequiredCheck(ctrl) {
    $(ctrl).attr("data-Required", "1");
}


function AlreadyExistsCheck(apiURL, ctrl1, ctrl2, ctrl3) {    
    $(ctrl1).attr("data-AlreadyExistsCheck", "1");
    $(ctrl1).attr("data-AlreadyExistsApiUrl", apiURL);   
    $(ctrl1).attr("data-AlreadyExistsValue1", ctrl1.attr("id"));
    if (ctrl2!="")
        $(ctrl1).attr("data-AlreadyExistsValue2", ctrl2.attr("id"));
    if (ctrl3!="")
        $(ctrl1).attr("data-AlreadyExistsValue3", ctrl3.attr("id"));
}

function KeyDown(e, ctrl, functionname) {
    if (e.which == 13) {
        e.preventDefault();
        var result = ErrChk(ctrl);
        if (result == "0") {
            moveNext(ctrl);
            if (functionname) {
                var fn = window[functionname];
                fn('OK');
            }
        }
        else {
            ShowMessage(result, functionname);
        }
    }
}

function ErrChk(ctrl) {
    var req = $(ctrl).attr("data-Required");
    if (req == "1") {
        if (!$(ctrl).val()) {
            return "E102";
        }
    }

    var aec = $(ctrl).attr("data-AlreadyExistsCheck");
    if (aec == "1") {
        var ApiURL = $(ctrl).attr("data-AlreadyExistsApiUrl");
        var val1 = $(ctrl).attr("data-AlreadyExistsValue1");
        var val2 = $(ctrl).attr("data-AlreadyExistsValue2");
        var val3 = $(ctrl).attr("data-AlreadyExistsValue3");
        val1 = $("#" + val1).val();
        if (val2 == undefined)
            val2 = "";
        else
            val2 = $("#" + val2).val();

        if (val3 == undefined)
            val3 = "";
        else
            val3 = $("#" + val3).val();
       
        var model = {
            Val1: val1,
            Val2: val2,
            Val3: val3
        }; 
        
        var response = CalltoApiController(ApiURL, model);
        var data = JSON.parse(response);
        if (data[0].MessageID == 'E101')
            return "0";
        else
            return data[0].MessageID;
        
    }

    return "0";
}

function moveNext(ctrl) {
    do {
        ctrl = $('[tabIndex=' + (+$(ctrl).attr("tabIndex") + 1) + ']');
        if (!$(ctrl).length) {
            ctrl = $('[tabIndex=1]');
            break;
        }
    } while ($(ctrl).is('[disabled=disabled]'));
    $(ctrl).select();
    $(ctrl).focus();
}

function ErrorCheckOnClick(e1) {
    var r1 = "0";
    $('#' + e1 + ' *').filter(':input').each(function () {
        var result = ErrChk(this);
        if (result != "0") {
            $(this).focus();
            r1 = result;
            return false;
        }
    });
    return r1;
}

function isNumberKey(evt) {    
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

//show error message based on result
function ShowAlert(iconName,titleMsg,errorMsg) {
    swal.fire({
        icon: iconName,
        title: titleMsg,
        text: errorMsg
    });
}

//function SetThousandSeparator(ctrl_id) { //function to add commas to textboxes
//var obj=$("#"+ctrl_id);
//var cleave = new Cleave(obj, {
//    numeral: true,
//    numeralThousandsGroupStyle: 'thousand'
//    });
//}
