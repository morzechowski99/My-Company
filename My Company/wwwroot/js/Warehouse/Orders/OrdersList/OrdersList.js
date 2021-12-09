let orderId = null
let sort = null
let filtersVisible = false
let statuses = []
let dateFrom = null
let dateTo = null

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

    let orderNumberSelect = $("#orderNumberSelect").selectize({
        valueField: "id",
        labelField: "id",
        searchField: "id",
        placeholder: "Numer zamówienia",
        create: false,
        load: function (query, callback) {
            if (orderNumberSelect.items.length == 0)
                orderNumberSelect.clearOptions()
            if (query.length < 4) return callback();
            $.ajax({
                url: "/Warehouse/Orders/GetNumbers",
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
        create: false,
        onChange: function (value) {
            orderId = value
        },
        onBlur: blurFunction
    })

    orderNumberSelect = orderNumberSelect[0].selectize

    $('#dateFrom').blur(function () {
        dateFrom = $(this).val()
        currentPage = 1
        loadData()
    })

    $('#dateTo').blur(function () {
        dateTo = $(this).val()
        currentPage = 1
        loadData()
    })

    $("#statusSelect").selectize({
        plugins: {
            "remove_button": {},
            "tag_limit": { tagLimit: 1 }
        },
        valueField: "id",
        labelField: "name",
        searchField: "name",
        options: [
            { id: 0, name: "Nowe" },
            { id: 1, name: "Opłacone" },
            { id: 2, name: "Skompletowane" },
            { id: 3, name: "Wysłane" },
            { id: 4, name: "Do odbioru" }
        ],
        placeholder: "Status",
        create: false,
        onChange: function (value) {
            statuses = value
        },
        onBlur: blurFunction
    })

    $('#sortSelect').change(function () {
        sort = $(this).val()
        currentPage = 1
        loadData()
    })

    registerBtns()

})

const loadData = function () {
    $('.spinner').removeClass("spinnerHidden")
    $("#table").html("")

    $.post('/Warehouse/Orders/GetList',
        {
            orderId: orderId,
            statuses: statuses,
            pageSize: pageSize,
            page: currentPage,
            sortOrder: sort,
            dateFrom: dateFrom,
            dateTo: dateTo
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