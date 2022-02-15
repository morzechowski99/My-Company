/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
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

    registerMainPageForm()

    $('#contentForm').submit(function (e) {
        e.preventDefault()
        if (!$("#contentForm").validate().form()) return;
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

    $('#addressForm').submit(function (e) {
        e.preventDefault()
        if (!$("#addressForm").validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")

        $.post('/Warehouse/Admin/ChangeDocumentAddressData', $(this).serialize())
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

const successAlertDelete = `<div class="alert alert-success alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Usunięto pomyślnie</h5>
</div>`

const showAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}

const registerMainPageForm = function () {
    resetForm('#addMainPageItemForm')
    $('#addMainPageItemForm').submit(function (e) {
        e.preventDefault()

        if (!$("#addMainPageItemForm").validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")
        const data = new FormData(this);

        $.ajax({
            type: 'POST',
            url: '/Warehouse/Admin/AddMainPageItem',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
        })
            .done(function (data) {
                showAlert(successAlert)
                $('.modal').modal('hide')
                $('#mainPage').html(data)
                registerMainPageForm()

            })
            .fail(function () {
                showAlert(failAlert)
                $('.modal').modal('hide')
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")

            })
    })

    $('.editMainPageItemPhotoForm').submit(function (e) {
        e.preventDefault()
        
        if (!$(this).validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")
        const data = new FormData(this);

        $.ajax({
            type: 'PUT',
            url: '/Warehouse/Admin/EditMainPageItemPhoto',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
        })
            .done(function (data) {
                showAlert(successAlert)
                $('.modal').modal('hide')
                $('#mainPage').html(data)
                registerMainPageForm()

            })
            .fail(function () {
                showAlert(failAlert)
                $('.modal').modal('hide')
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")

            })
    })

    $('.editMainPageItemForm').submit(function (e) {
        e.preventDefault()
  
        if (!$(this).validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")
        const data = new FormData(this);

        $.ajax({
            type: 'PUT',
            url: '/Warehouse/Admin/EditMainPageItem',
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
        })
            .done(function (data) {
                showAlert(successAlert)
                $('.modal').modal('hide')
                $('#mainPage').html(data)
                registerMainPageForm()
            })
            .fail(function () {
                showAlert(failAlert)
                $('.modal').modal('hide')
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")

            })
    })

    $('.deleteMainPageItem').click(function (e) {
        e.stopPropagation()

        $('.spinner').removeClass("spinnerHidden")

        $.ajax({
            type: 'DELETE',
            url: '/Warehouse/Admin/DeleteMainPageItem?order=' + $(this).data('order'),
        })
            .done(function (data) {
                showAlert(successAlert)
                $('#mainPage').html(data)
                registerMainPageForm()

            })
            .fail(function () {
                showAlert(failAlert)
                $('.modal').modal('hide')
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")

            })
    })

    $('.moveDown').click(function (e) {
        e.stopPropagation()
        $('.spinner').removeClass("spinnerHidden")
        moveItemAjax($(this).data('order'),1)
    })

    $('.moveUp').click(function (e) {
        e.stopPropagation()
        $('.spinner').removeClass("spinnerHidden")
        moveItemAjax($(this).data('order'),-1)
    })
}

const resetForm = function (form) {
    $(form).find('input').val('')
    $(form).find('textarea').text('')
    $(form).find('select').val('-1')
    //Remove current form validation information
    $(form)
        .removeData("validator")
        .removeData("unobtrusiveValidation");

    //Parse the form again
    $.validator
        .unobtrusive
        .parse(form);
}

const moveItemAjax = function (order, direction) {
    $.ajax({
        type: 'PUT',
        url: '/Warehouse/Admin/MoveMainPageItem?order=' + order + '&direction=' + direction,
    })
        .done(function (data) {
            showAlert(successAlert)
            $('#mainPage').html(data)
            registerMainPageForm()
        })
        .fail(function () {
            showAlert(failAlert)
        })
        .always(function () {
            $('.spinner').addClass("spinnerHidden")
        })
}