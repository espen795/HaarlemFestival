// Data uit de tabel filteren.
function FilterData() {
    var filters = [];


    // Voor elke filter
    $('.filters').each(function () {

        if ($(this).val() != "") // Als de filter is geselecteerd.
        {
            filters.push($(this).val()) // Filter toevoegen.
        }
    })


    // TEST: Alle Filters in de console neerzetten.
    console.log(filters);

    // Alle rows van de table vinden.
    var tr = $(".data").find("tr");
    tr.hide(); // Alle rows van de table verbergen.

    // Als er geen filter is geselecteerd.
    if (filters.length == 0) {
        tr.show(); // Alle rijen laten zien.
        return; // Uit de functie gaan.
    }

    // Elke rij filteren.
    tr.filter(function () {
        var $t = $(this);
        for (var index = 0; index < filters.length; ++index) {

            if ($t.is(":not(:contains('" + filters[index] + "'))")) // Als de rij niet voldoet aan de filters
            {
                return false; // Rij verbergen.
            }
        }

        return true; // Rij laten zien.
    })
        .show();
}