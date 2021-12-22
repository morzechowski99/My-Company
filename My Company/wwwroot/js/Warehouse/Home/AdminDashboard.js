const months = ['styczeń', 'luty', 'marzec', 'kwiecień', 'maj', 'czerwiec', 'lipiec', 'sierpień', 'wrzesień', 'październik', 'listopad', 'grudzień']
const days = ['niedziela', 'poniedziałek', 'wtorek', 'środa', 'czwartek', 'piątek', 'sobota']
const modes = {
    orders: 0,
    completions: 1,
    packing: 2
}
const ranges = {
    week: 0,
    month: 1,
    year: 2
}
const label1 = ['Zamówienia','Kompletowania','Pakowania']
const label2 = ['Tydzień','Miesiące','Lata']

let selectedMode = modes.orders
let selectedRange = ranges.week
let chart = null;



$(function () {
    createChart()


    $('#rangeSelect').change(function () {
        const val = $(this).val()
        selectedRange = parseInt(val)
        createChart()
    })

    $('#modeSelect').change(function () {
        const val = $(this).val()
        selectedMode = parseInt(val)
        createChart()
    })

})



const createChart = () => {
    $('.spinner').removeClass('spinnerHidden')
    if (chart)
        chart.destroy()
    const ctx = document.getElementById('chart').getContext('2d');

    $.get(`/Warehouse/Home/GetChartData?mode=${selectedMode}&range=${selectedRange}`)
        .always(function () {
            $('.spinner').addClass('spinnerHidden')
        })
        .done(function (data) {
            chart = new Chart(ctx, {
                type: 'bar',
               
                data: {
                    labels: getLabels(data.map(i => i.index)),
                    datasets: [{
                        label: `${label1[selectedMode]} / ${label2[selectedRange]}`,
                        data: data.map(i => i.value),
                        backgroundColor: 'rgba(2, 117, 216, 0.7)',
                        borderColor: 'rgba(2, 117, 216, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                stepSize: 1,
                            }
                        },
                    },
                }
            });
        })
}

const getLabels = (indexes) => {
    switch (selectedRange) {
        case ranges.week:
            return indexes.map(i => days[i % 7])
        case ranges.month:
            return indexes.map(i => months[i-1])
        default:
            return indexes
    }
}