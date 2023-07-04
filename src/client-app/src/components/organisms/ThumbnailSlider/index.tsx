import * as React from "react";
import { useRef, useEffect, useState } from "react";
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
  background-color: ${(p) => p.theme.color.darkBg};
  position: relative;
  outline: none;
`;

const HorizontalListScroll = styled(ListScroll)`
  background-color: ${(p) => p.theme.color.darkBg};
  border-top: 1px solid ${(p) => p.theme.color.darkBg};
  border-bottom: 1px solid ${(p) => p.theme.color.darkBg};

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
    background-color: ${(p) => p.theme.color.darkBg};
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
  color: ${(p) => p.theme.color.neutralText};
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

type Props = {
  displayNumber?: number;
  images?: ThumbnailPicture[];
  currentImage?: ThumbnailPicture;
};

class ThumbnailPicture {
  groupIndex?: number;
  index?: number;
  pictureUrl?: string;
  url?: string;
  name?: string;
}

const Index = (props: Props) => {
  const currentRef = useRef<any>();
  const { images, currentImage } = props;
  let { displayNumber } = props;
  const numberOfImages = images?.length ?? 0;
  const numberOfDisplay = displayNumber ? displayNumber : numberOfImages;
  const [thumbnailPicture, setThumbnailPicture] = useState<ThumbnailPicture>(
    new ThumbnailPicture()
  );

  const renderRelationImages = (relationImages?: ThumbnailPicture[]): any[] => {
    if (!relationImages) {
      return [];
    }

    let groupIndex = 0;
    const groupImages = relationImages.map((item, index) => {
      if (index > 0 && index % numberOfDisplay === 0) {
        groupIndex += 1;
      }
      item.groupIndex = groupIndex;
      item.index = index;

      return item;
    });

    const filteredImages = groupImages.filter(
      (data) => data.groupIndex === thumbnailPicture.groupIndex
    );

    return filteredImages.map((item, index) => {
      let className = "";
      if (index === 0) {
        className = "first-item";
      } else if (index === numberOfDisplay - 1) {
        className = "last-item";
      }

      if (item === thumbnailPicture) {
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
  };

  const onChangeThumbnail = (item: ThumbnailPicture) => {
    const { images } = props;
    images?.forEach((image) => {
      return image;
    });

    setThumbnailPicture(item);
  };

  const onNext = () => {
    const { images } = props;
    if (!images || images.length <= 1) {
      return;
    }

    let image: ThumbnailPicture;
    if (
      thumbnailPicture.index !== undefined &&
      thumbnailPicture.index >= 0 &&
      thumbnailPicture.index < numberOfImages - 1
    ) {
      image = images[thumbnailPicture.index + 1];
    } else {
      image = images[0];
    }

    setThumbnailPicture(image);
  };

  const onPrev = () => {
    const { images } = props;
    if (!images || images.length <= 1) {
      return;
    }
    let image: ThumbnailPicture;
    if (thumbnailPicture.index && thumbnailPicture.index > 0) {
      image = images[thumbnailPicture.index - 1];
    } else {
      image = images[numberOfImages - 1];
    }
    setThumbnailPicture(image);
  };

  useEffect(() => {
    if (!currentImage) {
      return;
    }
    setThumbnailPicture(currentImage);
  }, [currentImage]);

  useEffect(() => {
    const thumbnailRef = currentRef.current;
    thumbnailRef.addEventListener("keydown", onChangeImage, false);
    return () => {
      thumbnailRef.removeEventListener("keydown", onChangeImage);
    };
  });

  const onChangeImage = (e: KeyboardEvent) => {
    const originator = e.key;

    if (originator === "39") {
      onNext();
    } else if (originator === "37") {
      onPrev();
    }
  };

  const onMouseOvered = (e: React.MouseEvent<HTMLDivElement>) => {
    currentRef.current.focus();
  };

  const renderThumbnailPicture = () => {
    if (!images || images.length === 0 || !currentImage) {
      return null;
    }
    const isThumbnailInList = images.some((x) => {
      return x.pictureUrl === currentImage.pictureUrl;
    });
    if (!isThumbnailInList) {
      return null;
    }
    return (
      <Thumbnail
        maxHeight={400}
        src={thumbnailPicture.pictureUrl}
        onClick={onNext}
        alt=""
      />
    );
  };

  const canSlide = numberOfImages > 1;
  return (
    <PostThumbnail>
      <MainThumbnail onMouseOver={onMouseOvered} ref={currentRef} tabIndex={1}>
        {canSlide ? (
          <NavigateLeft onClick={onPrev}>
            <PrevButton>
              <FontAwesomeIcon icon="angle-left" />
            </PrevButton>
          </NavigateLeft>
        ) : null}
        {renderThumbnailPicture()}
        {canSlide ? (
          <NavigateRight onClick={onNext}>
            <NextButton>
              <FontAwesomeIcon icon="angle-right" />
            </NextButton>
          </NavigateRight>
        ) : null}
      </MainThumbnail>
      {canSlide ? (
        <HorizontalListScroll
          numberOfDisplay={numberOfDisplay}
          list={() => renderRelationImages(images)}
        />
      ) : null}
    </PostThumbnail>
  );
};

export default Index;
