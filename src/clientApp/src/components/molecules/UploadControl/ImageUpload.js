import React, { Component } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

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

class ImageUpload extends Component {
  constructor() {
    super();

    this.fileRef = React.createRef();
  }

  handleImageChange = (e) => {
    e.preventDefault();

    const { target } = e;
    if (target && target.files && target.files.length > 0) {
      const file = target.files[0];

      const reader = new FileReader();
      reader.onloadend = () => {
        if (this.props.onChange) {
          this.props.onChange({
            target: target,
            file,
            preview: reader.result,
          });
        }
      };

      reader.readAsDataURL(file);
    }
  };

  openFileUpload = (e) => {
    if (this.fileRef && this.fileRef.current) {
      this.fileRef.current.click();
    }
  };

  render() {
    return (
      <div className={this.props.className}>
        <UploadButton onClick={this.openFileUpload}>
          <FontAwesomeIcon icon="camera" />
          <span>{this.props.children}</span>
        </UploadButton>
        <InputFile
          name={this.props.name}
          ref={this.fileRef}
          type="file"
          accept=".jpg,.jpeg,.png"
          onChange={(e) => this.handleImageChange(e)}
        />
      </div>
    );
  }
}

export default ImageUpload;
