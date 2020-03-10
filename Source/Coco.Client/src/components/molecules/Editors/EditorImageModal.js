import React from "react";
import Tabs from "../Tabs/Tabs";
import EditorImageUploader from "./EditorImageUploader";
import EditorImageLink from "./EditorImageLink";
import styled from "styled-components";

const Root = styled.div`
  min-width: 400px;
  margin: ${p => p.theme.size.tiny} ${p => p.theme.size.tiny} 0
    ${p => p.theme.size.tiny};
  min-height: 200px;
  background: ${p => p.theme.color.dark};
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const ImageEditorTabs = styled(Tabs)`
  ul.tabs-bar {
    border-bottom: 1px solid ${p => p.theme.rgbaColor.darker};
  }
  ul.tabs-bar li button {
    background: transparent;
    color: ${p => p.theme.color.light};
  }

  ul.tabs-bar li.actived button {
    background: ${p => p.theme.rgbaColor.darker};
    color: ${p => p.theme.color.white};
  }
`;

export default props => {
  const onClose = () => {
    props.onClose();
  };

  const onUploadImage = () => {};

  const onAddImageLink = () => {};

  return (
    <Root>
      <ImageEditorTabs
        tabs={[
          {
            title: "Upload",
            tabComponent: () => (
              <EditorImageUploader
                onAddImage={onUploadImage}
                onClose={onClose}
              />
            )
          },
          {
            title: "Đường dẫn",
            tabComponent: () => (
              <EditorImageLink onAddImage={onAddImageLink} onClose={onClose} />
            )
          }
        ]}
      />
    </Root>
  );
};
