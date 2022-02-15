/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
let searchValue = ""
let sort = null
let nip = ""
let email = ""

$(function () {
    let filtersVisible = true
    let nipFilter = $("#nipFilter").selectize({
        valueField: "nip",
        labelField: "nip",
        searchField: "nip",
        options: [],
        create: false,
        load: function (query, callback) {
            nipFilter.clearOptions()
            if (query.length < 3) return callback();
            $.ajax({
                url: "/Warehouse/Suppliers/GetNIPs",
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
        onChange: function (value) {
            nip = value
            currentPage = 1
            loadData()
        }
    })
    nipFilter = nipFilter[0].selectize

    let emailFilter = $("#emailFilter").selectize({
        valueField: "email",
        labelField: "email",
        searchField: "email",
        options: [],
        create: false,
        load: function (query, callback) {
            emailFilter.clearOptions()
            if (query.length < 3) return callback();
            $.ajax({
                url: "/Warehouse/Suppliers/GetEmails",
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
        onChange: function (value) {
            email = value
            currentPage = 1
            loadData()
        }
    })
    emailFilter = emailFilter[0].selectize

    $('#showHideFilters').click(function () {
        filtersVisible = !filtersVisible
        if (filtersVisible) {
            $('.filter').show('fast')
            $(this).text("Ukryj filtry")
            $('.filtersContainer').addClass('justify-content-between')
            $('.filtersContainer').removeClass('justify-content-end')
        } else {
            $('.filter').hide('fast')
            $(this).text("Pokaż filtry")
            $('.filtersContainer').addClass('justify-content-end')
            $('.filtersContainer').removeClass('justify-content-between')
        }
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

    $('#searchBtn').click(function () {
        searchValue = $("#searchValue").val()
        currentPage = 1
        loadData()
    })

    $('#deleteSupplierBtn').click(function () {
        const id = $("#deleteSupplierModal input[name=supplierId]").val()
        deleteSupplier(id)
    })

    registerBtns()

})

const registerBtns = function () {

    $('.openDeleteSupplierDialog').click(function (e) {
        const id = $(this).data("id")
        $("#deleteSupplierModal input[name=supplierId]").val(id)
    })

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

    $.get('/Warehouse/Suppliers/GetList',
        {
            searchString: searchValue,
            pageSize: pageSize,
            page: currentPage,
            sortOrder: sort,
            email: email,
            nip: nip
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