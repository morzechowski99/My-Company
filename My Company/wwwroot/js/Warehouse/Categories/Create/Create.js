/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
let attributesCount = 0

$.validator.setDefaults({ ignore: null });

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
        if (!$("#createCategoryForm").validate().form()) return;
        x.eq(currentTab).css("display", "none")
        currentTab = currentTab + n
        if (currentTab >= x.length) {
            $("#createCategoryForm").submit()
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

    $("#parentSelect").selectize({
        allowEmptyOption: false,
    });

    $("#parentSelect").change(function (e) {
        const option = $(this).find('option')
        let tree
        const name = $("#nameInput").val()
        if (!option.val() || option.val() == -1)
            tree = name
        else
            tree = `${option.html()}/${name}`
        $("#tree").html(tree)
        const id = $(this).val();
        if (id && id !== -1) {
            $("#inheritedAttributes").load("/Warehouse/Categories/GetInheritedAttributes?id=" + id)
            $("#inheritedContainer").show()
        }
        else {
            $("#inheritedAttributes").html("")
            $("#inheritedContainer").hide()
        }
    })

    registerBtns()

    $("#addAtrtibuteBtn").click(function () {
        var template = $(`<div class="row" data-id="${attributesCount}">
                <div class="form-group col-md-4">
                    <label class="control-label" for="Attributes_${attributesCount}__Name">Nazwa</label>
                    <input class="form-control" type="text" data-val="true" data-val-maxlength="Pole mo&#x17C;e mie&#x107; maksymalnie 30 znak&#xF3;w"
                    data-val-maxlength-max="30" data-val-required="Pole wymagane" id="Attributes_${attributesCount}__Name" 
                    maxlength="30" name="Attributes[${attributesCount}].Name" value="" />
                    <span class="text-danger field-validation-valid" data-valmsg-for="Attributes[${attributesCount}].Name" data-valmsg-replace="true"></span>
                </div>
                <div class="form-group col-md-4">
                    <label class="control-label" for="Attributes_${attributesCount}__Type">Typ</label>
                    <select class="form-control" data-val="true" data-val-required="Pole wymagane" id="Attributes_${attributesCount}__Type" name="Attributes[${attributesCount}].Type">
                       
                    </select>
                    <span class="text-danger field-validation-valid" data-valmsg-for="Attributes[${attributesCount}].Type" data-valmsg-replace="true"></span>
                    
                </div>
                 <div class="col-md-4 p-3 h-50 d-flex mt-3 justify-content-center align-items-md-end">
                    <button type="button" class="btn btn-block btn-outline-danger deleteAttributeBtn" data-id="${attributesCount}">Usuń</button>
                </div>
              
            </div>`)

        template.find("select").append($("#templateAttributes").html())

        $("#newAttributesDiv").append(template)

        //Remove current form validation information
        $("#createCategoryForm")
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        //Parse the form again
        $.validator
            .unobtrusive
            .parse("form");
        attributesCount++

        registerBtns()
    })
    $("#createCategoryForm").submit(function () {
        if (!$("#createCategoryForm").validate().form()) return;
    })


    
})

const registerBtns = function () {
    $(".deleteAttributeBtn").click(function () {
        const id = $(this).data("id")
    
        $(`div[data-id=${id}]`).remove()
        fixAttributes(id)
    })
}

const fixAttributes = function (deletedId) {
    for (let i = deletedId + 1; i <= attributesCount; i++) {
        
        const container = $(`div[data-id=${i}]`)
        container.find('input').attr("name", `Attributes[${i-1}].Name`)
        container.find('select').attr("name", `Attributes[${i-1}].Type`)
        container.data("id",i-1)
    }
    attributesCount--
}