function GetCartProducts(rowData) {
    var html = "";
    var idR = rowData.id_cart;
    var shop = rowData.shop;
    var div = $('<div>')
        .addClass('loading')
        .append('Pobiera dane przyjacielu...');
    $.ajax({
        url: '/Orders/GetCartProducts',
        type: 'POST',
        data: {
            cart_id: idR,
            Shop_id: shop
        },
        dataType: 'json',
        success: function (json) {
            if (json !== '</table>') {

                div.text('');
                div.removeClass('loading');
                div.addClass('child-table');
                html += '<div class="col-lg-6"><h4>Produkty w koszyku</h4><table width="100%"><tr style="background-color:#efefef">';
                html += '<td><b>Produkt</b></td><td><b>Cena brutto</b></td><td><b>Ilość</b></td><td>ID produktu</td></tr>';
                html += json;
                html += '</table></div>';

                div.append(html);
            }
            else {
                div.text('');
                div.removeClass('loading');
                div.addClass('child-table');
                html += '<div class="col-lg-6"><h4>Produkty w koszyku</h4>';

                html += '<p style="text-align:center"><b>BRAK DANYCH</b></p>';
                html += '</div>';

                div.append(html);
            }
        }
    });

    // div.append('</div>');
    return div;
}

$(document).ready(function () {
    
    // Tabela z zamówieniami
    var tableB = $('#all_orders_sp24').DataTable({

        "destroy": true,
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "paging": true,
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
                }
            ],
        "columns": [
           
            {
                "render": function (data, type, full, meta) {
                    if (full.shop === "Sp2")
                        return "<img src='/images/small.jpg' width='30px' />";
                    else
                        return "<img src='/images/sp1.jpg' width='30px' />";
                }
            },
            { "data": "id_order", "name": "Id" },
            { "data": "created", "name": "name"},
            { "data": "payment", "name": "reference"},
            { "data": "reference", "name": "gross_price"},
            { "data": "total_shipping", "name": "lowest_price" },
            { "data": "total_paid", "name": "distributor_id" },
            { "data": "id_customer", "name": "Customer"},
            { "data": "id_address_d", "name": "Delivery Address"},
            { "data": "id_address_i", "name": "Invoice Address" },
            { "data": "additionalInfo", "name": "AdditionalInfo" },
            { "data": "current_state", "name": "Current State" },
            {
                "render": function (data, type, full, meta) {
                    if (full.shop === "Sp2")
                        return '<a target="_blank" class="btn btn-default" href="https://www.sprzegla24.pl/admin125fzmhfc/index.php?controller=AdminOrders&id_order=' + full.id_order + '&vieworder">Edytuj</a>';
                    else
                        return '<a target="_blank" class="btn btn-default" href="https://www.sprzeglo.com.pl/admin271tuwg4u/index.php?controller=AdminOrders&id_order=' + full.id_order + '&vieworder">Edytuj</a>';
                }
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
        "initComplete": function () {
            this.api().columns([2, 3]).every(function () {
                var column = this;
                var select = $('<select class="form-control"><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $(this).val();
                        column.search(this.value).draw();
                    });

                // Only contains the *visible* options from the first page
                console.log(column.data().unique());

                // If I add extra data in my JSON, how do I access it here besides column.data?
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
        
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
            row.child(GetCartProducts(row.data())).show();
            tr.addClass('shown');
        }
    });
    // click

    // filtry
    $("#submit_orders_filters").on('click', function () {
        event.preventDefault();
        
        $("#all_orders_sp24").DataTable().ajax.reload();
    });
    // filtry
 
});
