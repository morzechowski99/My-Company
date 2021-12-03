$.validator.addMethod("requiredifchecked", function (value, element, params) {
    if ($(params).prop('checked') == false) {
        return true;
    }
    else if (!value || value.length == 0) {
        return false;
    }
    return true;
});

$.validator.unobtrusive.adapters.add("requiredifchecked", ["otherpropertyname"], function (options) {
    options.rules["requiredifchecked"] = "#" + options.params.otherpropertyname;
    options.messages["requiredifchecked"] = options.message;
});


$(function () {
    $('#baseInfoForm').submit(function (e) {
        e.preventDefault()
        if (!$("#baseInfoForm").validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")

        $.post('/Warehouse/Admin/ChangeBaseInfo', $(this).serialize())
            .done(function (data) {
                showAlert(successAlert)
            })
            .fail(function () {
                showAlert(failAlert)
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")
            })
    })

    $('#contentForm').submit(function (e) {
        e.preventDefault()
        if (!$("#baseInfoForm").validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")

        $.post('/Warehouse/Admin/ChangeContentValues', $(this).serialize())
            .done(function (data) {
                showAlert(successAlert)
            })
            .fail(function () {
                showAlert(failAlert)
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")
            })
    })

    $('#photoInput').change(function (e) {
        e.stopImmediatePropagation();
        const [file] = this.files

        if (file) {
            const src = URL.createObjectURL(file)
            $('#newPhoto').attr("src", src)
            $('#newPhoto').show()
        } else {
            $('#newPhoto').hide()
        }
    })

    $('#traditionalCheck').change(function () {
        const checked = $(this).prop('checked')
        if (checked)
            $('#traditional-transfer-data').show('fast')
        else
            $('#traditional-transfer-data').hide('fast')
    })

    $('#dotPayCheck').change(function () {
        const checked = $(this).prop('checked')
        if (checked)
            $('#dotPay-data').show('fast')
        else
            $('#dotPay-data').hide('fast')
    })

    $('#personalCheck').change(function () {
        const checked = $(this).prop('checked')
        if (checked)
            $('#personal-pickup-data').show('fast')
        else
            $('#personal-pickup-data').hide('fast')
    })

    $('#inPostCheck').change(function () {
        const checked = $(this).prop('checked')
        if (checked)
            $('#inpost-data').show('fast')
        else
            $('#inpost-data').hide('fast')
    })
})

const failAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Wystąpił niespodziewany błąd. Przepraszamy</h5>
</div>`

const successAlert = `<div class="alert alert-success alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Zapisano pomyślnie</h5>
</div>`

const showAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}