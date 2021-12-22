$(function () {

    const warehouseId = getRouteValueAt(3)

    setResetModalOnClose()

    $(".rowExpander").click(showHideRows)

    $("#addSectorsForm").submit(function (e) {
        e.preventDefault()
        if (!$(this).validate().form()) return;
        $.post("/Warehouse/Warehouses/AddSectors", $(this).serialize())
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
                data.sectors.forEach((sector, idx, array) => {
                    newrow = $(`<tr style="display:flex" aria-rowId="${data.id}" class="bg-light">
                    <td class="col-2">
                            ${data.rowName}${sector.order}
                    </td>
                </tr>`)
                    const td2 = $(` <td class="col-sm-6 col-md-7">${barcode(sector.id)}</td>`)
                    if (idx === array.length - 1) {
                        td2.append(getDeleteSectorBtn(sector.id, false))

                    }
                    newrow.append(td2)
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
        $.post("/Warehouse/Warehouses/AddRow?warehouseId=" + warehouseId, $(this).serialize())
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

                const tableBody = $('.table tbody')
                const newFirstRow = $(`<tr class="d-flex rowDetails" id="row-${data.id}"</tr>`)
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

                //append elements
                actionsTd.append(addSectorsBtn)
                actionsTd.append('&nbsp;')
                actionsTd.append(removeRowBtn)
                actionsTd.append('&nbsp;')
                if (data.order != 1) {
                    //swapBtn
                    const swapUpBtn = $(`<button class="btn moveUpBtn" data-rowId="${data.id}"><i class="bi bi-arrow-up"></i></button> `)
                    swapUpBtn.click(function (e) {
                        const rowId = $(this).data("rowid")
                        $(".spinner").removeClass('spinnerHidden')
                        swapRows(rowId, -1)
                        $(".spinner").addClass('spinnerHidden')
                    })
                    actionsTd.append(swapUpBtn)
                    const prevRowDetails = $('.rowDetails').last()
                    const id = prevRowDetails.find('.btn').data("rowid")
                    const swapDownBtn = $(`<button class="btn moveDownBtn m-0" data-rowId="${id}"><i class="bi bi-arrow-down"></i></button>`)
                    swapDownBtn.click(function () {
                        const rowId = $(this).data("rowid")
                        $(".spinner").removeClass('spinnerHidden')
                        swapRows(rowId, 1)
                        $(".spinner").addClass('spinnerHidden')
                    })

                    prevRowDetails.find('td').last().append(swapDownBtn)
                }
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
                        <td class= "col-sm-6 col-md-7" >
                            ${barcode(sector.id)}
                            ${index == sectors.length - 1 ? getDeleteSectorBtn(sector.id, false).prop('outerHTML') : ''}
                        </td >
                    </tr>`)
                    tableBody.append(tr)
                })
                $('.modal').modal('hide')
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

    $(".openRemoveSectorModal").click(function (e) {
        const sectorId = $(this).data("sector")
        $("#removeSectorModal input[name=sectorId]").val(sectorId)
    })

    $("#removeRowBtn").click(function (e) {
        const rowId = $("#removeRowModal input[name=RowId]").val()
        $(".spinner").removeClass('spinnerHidden')
        deleteRow(rowId)
        $(".spinner").addClass('spinnerHidden')
    })

    $("#removeSectorBtn").click(function (e) {
        const sectorId = $("#removeSectorModal input[name=sectorId]").val()
        $(".spinner").removeClass('spinnerHidden')
        deleteSector(sectorId)
        $(".spinner").addClass('spinnerHidden')
    })
})

const swapRows = function (rowId, direction) {
    $.ajax({
        url: '/Warehouse/Warehouses/SwapRows',
        type: 'PUT',
        data: {
            rowId: rowId,
            direction: direction
        },
        success: function (data) {

            if (direction == 1) {
                $(`#row-${rowId}`).insertAfter($(`#row-${data}`))

            } else {
                $(`#row-${rowId}`).insertBefore($(`#row-${data}`))

            }

            fixArrows()
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
        url: '/Warehouse/Warehouses/DeleteRow',
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
            $(`#row-${data}`).hide("slow", null, function () { $(this).remove() })
            const detailsRows = $('.rowDetails').toArray()

            if (detailsRows.length === 2) {
                $(detailsRows[0]).find('.moveUpBtn').remove()
                $(detailsRows[0]).find('.moveDownBtn').remove()
            }
            else {
                $(detailsRows[0]).find('.moveUpBtn').remove()
                $(detailsRows[1]).find('.moveDownBtn').remove()
            }

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
                $(row).css('display', 'flex')

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

const fixArrows = function () {
    $('.rowDetails').find('.moveUpBtn').remove()
    $('.rowDetails').find('.moveDownBtn').remove()
    const rows = $('.rowDetails').toArray()
    if (rows.length === 0 || rows.length === 1) return
    else if (rows.length === 2) {
        const id1 = $(rows[0]).find('td').first().attr('aria-rowId')
        $(rows[0]).find('td').last().append(getBtnDown(id1))
        const id2 = $(rows[1]).find('td').first().attr('aria-rowId')
        $(rows[1]).find('td').last().append(getBtnUp(id2))
    }
    else {
        rows.forEach((row, index, array) => {
            if (index === 0) {
                const id1 = $(row).find('td').first().attr('aria-rowId')
                $(row).find('td').last().append(getBtnDown(id1))
            }
            else if (index === array.length - 1) {
                const id2 = $(row).find('td').first().attr('aria-rowId')
                $(row).find('td').last().append(getBtnUp(id2))
            }
            else {
                const id = $(row).find('td').first().attr('aria-rowId')
                $(row).find('td').last().append(getBtnUp(id))
                $(row).find('td').last().append(getBtnDown(id))
            }
        })
    }
}

const getBtnDown = function (rowid) {
    const swapDownBtn = $(`<button class="btn moveDownBtn m-0" data-rowId="${rowid}"><i class="bi bi-arrow-down"></i></button>`)
    swapDownBtn.click(function () {
        const rowId = $(this).data("rowid")
        $(".spinner").removeClass('spinnerHidden')
        swapRows(rowId, 1)
        $(".spinner").addClass('spinnerHidden')
    })
    return swapDownBtn
}

const getBtnUp = function (rowid) {
    const swapUpBtn = $(`<button class="btn moveUpBtn" data-rowId="${rowid}"><i class="bi bi-arrow-up"></i></button> `)
    swapUpBtn.click(function (e) {
        const rowId = $(this).data("rowid")
        $(".spinner").removeClass('spinnerHidden')
        swapRows(rowId, -1)
        $(".spinner").addClass('spinnerHidden')
    })
    return swapUpBtn
}

const deleteSector = function (sectorId) {
    $.ajax({
        url: '/Warehouse/Warehouses/DeleteSector',
        type: 'DELETE',
        data: {
            sectorId: sectorId,
        },
        success: function (data) {
            $(document).find(`tr[aria-rowId=${data.rowId}]`).not(".rowDetails").remove()

            const row = $(`#row-${data.rowId}`)

            data.sectors.reverse().forEach((sector, idx) => {
                const tr = $(`<tr style="display:flex" aria-rowId="${data.rowId}" class="bg-light"></tr>`)
                const td1 = $(` <td class="col-2">
                            ${data.rowName}${sector.order}
                        </td>`)
                tr.append(td1)
                const td2 = $(` <td class="col-sm-6 col-md-7"></td>`)

                if (idx === 0) {
                    td2.append(getDeleteSectorBtn(sector.id, !sector.deletable))
                    if (!sector.deletable)
                        td2.append($(`<span class="text-danger">Usuń wszystkie produkty z sektora</span>`))
                }
                tr.append(td2)
                row.after(tr)
            })
        },
        error: function (data) {
            $('.alert .alert-content').text("Wystąpił błąd")
            showAlert()
        }
    })
}

const getDeleteSectorBtn = function (sectorId, disabled) {
    let btn;
    if (!disabled) {
        btn = $(`<button class="btn openRemoveSectorModal m-0" data-toggle="modal" data-target="#removeSectorModal" data-sector="${sectorId}"><i class="bi bi-trash-fill"></i></button>`)
        btn.click(function (e) {
            const sectorId = $(this).data("sector")
            $("#removeSectorModal input[name=sectorId]").val(sectorId)
        })
    }
    else
        btn = $(`<button disabled class="btn m-0"><i class="bi bi-trash-fill"></i></button> `)
    return btn
}

const barcode = function (id) {
    const code = getBarcode(id)
    return `<a href="https://bwipjs-api.metafloor.com/?bcid=code128&amp;text=${code}&amp;scale=3&amp;includetext" 
    target="_blank">Drukuj kod</a>`
}

const getBarcode = function (id) {
    return ("0000000000000" + id).substr(-13, 13)
}