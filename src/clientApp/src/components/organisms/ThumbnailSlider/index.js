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
  const images = props.images;
  let image = null,
    numberOfImages = 0;

  if (images) {
    images[0].isActive = true;
    image = images[0];
    image.index = 0;
    numberOfImages = images.length;
  }

  let { displayNumber } = props;
  const [slideState, setSlideState] = useState({
    currentImage: image,
    numberOfDisplay: displayNumber ? displayNumber : numberOfImages,
    numberOfImages: numberOfImages,
  });

  const currentRef = useRef();

  const renderRelationImages = (relationImages) => {
    const { numberOfDisplay, currentImage } = slideState;

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
        (data) => data.groupIndex === currentImage.groupIndex
      );

      return relationImages.map((item, index) => {
        let className =
          index === 0
            ? "first-item"
            : index === numberOfDisplay - 1
            ? "last-item"
            : "";

        if (item === currentImage) {
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

    var newSlideState = {
      ...slideState,
      currentImage: item,
    };
    setSlideState(newSlideState);
  };

  const onNext = () => {
    const { numberOfImages, currentImage } = slideState;
    const { images } = props;
    let image = null;
    if (currentImage.index < numberOfImages - 1) {
      image = images[currentImage.index + 1];
    } else {
      image = images[0];
    }

    var newSlideState = {
      ...slideState,
      currentImage: image,
    };
    setSlideState(newSlideState);
  };

  const onPrev = () => {
    let { currentImage } = slideState;
    const { images } = props;
    let image = null;
    if (currentImage.index > 0) {
      image = images[currentImage.index - 1];
    } else {
      const { numberOfImages } = slideState;
      image = images[numberOfImages - 1];
    }
    var newSlideState = {
      ...slideState,
      currentImage: image,
    };
    setSlideState(newSlideState);
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

  const { currentImage, numberOfDisplay } = slideState;

  return (
    <PostThumbnail>
      <MainThumbnail onMouseOver={onMouseOvered} ref={currentRef} tabIndex="1">
        <NavigateLeft onClick={onPrev}>
          <PrevButton>
            <FontAwesomeIcon icon="angle-left" />
          </PrevButton>
        </NavigateLeft>
        {currentImage ? (
          <Thumbnail src={currentImage.thumbnailUrl} onClick={onNext} alt="" />
        ) : null}
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
