

function CreateIncomeChart(id, jazzIncome, dinnerIncome, talkingIncome, historicIncome)
{
    var ctx = document.getElementById(id).getContext("2d");

    var myDoughnutChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Jazz@Patronaat", "Dinner in Haarlem", "Talking Haarlem", "Historic Haarlem"],
            datasets: [{
                label: 'Income per event.',
                data: [jazzIncome, dinnerIncome, talkingIncome, historicIncome],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)'
                ],
                borderWidth: 1
            }]
        }
    });
}

function CreateTicketAvailabilityChart(id, availableTickets, soldTickets) {
    var ctx = document.getElementById(id).getContext("2d");

    var myDoughnutChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Available Tickets", "Sold Tickets"],
            datasets: [{
                label: 'Available tickets (spread over all events)',
                data: [availableTickets, soldTickets],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)'
                ],
                borderWidth: 1
            }]
        }
    });
}