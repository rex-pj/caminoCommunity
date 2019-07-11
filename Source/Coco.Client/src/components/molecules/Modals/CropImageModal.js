import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelFooter, PanelBody } from "../../atoms/Panels";
import { Button, ButtonSecondary } from "../../atoms/Buttons";
import AvatarEditor from "react-avatar-editor";
import Slider, { Range } from "rc-slider";

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
        width: 200,
        height: 200,
        scale: 1,
        keepSelection: true
      }
    };

    this.setEditorRef = editor => (this.editor = editor);
  }

  onExecute = e => {
    if (this.editor) {
      const canvas = this.editor.getImage().toDataURL();

      const { src } = this.state;
      if (this.props.onExecute) {
        this.props.onExecute({
          event: e,
          croppedImageUrl: canvas,
          sourceImageUrl: src
        });
      }
    }
  };

  render() {
    const { crop, src } = this.state;
    return (
      <Fragment>
        <PanelBody>
          <ImageWrap>
            <AvatarEditor
              ref={this.setEditorRef}
              image={src}
              width={crop.width}
              height={crop.height}
              border={50}
              color={[255, 255, 255, 0.6]} // RGBA
              scale={crop.scale}
              rotate={0}
            />
          </ImageWrap>
          <Slider />
        </PanelBody>
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
