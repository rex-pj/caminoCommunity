import React, { useState } from "react";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import NoImage from "../../atoms/NoImages/no-image";
import Overlay from "../../atoms/Overlay";

const CollapsedThumbnail = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
  overflow-y: hidden;
  max-height: 300px;
  position: relative;
`;

const ExpandedThumbnail = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 80px;
  top: auto;
  bottom: 0;
  cursor: pointer;
`;

export default function (props) {
  const { imageUrl } = props;
  const [isShowFullImage, setShowFullImage] = useState(false);
  function showFullImage() {
    setShowFullImage(!isShowFullImage);
  }

  if (!imageUrl) {
    return <NoImage className="no-image mt-2"></NoImage>;
  } else if (!isShowFullImage) {
    return (
      <CollapsedThumbnail>
        <Thumbnail src={imageUrl} alt="" />
        <ThumbnailOverlay onClick={showFullImage}></ThumbnailOverlay>
      </CollapsedThumbnail>
    );
  } else if (isShowFullImage) {
    return (
      <ExpandedThumbnail>
        <Thumbnail src={imageUrl} alt="" />
      </ExpandedThumbnail>
    );
  }
}
