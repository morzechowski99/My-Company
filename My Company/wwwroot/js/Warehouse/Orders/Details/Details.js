$(function () {
    $("#getInvoice").click(function (e) {
        e.preventDefault()
        var id = $(this).data("id")
        $.get({
            url: '/Warehouse/Orders/GetInvoicePdf/' + id,
            xhrFields: {
                responseType: 'blob'
            },
        })
            .done(function (blob) {
                openFile(blob)
            })
    })

    $("#getWz").click(function (e) {
        e.preventDefault()
        var id = $(this).data("id")
        $.get({
            url: '/Warehouse/Orders/GetWZPdf/' + id,
            xhrFields: {
                responseType: 'blob'
            },
        })
            .done(function (blob) {
                openFile(blob)
            })

    })

    $("#changeStatusForm").submit(function (e) {
        e.preventDefault()
        $('.modal').modal('hide')
        if (!$(this).validate().form()) return;
        $('.spinner').removeClass("spinnerHidden")
        $.ajax({
            data: $(this).serialize(),
            url: "/Warehouse/Orders/ChangePaymentStatus",
            type: "PUT"
        })
            .done(function (data) {
                showAlert(successAlert)
                $('#statusSpan').text(data)
                const val = $("#paidSelect").val();
                $("#paidSpan").text(val == "true" ? "TAK" : "NIE")
            })
            .always(function () {
                $('.spinner').addClass("spinnerHidden")
            })
            .fail(function () {
                showAlert(errorAlert)
            })
    })
})

const openFile = function (blob) {
    let binaryData = []
    binaryData.push(blob)
    const data = window.URL.createObjectURL(
        blob
    )
    let link = document.createElement('a')
    link.href = data
    link.target = '_blank'
    link.click()
    setTimeout(function () {
        // For Firefox it is necessary to delay revoking the ObjectURL
        window.URL.revokeObjectURL(data)
    }, 100)
}

const errorAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h6 class="alert-content">Wystąpił niespodziewany błąd. Przepraszamy</h5>
</div>`

const showAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}

const successAlert = `<div class="alert alert-info alert-dismissible fade show" role="alert">
    Zapisano pomyślnie
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>`


