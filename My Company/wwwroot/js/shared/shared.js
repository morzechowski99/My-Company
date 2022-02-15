/*Program powstał na Wydziale Informatyki Politechniki Białostockiej*/
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


//source: https://github.com/shaack/bootstrap-detect-breakpoint
const bootstrapDetectBreakpoint = function () {
    // cache some values on first call
    if (!this.breakpointValues) {
        this.breakpointNames = ["xl", "lg", "md", "sm", "xs"]
        this.breakpointValues = []
        for (const breakpointName of this.breakpointNames) {
            this.breakpointValues[breakpointName] = window.getComputedStyle(document.documentElement).getPropertyValue('--breakpoint-' + breakpointName)
        }
    }
    let i = this.breakpointNames.length
    for (const breakpointName of this.breakpointNames) {
        i--
        if (window.matchMedia("(min-width: " + this.breakpointValues[breakpointName] + ")").matches) {
            return { name: breakpointName, index: i }
        }
    }
    return null
}

const getRouteValueAt = (idx) => {

    const routeTokens = location.pathname.replace(/^\/+/g, '').split('/')

    return routeTokens.length > idx ? routeTokens[idx] : undefined
}