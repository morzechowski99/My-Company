$(function () {
    $('#cartForm').submit(function (e) {
        e.preventDefault()
        $('.spinner').removeClass("spinnerHidden")
        const count = $('#itemsCount').val();
        if (isNaN(count) || count < 1) {
            $('#cartValidate').text('Niepoprawna liczba')
            return
        }

        $.post('/Cart/AddToCart',
            $(this).serialize())
            .fail(function () {
                showAlert(failAlert)
            })
            .done(function (data) {
                $('#cartContent').html($(data).children('#cartContent').html())
                $('#cartIcon').html($(data).children('#cartIcon').html())
                $('#cart').popover({
                    html: true,
                    content: function () {
                        return $('#cartContent').html()
                    },
                    container: 'body',
                    boundary: 'viewport',
                    arrow: false
                })
                $('#cart').on('shown.bs.popover', function () {
                    setRemoveBtn()
                })
                showAlert(successAlert)
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")
            })
    })

    $('#itemsCount').on('input', function () {
        $('#cartValidate').text('')
    })
})

const successAlert = `<div class="alert alert-info alert-dismissible fade show" role="alert">
   Produkt pomyślnie dodany do koszyka <a class="alert-link" href="/Cart/Cart">Przejdź do koszyka</a>
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>`

const failAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Wystąpił niespodziewany błąd. Przepraszamy</h5>
</div>`

const showAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}