import React, { Component } from "react";
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
  background-color: ${p => p.theme.color.lighter};
  position: relative;
  outline: none;
`;

const HorizontalListScroll = styled(ListScroll)`
  background-color: ${p => p.theme.color.lighter};
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

class ThumbnailSlider extends Component {
  constructor(props) {
    super(props);

    const images = props.images;
    let currentImage = null,
      numberOfImages = 0;
    if (images) {
      images[0].isActive = true;
      currentImage = images[0];
      currentImage.index = 0;
      numberOfImages = images.length;
    }

    let { numberOfDisplay } = props;
    this.state = {
      currentImage: currentImage,
      numberOfDisplay: numberOfDisplay ? numberOfDisplay : numberOfImages,
      numberOfImages: numberOfImages
    };

    this.currentRef = React.createRef();
  }

  renderRelationImages = relationImages => {
    const { numberOfDisplay, currentImage } = this.state;

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
            onClick={e => this.onChangeThumbnail(item)}
          />
        );
      });
    } else {
      return null;
    }
  };

  onChangeThumbnail = item => {
    const { images } = this.props;
    images.map(item => {
      return item;
    });

    this.setState({
      currentImage: item
    });
  };

  onNext = () => {
    const { numberOfImages, currentImage } = this.state;
    const { images } = this.props;
    let image = null;
    if (currentImage.index < numberOfImages - 1) {
      image = images[currentImage.index + 1];
    } else {
      image = images[0];
    }
    this.setState({
      currentImage: image
    });
  };

  onPrev = () => {
    let { currentImage } = this.state;
    const { images } = this.props;
    let image = null;
    if (currentImage.index > 0) {
      image = images[currentImage.index - 1];
    } else {
      const { numberOfImages } = this.state;
      image = images[numberOfImages - 1];
    }
    this.setState({
      currentImage: image
    });
  };

  componentDidMount() {
    this.currentRef.current.addEventListener(
      "keydown",
      this.onChangeImage,
      false
    );
  }

  componentWillUnmount() {
    this.currentRef.current.removeEventListener("keydown", this.onChangeImage);
  }

  onChangeImage = e => {
    var originator = e.keyCode || e.which;

    if (originator === 39) {
      this.onNext();
    } else if (originator === 37) {
      this.onPrev();
    }
  };

  onMouseOvered = e => {
    this.currentRef.current.focus();
  };

  render() {
    const { images } = this.props;
    const { currentImage, numberOfDisplay } = this.state;

    return (
      <PostThumbnail>
        <MainThumbnail
          onMouseOver={this.onMouseOvered}
          ref={this.currentRef}
          tabIndex="1"
        >
          <NavigateLeft onClick={this.onPrev}>
            <PrevButton>
              <FontAwesomeIcon icon="angle-left" />
            </PrevButton>
          </NavigateLeft>
          {currentImage ? (
            <Thumbnail
              src={currentImage.thumbnailUrl}
              onClick={this.onNext}
              alt=""
            />
          ) : null}
          <NavigateRight onClick={this.onNext}>
            <NextButton>
              <FontAwesomeIcon icon="angle-right" />
            </NextButton>
          </NavigateRight>
        </MainThumbnail>
        <div>
          <HorizontalListScroll
            numberOfDisplay={numberOfDisplay}
            list={this.renderRelationImages(images)}
          />
        </div>
      </PostThumbnail>
    );
  }
}

export default ThumbnailSlider;
