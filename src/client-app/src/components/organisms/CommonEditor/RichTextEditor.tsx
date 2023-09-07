/**
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */

import * as React from "react";
import { AutoFocusPlugin } from "@lexical/react/LexicalAutoFocusPlugin";
import { ClearEditorPlugin } from "@lexical/react/LexicalClearEditorPlugin";
import { CharacterLimitPlugin } from "@lexical/react/LexicalCharacterLimitPlugin";
import { CheckListPlugin } from "@lexical/react/LexicalCheckListPlugin";
import AutoEmbedPlugin from "./plugins/AutoEmbedPlugin";
import YouTubePlugin from "./plugins/YouTubePlugin";
import LexicalErrorBoundary from "@lexical/react/LexicalErrorBoundary";
import { HashtagPlugin } from "@lexical/react/LexicalHashtagPlugin";
import { HistoryPlugin } from "@lexical/react/LexicalHistoryPlugin";
import { HorizontalRulePlugin } from "@lexical/react/LexicalHorizontalRulePlugin";
import { ListPlugin } from "@lexical/react/LexicalListPlugin";
import { PlainTextPlugin } from "@lexical/react/LexicalPlainTextPlugin";
import { RichTextPlugin } from "@lexical/react/LexicalRichTextPlugin";
import { TabIndentationPlugin } from "@lexical/react/LexicalTabIndentationPlugin";
import { TablePlugin } from "@lexical/react/LexicalTablePlugin";
import { useEffect, useRef, useState } from "react";
import { CAN_USE_DOM } from "./shared/canUseDOM";
import { useSettings } from "./context/SettingsContext";
import { useSharedHistoryContext } from "./context/SharedHistoryContext";
import TableCellNodes from "./nodes/TableCellNodes";
import AutocompletePlugin from "./plugins/AutocompletePlugin";
import AutoLinkPlugin from "./plugins/AutoLinkPlugin";
import CodeHighlightPlugin from "./plugins/CodeHighlightPlugin";
import CollapsiblePlugin from "./plugins/CollapsiblePlugin";
import ComponentPickerPlugin from "./plugins/ComponentPickerPlugin";
import DragDropPaste from "./plugins/DragDropPastePlugin";
import DraggableBlockPlugin from "./plugins/DraggableBlockPlugin";
import EmojiPickerPlugin from "./plugins/EmojiPickerPlugin";
import EmojisPlugin from "./plugins/EmojisPlugin";
import FloatingLinkEditorPlugin from "./plugins/FloatingLinkEditorPlugin";
import FloatingTextFormatToolbarPlugin from "./plugins/FloatingTextFormatToolbarPlugin";
import ImagesPlugin from "./plugins/ImagesPlugin";
import KeywordsPlugin from "./plugins/KeywordsPlugin";
import LinkPlugin from "./plugins/LinkPlugin";
import ListMaxIndentLevelPlugin from "./plugins/ListMaxIndentLevelPlugin";
import { MaxLengthPlugin } from "./plugins/MaxLengthPlugin";
import MentionsPlugin from "./plugins/MentionsPlugin";
import PollPlugin from "./plugins/PollPlugin";
import TabFocusPlugin from "./plugins/TabFocusPlugin";
import TableCellActionMenuPlugin from "./plugins/TableActionMenuPlugin";
import TableCellResizer from "./plugins/TableCellResizer";
import TableOfContentsPlugin from "./plugins/TableOfContentsPlugin";
import { TablePlugin as NewTablePlugin } from "./plugins/TablePlugin";
import ToolbarPlugin from "./plugins/ToolbarPlugin";
import PlaygroundEditorTheme from "./themes/PlaygroundEditorTheme";
import ContentEditable from "./ui/ContentEditable";
import Placeholder from "./ui/Placeholder";
import ActionsPlugin from "./plugins/ActionPlugin";
import EditorOnChangePlugin from "./plugins/EditorOnChangePlugin";
import { $generateNodesFromDOM } from "@lexical/html";
import { $insertNodes, LexicalEditor } from "lexical";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";

interface Props {
  value?: string;
  onChange: (editor: LexicalEditor) => void;
}

const RichTextEditor = React.forwardRef((props: Props, ref: any) => {
  const { historyState } = useSharedHistoryContext();
  const {
    settings: { isCollab, isAutocomplete, isMaxLength, isCharLimit, isCharLimitUtf8, isRichText, showTableOfContents, tableCellMerge, tableCellBackgroundColor },
  } = useSettings();

  const getPlaceholder = () => {
    if (isCollab) {
      return "Enter some collaborative rich text...";
    }

    if (isRichText) {
      return "Enter some rich text...";
    }

    return "Enter some plain text...";
  };

  const placeholder = <Placeholder>{getPlaceholder()}</Placeholder>;
  const [floatingAnchorElem, setFloatingAnchorElem] = useState<HTMLDivElement | null>(null);
  const [isSmallWidthViewport, setIsSmallWidthViewport] = useState<boolean>(false);
  const initialRef = useRef<boolean>(true);

  const [editor] = useLexicalComposerContext();

  const onRef = (_floatingAnchorElem: HTMLDivElement) => {
    if (_floatingAnchorElem !== null) {
      setFloatingAnchorElem(_floatingAnchorElem);
    }
  };

  const cellEditorConfig = {
    namespace: "Playground",
    nodes: [...TableCellNodes],
    onError: (error: Error) => {
      throw error;
    },
    theme: PlaygroundEditorTheme,
  };

  useEffect(() => {
    const updateViewPortWidth = () => {
      const isNextSmallWidthViewport = CAN_USE_DOM && window.matchMedia("(max-width: 1025px)").matches;

      if (isNextSmallWidthViewport !== isSmallWidthViewport) {
        setIsSmallWidthViewport(isNextSmallWidthViewport);
      }
    };
    updateViewPortWidth();
    window.addEventListener("resize", updateViewPortWidth);

    return () => {
      window.removeEventListener("resize", updateViewPortWidth);
    };
  }, [isSmallWidthViewport]);

  useEffect(() => {
    const { value } = props;
    if (!value || !initialRef.current) {
      return;
    }

    editor.update(() => {
      const parser = new DOMParser();
      var aaa = `<p class="PlaygroundEditorTheme__paragraph" dir="ltr"><span>Phasellus purus quam, laoreet eu tristique a, fermentum vel lacus. In id felis nisi. Phasellus metus elit, vulputate eu egestas vel, sollicitudin vitae mauris. Aliquam mattis tortor eu ligula pulvinar, blandit rutrum turpis mattis. Vivamus scelerisque ante eu turpis facilisis bibendum. Fusce scelerisque, nulla et commodo pharetra, metus nunc lobortis sapien, ac iaculis orci quam vitae lectus. Nullam aliquam fringilla laoreet. Sed sit amet arcu at dolor ultrices lobortis. In porta viverra neque ac convallis. Nunc eu feugiat turpis. Nulla vel ullamcorper magna, vel semper lectus. Fusce condimentum sit amet elit id consectetur.</span></p><p class="PlaygroundEditorTheme__paragraph" dir="ltr"><br/></p><p class="PlaygroundEditorTheme__paragraph" dir="ltr"><span>Phasellus purus quam, laoreet eu tristique a, fermentum vel lacus. In id felis nisi. Phasellus metus elit, vulputate eu egestas vel, sollicitudin vitae mauris. Aliquam mattis tortor eu ligula pulvinar, blandit rutrum turpis mattis. Vivamus scelerisque ante eu turpis facilisis bibendum. Fusce scelerisque, nulla et commodo pharetra, metus nunc lobortis sapien, ac iaculis orci quam vitae lectus. Nullam aliquam fringilla laoreet. Sed sit amet arcu at dolor ultrices lobortis. In porta viverra neque ac convallis. Nunc eu feugiat turpis. Nulla vel ullamcorper magna, vel semper lectus. Fusce condimentum sit amet elit id consectetur.</span></p>`;
      const dom = parser.parseFromString(aaa, "text/html");
      const nodes = $generateNodesFromDOM(editor, dom);
      $insertNodes(nodes);
      initialRef.current = false;
    });
  }, [initialRef, editor]);

  return (
    <div ref={ref}>
      {isRichText && <ToolbarPlugin />}
      <div className={`editor-container ${!isRichText ? "plain-text" : ""}`}>
        {isMaxLength && <MaxLengthPlugin maxLength={30} />}
        <DragDropPaste />
        <AutoFocusPlugin />
        <ClearEditorPlugin />
        <ComponentPickerPlugin />
        <EmojiPickerPlugin />
        <MentionsPlugin />
        <AutoEmbedPlugin />
        <YouTubePlugin />
        <EmojisPlugin />
        <HashtagPlugin />
        <KeywordsPlugin />
        <AutoLinkPlugin />
        {isRichText ? (
          <>
            <HistoryPlugin externalHistoryState={historyState} />
            <RichTextPlugin
              contentEditable={
                <div className="editor-scroller">
                  <div className="editor" ref={onRef}>
                    <ContentEditable />
                  </div>
                </div>
              }
              placeholder={placeholder}
              ErrorBoundary={LexicalErrorBoundary}
            />
            <CodeHighlightPlugin />
            <ListPlugin />
            <CheckListPlugin />
            <ListMaxIndentLevelPlugin maxDepth={7} />
            <TablePlugin hasCellMerge={tableCellMerge} hasCellBackgroundColor={tableCellBackgroundColor} />
            <TableCellResizer />
            <NewTablePlugin cellEditorConfig={cellEditorConfig}>
              <AutoFocusPlugin />
              <RichTextPlugin contentEditable={<ContentEditable className="TableNode__contentEditable" />} placeholder={null} ErrorBoundary={LexicalErrorBoundary} />
              <MentionsPlugin />
              <HistoryPlugin />
              <ImagesPlugin captionsEnabled={false} />
              <LinkPlugin />
              <FloatingTextFormatToolbarPlugin />
            </NewTablePlugin>
            <ImagesPlugin />
            <LinkPlugin />
            <PollPlugin />
            <HorizontalRulePlugin />
            <TabFocusPlugin />
            <TabIndentationPlugin />
            <CollapsiblePlugin />
            {floatingAnchorElem && !isSmallWidthViewport && (
              <>
                <DraggableBlockPlugin anchorElem={floatingAnchorElem} />
                <FloatingLinkEditorPlugin anchorElem={floatingAnchorElem} />
                <TableCellActionMenuPlugin anchorElem={floatingAnchorElem} cellMerge={true} />
                <FloatingTextFormatToolbarPlugin anchorElem={floatingAnchorElem} />
              </>
            )}
          </>
        ) : (
          <>
            <PlainTextPlugin contentEditable={<ContentEditable />} placeholder={placeholder} ErrorBoundary={LexicalErrorBoundary} />
            <HistoryPlugin externalHistoryState={historyState} />
          </>
        )}
        {(isCharLimit || isCharLimitUtf8) && <CharacterLimitPlugin charset={isCharLimit ? "UTF-16" : "UTF-8"} maxLength={5} />}
        {isAutocomplete && <AutocompletePlugin />}
        <div>{showTableOfContents && <TableOfContentsPlugin />}</div>
        <ActionsPlugin />
        <EditorOnChangePlugin onChange={props.onChange} />
      </div>
    </div>
  );
});

export { RichTextEditor };
