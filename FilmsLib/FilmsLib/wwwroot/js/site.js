// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.data-table').DataTable({
        dom: 'Bfrtip',
        responsive: true,
        searching: true,
        scrollY: false,

        buttons: [
            
        ],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.20/i18n/Ukrainian.json"
        },
    });

    $(".rateYo").rateYo({
        numStars: 10,
        maxValue: 10,
        fullStar: true,
        onChange: function (rating, rateYouInstance) {
            $("#Mark").val(rating);
            console.log($("#Mark"));
        }
    });

});
