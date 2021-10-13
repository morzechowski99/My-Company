/*additional validating methods*/

$.validator.addMethod('filesize', function (value, element, arg) {
    if (element.files[0].size <= arg) {
        return true;
    } else {
        return false;
    }
});

/*usage data-val-filesize="error message", data-val-filesize-size="size in bytes"*/
$.validator.unobtrusive.adapters.addSingleVal("filesize", "size")