/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
const filters = {
    supplierId: null,
    dateFrom: null,
    dateTo: null,
    sortOrder: 0,
    PZNumber: null
}

$(function () {
    let filtersVisible = true

    let supplierSelect = $("#supplierSelect").selectize({
        valueField: "id",
        labelField: "description",
        searchField: "description",
        options: [],
        placeholder: "Dostawca",
        create: false,
        load: function (query, callback) {
            if (supplierSelect.items.length == 0)
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
        onChange: function (value) {
            filters.supplierId = value
        },
        onBlur: blurFunction
    })
    supplierSelect = supplierSelect[0].selectize

    $('#showHideFilters').click(function () {
        filtersVisible = !filtersVisible
        if (filtersVisible) {
            $('.filter').show('fast')
            $(this).text("Ukryj filtry")
        } else {
            $('.filter').hide('fast')
            $(this).text("Pokaż filtry")
        }

    })

    $('#sortSelect').change(function () {
        filters.sortOrder = $(this).val()
        currentPage = 1
        loadData()
    })

    $('#dateFrom').change(function () {
        filters.dateFrom = $(this).val()
        currentPage = 1
        loadData()
    })

    $('#dateTo').change(function () {
        filters.dateTo = $(this).val()
        currentPage = 1
        loadData()
    })

    $('#PZNumber').change(function () {
        filters.PZNumber = $(this).val()
        currentPage = 1
        loadData()
    })

    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            searchValue = $("#searchValue").val()
            loadData()
        }
    });

    registerBtns()

})

const registerBtns = function () {

    $('.changePageSize').click(function (e) {
        e.preventDefault()
        pageSize = e.target.value
        currentPage = 1
        loadData()
    })

    $('.prevPageBtn').click(function (e) {
        e.preventDefault()
        currentPage--
        loadData()
    })

    $('#changePageBtn1').click(function (e) {
        e.preventDefault()
        currentPage = $("#pageValue1").val()
        if (currentPage > totalPages)
            currentPage = totalPages
        loadData()
    })

    $('#changePageBtn2').click(function (e) {
        e.preventDefault()
        currentPage = $("#pageValue2").val()
        if (currentPage > totalPages)
            currentPage = totalPages
        loadData()
    })

    $('.nextPageBtn').click(function (e) {
        e.preventDefault()
        currentPage++
        loadData()
    })

}

const loadData = function () {
    $('.spinner').removeClass("spinnerHidden")
    $("#table").html("")

    $.get('/Warehouse/Deliveries/GetList',
        {
            pageSize: pageSize,
            page: currentPage,
            ...filters
        })
        .done(function (data) {
            $('.spinner').addClass("spinnerHidden")
            $("#table").html(data)
            registerBtns()

        })
}

const alertTemplate = `<div class="alert alert-success alert-dismissible fade show" role="alert">
    <h4>Usunięto pomyślnie</h4>
</div>`

const deleteSupplier = function (id) {
    $('.spinner').removeClass("spinnerHidden")

    $.ajax({
        url: '/Warehouse/Suppliers/Delete/' + id,
        type: 'DELETE',
        success: function (data) {
            $('body').append(alertTemplate)
            setTimeout(function () {
                $('.alert-success').alert('close')
            }, 5000)
            loadData()
        },
        error: function (data) {
            showAlert()
            $('.spinner').addClass("spinnerHidden")
        }
    })
}
const blurFunction = function () {
    currentPage = 1
    loadData()
}
