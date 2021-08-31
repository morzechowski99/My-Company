let searchValue = ""
let role = null
let sort = null
let selectedUser = null

$(function () {

    $('#searchBtn').click(function () {
        searchValue = $("#searchValue").val()
        loadData()
    })

    $('#roleSelect').change(function () {
        role = $(this).val()
        loadData()
    })

    $('#sortSelect').change(function () {
        sort = $(this).val()
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
        loadData()
    })

    $('.prevPageBtn').click(function (e) {
        e.preventDefault()
        currentPage--
        loadData()
    })

    $('#changePageBtn1').click(function (e) {
        e.preventDefault()
        console.log($("#pageValue1").val())
        currentPage = $("#pageValue1").val()
        loadData()
    })

    $('#changePageBtn2').click(function (e) {
        e.preventDefault()
        currentPage = $("#pageValue2").val()
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