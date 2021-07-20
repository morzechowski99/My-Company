
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
        $("#myform").validate()
        // Exit the function if any field in the current tab is invalid:
        //if (n == 1 && !validateForm()) return false;
        // Hide the current tab:
        x.eq(currentTab).css("display","none")
        // Increase or decrease the current tab by 1:
        currentTab = currentTab + n;
        // if you have reached the end of the form...
        if (currentTab >= x.length) {
            // ... the form gets submitted:
            document.getElementById("regForm").submit();
            return false;
        }
        // Otherwise, display the correct tab:
        showTab(currentTab);
    }

    $("#prevBtn").click(function () {
        nextPrev(-1)
    })

    $("#nextBtn").click(function () {
        nextPrev(1)
    })
})