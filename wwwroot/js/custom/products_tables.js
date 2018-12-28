function getOpponents(rowData) {
    var html = "";
    // html += '<div class="col-lg-6"><h4>Ceny konkurencji</h4><table width="100%"><tr style="background-color:#efefef">';
    // html += '<td><b>Konkurencja</b></td><td><b>Cena brutto</b></td><td><b>Stan</b></td></tr>';
    // html += '';
    // html += '</table></div>';
    var div = $('div.child-table')
    $.ajax({
        url: '/Products/GetDistributorsPrices',
        type: 'POST',
        data: {
            id: rowData.id
        },
        dataType: 'json',
        beforeSend: function () {
            $('.images-loader').show();
        },
        complete: function () {
            $('.images-loader').hide();
        },
        success: function (json) {
            if (json != '</table>') {

                html += '<div class="col-lg-6"><h4>Ceny dostawców</h4><table width="100%"><tr style="background-color:#efefef">';
                html += '<td><b>Dostawca</b></td><td><b>Magazyn</b></td><td><b>Cena brutto</b></td><td><b>Stan</b></td></tr>';
                html += json;
                html += '</table></div>';

                div.append(html);
            }
            else {
                html += '<div class="col-lg-6"><h4>Ceny dostawców</h4>';

                html += '<p style="text-align:center"><b>BRAK DANYCH</b></p>';
                html += '</div>';

                div.append(html);
            }

        }
    });
}
function format(rowData) {
    var html = "";
    var idR = rowData.id;
    var div = $('<div>')
        .addClass('loading')
        .append('Pobiera dane przyjacielu...');
    $.ajax({
        url: '/Products/GetDistributorsPrices',
        type: 'POST',
        data: {
            id: idR
        },
        dataType: 'json',
        success: function (json) {
            if (json !== '</table>') {

                div.text('');
                div.removeClass('loading');
                div.addClass('child-table');
                html += '<div class="col-lg-6"><h4>Ceny dostawców</h4><table width="100%"><tr style="background-color:#efefef">';
                html += '<td><b>Dostawca</b></td><td><b>Magazyn</b></td><td><b>Cena brutto</b></td><td><b>Stan</b></td></tr>';
                html += json;
                html += '</table></div>';

                div.append(html);
            }
            else {
                div.text('');
                div.removeClass('loading');
                div.addClass('child-table');
                html += '<div class="col-lg-6"><h4>Ceny dostawców</h4>';

                html += '<p style="text-align:center"><b>BRAK DANYCH</b></p>';
                html += '</div>';

                div.append(html);
            }
            $.ajax({
                type: 'POST',
                url: '/Products/GetOpponentsPrices/' + idR + '/',
                data: {
                    id: idR
                },
                contentType: 'application/json',
                beforeSend: function () {
                    $('.images-loader').show();
                },
                complete: function () {
                    $('.images-loader').hide();
                },
                success: function (data) {
                    console.log(data);
                    html = "";
                    if (data != '</table>') {
                        html += '<div class="col-lg-6"><h4>Ceny konkurencji</h4><table width="100%"><tr style="background-color:#efefef">';
                        html += '<td><b>Konkurencja</b></td><td><b>Cena brutto</b></td><td><b>Stan</b></td></tr>';
                        html += data;
                        html += '</table></div>';
                    }
                    else {
                        html += '<div class="col-lg-6"><h4>Ceny konkurencji</h4>';
                        html += '<p style="text-align:center"><b>BRAK DANYCH</b></p>';
                        html += '</div>';
                    }
                    div.append(html);
                }
            });

        }
    });

    // div.append('</div>');
    return div;
}
$(document).ready(function () {

    var tableA = $('#products_list').DataTable({
        "processing": false, // for show progress bar
        "paging": false,
        "serverSide": false, // for process server side
        "retrieve": false,
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "columnDefs":
            [{
                "targets": [0],
                "visible": true,
                "searchable": false
            }],
        "columns": [
            { "data": "id", "name": "Id", "width": "10%" },
            { "data": "name", "name": "name", "width": "32%" },
            { "data": "reference", "name": "reference", "width": "15%" },
            { "data": "gross_price", "name": "gross_price", "width": "9%" },
            { "data": "lowestPrice", "name": "lowest_price", "width": "9%" },
            { "data": "distributorId", "name": "distributor_id", "width": "15%", "className": "distributors" },
            { "data": "markup", "name": "markup", "width": "10%" },
        ],
        "order": [[1, 'asc']],

    });

    $("#products_list tbody").on('click', 'tr', function () {
        //alert("dd");
        var tr = $(this).closest('tr');

        var row = tableA.row(tr);
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            //alert("da");
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
                // click

    // obsługujemy koszyk - ciasteczka


    // koszyk - ciasteczka
});