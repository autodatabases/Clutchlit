function CheckCategory() {
    return $("#allegro_category").val();
}
function CheckManufacturer() {
    return $("#allegro_manufacturer").val();
}
$(document).ready(function () {

    // massive action page
    var table = $('#massiveAction').DataTable({
        "destroy": true,
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/AllegroAuctions/GetAllegroAuctionsList",
            "type": "POST",
            "datatype": "json",
            "contentType": "application/x-www-form-urlencoded; charset=UTF-8",
            "data": function (d) {
                d.FlagCategory = CheckCategory();
                d.FlagManufacturer = CheckManufacturer();
            }
        },
        "columnDefs":
            [{
                "targets": [0],
                "visible": true,
                "searchable": false,
                "checkboxes": {
                    "selectRow": true
                }
            }],
        "select": {
            "style": "multi"
        },
        "columns": [
            { "data": "auctionId", "name": "Select" },
            { "data": "auctionId", "name": "AuctionId" },
            { "data": "allegroId", "name": "AllegroId" },
            { "data": "productId", "name": "ProductId" },
            { "data": "auctionTitle", "name": "AuctionTitle" },
            { "data": "category", "name": "Category" },
            { "data": "status", "name": "Status" }
        ],
        "order": [[1, 'asc']],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData["status"] === true) {
                $('td', nRow).css('background-color', '#93db57');
            }
            else {
                $('td', nRow).css('background-color', '#b23131');
            }
        }
    });


    $('#create_selected').click(function (event) {

        if (confirm("Czy jesteś pewien, że chcesz wystawić zaznaczone aukcje?")) {
            var idR;
            var rows_selected = table.column(0).checkboxes.selected();
            $.each(rows_selected, function (index, rowId) {
                idR = rowId;
                $.ajax({
                    type: "PUT",
                    url: "/AllegroAuctions/PostDraftAuction/" + idR + "/", 
                    data: { "id": idR },
                    contentType: "application/json",
                    dataType: "json",
                    success: function (msg) {
                        alert(msg);
                    },
                    error: function (msg) {
                        alert("Coś poszło nie tak. Aukcja ID: " + idR + " nie została wystawiona :(");
                    }
                });
            });
        }
    });
    // massive action page

    $("#confirmCategory").hide();
    $("#maincategories").change(function () {
        $('#category2').find('option').remove().end().append('<option  selected=""> - Wybierz kateogrię - </option>').val('kategoria');
        var url = '/AllegroAuctions/GetChildCategories';
        var ddlsource = "#mark";

        $.getJSON(url, { parent_id: $('#maincategories').val() }, function (data) {
            var items = ' ';
            $("#category2").empty();
            $.each(data, function (i, row) {
                items += "<option value='" + row.value + "'>" + row.text + "</option>";
            });
            $('#category2').html(items);
        });
    });

    $("#category2").change(function () {
        $('#category3').find('option').remove().end().append('<option  selected=""> - Wybierz kateogrię - </option>').val('kategoria');
        var url = '/AllegroAuctions/GetChildCategories';
        var ddlsource = "#mark";

        $.getJSON(url, { parent_id: $('#category2').val() }, function (data) {
            var items = ' ';
            $("#category3").empty();
            $.each(data, function (i, row) {
                items += "<option value='" + row.value + "'>" + row.text + "</option>";
            });
            $('#category3').html(items);
        });
    });

    $("#category3").change(function () {
        $('#category4').find('option').remove().end().append('<option  selected=""> - Wybierz kateogrię - </option>').val('kategoria');
        var url = '/AllegroAuctions/GetChildCategories';
        var ddlsource = "#mark";

        $.getJSON(url, { parent_id: $('#category3').val() }, function (data) {
            var items = ' ';
            $("#category4").empty();
            $.each(data, function (i, row) {
                items += "<option value='" + row.value + "'>" + row.text + "</option>";
            });
            $('#category4').html(items);
        });
    });

    $("#category4").change(function () {
        $('#category5').find('option').remove().end().append('<option  selected=""> - Wybierz kateogrię - </option>').val('kategoria');
        $('#confirmCategory').show();
        var url = '/AllegroAuctions/GetChildCategories';
        var ddlsource = "#mark";

        $.getJSON(url, { parent_id: $('#category4').val() }, function (data) {
            var items = ' ';
            $("#category5").empty();
            $.each(data, function (i, row) {
                items += "<option value='" + row.value + "'>" + row.text + "</option>";
            });
            $('#category5').html(items);
        });
    });

    // pobieramy parametry dla kategorii
    $("#confirmCategory").on('click', function (e) {
        e.preventDefault();
        var categoryId = $("#category5").val();
        $.ajax({
            type: 'POST',
            url: '/AllegroAuctions/GetParametersForCategory',
            data: {
                catId: categoryId
            },
            dataType: 'html',
            beforeSend: function () {
                $('.images-loader').show();
            },
            complete: function () {
                $('.images-loader').hide();
            },
            success: function (data) {
                console.log(data);
                $('#offer_parameters').empty();
                $('#offer_parameters').html(data);
            }
        });
    });
    // pobieramy parametry dla kategorii
    // filtry
    $("#submit_allegro_filters").on('click', function (event) {
        event.preventDefault();
        $("#massiveAction").DataTable().ajax.reload();
    });
    // filtry
    // wysyłamy zdjęcia na serwer
    $("#confirmPhotos").on('click', function (e) {
        e.preventDefault();
        var fileupload = $("#imageUploadForm").get(0);
        var files = fileupload.files;
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append("files", files[i]);
        }

        $.ajax({
            type: "POST",
            url: "/AllegroAuctions/UploadPhotos",
            contentType: false,
            processData: false,
            cache: false,
            async: false,
            data: data,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress",
                    function (evt) {
                        if (evt.lengthComputable) {
                            var progress = Math.round(evt.loaded / evt.total * 100);
                            $('#progress').append(progress);
                            $('.images-loader').show();
                        }
                    },
                    false);
                return xhr;
            }
        }).done(function (data) {
            $('.images-loader').hide();
            alert("Dodano pomyślnie!");
            var array = data.split(';');
            var string_to_put = "";

            for (i = 0; i < array.length; ++i) {
                string_to_put += '<span class="allegro_img col-md-3"><img src="' + array[i] + '" width="100%" /></span>';
            }

            $('#progress').html('');
            $('#progress').html(string_to_put);

            $("#imageUploadForm").val();
        }).fail(function (jqXhr, textStatus, errorThrown) {
            if (errorThrown === "abort") {
                alert("Uploading was aborted");
            } else {
                alert("Błąd! Zdjęcia ma niepoprawne rozszerzenie lub wielkość");
            }
        }).always(function (data, textStatus, jqXhr) { });


    });
});