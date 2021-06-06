$(function () {

    $('textarea#Description').tinymceEditor();

    $('#productPictures').on('click', '.remove-picture', function () {
        $(this).parent('.picture-item').remove();
        rerenderPicturesIndexes();
    });

    function rerenderPicturesIndexes() {
        var pictureItems = $("#productPictures").find('.picture-item');
        $.each(pictureItems, function (index, value) {
            var pictureItem = $(value);
            var hiddenPicturess = pictureItem.find('input[type=hidden]');
            $.each(hiddenPicturess, function (hIndex, hValue) {
                var inputSubfix = hValue.name.split('.')[1];
                hValue.name = 'Picturess[' + index + '].' + inputSubfix;
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
            var productPicturess = $("#productPicturess");
            var pictureItems = productPicturess.find('.picture-item');
            var appendedIndex = pictureItems.length;

            var base64Data = 'data:' + data.result.contentType + ';base64,' + data.result.url;
            var appendedHtml = '<div class="col-6 col-sm-6 col-md-4 col-lg-3 picture-item">';
            appendedHtml += '<div class="mb-3">';
            appendedHtml += '<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">';
            appendedHtml += '</div>';
            appendedHtml += '<input type="hidden" name="Picturess[' + appendedIndex + '].Base64Data" value="' + base64Data + '" />';
            appendedHtml += '<input type="hidden" name="Picturess[' + appendedIndex + '].FileName" value="' + data.result.name + '" />';
            appendedHtml += '<input type="hidden" name="Picturess[' + appendedIndex + '].ContentType" value="' + data.result.contentType + '" />';
            appendedHtml += '<span class="remove-picture">X</span>';
            appendedHtml += '</div>';
            productPicturess.append(appendedHtml);
        }
    });
    $(".select2-remote-ajax").select2Ajax();
});