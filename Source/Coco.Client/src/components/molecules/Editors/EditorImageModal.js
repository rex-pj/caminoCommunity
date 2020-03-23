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
  background-color: ${p => p.theme.rgbaColor.exDark};
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const ImageEditorTabs = styled(Tabs)`
  ul.tabs-bar {
    border-bottom: 1px solid ${p => p.theme.rgbaColor.cyanLight};
  }
  ul.tabs-bar li button {
    background-color: transparent;
    color: ${p => p.theme.color.light};
  }

  ul.tabs-bar li.actived button {
    background-color: ${p => p.theme.rgbaColor.cyanLight};
    color: ${p => p.theme.color.white};
  }
`;

export default props => {
  const { editorState, convertImageCallback } = props;
  const onClose = () => {
    props.onClose();
  };

  const onAddImage = e => {
    props.onAccept(e);
  };

  return (
    <Root>
      <ImageEditorTabs
        tabs={[
          {
            title: "Upload",
            tabComponent: () => (
              <EditorImageUploader
                onAddImage={onAddImage}
                onClose={onClose}
                convertImageCallback={convertImageCallback}
                editorState={editorState}
              />
            )
          },
          {
            title: "Đường dẫn",
            tabComponent: () => (
              <EditorImageLink
                onAddImage={onAddImage}
                onClose={onClose}
                editorState={editorState}
              />
            )
          }
        ]}
      />
    </Root>
  );
};
