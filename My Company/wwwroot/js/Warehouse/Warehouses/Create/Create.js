
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
        x.eq(currentTab).css("display","none")
        currentTab = currentTab + n
        if (currentTab >= x.length) {
            document.getElementById("regForm").submit();
            return false
        }
        showTab(currentTab)
    }

    function addSector() {
        const name = $("#sectorNameInput").val()
        const count = Number.parseInt($("#sectorCountInput").val())
        $("#sectorNameInput").val("")
        $("#sectorCountInput").val("")

        const row = $("<div></div>");
        for (let i = 0; i < count; i++) {
            row.append(`<button type="button" class="btn">${name}${i+1}</button>`)
        }

        $("#warehousePlan").append(row)
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
})