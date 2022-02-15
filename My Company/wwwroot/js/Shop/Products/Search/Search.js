let filtersVisible = false
let defaultFilters = filters = {
    pageSize: 12,
    sortOrder: 0,
    page: 1,
    categoryId: getRouteValueAt(2) == -1 ? undefined : getRouteValueAt(2),
    priceFrom: undefined,
    priceTo: undefined,
    attributes: []
}

const STORAGE_FILTERS = 'storage_filters'

$(function () {

    filters = JSON.parse(sessionStorage.getItem(STORAGE_FILTERS))
    if (filters == null || filters.categoryId != defaultFilters.categoryId) {
        sessionStorage.removeItem(STORAGE_FILTERS)
        filters = defaultFilters
    }
    else {
        setInputs()
    }

    loadData()

    if (bootstrapDetectBreakpoint().index < 2)
        $('.filters').hide()

    $(window).resize(function () {
        if (bootstrapDetectBreakpoint().index < 2) {
            if (!filtersVisible)
                $('.filters').hide()
        }
        else
            $('.filters').show()
    })

    $('#showHideFiltersBtn').click(function () {
        filtersVisible = !filtersVisible
        $('.filters').toggle('slow')
        $(this).text(filtersVisible ? "Ukryj filtry" : "Pokaż filtry")
    })

    $('#priceFrom').on('input', function (e) {
        if (isNaN(e.target.value.replace(',', '.')))
            $(this).val('')
    })

    $('#priceTo').on('input', function (e) {
        if (isNaN(e.target.value.replace(',', '.')))
            $(this).val('')
    })

    $('#priceFrom').blur(function (e) {
        var newValue = e.target.value.replace('.', ',')
        if (filters.priceFrom == newValue)
            return
        filters.priceFrom = newValue
        filters.page = 1
        loadData()
    })

    $('#priceTo').blur(function (e) {
        var newValue = e.target.value.replace('.', ',')
        if (filters.priceTo == newValue)
            return
        filters.priceTo = newValue
        filters.page = 1
        loadData()
    })

    $('.checkBoxAttribute').change(function () {
        const id = $(this).data('id')
        const type = $(`.type[data-id="${id}"]`).val()
        const attribute = filters.attributes.find(a => a.id == id)
        if (attribute) {
            if ($(this).prop('checked'))
                attribute.value = "true"
            else
                attribute.value = "false"
        } else {
            filters.attributes.push({ id: id, type: type, value: $(this).val() })
        }
        loadData()
    })

    $('.dictionary-attribute').change(function () {
        const id = $(this).data('id')
        const type = $(`.type[data-id="${id}"]`).val()
        const attribute = filters.attributes.find(a => a.id == id)
        if (attribute) {
            if ($(this).prop('checked'))
                attribute.values.push($(this).val())
            else
                attribute.values = attribute.values.filter(val => val != $(this).val())
        } else {
            filters.attributes.push({ id: id, type: type, values: [$(this).val()] })
        }
        loadData()
    })

    $('.text-attribute').blur(function () {
        const id = $(this).data('id')
        const type = $(`.type[data-id="${id}"]`).val()
        const attribute = filters.attributes.find(a => a.id == id)
        const value = $(this).val()
        if (attribute) {
            if (attribute.value == value)
                return
            else if (!value)
                filters.attributes = filters.attributes.filter(a => a.id != attribute.id)
            else
                attribute.value = value
        } else if (value) {
            filters.attributes.push({ id: id, type: type, value: $(this).val() })
        }
        else
            return
        loadData()
    })

    $('.numericFromAttribute, .dateFromAttribute').blur(function () {
        const id = $(this).data('id')
        const type = $(`.type[data-id="${id}"]`).val()
        const attribute = filters.attributes.find(a => a.id == id)
        const value = $(this).val()
        if (attribute) {
            if (attribute.valueFrom == value)
                return
            else if (!value)
                filters.attributes = filters.attributes.filter(a => a.id != attribute.id)
            else
                attribute.valueFrom = value
        } else if (value) {
            filters.attributes.push({ id: id, type: type, valueFrom: $(this).val() })
        }
        else
            return
        loadData()
    })

    $('.numericToAttribute, .dateToAttribute').blur(function () {
        const id = $(this).data('id')
        const type = $(`.type[data-id="${id}"]`).val()
        const attribute = filters.attributes.find(a => a.id == id)
        const value = $(this).val()
        if (attribute) {
            if (attribute.valueTo == value)
                return
            else if (!value)
                filters.attributes = filters.attributes.filter(a => a.id != attribute.id)
            else
                attribute.valueTo = value
        } else if (value) {
            filters.attributes.push({ id: id, type: type, valueTo: $(this).val() })
        }
        else
            return
        loadData()
    })

    registerBtns()
    $(window).bind('beforeunload', function () {
        filters.page = 1
        sessionStorage.setItem(STORAGE_FILTERS, JSON.stringify(filters))
    });

    $('#resetFiltersBtn').click(function () {
        resetFilters()
        filters = defaultFilters 
        loadData()
    })
})

const loadData = function () {
    $('.spinner').removeClass("spinnerHidden")
    $('.overlay').css('display', 'initial')

    $.post('/Products/GetList',
        {
            ...filters
        })
        .done(function (data) {
            $('.spinner').addClass("spinnerHidden")
            $('.overlay').css('display', 'none')
            $("#products").html(data)
            registerBtns()
            $('#sortSelect').val(filters.sortOrder)
        })
}

const scrollToTop = () => {
    $('html, body').animate({
        scrollTop: $('body').offset().top
    }, 'fast');
}

const registerBtns = function () {

    $('.changePageSize').click(function (e) {
        filters.pageSize = e.target.value
        filters.page = 1
        scrollToTop()
        loadData()
    })

    $('.page-link').click(function (e) {
        e.preventDefault()
        const newValue = $(this).data('page')
        if (newValue === '+1')
            filters.page++
        else if (newValue == '-1')
            filters.page--
        else if (filters.page != newValue)
            filters.page = newValue
        else
            return
        scrollToTop()
        loadData()
    })

    $('#sortSelect').change(function () {
        filters.sortOrder = $(this).val()
        filters.page = 1
        scrollToTop()
        loadData()
    })
}

const setInputs = function () {
    $('#priceFrom').val(filters.priceFrom)
    $('#priceTo').val(filters.priceTo)
    filters.attributes.forEach(attr => {
        switch (attr.type) {
            case 'Bool':
                const input = $(`.checkBoxAttribute[data-id="${attr.id}"]`)
                if (attr.value == 'true') {
                    input.val('true')
                    input.prop('checked', true)
                }
                else
                    input.val('false')
                break
            case 'Numeric':
                $(`.numericFromAttribute[data-id="${attr.id}"]`).val(attr.valueFrom)
                $(`.numericToAttribute[data-id="${attr.id}"]`).val(attr.valueTo)
                break
            case 'Date':
                $(`.dateFromAttribute[data-id="${attr.id}"]`).val(attr.valueFrom)
                $(`.dateToAttribute[data-id="${attr.id}"]`).val(attr.valueTo)
                break
            case 'Text':
                $(`.text-attribute[data-id="${attr.id}"]`).val(attr.value)
                break
            case 'Dictionary':
                const inputs = $(`.dictionary-attribute[data-id="${attr.id}"]`).toArray()
                inputs.forEach(i => {
                    if (attr.values.find(v => v == $(i).val())) {
                        $(i).prop('checked', true)
                    }
                })
                break
        }
    })
}

const resetFilters = function () {
    $('#priceFrom').val('')
    $('#priceTo').val('')
    filters.attributes.forEach(attr => {
        switch (attr.type) {
            case 'Bool':
                const input = $(`.checkBoxAttribute[data-id="${attr.id}"]`)

                input.prop('checked', false)
                input.val('false')
                break
            case 'Numeric':
                $(`.numericFromAttribute[data-id="${attr.id}"]`).val('')
                $(`.numericToAttribute[data-id="${attr.id}"]`).val('')
                break
            case 'Date':
                $(`.dateFromAttribute[data-id="${attr.id}"]`).val('')
                $(`.dateToAttribute[data-id="${attr.id}"]`).val('')
                break
            case 'Text':
                $(`.text-attribute[data-id="${attr.id}"]`).val('')
                break
            case 'Dictionary':
                const inputs = $(`.dictionary-attribute[data-id="${attr.id}"]`).toArray()
                inputs.forEach(i => {
                    $(i).prop('checked', false)
                })
                break
        }
    })
}