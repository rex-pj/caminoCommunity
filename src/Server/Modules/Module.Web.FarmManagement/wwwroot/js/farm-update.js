$(function () {

    $('textarea#Description').tinymceEditor();

    $('#farmPictures').on('click', '.remove-picture', function () {
        $(this).parent('.picture-item').remove();
        rerenderPictureIndexes();
    });

    function rerenderPictureIndexes() {
        var pictureItems = $("#farmPictures").find('.picture-item');
        $.each(pictureItems, function (index, value) {
            var pictureItem = $(value);
            var hiddenPictures = pictureItem.find('input[type=hidden]');
            $.each(hiddenPictures, function (hIndex, hValue) {
                var inputSubfix = hValue.name.split('.')[1];
                hValue.name = 'Pictures[' + index + '].' + inputSubfix;
            });
        });
    }

    $('#pictureUpload').fileupload({
        dataType: "json",
        progress: function (e, data) {
            $('#uploadProgress').removeClass('d-none');
        },
        done: function (e, data) {
            $('#uploadProgress').addClass('d-none');
            var blobImage = base64toBlob(data.result.url, data.result.contentType);
            var blobUrl = URL.createObjectURL(blobImage);
            var farmPictures = $("#farmPictures");
            var pictureItems = farmPictures.find('.picture-item');
            var appendedIndex = pictureItems.length;

            var base64Data = 'data:' + data.result.contentType + ';base64,' + data.result.url;
            var appendedHtml = '<div class="col-6 col-sm-6 col-md-4 col-lg-3 picture-item">';
            appendedHtml += '<div class="form-group mb-3">';
            appendedHtml += '<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">';
            appendedHtml += '</div>';
            appendedHtml += '<input type="hidden" name="Pictures[' + appendedIndex + '].Base64Data" value="' + base64Data + '" />';
            appendedHtml += '<input type="hidden" name="Pictures[' + appendedIndex + '].FileName" value="' + data.result.name + '" />';
            appendedHtml += '<input type="hidden" name="Pictures[' + appendedIndex + '].ContentType" value="' + data.result.contentType + '" />';
            appendedHtml += '<span class="remove-picture">X</span>';
            appendedHtml += '</div>';
            farmPictures.append(appendedHtml);
        }
    });
    $(".select2-remote-ajax").select2Ajax();
});