import React, { Fragment } from "react";
import styled from "styled-components";
import { Textbox } from "../../atoms/Textboxes";
import { LabelNormal } from "../../atoms/Labels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import NoImage from "../../atoms/NoImages/no-image";
import { Image } from "../../atoms/Images";

const FormInput = styled.div`
  text-align: center;
  margin: ${p => p.theme.size.tiny} auto;

  svg {
    color: ${p => p.theme.color.light};
    margin-right: ${p => p.theme.size.exTiny};
    font-size: ${p => p.theme.size.small};
    path {
      color: inherit;
    }
  }

  ${LabelNormal} {
    color: ${p => p.theme.color.light};
    margin-right: ${p => p.theme.size.exTiny};
    font-weight: bold;
    font-size: ${p => p.theme.size.distance};
  }

  .image-title {
    width: 300px;
  }
`;

const ImageWrap = styled.div`
  text-align: center;
`;

const EmptyImage = styled(NoImage)`
  border-radius: ${p => p.theme.borderRadius.medium};
  width: 200px;
  height: 200px;
  display: inline-block;
  font-size: ${p => p.theme.size.large};
`;

export default props => {
  const {
    src,
    alt,
    width,
    height,
    handleInputChange,
    onWithScaleChanged,
    isValid
  } = props;

  return (
    <Fragment>
      {src && isValid ? (
        <Fragment>
          <FormInput>
            <div className="row">
              <div className="col-md-12">
                <LabelNormal>Title</LabelNormal>
                <Textbox
                  className="image-title"
                  name="alt"
                  value={alt}
                  autoComplete="off"
                  onChange={e => handleInputChange(e)}
                />
              </div>
            </div>
          </FormInput>
          <FormInput>
            <div className="row">
              <div className="col-md-6">
                <FontAwesomeIcon icon="arrows-alt-h" />
                <Textbox
                  name="width"
                  value={width}
                  autoComplete="off"
                  onChange={e => onWithScaleChanged(e)}
                />
              </div>
              <div className="col-md-6">
                <FontAwesomeIcon icon="arrows-alt-v" />
                <Textbox
                  name="height"
                  value={height}
                  autoComplete="off"
                  onChange={e => onWithScaleChanged(e)}
                />
              </div>
            </div>
          </FormInput>
        </Fragment>
      ) : null}
      <ImageWrap>
        {src && isValid ? (
          <Image src={src} alt={alt} width={width} height={height} />
        ) : (
          <EmptyImage />
        )}
      </ImageWrap>
    </Fragment>
  );
};
