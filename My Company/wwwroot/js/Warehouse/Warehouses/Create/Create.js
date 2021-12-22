class SectorRow {
    constructor(name, count) {
        this.name = name
        this.count = count
    }
}

const rows = [];

$(function () {
    let currentTab = 0;
    showTab(currentTab);

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
        if (!$("#myForm").validate().form()) return;
        x.eq(currentTab).css("display", "none")
        currentTab = currentTab + n
        if (currentTab >= x.length) {
            $("#myForm").submit()
            currentTab--
        }
        showTab(currentTab)
    }

    function addSector() {
        if (!$("#myForm").validate().form()) return;
        const name = $("#sectorNameInput").val()

        if (rows.find(r => r.name === name)) {
            $('#nameValidate').html("Nazwa musi być unikalna")
            return
        }

        $("#rowsCountValidateMessage").html("")
        $("#addSectorModal").modal('hide')
        const count = Number.parseInt($("#sectorCountInput").val())
        $("#sectorNameInput").val("")
        $("#sectorCountInput").val("")

        rows.push(new SectorRow(name, count))

        rewriteTable();
    }

    function rewriteTable() {
        $("#warehousePlan > tbody").empty()

        rows.forEach((r, idx, arr) => {
            const row = $(`<tr> 
            <td>
                ${r.name}
            </td>
            <td>
                ${r.count}
            </td>
            <td>
               <button class="btn" id="row-${r.name}" type="button"> <i class="bi bi-trash"></i></button>
                ${idx !== arr.length - 1 ? `<button class="btn" id="row-swapDown-${r.name}" type="button"> <i class="bi bi-arrow-down"></i></button>` : ''}
                ${idx !== 0 ? `<button class="btn" id="row-swapUp-${r.name}" type="button"> <i class="bi bi-arrow-up"></i></button>` : ''}
            </td>           
        </tr>`)
            $("#warehousePlan > tbody").append(row)
            $(`#row-${r.name}`).click(function () {
                deleteRow(r.name)
            })

            $(`#row-swapDown-${r.name}`).click(function () {
                swap(r.name, 1)
            })

            $(`#row-swapUp-${r.name}`).click(function () {
                swap(r.name, -1)
            })
        })
    }

    function swap(name, direction) {
        const idx = rows.findIndex(r => r.name === name)
        const temp = rows[idx]
        if (direction === 1) {
            rows[idx] = rows[idx + 1]
            rows[idx + 1] = temp
        }
        else {
            rows[idx] = rows[idx - 1]
            rows[idx - 1] = temp
        }
        rewriteTable()
    }

    function deleteRow(name) {
        rows = rows.filter(row => {
            row.name !== name
        })
        rewriteTable()
    }

    $("#prevBtn").click(function () {
        nextPrev(-1)
    })

    $("#nextBtn").click(function () {
        nextPrev(1)
    })

    $("#addSectorBtn").click(function () {
        addSector()
    })

    $("#sectorNameInput").change(function () {
        $('#nameValidate').html("")
    })

    $("#myForm").submit(function (e) {
        if (rows.length === 0) {
            $("#rowsCountValidateMessage").html("Należy dodać co najmniej jeden rząd")
            e.preventDefault()
            return
        }

        $("#sectorNameInput").remove()
        $("#sectorCountInput").remove()

        rows.forEach((row, idx) => {
            $(this).append(`<input type="hidden" name="Sectors[${idx}].Name" value="${row.name}"/>`)
            $(this).append(`<input type="hidden" name="Sectors[${idx}].Count" value="${row.count}"/>`)
        })

    })
})