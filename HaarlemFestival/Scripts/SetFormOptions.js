function SetFormOptions() {
    // Ervoor zorgen dat de form niet de actie uitvoerd.
    $("form").on("submit", function (form) {
        form.preventDefault();
    });

    // Als een form gesubmit wordt
    $("form").submit(function () {
        SubmitForm(this);
    });
}