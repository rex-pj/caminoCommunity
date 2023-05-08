import * as React from "react";
import { ChangeEvent, Fragment } from "react";
import styled from "styled-components";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { LabelNormal } from "../../atoms/Labels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ImageInfoModel } from "../../../models/imageInfoModel";

const FormInput = styled.div`
  text-align: center;
  margin: ${(p) => p.theme.size.tiny} auto;

  svg {
    color: ${(p) => p.theme.color.neutralText};
    margin-right: ${(p) => p.theme.size.exTiny};
    font-size: ${(p) => p.theme.size.small};
    path {
      color: inherit;
    }
  }

  ${LabelNormal} {
    color: ${(p) => p.theme.color.neutralText};
    margin-right: ${(p) => p.theme.size.exTiny};
    font-weight: bold;
    font-size: ${(p) => p.theme.size.distance};
  }

  .image-title {
    width: 300px;
  }
`;

interface EditorImageInfoProps {
  imageData: ImageInfoModel;
  handleInfoChange: (e: ChangeEvent<HTMLInputElement>) => void;
  onScaleChanged: (e: ChangeEvent<HTMLInputElement>) => void;
}

const EditorImageInfo: React.FC<EditorImageInfoProps> = (props) => {
  const { imageData, handleInfoChange, onScaleChanged } = props;

  const handleScaleChange = (e: ChangeEvent<HTMLInputElement>) => {
    onScaleChanged(e);
  };

  const handleAltChange = (e: ChangeEvent<HTMLInputElement>) => {
    handleInfoChange(e);
  };

  const { src, alt, width, height } = imageData;
  return (
    <Fragment>
      {src.value ? (
        <Fragment>
          <FormInput>
            <div className="row">
              <div className="col-md-12">
                <LabelNormal>Title</LabelNormal>
                <PrimaryTextbox
                  className="image-title"
                  name="alt"
                  value={alt.value}
                  autoComplete="off"
                  onChange={(e) => handleAltChange(e)}
                />
              </div>
            </div>
          </FormInput>
          <FormInput>
            <div className="row">
              <div className="col-md-6">
                <FontAwesomeIcon icon="arrows-alt-h" />
                <PrimaryTextbox
                  name="width"
                  value={width.value}
                  autoComplete="off"
                  onChange={(e) => handleScaleChange(e)}
                />
              </div>
              <div className="col-md-6">
                <FontAwesomeIcon icon="arrows-alt-v" />
                <PrimaryTextbox
                  name="height"
                  value={height.value}
                  autoComplete="off"
                  onChange={(e) => handleScaleChange(e)}
                />
              </div>
            </div>
          </FormInput>
        </Fragment>
      ) : null}
    </Fragment>
  );
};

export default EditorImageInfo;
