$(function () {

    $('textarea#Description').tinymceEditor();

    $('#productThumbnails').on('click', '.remove-thumbnail', function () {
        $(this).parent('.thumbnail-item').remove();
        rerenderThumbnailIndexes();
    });

    function rerenderThumbnailIndexes() {
        var thumbnailItems = $("#productThumbnails").find('.thumbnail-item');
        $.each(thumbnailItems, function (index, value) {
            var thumbnailItem = $(value);
            var hiddenThumbnails = thumbnailItem.find('input[type=hidden]');
            $.each(hiddenThumbnails, function (hIndex, hValue) {
                var inputSubfix = hValue.name.split('.')[1];
                hValue.name = 'Thumbnails[' + index + '].' + inputSubfix;
            });
        });
    }

    $('#thumbnailUpload').fileupload({
        dataType: "json",
        progress: function (e, data) {
            $('#uploadProgress').removeClass('d-none');
        },
        done: function (e, data) {
            $('#uploadProgress').addClass('d-none');
            var blobImage = base64toBlob(data.result.url, data.result.contentType);
            var blobUrl = URL.createObjectURL(blobImage);
            var productThumbnails = $("#productThumbnails");
            var thumbnailItems = productThumbnails.find('.thumbnail-item');
            var appendedIndex = thumbnailItems.length;

            var base64Data = 'data:' + data.result.contentType + ';base64,' + data.result.url;
            var appendedHtml = '<div class="col-6 col-sm-6 col-md-4 col-lg-3 thumbnail-item">';
            appendedHtml += '<div class="form-group mb-3">';
            appendedHtml += '<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">';
            appendedHtml += '</div>';
            appendedHtml += '<input type="hidden" name="Thumbnails[' + appendedIndex + '].Base64Data" value="' + base64Data + '" />';
            appendedHtml += '<input type="hidden" name="Thumbnails[' + appendedIndex + '].FileName" value="' + data.result.name + '" />';
            appendedHtml += '<input type="hidden" name="Thumbnails[' + appendedIndex + '].ContentType" value="' + data.result.contentType + '" />';
            appendedHtml += '<span class="remove-thumbnail">X</span>';
            appendedHtml += '</div>';
            productThumbnails.append(appendedHtml);
        }
    });
    $(".select2-remote-ajax").select2Ajax();
});