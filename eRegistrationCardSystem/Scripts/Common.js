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

//show error message based on result
function ShowAlert(iconName, titleMsg, errorMsg) {
    swal.fire({
        icon: iconName,
        text: titleMsg,
        title: errorMsg,
        width: 530
    });
}

