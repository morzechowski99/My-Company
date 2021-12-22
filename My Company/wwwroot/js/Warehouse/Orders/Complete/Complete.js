let productId = undefined
let sectorId = undefined
let sectors = []

const canAddProduct = function () { return productId && sectorId }

$(function () {
    $(window).resize(function () {
        const currBreakPoint = bootstrapDetectBreakpoint()
        if (currBreakPoint.index < 2)
            $("#sectorSelectContainer").removeClass("border-left")
        else {
            $("#sectorSelectContainer").addClass("border-left")
        }
    });

    registerDeleteItemBtns()

    $("#eanInput").on("input", function (e) {
        const ean = e.target.value
        productSelect.clear()
        if (ean.length < 13) {
            productId = undefined
            $("#addProductsBtn").prop('disabled', !canAddProduct());
            return;
        }
        $(this).prop('disabled', true);

        validateProductEan(ean)
    })

    $("#sectorEanInput").on("input", function (e) {
        $("#rowSelect").val(null)
        $("#sectorSelect").empty()
        const ean = e.target.value
        if (ean.length < 13) {
            sectorId = undefined
            $("#addProductsBtn").prop('disabled', !canAddProduct());
            return;
        }
        $(this).prop('disabled', true)
        validateSectorEan(ean)
    })

    productSelect = $("#productSelect").selectize({
        valueField: "id",
        labelField: "description",
        searchField: "description",
        options: [],
        placeholder: "Zacznij pisać, aby wybrać",
        create: false,
        onChange: function (id) {
            $("#eanInput").val('')
            if (!id) {
                $("#addProductsBtn").prop('disabled', !canAddProduct());
                return;
            }           
            productId = id
            $("#addProductsBtn").prop('disabled', !canAddProduct());
        },
    })
    productSelect = productSelect[0].selectize

    getOrderProducts();

    $.ajax({
        url: "/Warehouse/Deliveries/GetSectors",
        type: "GET",
        error: function (data) {
            showErrorAlert("Błąd pobierania danych")
        },
        success: function (data) {
            sectors = data
        },
    });

    $("#rowSelect").change(function (e) {
        $('#sectorEanInput').val('')
        const id = e.target.value
        const $sectorSelect = $("#sectorSelect")
        $sectorSelect.empty()
        const rowSectors = sectors.filter(s => s.rowId == id).sort((s1, s2) => s1.order - s2.order)
        const firstsector = rowSectors[0]
        if (firstsector) {
            sectorId = firstsector.id
        } else {
            sectorId = null
        }
        $("#addProductsBtn").prop('disabled', !canAddProduct());
        rowSectors.forEach(s => $sectorSelect.append(`<option value="${s.id}">${s.order}</option>`))
    })

    $("#sectorSelect").change(function (e) {
        sectorId = e.target.value
        $("#addProductsBtn").prop('disabled', !canAddProduct());
    })

    $("#addProductsBtn").click(function (e) {
        $(this).prop('disabled', true)
        if (!canAddProduct()) {
            showErrorAlert("Coś poszło nie tak...")
            return
        }
        addItemToPicking()
    })

    $("#count").on("change", function (e) {
        if (!e.target.value || e.target.value < 1)
            $(this).val(1)
    })

})

const validateProductEan = function (ean) {
    const orderId = getRouteValueAt(3)
    $.ajax({
        url: "/Warehouse/Orders/ValidateProductEan",
        type: "GET",
        data: {
            ean: ean,
            orderId: orderId
        },
        error: function (data) {
            showErrorAlert(errorMessages[data.responseText])
            productSelect.clear()
            resetEanInput()
            productId = undefined;
        },
        success: function (data) {
            if (!data.isInOrder) {
                showErrorAlert(errorMessages[data.error])
                resetEanInput()
                productId = undefined;
                productSelect.clear()
                return
            }
            $("#eanInput").prop('disabled', false)

            productId = data.productId
        },
    })
        .always(function () {
            $("#addProductsBtn").prop('disabled', !canAddProduct());
        });
}

const validateSectorEan = function (ean) {
    $.ajax({
        url: "/Warehouse/Deliveries/ValidateSectorEan",
        type: "GET",
        data: {
            ean: ean,
        },
        error: function (data) {
            showErrorAlert(errorMessages[data.responseText])
            resetSectorEanInput()
            sectorId = undefined
        },
        success: function (data) {
            if (data.error) {
                showErrorAlert(errorMessages[data.error])
                resetSectorEanInput()
                return
            }
            $("#sectorEanInput").prop('disabled', false)
            sectorId = data
        },
    })
        .always(function () {
            $("#addProductsBtn").prop('disabled', !canAddProduct());
        });
}

const showErrorAlert = function (message) {
    const failAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h5 class="alert-content">${message}</h5>
</div>`
    $('body').append(failAlert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}

const errorMessages = {
    "invalid Code": "Produkt o takim kodzie nie istnieje",
    "sector not exists": "Sektor nie istnieje",
    "internal error": "Wystąpił błąd",
    "product does not exists in order": "Znaleziony produkt nie znajduje się w kopletowanym zamówieniu",
    "invalid orderId": "Zły numer zamówienia",
    "product in order not found": "Produkt nie znajduje się w kopletowanym zamówieniu",
    "too less products in sector": "W wybranym sektorze jest za mało sztuk tego produktu",
    "too many products picked":"Wzięto więcej sztuk niż potrzeba"
}

const resetEanInput = function () {
    $("#eanInput").prop('disabled', false)
    $("#eanInput").val('')
}

const resetsectoreaninput = function () {
    $("#sectoreaninput").prop('disabled', false)
    $("#sectoreaninput").val('')
}

const reset = function () {
    $("#eanInput").val('')
    $("#sectorEanInput").val('')
    productSelect.clear()
    $("#rowSelect").val(null)
    $("#rowSelect").change()
    $('#selectedProduct').html('')
    $("#count").val(1)
}

const getOrderProducts = function () {
    const orderId = getRouteValueAt(3)
    $.ajax({
        url: "/Warehouse/Orders/GetProductsInOrder",
        type: "GET",
        data: {
            orderId: orderId
        },
        error: function (data) {
            showErrorAlert(errorMessages[data.responseText])
        },
        success: function (data) {
            productSelect.addOption(data)
        },
    })
}

const addItemToPicking = function () {
    const orderId = getRouteValueAt(3)
    $(".spinner").removeClass('spinnerHidden')
    $.post('/Warehouse/Orders/AddProductToPicking', { orderId: orderId, productId: productId, sectorId: sectorId, count: $("#count").val() })
        .always(function () {
            $(".spinner").addClass('spinnerHidden')
            reset()
        })
        .fail(function () {
            showErrorAlert("Wystąpił błąd")
        })
        .done(function (data) {
            if (data.error) {
                showErrorAlert(errorMessages[data.error])
                return
            }
            $("#details").html(data)
            registerDeleteItemBtns()
        })
}

const registerDeleteItemBtns = function () {
    $(".deletePickedItem").click(function () {
        deleteItem($(this).data('id'))
    })

    $("#submit").click(function () {
        const orderId = getRouteValueAt(3)
        $(".spinner").removeClass('spinnerHidden')
        $.post('/Warehouse/Orders/CompletePicking', { orderId: orderId})
            .fail(function () {
                showErrorAlert("Wystąpił błąd")
                $(".spinner").addClass('spinnerHidden')
            })
            .done(function () {
                window.location.href = '/Warehouse/Orders'
            })
    })
}

const deleteItem = function (id) {
    $(".spinner").removeClass('spinnerHidden')
    $.ajax({
        url: "/Warehouse/Orders/DeletePickingItem",
        type: "PUT",
        data: {
            pickingItemId: id
        },
        error: function (data) {
            showErrorAlert("Wystąpił problem")
        },
        success: function (data) {
            $("#details").html(data)
            registerDeleteItemBtns()
        },
    }).always(function () {
        $(".spinner").addClass('spinnerHidden')
    })
}