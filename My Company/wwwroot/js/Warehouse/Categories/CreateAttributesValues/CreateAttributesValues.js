const values = []
let editingIdx

$(function () {
    let currentTab = 0;
    showTab(currentTab);

    for (let i = 0; i < lenght; i++)
        values.push([]);

    function showTab(n) {
        const x = $(".tab")
        x[n].style.display = "block";

        if (n == 0) {
            $("#prevBtn").css("display", "none")
        } else {
            $("#prevBtn").css("display", "inline")
        }
        if (n == (x.length - 1)) {
            $("#nextBtn").html("Dodaj")
        } else {
            $("#nextBtn").html("Następny")
        }

        fixStepIndicator(n)

    }

    function fixStepIndicator(n) {

        const x = $(".step")
        x.removeClass("btn-primary")
        x.eq(n).addClass("btn-primary")

    }

    function nextPrev(n) {
        const x = $(".tab")
        if (!$("#attributesValuesForm").validate().form()) return;
        x.eq(currentTab).css("display", "none")
        currentTab = currentTab + n
        if (currentTab >= x.length) {
            $("#attributesValuesForm").submit()
            currentTab--
        }
        showTab(currentTab)
    }

    $("#prevBtn").click(function () {
        nextPrev(-1)
    })

    $("#nextBtn").click(function () {
        nextPrev(1)
    })

    $('.addValueBtn').click(function () {
        const id = $(this).data("id")
        if (!validateAdd(id))
            return

        const value = $(`input[name="name-${id}"]`).val()
        $(`input[name="name-${id}"]`).val('')

        values[id].push(value)
        printValues(id)
    })

    $('.editValueBtn').click(function () {
        const id = $(this).data("id")
        if (!validateEdit(id))
            return

        const value = $(`input[name="edit-${id}"]`).val()
        $(`input[name="edit-${id}"]`).val('')

        values[id][editingIdx] = value
        printValues(id)
        $(`#editDiv-${id}`).hide()
    })

    registerBtns()

})

const registerBtns = function () {
    $(".removeValueBtn").click(function () {
        const id = $(this).data('id')
        const idx = $(this).data('index')

        values[id] = values[id].filter((value, index) => index != idx)

        printValues(id)
    })

    $(".item").click(function (e) {
        e.preventDefault()
        const id = $(this).data('id')
        const idx = $(this).data('index')

        editingIdx = idx

        $(`#editDiv-${id}`).show()
        $(`input[name="edit-${id}"]`).val(values[id][idx])


    })
}

const validateAdd = function (i) {
    $(`#validationName-${i}`).html('')
    const value = $(`input[name="name-${i}"]`).val()
    if (!value || value == '') {
        $(`#validationName-${i}`).html('Wartość nie może być pusta')
        return false
    }
    else if (!values[i] || values[i].find(item => item === value)) {
        $(`#validationName-${i}`).html('Taka wartość już istnieje')
        return false
    }
    else return true

}

const validateEdit = function (i) {
    $(`#validationEdit-${i}`).html('')
    const value = $(`input[name="edit-${i}"]`).val()
    if (!value || value == '') {
        $(`#validationEdit-${i}`).html('Wartość nie może być pusta')
        return false
    }
    else if (!values[i] || values[i].find(item => item === value)) {
        $(`#validationEdit-${i}`).html('Taka wartość już istnieje')
        return false
    }
    else return true

}

const printValues = function (id) {
    const valuesDiv = $(`#values-${id}`)
    valuesDiv.find('input').remove()
    const ul = $(`#values-${id} ul`)
    ul.empty()
    values[id].forEach((value, idx) => {
        valuesDiv.append(`<input type="hidden" name="[${id}].Values[${idx}]" value="${value}"/>`)
        ul.append(`<li class="list-group-item d-flex justify-content-between list-group-item-action align-items-center item" data-id="${id}" data-index="${idx}">
            ${value}
            <button type="button" class="removeValueBtn btn btn-outline-danger"" data-id="${id}" data-index="${idx}">
                 <i class="bi bi-x"></i>
            </button>
        </li>`)
    })

    registerBtns()
}