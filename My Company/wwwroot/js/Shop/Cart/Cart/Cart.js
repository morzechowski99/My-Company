/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
const COOKIE_KEY = 'cart'

var formatter = new Intl.NumberFormat('pl', {
    style: 'currency',
    currency: 'PLN',

});

$(function () {
    $('.deleteFromCart').click(function () {
        deleteFromCart($(this).data('id'))
    })

    $('.substractQuantity').click(function () {
        changeQuantity(-1, $(this).data('id'))
    })

    $('.addQuantity').click(function () {
        changeQuantity(1, $(this).data('id'))
    })

    $("#cartIcon").remove()
})

const deleteFromCart = function (id) {
    let cart = JSON.parse(Cookies.get(COOKIE_KEY))
    cart = cart.filter(i => i.Id != id)
    Cookies.set(COOKIE_KEY, JSON.stringify(cart), { expires: 365 })
    const cartValue = $('#total1').text()
    const cartValueDouble = convertCurrencyToFload(cartValue)
    const itemValue = $(`tr[data-id="${id}"]`).find('.price').text()
    const itemValueDouble = convertCurrencyToFload(itemValue)
    const newCartValue = toMoney(cartValueDouble - itemValueDouble)
    $('#total1').text(formatter.format(newCartValue))
    $('#total2').text(formatter.format(newCartValue))
    $(`tr[data-id="${id}"]`).remove()
    if ($('.itemDetails').toArray().length == 0) {
        $('#cardContainer').empty()
        $('#cardContainer').append(` <div class="text-center m-5">
            <h6>Twój koszyk jest pusty</h6>
        </div>`)
    }
}

const changeQuantity = function (value, id) {
    const quantity = $(`tr[data-id="${id}"]`).find('.quantityValue').first().text()
    if (value == -1 && quantity == 1)
        return;

    const newQuantity = parseInt(quantity) + value
    let cart = JSON.parse(Cookies.get(COOKIE_KEY))
    const item = cart.find(i => i.Id == id)
    if(item)
        item.Quantity = newQuantity
    Cookies.set(COOKIE_KEY, JSON.stringify(cart), { expires: 365 })
    const itemPrice = convertCurrencyToFload($(`tr[data-id="${id}"]`).find('.itemPrice').text())
    const cartValue = $('#total1').text()
    let cartValueDouble = convertCurrencyToFload(cartValue)
    const newValue = toMoney(itemPrice * newQuantity)
    $(`tr[data-id="${id}"]`).find('.price').text(formatter.format(newValue))
    cartValueDouble = toMoney(value * itemPrice + cartValueDouble)
    $('#total1').text(formatter.format(cartValueDouble))
    $('#total2').text(formatter.format(cartValueDouble))
    $(`tr[data-id="${id}"]`).find('.quantityValue').text(newQuantity)
}

const convertCurrencyToFload = function (currency) {
    return Number.parseFloat(currency.substring(0, currency.length - 2).replace(/\s/g, '').replace(',', '.'))
}

const toMoney = function (value) {
    return (Math.round(value * Math.pow(10, 2)) / Math.pow(10, 2)).toFixed(2)
}