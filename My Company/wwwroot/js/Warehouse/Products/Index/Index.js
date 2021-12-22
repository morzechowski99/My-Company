let searchValue = ""
let sort = null
let filtersVisible = false
let states = []
let eans = []
let suppliers = []
let statuses = []
let categories = []

$(function () {

    if (bootstrapDetectBreakpoint().index < 2)
        $('#filters').hide()

    $(window).resize(function () {
        if (bootstrapDetectBreakpoint().index < 2) {
            if (!filtersVisible)
                $('#filters').hide()
        }
        else
            $('#filters').show()
    })

    $('#showHideFiltersBtn').click(function () {
        filtersVisible = !filtersVisible
        $('#filters').toggle('slow')
        $(this).text(filtersVisible ? "Ukryj filtry" : "Pokaż filtry")
    })

    $('#searchBtn').click(function () {
        searchValue = $("#searchValue").val()
        currentPage = 1
        loadData()
    })

    $("#stateSelect").selectize({
        plugins: {
            "remove_button": {},
            "tag_limit": { tagLimit: 2 }
        },
        valueField: "id",
        labelField: "name",
        searchField: "name",
        options: [
            { id: 2, name: "Zapas" },
            { id: 1, name: "Niewielka nadwyżka" },
            { id: 0, name: "Poniżej zapotrzebowania" }
        ],
        placeholder: "Stan",
        create: false,
        onChange: function (value) {
            states = value
        },
        onBlur: blurFunction
    })

    let eanSelect = $("#eanSelect").selectize({
        plugins: {
            "remove_button": {},
            "tag_limit": { tagLimit: 1 }
        },
        valueField: "code",
        labelField: "code",
        searchField: "code",
        placeholder: "Kod EAN",
        load: function (query, callback) {
            if (eanSelect.items.length == 0)
                eanSelect.clearOptions()
            if (query.length < 3) return callback();
            $.ajax({
                url: "/Warehouse/Products/GetEANs",
                type: "GET",
                data: {
                    prefix: query,
                },
                error: function () {
                    callback();
                },
                success: function (res) {
                    callback(res);
                },
            });
        },
        create: false,
        onChange: function (value) {
            eans = value
        },
        onBlur: blurFunction
    })
    eanSelect = eanSelect[0].selectize

    let supplierSelect = $("#supplierSelect").selectize({
        plugins: {
            "remove_button": {},
            "tag_limit": { tagLimit: 1 }
        },
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
            suppliers = value
        },
        onBlur: blurFunction
    })
    supplierSelect = supplierSelect[0].selectize

    $("#statusSelect").selectize({
        plugins: {
            "remove_button": {},
            "tag_limit": { tagLimit: 1 }
        },
        valueField: "id",
        labelField: "name",
        searchField: "name",
        options: [
            { id: 2, name: "Zarchiwizowany" },
            { id: 1, name: "Tymczasowo wyłączony" },
            { id: 0, name: "Aktywny" }
        ],
        placeholder: "Status",
        create: false,
        onChange: function (value) {
            statuses = value
        },
        onBlur: blurFunction
    })

    $("#categorySelect").selectize({
        plugins: {
            "remove_button": {},
            "tag_limit": { tagLimit: 1 }
        },
        placeholder: "Kategoria",
        create: false,
        onChange: function (value) {
            categories = value
        },
        onBlur: blurFunction
    })

    $('#sortSelect').change(function () {
        sort = $(this).val()
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

const loadData = function () {
    $('.spinner').removeClass("spinnerHidden")
    $("#table").html("")

    $.post('/Warehouse/Products/GetList',
        {
            searchString: searchValue,
            statuses: statuses,
            states: states,
            eans: eans,
            suppliers: suppliers,
            categories: categories,
            pageSize: pageSize,
            page: currentPage,
            sortOrder: sort
        })
        .done(function (data) {
            $('.spinner').addClass("spinnerHidden")
            $("#table").html(data)
            registerBtns()

        })
}

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

const blurFunction = function () {
    currentPage = 1
    loadData()
}