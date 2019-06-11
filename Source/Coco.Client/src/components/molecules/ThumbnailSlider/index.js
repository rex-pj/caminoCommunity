import React, { useEffect, useRef, useState } from "react";
import ListScroll from "../../molecules/ListScroll";
import { Thumbnail } from "../../molecules/Thumbnails";
import styled from "styled-components";
import { ButtonTransparent } from "../../atoms/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const PostThumbnail = styled.div`
  margin-top: 0;
  position: relative;
`;

const MainThumbnail = styled.div`
  overflow: hidden;
  background-color: ${p => p.theme.color.exLight};
  position: relative;
  outline: none;
`;

const HorizontalListScroll = styled(ListScroll)`
  background-color: ${p => p.theme.color.exLight};
  border-top: 1px solid ${p => p.theme.color.light};
  border-bottom: 1px solid ${p => p.theme.color.light};

  li {
    height: 140px;

    .first-item {
      padding-left: ${p => p.theme.size.exSmall};
    }

    .last-item {
      padding-right: ${p => p.theme.size.exSmall};
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
  padding: 0 ${p => p.theme.size.exTiny};

  :before {
    content: " ";
    display: inline-block;
    height: 100%;
    vertical-align: middle;
  }

  &.actived,
  :hover {
    background-color: ${p => p.theme.color.light};
  }
`;

const NavigateBar = styled.div`
  position: absolute;
  top: 0;
  bottom: 0;
  width: ${p => p.theme.size.medium};
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
  margin-top: -${p => p.theme.size.normal};
  height: ${p => p.theme.size.normal};
  color: ${p => p.theme.color.light};
  font-size: ${p => p.theme.fontSize.large};
  line-height: 1;
  padding: 0;
  text-align: center;
`;

const PrevButton = styled(NavigateButton)`
  left: ${p => p.theme.size.exTiny};
`;

const NextButton = styled(NavigateButton)`
  right: ${p => p.theme.size.exTiny};
`;

function ThumbnailSlider(props) {
  const { images } = props;
  let { numberOfDisplay } = props;

  const currentRef = useRef();
  let currentImage = null,
    numberOfImages = 0;

  if (images) {
    images[0].isActive = true;
    currentImage = images[0];
    currentImage.index = 0;
    numberOfImages = images.length;
  }

  numberOfDisplay = numberOfDisplay ? numberOfDisplay : numberOfImages;

  const [defaultState, setDefaultState] = useState({
    currentImage: currentImage
  });

  function renderRelationImages(relationImages) {
    const { currentImage } = defaultState;

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
        data => data.groupIndex === currentImage.groupIndex
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
            onClick={() => onChangeThumbnail(item)}
          />
        );
      });
    } else {
      return null;
    }
  };

  function onChangeThumbnail(item) {
    const { images } = props;
    images.map(item => {
      return item;
    });

    setDefaultState({
      currentImage: item
    });
  };

  function onNext() {
    const { currentImage } = defaultState;
    const { images } = props;
    let image = null;
    if (currentImage.index < numberOfImages - 1) {
      image = images[currentImage.index + 1];
    } else {
      image = images[0];
    }

    setDefaultState({
      currentImage: image
    });
  };

  function onPrev() {
    let { currentImage } = defaultState;
    const { images } = props;
    let image = null;
    if (currentImage.index > 0) {
      image = images[currentImage.index - 1];
    } else {
      image = images[numberOfImages - 1];
    }
    setDefaultState({
      currentImage: image
    });
  };

  useEffect(function () {
    currentRef.current.addEventListener(
      "keydown",
      onChangeImage,
      false
    );

    return function cleanup() {
      currentRef.current.removeEventListener("keydown", onChangeImage);
    };
  })

  function onChangeImage(e) {
    var originator = e.keyCode || e.which;

    if (originator === 39) {
      onNext();
    } else if (originator === 37) {
      onPrev();
    }
  };

  function onMouseOvered(e) {
    currentRef.current.focus();
  };

  return (
    <PostThumbnail>
      <MainThumbnail
        onMouseOver={onMouseOvered}
        ref={currentRef}
        tabIndex="1"
      >
        <NavigateLeft onClick={onPrev}>
          <PrevButton>
            <FontAwesomeIcon icon="angle-left" />
          </PrevButton>
        </NavigateLeft>
        {defaultState.currentImage ? (
          <Thumbnail
            src={defaultState.currentImage.thumbnailUrl}
            onClick={onNext}
            alt=""
          />
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

export default ThumbnailSlider;
