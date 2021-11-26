var formatter = new Intl.NumberFormat('pl', {
    style: 'currency',
    currency: 'PLN',
});

$(function () {
    $('input[name="DeliveryType"]').change(function () {
        const val = $('input[name="DeliveryType"]:checked').val()
        $('#shippingValue').text(formatter.format($(this).data('price')))
        calculateTotal()
        switch (val) {
            case 'personalPickup':
                $('#warehuseAddress').show()
                $('#choseParcelLocker').hide()
                break;
            case 'PaczkomatyInPost':
                $('#warehuseAddress').hide()
                $('#choseParcelLocker').show()
                break;
            default:
                $('#warehuseAddress').hide()
                $('#choseParcelLocker').hide()
                break
        }
    })

    $('input[name="PaymentMethod"]').change(function () {
        $('#paymentValue').text(formatter.format($(this).data('price')))
        calculateTotal()
    })

    window.easyPackAsyncInit = function () {
        easyPack.init({
            defaultLocale: 'pl',
            mapType: 'osm',
            searchType: 'osm',
            points: {
                types: ['parcel_locker']
            },
            map: {
                initialTypes: ['parcel_locker']
            }
        });
    };

    const openModal = function () {
        easyPack.modalMap(function (point, modal) {
            modal.closeModal();
            $('#packLocekerNameInput').val(point.name)
            $('#pickingMachineAddressName').text(point.name)
            $('#pickingMachineAddressLine1').text(point.address.line1)
            $('#pickingMachineAddressLine2').text(point.address.line2)
            $('#packLockerValidate').text('')
        }, { width: '100%', height: '100%' });
    }

    $('#choosePickingMachine').click(function () { openModal() })

    $('.addressContainer').click(function (e) {
        $('.addressContainer').addClass('border-secondary')
        $('.addressContainer').removeClass('border-success')
        $(this).addClass('border-success')
        $(this).removeClass('border-secondary')

        $('input[name="ShippingAddress.FirstName"]').val($(this).find('.addressFirstName').val())
        $('input[name="ShippingAddress.LastName"]').val($(this).find('.addressLastName').val())
        $('input[name="ShippingAddress.Street"]').val($(this).find('.addressStreet').val())
        $('input[name="ShippingAddress.ZipCode"]').val($(this).find('.addressZipCode').val())
        $('input[name="ShippingAddress.City"]').val($(this).find('.addressCity').val())
        $('input[name="ShippingAddress.PhoneNumber"]').val($(this).find('.addressPhoneNumber').val())
        $('input[name="AddressId"]').val($(this).find('.addressId').val())
    })

    $("form").submit(function (e) {
        var val = $("button[type=submit][clicked=true]").val();
        $("button[type=submit][clicked=true]").removeAttr('clicked');
        if (val === 'summaryBtn') {
            e.preventDefault()
            if (!$(this).validate().form()) return;
            if ($('input[name="DeliveryType"]:checked').val() === 'PaczkomatyInPost' && !$('input[name="PackLockerName"]').val()) {
                $('#packLockerValidate').text("Wybierz paczkomat")
                return
            }
            $('input').prop("disabled", false)
            const data = $(this).serialize()
            $('input').prop("disabled", true)
            $('.spinner').removeClass('spinnerHidden')
            $.post({
                url: '/Order/GetSummary?',
                data: data,
                success: function (data) {
                    $('#summary').html(data)
                    $('#formContainer').hide()
                    $('#header').text("Podsumowanie zamówienia")
                    $('#backBtn').click(function () {
                        $('#summary').html('')
                        $('#formContainer').show()
                        $('#header').text("Nowe zamówienie")
                    })
                    $('#submitBtn').click(function () {
                        $('form').submit()
                        
                    })
                },
            }).always(function () {
                $('input').prop("disabled", false)
                $('.spinner').addClass('spinnerHidden')
            }).fail(function () {
                showAlert(failAlert)
            })

        }
    });

    $("form button[type=submit]").click(function () {
        $("button[type=submit]", $(this).parents("form")).removeAttr("clicked");
        $(this).attr("clicked", "true");
    })

    
})

const calculateTotal = function () {
    const cartValue = convertCurrencyToFload($('#cartValue').text())
    const shippingValue = convertCurrencyToFload($('#shippingValue').text())
    const paymentValue = convertCurrencyToFload($('#paymentValue').text())
    const total = toMoney(cartValue + shippingValue + paymentValue)
    $('#totalValue').text(formatter.format(total))
}

const convertCurrencyToFload = function (currency) {
    return Number.parseFloat(currency.substring(0, currency.length - 2).replace(/\s/g, '').replace(',', '.'))
}

const toMoney = function (value) {
    return (Math.round(value * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)
}

const failAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Wystąpił niespodziewany błąd. Przepraszamy</h5>
</div>`

const showAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}