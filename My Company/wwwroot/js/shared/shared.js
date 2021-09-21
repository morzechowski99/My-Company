function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function showAlert() {
    $('.alert').addClass('show')
    $('.alert').removeClass('hide')
    setTimeout(function () {
        $('.alert').addClass('hide')
        $('.alert').removeClass('show')
    }, 5000)
}

function setResetModalOnClose() {
    $(".modal").on("hidden.bs.modal", function () {
        $(".modal input").val('');
        $(".modal .validationMessage").text('');
    });
}

function registerTooltips() {
    $('[data-toggle="tooltip"]').tooltip()
}