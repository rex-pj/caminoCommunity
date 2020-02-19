import React, { useEffect, useState } from "react";
import { Editor, EditorState, RichUtils, CompositeDecorator } from "draft-js";
import styled from "styled-components";
import EditorToolbar from "./EditorToolbar";
import EditorModal from "./EditorModal";

const Root = styled.div`
  position: relative;
`;

const Container = styled.div`
  margin-bottom: 15px;
  background: ${p => p.theme.color.white};
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
`;

const ConttentBox = styled.div`
  padding: ${p => p.theme.size.distance};
`;

const styles = {
  link: {
    textDecoration: "underline"
  }
};

export default props => {
  const findLinkEntities = (contentBlock, callback, contentState) => {
    contentBlock.findEntityRanges(character => {
      const entityKey = character.getEntity();
      return (
        entityKey !== null &&
        contentState.getEntity(entityKey).getType() === "LINK"
      );
    }, callback);
  };

  const Link = props => {
    const { url } = props.contentState.getEntity(props.entityKey).getData();
    return (
      <a href={url} style={styles.link}>
        {props.children}
      </a>
    );
  };

  const decorator = new CompositeDecorator([
    {
      strategy: findLinkEntities,
      component: Link
    }
  ]);

  const { placeholder, className } = props;
  const [editorState, setEditorState] = React.useState(
    EditorState.createEmpty(decorator)
  );

  const [shouldOpenModal, setModalOpen] = useState(false);

  const styleMap = {
    CODE: {
      backgroundColor: "rgba(0, 0, 0, 0.5)",
      fontFamily: '"Inconsolata", "Menlo", "Consolas", monospace',
      fontSize: 16,
      padding: 2
    },
    HIGHLIGHT: {
      background: "#fffe0d"
    }
  };

  var INLINE_STYLES = [
    { icon: "bold", style: "BOLD" },
    { icon: "italic", style: "ITALIC" },
    { icon: "underline", style: "UNDERLINE" },
    { icon: "strikethrough", style: "STRIKETHROUGH" },
    { icon: "highlighter", style: "HIGHLIGHT" }
  ];

  const BLOCK_TYPES = [
    { icon: "quote-left", style: "blockquote" },
    { icon: "list-ul", style: "unordered-list-item" },
    { icon: "list-ol", style: "ordered-list-item" }
  ];

  const HEADING_TYPES = [
    { label: "Normal Heading", style: "unstyled" },
    { label: "Heading 1", style: "header-one" },
    { label: "Heading 2", style: "header-two" },
    { label: "Heading 3", style: "header-three" },
    { label: "Heading 4", style: "header-four" },
    { label: "Heading 5", style: "header-five" },
    { label: "Heading 6", style: "header-six" }
  ];

  const editor = React.useRef(null);

  const onChange = editorState => {
    return setEditorState(editorState);
  };

  const focusEditor = () => {
    editor.current.focus();
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

  const focus = () => {
    focusEditor();
  };

  const clearFormat = newEditorState => {
    onChange(newEditorState);
  };

  const onLinkModalOpen = isOpen => {
    setModalOpen(!!isOpen);
  };

  return (
    <Root className={className}>
      <Container onClick={focusEditor}>
        <EditorToolbar
          editorState={editorState}
          toggleBlockType={toggleBlockType}
          toggleInlineStyle={toggleInlineStyle}
          inlineTyles={INLINE_STYLES}
          blockTyles={BLOCK_TYPES}
          headingTypes={HEADING_TYPES}
          focusEditor={focus}
          clearFormat={clearFormat}
          onRemoveLink={removeLink}
          onLinkModalOpen={onLinkModalOpen}
        />
        <ConttentBox>
          <Editor
            customStyleMap={styleMap}
            ref={editor}
            editorState={editorState}
            onChange={onChange}
            handleKeyCommand={handleKeyCommand}
            placeholder={placeholder ? placeholder : "Enter some text..."}
          />
        </ConttentBox>
      </Container>
      <EditorModal
        className="modal"
        isOpen={shouldOpenModal}
        onClose={onLinkModalOpen}
        onAddLink={onAddLink}
        editorState={editorState}
      />
    </Root>
  );
};
