/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
let searchValue = ""
let role = null
let sort = null
let selectedUser = null

$(function () {

    $('#searchBtn').click(function () {
        searchValue = $("#searchValue").val()
        currentPage = 1
        loadData()
    })

    $('#roleSelect').change(function () {
        role = $(this).val()
        currentPage = 1
        loadData()
    })

    $('#sortSelect').change(function () {
        sort = $(this).val()
        currentPage = 1
        loadData()
    })

    $('#lockEmployeeBtn').click(function (e) {
        lockUser(selectedUser)
    })

    $('#unlockEmployeeBtn').click(function (e) {
        unlockUser(selectedUser)
    })

    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            searchValue = $("#searchValue").val()
            currentPage = 1
            loadData()
        }
    });

    registerBtns()
})

const loadData = function () {
    $('.spinner').removeClass("spinnerHidden")
    $("#table").html("")

    $.get('/Warehouse/Employee/GetList',
        {
            searchString: searchValue,
            pageSize: pageSize,
            page: currentPage,
            roleId: role,
            sortOrder: sort
        })
        .done(function (data) {
            $('.spinner').addClass("spinnerHidden")
            $("#table").html(data)
            registerBtns()

        })
}

const lockUser = function (userId) {
    $.ajax({
        url: '/Warehouse/Employee/LockUser?userId=' + userId,
        type: 'PUT',
        success: function (data) {
            loadData()
        },
        error: function () {
            showAlert()
        }
    })
}

const unlockUser = function (userId) {
    $.ajax({
        url: '/Warehouse/Employee/UnlockUser?userId=' + userId,
        type: 'PUT',
        success: function (data) {
            loadData()
        },
        error: function () {
            showAlert()
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

    $('.lockUserBtn').click(function (e) {
        e.preventDefault()
        selectedUser = $(this).data("id")
        $("#lockEmployeeModal").modal('show')
    })

    $('.unlockUserBtn').click(function (e) {
        e.preventDefault()
        selectedUser = $(this).data("id")
        $("#unlockEmployeeModal").modal('show')
    })

}