import React, { Component } from "react";
import styled from "styled-components";
import { ButtonTransparent } from "../../atoms/Buttons";
import DefaultModal from "../../atoms/Modals/DefaultModal";

const InputFile = styled.input.attrs(p => ({ type: "file" }))`
  display: none;
`;

const UploadButton = styled(ButtonTransparent)``;

class ImageUpload extends Component {
  constructor(props) {
    super(props);
    this.state = { file: "", imagePreviewUrl: "" };

    this.fileRef = React.createRef();
  }

  handleImageChange = e => {
    e.preventDefault();

    let reader = new FileReader();
    let file = e.target.files[0];

    reader.onloadend = () => {
      this.setState({
        file: file,
        imagePreviewUrl: reader.result
      });
    };

    reader.readAsDataURL(file);
  };

  openFileUpload = e => {
    if (this.fileRef && this.fileRef.current) {
      this.fileRef.current.click();
    }
  };

  render() {
    let { imagePreviewUrl } = this.state;
    let imagePreview = null;
    if (imagePreviewUrl) {
      imagePreview = <img alt="" src={imagePreviewUrl} />;
    }

    return (
      <div className={this.props.className}>
        <UploadButton onClick={this.openFileUpload}>Click</UploadButton>
        <InputFile
          ref={this.fileRef}
          type="file"
          onChange={e => this.handleImageChange(e)}
        />
        <DefaultModal>{imagePreview}</DefaultModal>
      </div>
    );
  }
}

export default ImageUpload;
