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

})