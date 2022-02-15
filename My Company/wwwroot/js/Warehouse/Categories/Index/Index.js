/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
let searchValue = ""
let sort = null

$(function () {

    $('#searchBtn').click(function () {
        searchValue = $("#searchValue").val()
        currentPage = 1
        loadData()
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

    $('#deleteCategoryBtn').click(function () {
        const categoryId = $("#deleteCategoryModal input[name=categoryId]").val()
        deleteCategory(categoryId)
    })

    registerBtns()

})

const loadData = function () {
    $('.spinner').removeClass("spinnerHidden")
    $("#table").html("")

    $.get('/Warehouse/Categories/GetList',
        {
            searchString: searchValue,
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

const alertTemplate = `<div class="alert alert-success alert-dismissible fade show" role="alert">
    <h4>Kategoria usunięta pomyślnie!</h4>
</div>`

const deleteCategory = function (categoryId) {
    $('.spinner').removeClass("spinnerHidden")

    $.ajax({
        url: '/Warehouse/Categories/Delete/' + categoryId,
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

    $('.openRemoveCategoryModal').click(function (e) {
        const selectedCategory = $(this).data("id")
        $("#deleteCategoryModal input[name=categoryId]").val(selectedCategory)
    })
}