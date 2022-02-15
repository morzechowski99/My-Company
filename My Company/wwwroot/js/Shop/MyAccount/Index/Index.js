/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
$(function () {

    registerChangePasswordForm()
    registerChangePersonalDataForm()

    $(".deleteAddress").click(function () {
        const id = $(this).data("id")

        $('.spinner').removeClass("spinnerHidden")
        $.ajax({
            method: "DELETE",
            url: '/MyAccount/DeleteAddress/' + id,
            success: function () {
                $('.addressModifyContainer').filter(`[data-id="${id}"]`).remove();
              
            },
            error: function () {
                showAlert(failAlert)
            }
        }).always(function () {
            $('.spinner').addClass("spinnerHidden")
        })
    })

})

const failAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Wystąpił niespodziewany błąd. Przepraszamy</h5>
</div>`

const showAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}

const registerChangePasswordForm = function () {
    $('#changePasswordForm').submit(function (e) {
        e.preventDefault()
        if (!$("#changePasswordForm").validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")

        $.post('/MyAccount/ChangePassword', $(this).serialize())
            .done(function (data) {
                $('#changePasswordDiv').html(data)
                //Remove current form validation information
                $("#changePasswordForm")
                    .removeData("validator")
                    .removeData("unobtrusiveValidation");

                //Parse the form again
                $.validator
                    .unobtrusive
                    .parse("#changePasswordForm");
                registerChangePasswordForm()
            })
            .fail(function () {
                showAlert(failAlert)
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")
            })
    })
}

const registerChangePersonalDataForm = function () {
    $('#changePersonalDataForm').submit(function (e) {
        e.preventDefault()
        if (!$("#changePersonalDataForm").validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")

        $.post('/MyAccount/ChangePersonalData', $(this).serialize())
            .done(function (data) {
                $('#changePersonalDataDiv').html(data)
                //Remove current form validation information
                $("#changePasswordForm")
                    .removeData("validator")
                    .removeData("unobtrusiveValidation");

                //Parse the form again
                $.validator
                    .unobtrusive
                    .parse("#changePersonalDataForm");
                registerChangePersonalDataForm()
            })
            .fail(function () {
                showAlert(failAlert)
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")
            })
    })
}