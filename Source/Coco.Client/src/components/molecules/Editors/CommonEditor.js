import React, { useEffect, useState, useRef } from "react";
import styled from "styled-components";
import EditorToolbar from "./EditorToolbar";
import EditorModal from "./EditorModal";
import EditorLinkModal from "./EditorLinkModal";
import EditorImageModal from "./EditorImageModal";
import {
  getEntityRange,
  getSelectionEntity,
  getSelectionText
} from "draftjs-utils";
import {
  Editor,
  EditorState,
  RichUtils,
  CompositeDecorator,
  AtomicBlockUtils
} from "draft-js";
import { styleMap, STYLES, HEADING_TYPES, findLinkEntities } from "./Utils";

const Root = styled.div`
  position: relative;
`;

const Container = styled.div`
  margin-bottom: 15px;
  background: ${p => p.theme.color.white};
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  min-height: ${p => (p.height ? `${p.height}px` : "100px")};
`;

const ConttentBox = styled.div`
  padding: ${p => p.theme.size.distance};
`;

const styles = {
  link: {
    textDecoration: "underline"
  }
};

const LinkComponent = props => {
  const { url } = props.contentState.getEntity(props.entityKey).getData();
  return (
    <a href={url} style={styles.link}>
      {props.children}
    </a>
  );
};

export default props => {
  const decorator = new CompositeDecorator([
    {
      strategy: findLinkEntities,
      component: LinkComponent
    }
  ]);

  const { placeholder, className, height, convertImageCallback } = props;
  const [editorState, setEditorState] = React.useState(
    EditorState.createEmpty(decorator)
  );

  const [isLinkPopupOpen, setLinkPopupOpen] = useState(false);
  const [isImagePopupOpen, setImagePopupOpen] = useState(false);

  const editorRef = useRef(null);

  const getCurrentValues = () => {
    const currentEntity = editorState
      ? getSelectionEntity(editorState)
      : undefined;

    const contentState = editorState.getCurrentContent();
    const currentValues = {};
    if (
      currentEntity &&
      contentState.getEntity(currentEntity).get("type") === "LINK"
    ) {
      currentValues.link = {};
      const entityRange =
        currentEntity && getEntityRange(editorState, currentEntity);
      const contentStateData = contentState
        .getEntity(currentEntity)
        .get("data");

      currentValues.link.target = currentEntity && contentStateData.url;
      currentValues.link.targetOption =
        currentEntity && contentStateData.targetOption;
      currentValues.link.title = entityRange && entityRange.text;
    }

    currentValues.selectionText = getSelectionText(editorState);
    return currentValues;
  };

  const onChange = editorState => {
    return setEditorState(editorState);
  };

  const focusEditor = () => {
    editorRef.current.focus();
  };

  const focus = () => {
    focusEditor();
  };

  useEffect(() => {
    focusEditor();

    return () => {
      clearTimeout();
    };
  }, []);

  const handleKeyCommand = (command, editorState) => {
    const newState = RichUtils.handleKeyCommand(editorState, command);
    if (newState) {
      onChange(newState);
    }
  };

  const toggleInlineStyle = style => {
    const newState = RichUtils.toggleInlineStyle(editorState, style);
    if (newState) {
      onChange(newState);
    }
  };

  const toggleBlockType = style => {
    const newState = RichUtils.toggleBlockType(editorState, style);
    if (newState) {
      onChange(newState);
    }
  };

  const onAddLink = e => {
    const { newEditorState, entityKey } = e;
    onChange(
      RichUtils.toggleLink(
        newEditorState,
        newEditorState.getSelection(),
        entityKey
      )
    );

    setTimeout(() => {
      focus();
    }, 0);
  };

  const removeLink = e => {
    e.preventDefault();
    const selection = editorState.getSelection();
    if (!selection.isCollapsed()) {
      setEditorState(RichUtils.toggleLink(editorState, selection, null));
    }
  };

  const clearFormat = newEditorState => {
    onChange(newEditorState);
  };

  const toggleLinkModal = isOpen => {
    if (!isOpen) {
      setTimeout(() => {
        focus();
      }, 0);
    }
    setLinkPopupOpen(!!isOpen);
  };

  const toggleImageModal = isOpen => {
    if (!isOpen) {
      setTimeout(() => {
        focus();
      }, 0);
    }
    setImagePopupOpen(!!isOpen);
  };

  const onAddImage = newEditorState => {
    onChange(newEditorState);
  };

  return (
    <Root className={className}>
      <Container onClick={focusEditor} height={height}>
        <EditorToolbar
          editorState={editorState}
          toggleBlockType={toggleBlockType}
          toggleInlineStyle={toggleInlineStyle}
          styles={STYLES}
          headingTypes={HEADING_TYPES}
          focusEditor={focus}
          clearFormat={clearFormat}
          onRemoveLink={removeLink}
          onLinkModalOpen={toggleLinkModal}
          onImageModalOpen={toggleImageModal}
        />
        <ConttentBox>
          <Editor
            customStyleMap={styleMap}
            ref={editorRef}
            editorState={editorState}
            onChange={onChange}
            handleKeyCommand={handleKeyCommand}
            placeholder={placeholder ? placeholder : "Enter some text..."}
          />
        </ConttentBox>
      </Container>
      <EditorModal
        onAccept={onAddLink}
        isOpen={isLinkPopupOpen}
        onClose={toggleLinkModal}
        editorState={editorState}
        modalBodyComponent={EditorLinkModal}
        currentValue={getCurrentValues()}
      />
      <EditorModal
        onAccept={onAddImage}
        isOpen={isImagePopupOpen}
        onClose={toggleImageModal}
        editorState={editorState}
        convertImageCallback={convertImageCallback}
        modalBodyComponent={EditorImageModal}
      />
    </Root>
  );
};
