$(function () {

    const warehouseId = getUrlVars()["id"]

    setResetModalOnClose()

    $(".rowExpander").click(showHideRows)

    $("#addSectorsForm").submit(function (e) {
        e.preventDefault()
        if (!$(this).validate().form()) return;
        $.post("AddSectors", $(this).serialize())
            .done(function (data) {
                const rows = $(document).find('tr[aria-rowId]').toArray()
                rows.forEach(row => {
                    if ($(row).attr("aria-rowId") == data.id)
                        $(row).remove()
                })
                const trprev = $(`#row-${data.id}`)
                const expander = trprev.find(".rowExpander i")
                expander.removeClass("bi-caret-right")
                expander.addClass("bi-caret-down")
                let prevrow = trprev;
                data.sectors.forEach(sector => {
                    newrow = $(`<tr style="display:flex" aria-rowId="${data.id}" class="bg-light">
                    <td class="col-2">
                            ${data.rowName}${sector.order}
                    </td>
                </tr>`)
                    prevrow.after(newrow)
                    prevrow = newrow
                })
                $('.modal').modal('hide')
            })

    })

    $("#addRowForm").submit(function (e) {
        e.preventDefault()
        if (!$(this).validate().form()) return;
        $(".spinner").removeClass('spinnerHidden')
        $.post("AddRow?warehouseId=" + warehouseId, $(this).serialize())
            .fail(function (data) {
                if (data.responseText == 'name already exists')
                    $('#addRowForm .validationMessage').text("Podana nazwa jest zajęta")
                else {
                    $('.alert .alert-content').text("Podczas dodawania wystapił problem")
                    $('.modal').modal('hide')
                    showAlert()
                }

                
            })
            .done(function (data) {
                alert(JSON.stringify(data))
                const tableBody = $('.table tbody')
                const newFirstRow = $(`<tr class="d-flex" id="row-${data.id}"</tr>`)
                /*expander*/
                const expander = $(` <td class="rowExpander col-2" aria-rowId="${data.id}">
                        <i class="bi bi-caret-right"></i>
                    </td>`)
                expander.click(showHideRows)
                //row name td
                const rowTd = $(`<td class="col-sm-6 col-md-7">
                        ${data.rowName}
                    </td>`)
                //actions
                const actionsTd = $(`<td class="col-sm-4 col-md-3"></td>`)
                //add sectors btn
                const addSectorsBtn = $(`<button class="btn openAddSectorModal" data-toggle="modal" data-target="#addSectorModal" data-rowId="${data.id}">
                    <i class="bi bi-plus-lg"></i>
                </button> `)
                addSectorsBtn.click(function (e) {
                    const rowId = $(this).data("rowid")
                    $("#addSectorsForm input[name=RowId]").val(rowId)
                })
                //remove row btn
                const removeRowBtn = $(`<button class="btn openRemoveRowModal" data-toggle="modal" data-target="#removeRowModal" data-rowId="${data.id}">
                    <i class="bi bi-trash-fill"></i>
                </button> `)
                removeRowBtn.click(function (e) {
                    const rowId = $(this).data("rowid")
                    $("#removeRowModal input[name=RowId]").val(rowId)
                })
                //swapBtn
                const swapBtn = $(`<button class="btn moveUpBtn" data-rowId="${data.id}"><i class="bi bi-arrow-up"></i></button> `)
                swapBtn.click(function (e) {
                    const rowId = $(this).data("rowid")
                    $(".spinner").removeClass('spinnerHidden')
                    swapRows(rowId, -1)
                    $(".spinner").addClass('spinnerHidden')
                })

                //append elements
                actionsTd.append(addSectorsBtn)
                actionsTd.append('&nbsp;')
                actionsTd.append(removeRowBtn)
                actionsTd.append('&nbsp;')
                actionsTd.append(swapBtn)
                newFirstRow.append(expander)
                newFirstRow.append(rowTd)
                newFirstRow.append(actionsTd)
                tableBody.append(newFirstRow)

                //next rows
                data.sectors.forEach((sector, index, sectors) => {
                    const tr = $(`<tr style="display:none" aria-rowId="${data.id}" class="bg-light">
                        <td class="col-2">
                            ${data.rowName}${sector.order}
                        </td>
                    </tr>`)
                    tableBody.append(tr)
                })
            })
        $(".spinner").addClass('spinnerHidden')


    })

    $(".openAddSectorModal").click(function (e) {
        const rowId = $(this).data("rowid")
        $("#addSectorsForm input[name=RowId]").val(rowId)
    })

    $(".moveUpBtn").click(function (e) {
        const rowId = $(this).data("rowid")
        $(".spinner").removeClass('spinnerHidden')
        swapRows(rowId, -1)
        $(".spinner").addClass('spinnerHidden')
    })

    $(".moveDownBtn").click(function (e) {
        const rowId = $(this).data("rowid")
        $(".spinner").removeClass('spinnerHidden')
        swapRows(rowId, 1)
        $(".spinner").addClass('spinnerHidden')
    })

    $(".openRemoveRowModal").click(function (e) {
        const rowId = $(this).data("rowid")
        $("#removeRowModal input[name=RowId]").val(rowId)
    })

    $("#removeRowBtn").click(function (e) {
        const rowId = $("#removeRowModal input[name=RowId]").val()
        $(".spinner").removeClass('spinnerHidden')
        deleteRow(rowId)
        $(".spinner").addClass('spinnerHidden')
    })
})

const swapRows = function (rowId, direction) {
    $.ajax({
        url: 'SwapRows',
        type: 'PUT',
        data: {
            rowId: rowId,
            direction: direction
        },
        success: function (data) {

            if (direction == 1) {
                $(`#row-${rowId}`).insertAfter($(`#row-${data}`))
                if ($(`#row-${rowId} button.moveUpBtn`).length == 0) {
                    $(`#row-${data} button.moveUpBtn`).remove()
                    const btnUp = $(`<button class="btn moveUpBtn" data-rowId="${rowId}">
                        <i class="bi bi-arrow-up"></i>
                    </button>`)
                    btnUp.click(function () {
                        const rowId = $(this).data("rowid")
                        $(".spinner").removeClass('spinnerHidden')
                        swapRows(rowId, -1)
                        $(".spinner").addClass('spinnerHidden')
                    })
                    $(`#row-${rowId} td button.moveDownBtn`).before(btnUp)
                }
                if ($(`#row-${data} button.moveDownBtn`).length == 0) {
                    $(`#row-${rowId} button.moveDownBtn`).remove()
                    const btnDown = $(`<button class="btn moveDownBtn" data-rowId="${data}">
                        <i class= "bi bi-arrow-down"></i>
                    </button>`)
                    btnDown.click(function () {
                        const rowId = $(this).data("rowid")
                        $(".spinner").removeClass('spinnerHidden')
                        swapRows(rowId, 1)
                        $(".spinner").addClass('spinnerHidden')
                    })
                    $(`#row-${data} td button.moveUpBtn`).after(btnDown)
                }

            } else {
                $(`#row-${rowId}`).insertBefore($(`#row-${data}`))
                if ($(`#row-${data} button.moveUpBtn`).length == 0) {
                    $(`#row-${rowId} button.moveUpBtn`).remove()
                    const btnUp = $(`<button class="btn moveUpBtn" data-rowId="${data}">
                        <i class="bi bi-arrow-up"></i>
                    </button>`)
                    btnUp.click(function () {
                        const rowId = $(this).data("rowid")
                        $(".spinner").removeClass('spinnerHidden')
                        swapRows(rowId, -1)
                        $(".spinner").addClass('spinnerHidden')
                    })
                    $(`#row-${data} td button.moveDownBtn`).before(btnUp)
                }
                if ($(`#row-${rowId} button.moveDownBtn`).length == 0) {
                    $(`#row-${data} button.moveDownBtn`).remove()
                    const btnDown = $(`<button class="btn moveDownBtn" data-rowId="${rowId}">
                        <i class= "bi bi-arrow-down"></i>
                    </button>`)
                    btnDown.click(function () {
                        const rowId = $(this).data("rowid")
                        $(".spinner").removeClass('spinnerHidden')
                        swapRows(rowId, 1)
                        $(".spinner").addClass('spinnerHidden')
                    })
                    $(`#row-${rowId} td button.moveUpBtn`).after(btnDown)
                }
            }
            const rows = $(document).find('tr[aria-rowId]').toArray()
            rows.reverse().forEach(row => {
                if ($(row).attr("aria-rowId") == rowId) {
                    $(`#row-${rowId}`).after($(row))
                }
                else if ($(row).attr("aria-rowId") == data) {
                    $(`#row-${data}`).after($(row))
                }
            })
        },
    })

}

const deleteRow = function (rowId) {
    $.ajax({
        url: 'DeleteRow',
        type: 'DELETE',
        data: {
            rowId: rowId,
        },
        success: function (data) {
            const rows = $(document).find('tr[aria-rowId]').toArray()
            rows.forEach(row => {
                if ($(row).attr("aria-rowId") == data) {
                    $(row).hide("slow", null, () => { $(row).remove() })
                }
            })
            $(`#row-${data}`).hide("slow", null, function() { $(this).remove() })
        },
        error: function (data) {
            if (data.responseText == "cannot delete this row")
                $('.alert .alert-content').text("Nie można usunąć tego rzędu. Przed usunięciem należy usunąć wszystkie produkty")
            else
                $('.alert .alert-content').text("Podczas usuwania wystąpił problem")
            showAlert()
        }
    })
}

const showHideRows = function () {
    const i = $(this).find("i")
    const id = $(this).attr("aria-rowId")
    const rows = $(document).find('tr[aria-rowId]').toArray()
    const expanded = i.hasClass("bi-caret-right")
    if (expanded) {
        i.removeClass("bi-caret-right")
        i.addClass("bi-caret-down")
        rows.forEach(row => {
            if ($(row).attr("aria-rowId") == id) {
                $(row).show("fast", null, () => { $(row).css('display', 'flex') })

            }
        })
    } else {
        i.addClass("bi-caret-right")
        i.removeClass("bi-caret-down")
        rows.forEach(row => {
            if ($(row).attr("aria-rowId") == id) {
                $(row).hide("fast")
            }
        })
    }
}