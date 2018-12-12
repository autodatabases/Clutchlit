$(document).ready(function () {

    // Tabela z zamówieniami
    var tableB = $('#all_orders_sp24').DataTable({

        "destroy": true,
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Orders/GetAllSp24",
            "type": "POST",
            "datatype": "json",
            "contentType": "application/x-www-form-urlencoded; charset=UTF-8",
            "data": ""
        },
        "columnDefs":
            [{
                "targets": [0],
                "visible": true,
                "searchable": false
            },
                {
                    "targets": [1],
                    "visible": false
                },
                {
                    "targets": [10],
                    "visible": false
                }
            ],
        "columns": [
            { "data": "shop", "name":"Shop"},
            { "data": "id_order", "name": "Id" },
            { "data": "created", "name": "name"},
            { "data": "payment", "name": "reference"},
            { "data": "reference", "name": "gross_price"},
            { "data": "total_shipping", "name": "lowest_price" },
            { "data": "total_paid", "name": "distributor_id" },
            { "data": "id_customer", "name": "markup"},
            { "data": "id_address_d", "name": "markup"},
            { "data": "id_address_i", "name": "markup" },
            { "data": "additionalInfo", "name": "markup" },
            { "data": "current_state", "name": "Current State" },
            {
                "render": function (data, type, full, meta) { return '<a target="_blank" class="btn btn-default" href="https://www.sprzegla24.pl/admin125fzmhfc/index.php?controller=AdminOrders&id_order=4341&vieworder' + full.id_order + '">Edytuj</a>'; }
            }
        ],
        "order": [[0, 'desc']],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData["current_state"] === "Zamówienie wysłane") {
                $('td', nRow).css('background-color', '#abd9ae');
            }
            else if (aData["current_state"] === "Wymagana dodatkowa weryfikacja zamówienia") {
                $('td', nRow).css('background-color', '#ffa0b4');
            }
            else if (aData["current_state"] === "Zamówienie w realizacji") {
                $('td', nRow).css('background-color', '#ffb140');
            }
            else if (aData["current_state"] === "Oczekiwanie na płatność") {
                $('td', nRow).css('background-color', '#00ffff');
            }
            else if (aData["current_state"] === "Anulowane") {
                $('td', nRow).css('background-color', '#dc143c');
            }
            else {
                $('td', nRow).css('background-color', '#fdff50');
            }
        },
        
    });


    $("#all_orders_sp24 tbody").on('click', 'tr', function () {
        //alert("dd");
        var tr = $(this).closest('tr');

        var row = tableB.row(tr);
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

 
});
