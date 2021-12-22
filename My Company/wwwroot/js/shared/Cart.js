$(function () {
    $('#cart').popover({
        html: true,
        content: function () {
            return $('#cartContent').html()
        },
        container: 'body',
        boundary: 'viewport',
        arrow:false
    })

    $('body').on('click', function (e) {
        $('[data-toggle=popover]').each(function () {
            // hide any open popovers when the anywhere else in the body is clicked
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    });
    $('#cart').on('shown.bs.popover', function () {
        setRemoveBtn()
    })

})

const setRemoveBtn = function () {
    $('.removeItemFromCartBtn').click(function (e) {
        e.preventDefault()
        e.stopImmediatePropagation()
        const href = $(this).prop('href')
        $.ajax({
            method: 'DELETE',
            url: href,
            success: function (data) {
                $('#cart').popover('hide')
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
                $('#cart').popover('show')
            },
            error: function () {
                showwAlert(errorAlert)
            }
        })
    })
}

const errorAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Wystąpił niespodziewany błąd. Przepraszamy</h5>
</div>`

const showwAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}