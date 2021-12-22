$.validator.setDefaults({ ignore: null });

let lastCategoryId = null

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
            $("#nextBtn").html("Dalej")
        }
    }

    function nextPrev(n) {
        const x = $(".tab")
        const inputsToValidate = x.eq(currentTab).find('input, select, textarea')
        if (inputsToValidate.length != 0)
            if (!inputsToValidate.valid()) return;
        x.eq(currentTab).css("display", "none")
        if (currentTab == 0 && n > 0) {
            loadAttributesHtml(lastCategoryId)
        }
        currentTab = currentTab + n
        if (currentTab >= x.length) {
            $("#createForm").submit()
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

    $("#decription").jqte({
        link: false,
        source: false
    })

    $(".categorySelect").change(function () {
        const id = $(this).data("id");
        const selects = $(".categorySelectContainer").toArray()
        selects.forEach(select => {
            const $select = $(select)
            if ($select.data("id") > id)
                $select.remove();
        })
        lastCategoryId = $(this).val()
        loadSubCategories($(this).val(), id + 1)
    })

    let supplierSelect = $("#supplierSelect").selectize({
        valueField: "id",
        labelField: "description",
        searchField: "description",
        options: [],
        placeholder: "Zacznij pisać, aby wybrać",
        create: false,
        load: function (query, callback) {
            supplierSelect.clearOptions()
            if (query.length < 2) return callback();
            $.ajax({
                url: "/Warehouse/Products/GetSuppliersToSelect",
                type: "GET",
                data: {
                    query: query,
                },
                error: function () {
                    callback();
                },
                success: function (res) {
                    callback(res);
                },
            });
        },
    })
    supplierSelect = supplierSelect[0].selectize

    registerPhotoInputs()

    $('#addNextPhotoBtn').click(function () {
        const count = $('.photoItem').length
        console.log($('.photoItem'))
        const template = `<div class="col-12 photoItem d-flex justify-content-center " data-id="${count}">
                    <div class="form-group col-md-6">
                        <input type="file" data-id="${count}" class="form-control-file photoInput" data-val="true"
                        data-val-filesize="Maksymalny rozmiar pliku to 5 MB" data-val-filesize-size="5000000"
                        data-val-required="Wybierz plik" accept="image/*" id="Photos_${count}_" name="Photos[${count}]" data-id="${count}"/>
                        <span class="text-danger" data-valmsg-for="Photos[${count}]" data-valmsg-replace="true"></span>
                    </div>
             </div>`

        $(".buttonContainer").before(template)

        registerPhotoInputs()

        $("#createForm")
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        //Parse the form again
        $.validator
            .unobtrusive
            .parse("form");
    })



})

const registerPhotoInputs = function () {
    $('.photoInput').change(function (e) {
        e.stopImmediatePropagation();
        const [file] = this.files
        const id = $(this).data('id')

        $(`.photoItem[data-id='${id}'] .card`).remove()
        if (file) {
            const src = URL.createObjectURL(file)

            $(this).before(`<div class="card border-success mx-2" >
                <img class="card-img-top" src="${src}" alt="">
                <div class="card-body">
                    ${id == 0 ? '<h6>To będzie zdjęcie główne</h6>' : ""}
                   <button class="m-2 w-100 btn btn-outline-danger deleteBtn"  type="button" data-id="${id}">Usuń</button/>
                </div>
            </div>`)

            $('.deleteBtn').click(function (e) {
                e.stopImmediatePropagation();
                const id = $(this).data('id')
                $(`.photoItem[data-id='${id}']`).remove()

                $('.photoItem').toArray().forEach((elem) => {
                    const $elem = $(elem)
                    const elemId = $elem.data('id')
                    if (id == 0 && elemId == 1)
                        $elem.find('.card-body').prepend("<h6>To będzie zdjęcie główne</h6>")
                    if (elemId > id) {
                        $elem.find('input').attr('name', `Photos[${elemId - 1}]`);
                        $elem.data('id', elemId - 1)
                    }
                })
            })


        }
    })
}

const loadSubCategories = function (id, containerId) {
    if (!id)
        return

    const container = $(`<div class="form-group categorySelectContainer" data-id="${containerId}">   
    </div>`)
    const select = $(`<select id="Categories_${containerId}_" name="Categories[${containerId}]" class="form-control categorySelect" data-id="${containerId}"></select>`)
    select.change(function () {
        const id = $(this).data("id");
        const selects = $(".categorySelectContainer").toArray()
        selects.forEach(select => {
            const $select = $(select)
            if ($select.data("id") > id)
                $select.remove();
        })
        lastCategoryId = $(this).val()
        loadSubCategories($(this).val(), id + 1)
    })
    container.append(select)
    let subCategoriesExists = false
    $.get("/Warehouse/Categories/GetChildCategories/" + id)
        .done(function (data) {

            if (data.length) {
                subCategoriesExists = true
                select.append(`<option value="${0}">Wybierz</option>`)
                data.forEach(category => {
                    select.append(`<option value="${category.id}">${category.categoryName}</option>`)
                })
            }
            if (subCategoriesExists)
                $("#categorySelects").append(container)
        })



}

const loadAttributesHtml = function (id) {
    $('#attributesTab').load('/Warehouse/Products/GetAttributesViewComponent/' + lastCategoryId,
        null,
        function (responseText, textStatus, req) {
            if (textStatus == "error") {
                $('#attributesTab').html('<h4 class="text-danger">Podczas pobierania danych z serwera wystąpił błąd. Prosimy przeładować stronę</h4>')
            }
        })

}