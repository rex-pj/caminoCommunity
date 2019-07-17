import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { PanelFooter, PanelBody } from "../../atoms/Panels";
import { Button, ButtonSecondary } from "../../atoms/Buttons";
import AvatarEditor from "react-avatar-editor";
import Slider from "rc-slider";

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

const SliderWrap = styled.div`
  max-width: 300px;
  margin: 0 auto;
`;

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      src: props.children,
      crop: {
        width: 250,
        height: 250,
        scale: 1
      }
    };

    this.setEditorRef = editor => (this.editor = editor);
  }

  onExecute = e => {
    if (this.editor) {
      const { crop, src } = this.state;
      let image = this.editor.getImage();
      if (!image) {
        return;
      }

      if (image.width < crop.width || image.height < crop.height) {
        image = this.editor.getImageScaledToCanvas();
      }

      // const rect = this.editor.calculatePosition();
      const rect = this.editor.getCroppingRect();

      if (this.props.onExecute) {
        this.props.onExecute({
          event: e,
          sourceImageUrl: src,
          xAxis: rect.x,
          yAxis: rect.y,
          width: rect.width,
          height: rect.height,
          contentType: ""
        });
      }
    }
  };

  onUpdateScale = e => {
    let { crop } = this.state;
    crop = {
      ...crop,
      scale: e
    };

    this.setState({
      crop
    });
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
          <SliderWrap>
            <Slider onChange={this.onUpdateScale} min={1} max={5} />
          </SliderWrap>
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
