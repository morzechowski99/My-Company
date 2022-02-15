/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
$(function () {

    registerBtns()

    $("#addAtrtibuteBtn").click(function () {
        var template = $(`<div class="row" data-id="${attributesCount}">
                 <div class="form-group col-md-4">
                    <label class="control-label" for="z2__Name">Nazwa</label>
                    <input class="form-control" type="text" data-val="true" data-val-maxlength="Pole mo&#x17C;e mie&#x107; maksymalnie 30 znak&#xF3;w" data-val-maxlength-max="30" data-val-required="Pole wymagane" id="z2__Name" maxlength="30" name="[${attributesCount}].Name" />
                    <span class="text-danger  field-validation-valid" data-valmsg-for="[${attributesCount}].Name" data-valmsg-replace="true"></span>
                </div>
                <div class="form-group col-md-4">
                    <label class="control-label" for="z2__Type">Typ</label>
                    <select class="form-control" id="z2__Name" name="[${attributesCount}].Type">
                    </select>
                    <span class="text-danger field-validation-valid" data-valmsg-for="[${attributesCount}].Type" data-valmsg-replace="true"></span>

                </div>
                <div class="col-md-4 p-3 h-50 d-flex mt-3 justify-content-center align-items-md-end">
                    <button type="button" data-toggle="tooltip"
                            title="Usunięcie atrybutu spowoduje usunięcie jego wartości w każdym produkcie tej kategorii"
                            class="btn btn-block btn-outline-danger deleteAttributeBtn" data-id="${attributesCount}">
                        Usuń
                    </button>
                </div>            
            </div>`)

        template.find("select").append($("#templateAttributes").html())

        $("#editAttributesDiv").append(template)

        //Remove current form validation information
        $("#editAttributesForm")
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        //Parse the form again
        $.validator
            .unobtrusive
            .parse("form");
        attributesCount++

        registerBtns()
    })

    $("#editAttributesForm").submit(function () {
        if (!$("#editAttributesForm").validate().form()) return;
    })



})

const registerBtns = function () {
    $(".deleteAttributeBtn").click(function (e) {
        e.stopImmediatePropagation()
        const id = $(this).data("id")

        $(`div[data-id=${id}]`).remove()
        fixAttributes(id)
        registerBtns()
    })
}

const fixAttributes = function (deletedId) {
    for (let i = deletedId + 1; i < attributesCount; i++) {
        const container = $(`div[data-id=${i}]`)
        container.find('input:not([type=hidden])').attr("name", `[${i - 1}].Name`)
        container.find('input[type=hidden]').attr("name", `[${i - 1}].Id`)
        container.find('select').attr("name", `[${i - 1}].Type`)
        container.attr("data-id", i - 1)
        container.find('span.text-danger').attr("data-valmsg-for", `[${i - 1}].Name`)
        container.find('.deleteAttributeBtn').attr("data-id", i - 1)
        
    }
    attributesCount--
}