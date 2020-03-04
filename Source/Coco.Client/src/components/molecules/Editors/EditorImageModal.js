import React, { Fragment } from "react";
import Tabs from "../Tabs/Tabs";
import EditorImageUploader from "./EditorImageUploader";
import EditorImageLink from "./EditorImageLink";

export default props => {
  const onClose = () => {
    props.onClose();
  };

  const onUploadImage = () => {};

  const onAddImageLink = () => {};

  return (
    <Fragment>
      <Tabs
        tabs={[
          {
            title: "Upload",
            tabComponent: () => (
              <EditorImageUploader
                onAddImage={onUploadImage}
                onClose={onClose}
              />
            )
          },
          {
            title: "Đường dẫn",
            tabComponent: () => (
              <EditorImageLink onAddImage={onAddImageLink} onClose={onClose} />
            )
          }
        ]}
      />
    </Fragment>
  );
};
