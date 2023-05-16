import * as React from "react";
import { useState } from "react";
import styled from "styled-components";
import Thumbnail from "./Thumbnail";
import NoImage from "../NoImages/no-image";
import Overlay from "../../atoms/Overlay";
import { ButtonOutlineCircleLight } from "../../atoms/Buttons/OutlineButtons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const CollapsedThumbnail = styled.div`
  overflow-y: hidden;
  max-height: 300px;
  position: relative;
`;

const ExpandedThumbnail = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
  position: relative;

  .btn-expand {
    position: absolute;
    left: 0;
    right: 0;
    bottom: ${(p) => p.theme.size.distance};
    margin: auto;
  }
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 80px;
  top: auto;
  bottom: 0;
  cursor: pointer;
`;

interface Props {
  imageUrl?: string;
}

const ListItemCollapseCover = (props: Props) => {
  const { imageUrl } = props;
  const [isImageExpanded, setImageExpand] = useState(false);
  function expandImage() {
    setImageExpand(true);
  }

  function collapseImage() {
    setImageExpand(false);
  }

  if (!imageUrl) {
    return <NoImage className="no-image mt-2"></NoImage>;
  }
  if (!isImageExpanded) {
    return (
      <CollapsedThumbnail>
        <Thumbnail src={imageUrl} alt="" />
        <ThumbnailOverlay onClick={expandImage}></ThumbnailOverlay>
      </CollapsedThumbnail>
    );
  }

  return (
    <ExpandedThumbnail>
      <Thumbnail src={imageUrl} alt="" />
      <ButtonOutlineCircleLight
        type="button"
        onClick={collapseImage}
        className="btn-expand"
      >
        <FontAwesomeIcon icon="angle-double-up"></FontAwesomeIcon>
      </ButtonOutlineCircleLight>
    </ExpandedThumbnail>
  );
};

export default ListItemCollapseCover;
