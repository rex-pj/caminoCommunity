import React from "react";
import { Editor } from "@tinymce/tinymce-react";
require.context(
  "file-loader?name=lib/cjs/main/ts/TinyMCE.js&context=node_modules/@tinymce/tinymce-react",
  true,
  /.*/
);

export default props => {
  return (
    <Editor
      {...props}
      init={{
        height: 500,
        menubar: false,
        plugins: [
          "advlist autolink lists link image charmap print preview anchor",
          "searchreplace visualblocks code fullscreen",
          "insertdatetime media table paste code help wordcount"
        ],
        toolbar: `$undo redo | formatselect | bold italic backcolor | 
              alignleft aligncenter alignright alignjustify | \
              bullist numlist outdent indent | removeformat | help`
      }}
      onChange={props.onChanged}
    />
  );
};
