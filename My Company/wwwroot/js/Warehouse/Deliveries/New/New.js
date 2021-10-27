$.validator.setDefaults({ ignore: null })
let supplierSelect
let productSelect
class Order {
    constructor() {
        this.products = []
        this.lastSelectedProduct = null
        this.lastSectorId = null
    }

    addProduct = (product) => {
        this.products.push(product)
    }

    deleteProduct = (idx) => {
        this.products = this.products.filter((p, i) => i != idx)
    }

    canAddProduct = () => { return this.lastSelectedProduct && this.lastSectorId }

    supplierDisabled = () => { return this.products.length > 0 }
}

const order = new Order()
let sectors = []

$(function () {
    supplierSelect = $("#supplierSelect").selectize({
        valueField: "id",
        labelField: "description",
        searchField: "description",
        options: [],
        placeholder: "Zacznij pisać, aby wybrać",
        create: false,
        onChange: function (id) {
            if (id)
                $("#addingProductContainer").show("fast")
            else
                $("#addingProductContainer").hide("fast")
        },
        load: function (query, callback) {
            supplierSelect.clearOptions()
            if (query.length < 2) return callback();
            $.ajax({
                url: "/Warehouse/Products/GetSuppliersToSelect",
                type: "GET",
                data: {
                    query: query,
                },
                error: function () {
                    callback();
                },
                success: function (res) {
                    callback(res);
                },
            });
        },
    })
    supplierSelect = supplierSelect[0].selectize

    $(window).resize(function () {
        const currBreakPoint = bootstrapDetectBreakpoint()
        if (currBreakPoint.index < 2)
            $("#sectorSelectContainer").removeClass("border-left")
        else {
            $("#sectorSelectContainer").addClass("border-left")
        }
    });

    $("#eanInput").on("input", function (e) {
        const ean = e.target.value
        productSelect.clear()
        if (ean.length < 13) {
            order.lastSelectedProduct = null;
            $("#addProductsBtn").prop('disabled', !order.canAddProduct());
            $('#selectedProduct').html('')
            return;
        }
        $(this).prop('disabled', true);

        fetchProductData(ean)
    })

    $("#sectorEanInput").on("input", function (e) {
        $("#rowSelect").val(null)
        $("#sectorSelect").empty()
        //$("#rowSelect").change()
        const ean = e.target.value
        if (ean.length < 13) {
            order.lastSectorId = null
            $("#addProductsBtn").prop('disabled', !order.canAddProduct());
            return;
        }
        $(this).prop('disabled', true)
        validateSectorEan(ean)
    })

    productSelect = $("#productSelect").selectize({
        valueField: "ean",
        labelField: "description",
        searchField: "description",
        options: [],
        placeholder: "Zacznij pisać, aby wybrać",
        create: false,
        onChange: function (ean) {
            $("#eanInput").val('')
            if (!ean) {
                order.lastSelectedProduct = null;
                $('#selectedProduct').html('')
                $("#addProductsBtn").prop('disabled', !order.canAddProduct());
                return;
            }
            fetchProductData(ean)
        },
        load: function (query, callback) {
            productSelect.clearOptions()
            if (query.length < 2) return callback();
            $.ajax({
                url: "/Warehouse/Deliveries/GetProductsByQuery",
                type: "GET",
                data: {
                    query: query,
                },
                error: function () {
                    callback();
                },
                success: function (res) {
                    callback(res);
                },
            });
        },
    })
    productSelect = productSelect[0].selectize

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
            order.lastSectorId = firstsector.id
        } else {
            order.lastSectorId = null
        }
        $("#addProductsBtn").prop('disabled', !order.canAddProduct());
        rowSectors.forEach(s => $sectorSelect.append(`<option value="${s.id}">${s.order}</option>`))
    })

    $("#sectorSelect").change(function (e) {
        order.lastSectorId = e.target.value
        $("#addProductsBtn").prop('disabled', !order.canAddProduct());
    })

    $("#addProductsBtn").click(function (e) {
        $(this).prop('disabled', true)
        if (!order.canAddProduct())
            return
        order.addProduct({ product: order.lastSelectedProduct, sector: order.lastSectorId, count: $("#count").val() })
        printProducts(order.products)
        reset()
        supplierSelect.disable()
        $('#submit').show()
    })

    $("#count").on("change", function (e) {
        if (!e.target.value || e.target.value < 1)
            $(this).val(1)
    })

    $('#submit').click(function () {
        $(".spinner").removeClass('spinnerHidden')
        const data = {
            supplierId: supplierSelect.getValue(),
            items: order.products.map(p => (
                {
                    productId: p.product.id,
                    sectorId: p.sector,
                    count: p.count
                }))
        }
        $.post('/Warehouse/Deliveries/New', data)
            .always(function () {
                $(".spinner").addClass('spinnerHidden')
            })
            .fail(function () {
                showErrorAlert("Wystąpił błąd")
            })
            .done(function (data) {
                location.href = `/Warehouse/Deliveries/Details/${data}`
            })
    })
})

const fetchProductData = function (ean) {
    const supplierId = $("#supplierSelect").val()
    $.ajax({
        url: "/Warehouse/Deliveries/GetProductByEan",
        type: "GET",
        data: {
            ean: ean,
            supplierId: supplierId
        },
        error: function (data) {
            showErrorAlert(errorMessages[data.responseText])
            productSelect.clear()
            resetEanInput()
            order.lastSelectedProduct = null
        },
        success: function (data) {
            if (data.error) {
                order.lastSelectedProduct = null
                showErrorAlert(errorMessages[data.error])
                resetEanInput()
                productSelect.clear()
                return
            }
            $("#eanInput").prop('disabled', false)

            order.lastSelectedProduct = {
                id: data.product.id,
                name: data.product.name,
                photo: data.product.photo
            }
            showProduct(data.product)
        },
    })
        .always(function () {
            $("#addProductsBtn").prop('disabled', !order.canAddProduct());
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
            order.lastSectorId = null
        },
        success: function (data) {
            if (data.error) {
                showErrorAlert(errorMessages[data.error])
                resetSectorEanInput()
                return
            }
            $("#sectorEanInput").prop('disabled', false)
            order.lastSectorId = data
        },
    }).always(function () {
        $("#addProductsBtn").prop('disabled', !order.canAddProduct());
    });;
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
    "invalid supplier for this product": "Produkt nie jest dostarczany przez wybranego dostawcę",
    "product is archived": "Wybrany produkt został zarchizizowany",
    "sector not exists": "Sektor nie istnieje",
    "internal error": "Wystąpił błąd"
}

const resetEanInput = function () {
    $("#eanInput").prop('disabled', false)
    $("#eanInput").val('')
}

const resetSectorEanInput = function () {
    $("#sectorEanInput").prop('disabled', false)
    $("#sectorEanInput").val('')
}

const showProduct = function (product) {
    $('#selectedProduct').html(`<figure class="figure">
        <img src="${product.photo}" class="figure-img img-fluid rounded" alt="${product.name}">
            <figcaption class="figure-caption">${product.name}</figcaption>
        </figure>`)

}

const printProducts = function (products) {
    $("#items").html('')
    const container = $('<div class="row"></div>')
    products.forEach((product, idx) => {
        const p = product.product
        const sector = sectors.find(s => s.id == product.sector)
        const rowOption = $("#rowSelect option").toArray().find(option => $(option).val() == sector.rowId)
        const rowName = $(rowOption).text()
        const count = product.count
        container.append(
            `<div class="col-6 col-md-3 my-2">
                <div class="card border-success">
                    <h6 class="card-header">${p.name}</h6 >
                    <img class="card-img-top" src="${p.photo}" alt="${p.name}">
                    <div class="card-body mb-0">
                        <b>Ilość</b>
                        <p class="mb-0">${count}</p>
                        <b>Sektor</b>
                        <p class="mb-0">${rowName}${sector.order}</p>
                    </div>
                    <div class="card-footer d-flex justify-content-center">
                        <button class="btn btn-link text-danger deleteProduct" type="button" data-id="${idx}">Usuń</button>
                    </div>
            </div>
        </div >`)

    })
    $("#items").append(container)
    $('.deleteProduct').click(function () {
        console.log('here')
        const id = $(this).data('id')
        order.deleteProduct(id)
        printProducts(order.products)
        if (order.products.length == 0) {
            supplierSelect.enable()
            $('#submit').hide()
        }
    })
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