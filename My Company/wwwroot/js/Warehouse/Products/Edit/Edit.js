/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
$.validator.setDefaults({ ignore: null });
let lastCategoryId = null
let photoToDeletePath = ''

$(function () {

    $(".categorySelect").change(function () {
        const id = $(this).data("id");
        const selects = $(".categorySelectContainer").toArray()
        selects.forEach(select => {
            const $select = $(select)
            if ($select.data("id") > id)
                $select.remove();
        })
        lastCategoryId = $(this).val()
        loadSubCategories($(this).val(), id + 1)
    })

    $("#categoryForm").submit(function (e) {
        e.preventDefault()
        toogleSpinner()
        $.ajax({
            url: "/Warehouse/Products/EditCategories",
            type: "PUT",
            data: $(this).serialize(),
            error: function () {
                showAlert(failAlert)
                toogleSpinner()
            },
            success: function () {
                loadNewAttributes()
            },

        })

    })

    $("#changeStatusForm").submit(function (e) {
        e.preventDefault()
        toogleSpinner()
        $.ajax({
            url: `/Warehouse/Products/ChangeStatus`,
            type: "PUT",
            data: $(this).serialize(),
            error: function () {
                showAlert(failAlert)
                toogleSpinner()
            },
            success: function () {
                showAlert(successAlert)
                toogleSpinner()
            },
        })
    })

    $("#attributesForm").submit(function (e) {
        e.preventDefault()
        toogleSpinner()
        const id = $('input[name="Id"]').val()
        $.ajax({
            url: `/Warehouse/Products/EditAttributes/${id}`,
            type: "PUT",
            data: $(this).serialize(),
            error: function () {
                showAlert(failAlert)
                toogleSpinner()
            },
            success: function () {
                showAlert(successAlert)
                toogleSpinner()
            },
        })
    })

    $("#basicInfoForm").submit(function (e) {
        e.preventDefault()
        toogleSpinner()
        $.ajax({
            url: "/Warehouse/Products/EditBasicInfo",
            type: "PUT",
            data: $(this).serialize(),
            error: function () {
                showAlert(failAlert)
                toogleSpinner()
            },
            success: function () {
                showAlert(successAlert)
                toogleSpinner()
            },
        })
    })

    $("#descriptionForm").submit(function (e) {
        e.preventDefault()
        toogleSpinner()
        $.ajax({
            url: "/Warehouse/Products/EditDescription",
            type: "PUT",
            data: $(this).serialize(),
            error: function () {
                showAlert(failAlert)
                toogleSpinner()
            },
            success: function () {
                showAlert(successAlert)
                toogleSpinner()
            }
        })
    })

    $("#addPhotoForm").submit(function (e) {
        e.preventDefault()
        toogleSpinner()
        const data = new FormData(this);
        $.ajax({
            url: "/Warehouse/Products/UploadPhoto",
            type: "POST",
            data: data,
            enctype: 'multipart/form-data',
            processData: false,
            contentType: false,
            cache: false,
            error: function () {
                showAlert(failAlert)
                toogleSpinner()
            },
            success: function (data) {
                $("#photosContainer").html(data)
                registerButtons()
                toogleSpinner()
            }
        })
    })

    $('#photoInput').change(function (e) {
        e.stopImmediatePropagation();
        const [file] = this.files

        if (file) {
            if (file.size > 5000000) {
                $("#photoInputValidation").text("Maksymalny rozmiar pliku to 5MB")
                return;
            } else {
                $("#photoInputValidation").text("")
                $("#addPhotoForm").submit()
            }
        }
    })

    $("#decriptionTextArea").jqte({
        link: false,
        source: false
    })

    let supplierSelect = $("#supplierSelect").selectize({
        valueField: "id",
        labelField: "description",
        searchField: "description",
        options: [],
        placeholder: "Zacznij pisać, aby wybrać",
        create: false,
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

    $("#deletePhoto").click(function () {
        deletePhoto()
    })

    registerButtons()
})

const loadSubCategories = function (id, containerId) {
    if (!id)
        return
    toogleSpinner()

    const container = $(`<div class="form-group categorySelectContainer" data-id="${containerId}">   
    </div>`)
    const select = $(`<select id="Categories_${containerId}_" name="Categories[${containerId}]" class="form-control categorySelect" data-id="${containerId}"></select>`)
    select.change(function () {

        const id = $(this).data("id");
        const selects = $(".categorySelectContainer").toArray()
        selects.forEach(select => {
            const $select = $(select)
            if ($select.data("id") > id)
                $select.remove();
        })
        lastCategoryId = $(this).val()
        loadSubCategories($(this).val(), id + 1)
    })
    container.append(select)
    let subCategoriesExists = false
    $.get("/Warehouse/Categories/GetChildCategories/" + id)
        .done(function (data) {

            if (data.length) {
                subCategoriesExists = true
                select.append(`<option value="${0}">Wybierz</option>`)
                data.forEach(category => {
                    select.append(`<option value="${category.id}">${category.categoryName}</option>`)
                })
            }
            if (subCategoriesExists)
                $("#categorySelects").append(container)
        })
        .always(function () {
            toogleSpinner()
        })
}

const successAlert = `<div class="alert alert-success alert-dismissible fade show" role="alert">
    <h5>Pomyślnie zapisano zmiany</h5>
</div>`

const failAlert = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
    <h5 class="alert-content">Wystąpił problem</h5>
</div>`

const showAlert = function (alert) {
    $('body').append(alert)
    setTimeout(function () {
        $('.alert').alert('close')
    }, 5000)
}

const toogleSpinner = function () {
    if ($('.spinner').hasClass("spinnerHidden"))
        $('.spinner').removeClass("spinnerHidden")
    else
        $('.spinner').addClass("spinnerHidden")
}

const loadNewAttributes = function () {
    $(".attributesContainer").html('')
    const id = $('input[name="Id"]').val();
    $.get(`/Warehouse/Products/GetAttributesComponent/${id}`)
        .done(function (data) {
            $(".attributesContainer").html(data);
            showAlert(successAlert)
            toogleSpinner()
        })
        .error(function () {
            showAlert(failAlert)
            toogleSpinner()
        })
}

const registerButtons = function () {
    $(".deleteBtn").click(function () {
        photoToDeletePath = $(this).data('path')
    })
}

const deletePhoto = function () {
    toogleSpinner()
    const id = $('input[name="Id"]').val()
    $.ajax({
        url: `/Warehouse/Products/DeletePhoto/${id}`,
        type: "DELETE",
        data: {
            path: photoToDeletePath,
        },
        error: function () {
            showAlert(failAlert)
            toogleSpinner()
        },
        success: function (data) {
            $("#photosContainer").html(data)
            registerButtons()
            toogleSpinner()
        },
    });
}