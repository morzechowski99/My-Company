/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
let editingIdx
let values = []
$(function () {

    $(".hiddenInput").each(function () {
        values.push($(this).val())
    });
    
    $('#backBtn').click(function () {
        window.history.back()
    })

    $('.addValueBtn').click(function () {
        if (!validateAdd())
            return

        const value = $(`input[name="name"]`).val()
        $(`input[name="name"]`).val('')

        values.push(value)
        printValues()
    })

    $('.editValueBtn').click(function () {
        if (!validateEdit())
            return

        const value = $(`input[name="edit"]`).val()
        $(`input[name="edit"]`).val('')

        values[editingIdx] = value
        printValues()
        $(`#editDiv`).hide()
    })

    registerBtns()

})

const registerBtns = function () {
    $(".removeValueBtn").click(function () {
        const idx = $(this).data('index')

        values = values.filter((value, index) => index != idx)

        printValues()
    })

    $(".item").click(function (e) {
        e.preventDefault()
        const idx = $(this).data('index')

        editingIdx = idx

        $(`#editDiv`).show()
        $(`input[name="edit"]`).val(values[idx])


    })
}

const validateAdd = function () {
    $(`#validationName`).html('')
    const value = $(`input[name="name"]`).val()
    if (!value || value == '') {
        $(`#validationName`).html('Wartość nie może być pusta')
        return false
    }
    else if (!values || values.find(item => item === value)) {
        $(`#validationName`).html('Taka wartość już istnieje')
        return false
    }
    else return true

}

const validateEdit = function () {
    $(`#validationEdit`).html('')
    const value = $(`input[name="edit"]`).val()
    if (!value || value == '') {
        $(`#validationEdit`).html('Wartość nie może być pusta')
        return false
    }
    else if (!values || values.find(item => item === value)) {
        $(`#validationEdit`).html('Taka wartość już istnieje')
        return false
    }
    else return true

}

const printValues = function () {
    const valuesDiv = $(`#values`)
    valuesDiv.find('input').remove()
    const ul = $(`#values ul`)
    ul.empty()
    values.forEach((value, idx) => {
        valuesDiv.append(`<input type="hidden" class="hiddenInput" name="[${idx}]" value="${value}"/>`)
        ul.append(`<li class="list-group-item d-flex justify-content-between list-group-item-action align-items-center item" data-index="${idx}">
            ${value}
            <button type="button" class="removeValueBtn btn btn-outline-danger" data-index="${idx}">
                 <i class="bi bi-x"></i>
            </button>
        </li>`)
    })

    registerBtns()
}