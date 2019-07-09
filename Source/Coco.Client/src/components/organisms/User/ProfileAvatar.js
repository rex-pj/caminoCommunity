import React, { Fragment } from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Overlay from "../../atoms/Overlay";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";

const ThumbnailOverlay = styled(Overlay)`
  height: 100px;
  top: auto;
  bottom: 0;
`;

const AvatarUpload = styled(ImageUpload)`
  position: absolute;
  bottom: 45px;
  left: 135px;
  z-index: 2;
  margin: auto;
`;

export default function(props) {
  const { userInfo } = props;

  return (
    <Fragment>
      <a href={userInfo.url} className="cover-link">
        <Thumbnail src={userInfo.coverImageUrl} alt="" />
        <ThumbnailOverlay />
      </a>
      <AvatarUpload />
    </Fragment>
  );
}
