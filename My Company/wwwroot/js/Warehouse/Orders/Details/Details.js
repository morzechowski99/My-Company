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





