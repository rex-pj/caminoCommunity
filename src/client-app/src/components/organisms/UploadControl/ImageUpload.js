import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { base64toBlob } from "../../../utils/Helper";

const InputFile = styled.input.attrs((p) => ({ type: "file" }))`
  display: none;
`;

const UploadButton = styled.span`
  display: block;

  svg,
  path {
    color: inherit;
  }

  > span {
    color: inherit;
    margin-left: ${(p) => p.theme.size.exTiny};
  }
`;

const ImageUpload = (props) => {
  const fileRef = React.createRef();

  const handleImageChange = (e) => {
    e.preventDefault();

    const { target } = e;
    if (target && target.files && target.files.length > 0) {
      const file = target.files[0];

      const reader = new FileReader();
      reader.onloadend = () => {
        if (props.onChange) {
          const blobImage = base64toBlob(reader.result, file.type);
          const blobUrl = URL.createObjectURL(blobImage);

          props.onChange({
            target: target,
            file,
            preview: reader.result,
            blobUrl: blobUrl,
          });
        }
      };

      reader.readAsDataURL(file);
    }
  };

  const openFileUpload = (e) => {
    if (fileRef && fileRef.current) {
      fileRef.current.click();
    }
  };

  return (
    <div className={props.className}>
      <UploadButton onClick={openFileUpload}>
        <FontAwesomeIcon icon="camera" />
        {props.children ? <span>{props.children}</span> : null}
      </UploadButton>
      <InputFile
        name={props.name}
        ref={fileRef}
        type="file"
        accept=".jpg,.jpeg,.png"
        onChange={(e) => handleImageChange(e)}
      />
    </div>
  );
};

export default ImageUpload;
