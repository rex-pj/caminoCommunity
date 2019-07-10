import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelFooter } from "../Panels";
import { Button, ButtonSecondary } from "../Buttons";
import ReactCrop from "react-image-crop";

const ImageWrap = styled.div`
  text-align: center;
  height: 80%;

  .ReactCrop {
    max-height: 450px;
  }

  img {
    max-width: 100%;
    height: 100%;
  }
`;

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      src: props.children,
      crop: {
        unit: "%",
        width: 30,
        height: 30,
        aspect: 50 / 50,
        keepSelection: true
      }
    };
  }

  onImageLoaded = image => {
    this.imageRef = image;
  };

  onCropComplete = crop => {
    this.makeClientCrop(crop);
  };

  onCropChange = (crop, percentCrop) => {
    this.setState({ crop });
  };

  async makeClientCrop(crop) {
    if (this.imageRef && crop.width && crop.height) {
      await this.getCroppedImg(this.imageRef, crop, "avatar.jpeg").then(
        croppedImageUrl => {
          this.setState({ croppedImageUrl });
        }
      );
    }
  }

  getCroppedImg(image, crop, fileName) {
    const scaleX = image.naturalWidth / image.width;
    const scaleY = image.naturalHeight / image.height;

    const canvas = document.createElement("canvas");
    canvas.width = crop.width;
    canvas.height = crop.height;
    const ctx = canvas.getContext("2d");

    ctx.drawImage(
      image,
      crop.x * scaleX,
      crop.y * scaleY,
      crop.width * scaleX,
      crop.height * scaleY,
      0,
      0,
      crop.width,
      crop.height
    );

    return new Promise((resolve, reject) => {
      canvas.toBlob(blob => {
        if (!blob) {
          console.error("Canvas is empty");
          return;
        }
        blob.name = fileName;
        window.URL.revokeObjectURL(this.fileUrl);
        this.fileUrl = window.URL.createObjectURL(blob);
        resolve(this.fileUrl);
      }, "image/jpeg");
    });
  }

  onExecute = e => {
    const { croppedImageUrl, src } = this.state;
    if (this.props.onExecute) {
      this.props.onExecute({
        event: e,
        croppedImageUrl: croppedImageUrl,
        sourceImageUrl: src
      });
    }
  };

  render() {
    const { crop, src } = this.state;
    return (
      <Fragment>
        <ImageWrap>
          <ReactCrop
            src={src}
            crop={crop}
            onImageLoaded={this.onImageLoaded}
            onComplete={this.onCropComplete}
            onChange={this.onCropChange}
          />
        </ImageWrap>
        <PanelFooter>
          <Button size="sm" onClick={this.onExecute}>
            Upload
          </Button>
          <ButtonSecondary size="sm" onClick={() => this.props.closeModal()}>
            Hủy
          </ButtonSecondary>
        </PanelFooter>
      </Fragment>
    );
  }
}
