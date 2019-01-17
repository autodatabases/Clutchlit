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

    // wysyłamy zdjęcia na serwer
    $("#confirmPhotos").on('click', function (e) {
        
            var formData = new FormData();
            var totalFiles = $("#imageUploadForm").files.length;
            for (var i = 0; i < totalFiles; i++) {
                var file = $("#imageUploadForm").files[i];
                formData.append("imageUploadForm", file);
            }
            $.ajax({
                type: "POST",
                url: '/AllegroAuctions/UploadPhotos',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    alert('succes!!');
                },
                //error: function(error) {
                //    alert("errror");
                //}
            });   
    });
    

});