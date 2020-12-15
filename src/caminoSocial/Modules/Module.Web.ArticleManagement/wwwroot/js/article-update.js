$(function () {

    $('textarea#Content').tinymceEditor();

    $('#thumbnailUpload').fileupload({
        dataType: "json",
        progress: function (e, data) {
            $('#uploadProgress').removeClass('d-none');
        },
        done: function (e, data) {
            $('#uploadProgress').addClass('d-none');
            var blobImage = base64toBlob(data.result.url, data.result.contentType);
            var blobUrl = URL.createObjectURL(blobImage);
            $('#Thumbnail').val(data.result.url);
            $('#ThumbnailFileType').val(data.result.contentType);
            $('#ThumbnailFileName').val(data.result.name);
            $('#articleThumbnail').html('<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">');
        }
    });
    $(".select2-remote-ajax").select2Ajax();
});