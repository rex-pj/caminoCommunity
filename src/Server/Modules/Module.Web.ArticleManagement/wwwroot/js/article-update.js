$(function () {

    $('textarea#Content').tinymceEditor();

    $('#pictureUpload').fileupload({
        dataType: "json",
        progress: function (e, data) {
            $('#uploadProgress').removeClass('d-none');
        },
        done: function (e, data) {
            $('#uploadProgress').addClass('d-none');
            var blobImage = base64toBlob(data.result.url, data.result.contentType);
            var blobUrl = URL.createObjectURL(blobImage);
            $('#picture').val(data.result.url);
            $('#pictureFileType').val(data.result.contentType);
            $('#pictureFileName').val(data.result.name);
            $('#articlePicture').html('<img src="' + blobUrl + '" alt="' + data.result.name + '" class="img-thumbnail">');
        }
    });
    $(".select2-remote-ajax").select2Ajax();
});