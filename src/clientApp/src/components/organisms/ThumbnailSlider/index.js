import React, { useRef, useEffect, useState } from "react";
import ListScroll from "../../molecules/ListScroll";
import { Thumbnail } from "../../molecules/Thumbnails";
import styled from "styled-components";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const PostThumbnail = styled.div`
  margin-top: 0;
  position: relative;
`;

const MainThumbnail = styled.div`
  overflow: hidden;
  background-color: ${(p) => p.theme.color.primary};
  position: relative;
  outline: none;
`;

const HorizontalListScroll = styled(ListScroll)`
  background-color: ${(p) => p.theme.color.primaryLight};
  border-top: 1px solid ${(p) => p.theme.color.primary};
  border-bottom: 1px solid ${(p) => p.theme.color.primary};

  li {
    height: 140px;

    .first-item {
      padding-left: ${(p) => p.theme.size.exSmall};
    }

    .last-item {
      padding-right: ${(p) => p.theme.size.exSmall};
    }

    img {
      max-height: 100%;
    }
  }
`;

const ThumbnailItem = styled(Thumbnail)`
  height: 100%;
  border-radius: 0;
  position: relative;
  cursor: pointer;
  padding: 0 ${(p) => p.theme.size.exTiny};

  :before {
    content: " ";
    display: inline-block;
    height: 100%;
    vertical-align: middle;
  }

  &.actived,
  :hover {
    background-color: ${(p) => p.theme.color.primaryDark};
  }
`;

const NavigateBar = styled.div`
  position: absolute;
  top: 0;
  bottom: 0;
  width: ${(p) => p.theme.size.medium};
  height: 100%;
  cursor: pointer;
`;

const NavigateLeft = styled(NavigateBar)`
  left: 0;
`;

const NavigateRight = styled(NavigateBar)`
  right: 0;
`;

const NavigateButton = styled(ButtonTransparent)`
  position: absolute;
  top: 50%;
  bottom: 50%;
  margin-top: -${(p) => p.theme.size.normal};
  height: ${(p) => p.theme.size.normal};
  color: ${(p) => p.theme.color.light};
  font-size: ${(p) => p.theme.fontSize.large};
  line-height: 1;
  padding: 0;
  text-align: center;
`;

const PrevButton = styled(NavigateButton)`
  left: ${(p) => p.theme.size.exTiny};
`;

const NextButton = styled(NavigateButton)`
  right: ${(p) => p.theme.size.exTiny};
`;

export default function (props) {
  const currentRef = useRef();
  const { images, currentImage } = props;
  let { displayNumber } = props;
  const numberOfImages = images.length;
  const numberOfDisplay = displayNumber ? displayNumber : numberOfImages;
  const [thumbnailImage, setThumbnailImage] = useState(currentImage);

  const renderRelationImages = (relationImages) => {
    if (relationImages) {
      let groupIndex = 0;
      relationImages = relationImages.map((item, index) => {
        if (index > 0 && index % numberOfDisplay === 0) {
          groupIndex += 1;
        }
        item.groupIndex = groupIndex;
        item.index = index;

        return item;
      });

      relationImages = relationImages.filter(
        (data) => data.groupIndex === thumbnailImage.groupIndex
      );

      return relationImages.map((item, index) => {
        let className =
          index === 0
            ? "first-item"
            : index === numberOfDisplay - 1
            ? "last-item"
            : "";

        if (item === thumbnailImage) {
          className += " actived";
        }

        return (
          <ThumbnailItem
            className={className}
            key={index}
            src={item.url}
            alt={item.name}
            onClick={(e) => onChangeThumbnail(item)}
          />
        );
      });
    } else {
      return null;
    }
  };

  const onChangeThumbnail = (item) => {
    const { images } = props;
    images.map((item) => {
      return item;
    });

    setThumbnailImage(item);
  };

  const onNext = () => {
    const { images } = props;
    let image = null;
    if (thumbnailImage.index < numberOfImages - 1) {
      image = images[thumbnailImage.index + 1];
    } else {
      image = images[0];
    }

    setThumbnailImage(image);
  };

  const onPrev = () => {
    const { images } = props;
    let image = null;
    if (thumbnailImage.index > 0) {
      image = images[thumbnailImage.index - 1];
    } else {
      image = images[numberOfImages - 1];
    }
    setThumbnailImage(image);
  };

  useEffect(() => {
    var thumbnailRef = currentRef.current;
    thumbnailRef.addEventListener("keydown", onChangeImage, false);
    return () => {
      thumbnailRef.removeEventListener("keydown", onChangeImage);
    };
  });

  const onChangeImage = (e) => {
    var originator = e.keyCode || e.which;

    if (originator === 39) {
      onNext();
    } else if (originator === 37) {
      onPrev();
    }
  };

  const onMouseOvered = (e) => {
    currentRef.current.focus();
  };

  const renderThumbnailImage = () => {
    if (thumbnailImage) {
      return (
        <Thumbnail src={thumbnailImage.thumbnailUrl} onClick={onNext} alt="" />
      );
    }
    return null;
  };

  return (
    <PostThumbnail>
      <MainThumbnail onMouseOver={onMouseOvered} ref={currentRef} tabIndex="1">
        <NavigateLeft onClick={onPrev}>
          <PrevButton>
            <FontAwesomeIcon icon="angle-left" />
          </PrevButton>
        </NavigateLeft>
        {renderThumbnailImage()}
        <NavigateRight onClick={onNext}>
          <NextButton>
            <FontAwesomeIcon icon="angle-right" />
          </NextButton>
        </NavigateRight>
      </MainThumbnail>
      <div>
        <HorizontalListScroll
          numberOfDisplay={numberOfDisplay}
          list={renderRelationImages(images)}
        />
      </div>
    </PostThumbnail>
  );
}
