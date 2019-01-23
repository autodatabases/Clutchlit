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
                            var progress = Math.round((evt.loaded / evt.total) * 100);
                            $('#progress').append(progress);
                        }
                    },
                    false);
                return xhr;
            }
        }).done(function (data) {
            alert("Uploading is done");
            var array = data.split(';');
            var string_to_put = "A";

            for (i = 0; i < array.length; ++i) {
                string_to_put += '<img src="'+array[i]+'" width="100px" />';
            }

            $('#progress').html('');
            $('#progress').html(string_to_put);

            $("#imageUploadForm").val();
        }).fail(function (jqXhr, textStatus, errorThrown) {
            if (errorThrown === "abort") {
                alert("Uploading was aborted");
            } else {
                alert("Uploading failed");
            }
        }).always(function (data, textStatus, jqXhr) { });


    });
});