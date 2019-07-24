import React, { Component, Fragment } from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Overlay from "../../atoms/Overlay";
import { ButtonTransparent } from "../../atoms/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";

const Wrap = styled.div`
  position: relative;
  height: inherit;

  a.cover-link {
    display: block;
    height: inherit;
    overflow: hidden;
  }
`;

const ScrollBox = styled.div`
  display: block;
  height: inherit;
  overflow: hidden;
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 100px;
  top: auto;
  bottom: 0;
`;

const FullOverlay = styled.div`
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: ${p => p.theme.rgbaColor.exDark};
  z-index: 2;
`;

const EditButton = styled(ButtonTransparent)`
  position: absolute;
  border: 0;
  background-color: ${p => p.theme.rgbaColor.exDark};
  left: ${p => p.theme.size.distance};
  top: ${p => p.theme.size.distance};

  :hover {
    border: 1px solid ${p => p.theme.color.light};
    background-color: ${p => p.theme.color.exLight};
    color: ${p => p.theme.color.dark};
  }
`;

const Tools = styled.div`
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  z-index: 3;
  height: ${p => p.theme.size.large};
  text-align: center;
`;

const CoverImageUpload = styled(ImageUpload)`
  display: inline-block;
  margin-right: ${p => p.theme.size.exTiny};
  vertical-align: middle;
  cursor: pointer;

  > span {
    text-align: center;
    border: 0;
    width: ${p => p.theme.size.large};
    height: ${p => p.theme.size.large};
    font-size: ${p => p.theme.fontSize.large};
    color: ${p => p.theme.color.exLight};
    border-radius: ${p => p.theme.borderRadius.normal};
    padding: ${p => p.theme.size.exSmall} 0;

    :hover {
      background-color: ${p => p.theme.rgbaColor.exDark};
    }

    svg,
    path {
      color: inherit;
    }
  }
`;

const CancelEditButton = styled(ButtonTransparent)`
  border: 0;
  width: ${p => p.theme.size.large};
  height: ${p => p.theme.size.large};
  font-size: ${p => p.theme.fontSize.large};
  color: ${p => p.theme.color.exLight};
  margin-left: ${p => p.theme.size.exTiny};
  vertical-align: middle;

  svg,
  path {
    color: inherit;
  }

  :hover {
    background-color: ${p => p.theme.rgbaColor.exDark};
  }
`;

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isInUpdateMode: false
    };
  }

  turnOnUpdateMode = () => {
    this.setState({
      isInUpdateMode: true
    });

    if (this.props.onToggleEditMode) {
      this.props.onToggleEditMode(true);
    }
  };

  turnOffUpdateMode = () => {
    this.setState({
      isInUpdateMode: false
    });

    if (this.props.onToggleEditMode) {
      this.props.onToggleEditMode(false);
    }
  };

  render() {
    const { userInfo } = this.props;
    const { isInUpdateMode } = this.state;
    return (
      <Wrap>
        {!isInUpdateMode ? (
          <Fragment>
            <EditButton size="sm" onClick={this.turnOnUpdateMode}>
              <FontAwesomeIcon icon="pencil-alt" />
            </EditButton>
            <a href={userInfo.url} className="cover-link">
              <Thumbnail src={userInfo.coverImageUrl} alt="" />
              <ThumbnailOverlay />
            </a>
          </Fragment>
        ) : (
          <ScrollBox>
            <Thumbnail src={userInfo.coverImageUrl} alt="" />
            <FullOverlay />
            <Tools>
              <CoverImageUpload />
              <CancelEditButton onClick={this.turnOffUpdateMode}>
                <FontAwesomeIcon icon="times" />
              </CancelEditButton>
            </Tools>
          </ScrollBox>
        )}
      </Wrap>
    );
  }
}
