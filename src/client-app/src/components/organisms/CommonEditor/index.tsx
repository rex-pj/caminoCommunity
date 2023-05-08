import * as React from "react";
import {
  useEffect,
  useState,
  useRef,
  forwardRef,
  useImperativeHandle,
} from "react";
import styled from "styled-components";
import EditorToolbar from "./EditorToolbar";
import EditorModal from "./EditorModal";
import { EditorLinkModal, EditorLinkModalAcceptEvent } from "./EditorLinkModal";
import EditorImageModal from "./EditorImageModal";
import {
  getEntityRange,
  getSelectionEntity,
  getSelectionText,
} from "draftjs-utils";
import {
  Editor,
  EditorState,
  RichUtils,
  CompositeDecorator,
  ContentState,
  EditorCommand,
} from "draft-js";
import { styleMap, STYLES, findLinkEntities, findImageEntities } from "./Utils";
import { stateFromHTML } from "draft-js-import-html";
import { DefaultButtonToggleEvent } from "./EditorButtons";

const Root = styled.div`
  position: relative;
`;

interface ContainerProps {
  height?: number;
}

const Container = styled.div<ContainerProps>`
  margin-bottom: 15px;
  background: ${(p) => p.theme.color.whiteBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  min-height: ${(p) => (p.height ? `${p.height}px` : "100px")};
`;

const ConttentBox = styled.div`
  padding: ${(p) => p.theme.size.distance};
`;

const styles = {
  link: {
    textDecoration: "underline",
  },
  image: {
    maxWidth: "100%",
  },
};

interface LinkComponentProps {
  contentState: ContentState;
  entityKey: string;
  children?: any;
}

const LinkComponent: React.FC<LinkComponentProps> = (props) => {
  const { url } = props.contentState.getEntity(props.entityKey).getData();
  return (
    <a href={url} style={styles.link}>
      {props.children}
    </a>
  );
};

interface ImageComponentProps {
  contentState: ContentState;
  entityKey: string;
}

const ImageComponent: React.FC<ImageComponentProps> = (props) => {
  const { src, width, height, alt } = props.contentState
    .getEntity(props.entityKey)
    .getData();
  return (
    <img
      src={src}
      width={width}
      height={height}
      alt={alt}
      style={styles.image}
    />
  );
};

interface CommonEditorProps {
  placeholder?: string;
  className?: string;
  height?: number;
  convertImageCallback?: (e: any) => void;
  onImageValidate?: (e: any) => void;
  onChanged: (editorState: EditorState) => void;
  contentHtml?: string;
  ref?: React.ForwardedRef<any>;
}

const CommonEditor: React.FC<CommonEditorProps> = forwardRef((props, ref) => {
  const decorator = new CompositeDecorator([
    {
      strategy: findLinkEntities,
      component: LinkComponent,
    },
    {
      strategy: findImageEntities,
      component: ImageComponent,
    },
  ]);

  const {
    placeholder,
    className,
    height,
    convertImageCallback,
    onImageValidate,
    onChanged,
    contentHtml,
  } = props;

  const initContentState = contentHtml
    ? EditorState.createWithContent(stateFromHTML(contentHtml))
    : EditorState.createEmpty(decorator);
  const [editorState, setEditorState] = useState(initContentState);
  let timeoutId: any;

  const [isLinkPopupOpen, setLinkPopupOpen] = useState(false);
  const [isImagePopupOpen, setImagePopupOpen] = useState(false);

  const editorRef = useRef<any>(null);

  const getCurrentValues = () => {
    const currentEntity = editorState
      ? getSelectionEntity(editorState)
      : undefined;

    if (!currentEntity) {
      return {};
    }

    // TODO: Replace to other editor
    // const contentState = editorState.getCurrentContent();
    // const entityType = contentState.getEntity(currentEntity).getType();

    // let currentValues: {
    //   selectionText: string;
    //   link?: {
    //     target: any;
    //     targetOption: any;
    //     title: string;
    //   };
    // } = {
    //   selectionText: getSelectionText(editorState),
    // };
    // if (entityType === "LINK") {
    //   const entityRange = getEntityRange(editorState, currentEntity);
    //   const contentStateData = contentState.getEntity(currentEntity).getData();

    //   currentValues.link = {
    //     target: contentStateData.url,
    //     targetOption: contentStateData.targetOption,
    //     title: entityRange && entityRange.text,
    //   };
    // }
    // return currentValues;
    return null;
  };

  const onChange = (editorState: EditorState) => {
    if (onChanged) {
      onChanged(editorState);
    }
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
      clearTimeout(timeoutId);
    };
  }, []);

  const handleKeyCommand = (
    command: EditorCommand,
    editorState: EditorState
  ) => {
    const newState = RichUtils.handleKeyCommand(editorState, command);
    if (newState) {
      onChange(newState);
      return "handled";
    }
    return "not-handled";
  };

  const toggleInlineStyle = (style: string) => {
    const newState = RichUtils.toggleInlineStyle(editorState, style);
    if (newState) {
      onChange(newState);
    }
  };

  const toggleBlockType = (style: string) => {
    const newState = RichUtils.toggleBlockType(editorState, style);
    if (newState) {
      onChange(newState);
    }
  };

  const onAddLink = (e: EditorLinkModalAcceptEvent) => {
    const { newEditorState, entityKey } = e;
    onChange(
      RichUtils.toggleLink(
        newEditorState,
        newEditorState.getSelection(),
        entityKey
      )
    );

    timeoutId = setTimeout(() => {
      focus();
    }, 0);
  };

  const removeLink = (e: DefaultButtonToggleEvent) => {
    e.preventDefault();
    const selection = editorState.getSelection();
    if (!selection.isCollapsed()) {
      setEditorState(RichUtils.toggleLink(editorState, selection, null));
    }
  };

  const clearFormat = (newEditorState: EditorState) => {
    onChange(newEditorState);
  };

  const toggleLinkModal = (isOpen: boolean) => {
    if (!isOpen) {
      timeoutId = setTimeout(() => {
        focus();
      }, 0);
    }
    setLinkPopupOpen(!!isOpen);
  };

  const toggleImageModal = (isOpen: boolean) => {
    if (!isOpen) {
      timeoutId = setTimeout(() => {
        focus();
      }, 0);
    }
    setImagePopupOpen(!!isOpen);
  };

  const onAddImage = (newEditorState: any) => {
    onChange(newEditorState);
  };

  useImperativeHandle(ref, () => ({
    clearEditor() {
      const newEditorState = EditorState.push(
        editorState,
        ContentState.createFromText(""),
        "remove-range"
      );
      setEditorState(newEditorState);
    },
  }));

  return (
    <Root className={className}>
      <Container onClick={focusEditor} height={height}>
        <EditorToolbar
          editorState={editorState}
          toggleBlockType={toggleBlockType}
          toggleInlineStyle={toggleInlineStyle}
          styles={STYLES}
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
            placeholder={placeholder ? placeholder : "Nội dung bài viết..."}
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
        onImageValidate={onImageValidate}
      />
    </Root>
  );
});

export { CommonEditor };
