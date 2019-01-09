$(document).ready(function () {

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
        })
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
        })
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
        })
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
        })
    });

    // pobieramy parametry dla kategorii
   $("#confirmCategory").on('click', 'tr', function () {
       var categoryId = $("#category5").val();
        alert(categoryId);
    });
    // pobieramy parametry dla kategorii

   

});