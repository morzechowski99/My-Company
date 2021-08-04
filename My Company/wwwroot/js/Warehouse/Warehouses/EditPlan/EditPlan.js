$(function () {

    $(".rowExpander").click(function () {
        const i = $(this).find("i")
        const id = $(this).attr("aria-rowId")
        const rows = $(document).find('tr[aria-rowId]').toArray()
        const expanded = i.hasClass("bi-caret-right")
        if (expanded) {
            i.removeClass("bi-caret-right")
            i.addClass("bi-caret-down")
            rows.forEach(row => {
                if ($(row).attr("aria-rowId") == id) {
                    $(row).show("fast", null, () => { $(row).css('display', 'flex')})
                    
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

    })

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
                    newrow = $(`<tr style="display:flex" aria-rowId="${data.id}">
                    <td class="col-2">
                            ${data.rowName}${sector.order}
                    </td>
                </tr>`)
                    prevrow.after(newrow)
                    prevrow = newrow
                })
        })
        
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
                    $(`#row-${rowId} td button.moveDownBtn`).before(` <button class="btn moveUpBtn" data-rowId="${rowId}">
                        <i class="bi bi-arrow-up"></i>
                    </button>`)
                }//todo dokonczyc te buttony
                if ($(`#row-${data} button.moveDownBtn`).length == 0)
                    $(`#row-${rowId} button.moveDownBtn`).remove()
            } else {
                $(`#row-${rowId}`).insertBefore($(`#row-${data}`))
                if ($(`#row-${data} button.moveUpBtn`).length == 0)
                    $(`#row-${rowId} button.moveUpBtn`).remove()
                if ($(`#row-${rowId} button.moveDownBtn`).length == 0)
                    $(`#row-${data} button.moveDownBtn`).remove()
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