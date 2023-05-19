/**
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */

import * as React from "react";
import { AutoFocusPlugin } from "@lexical/react/LexicalAutoFocusPlugin";
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
import { LexicalComposer } from "@lexical/react/LexicalComposer";
import { useEffect, useState } from "react";
import { CAN_USE_DOM } from "./shared/canUseDOM";

import { useSettings } from "./context/SettingsContext";
import {
  SharedHistoryContext,
  useSharedHistoryContext,
} from "./context/SharedHistoryContext";
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
import {
  TablePlugin as NewTablePlugin,
  TableContext,
} from "./plugins/TablePlugin";
import ToolbarPlugin from "./plugins/ToolbarPlugin";
import PlaygroundEditorTheme from "./themes/PlaygroundEditorTheme";
import ContentEditable from "./ui/ContentEditable";
import Placeholder from "./ui/Placeholder";
import { SharedAutocompleteContext } from "./context/SharedAutocompleteContext";
import PlaygroundNodes from "./nodes/PlaygroundNodes";
import { $createHeadingNode, $createQuoteNode } from "@lexical/rich-text";
import { $createParagraphNode, $createTextNode, $getRoot } from "lexical";

const RichEditor = () => {
  const { historyState } = useSharedHistoryContext();
  const {
    settings: {
      isCollab,
      isAutocomplete,
      isMaxLength,
      isCharLimit,
      isCharLimitUtf8,
      isRichText,
      showTreeView,
      showTableOfContents,
      tableCellMerge,
      tableCellBackgroundColor,
      emptyEditor,
    },
  } = useSettings();
  const text = isCollab
    ? "Enter some collaborative rich text..."
    : isRichText
    ? "Enter some rich text..."
    : "Enter some plain text...";
  const placeholder = <Placeholder>{text}</Placeholder>;
  const [floatingAnchorElem, setFloatingAnchorElem] =
    useState<HTMLDivElement | null>(null);
  const [isSmallWidthViewport, setIsSmallWidthViewport] =
    useState<boolean>(false);

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
      const isNextSmallWidthViewport =
        CAN_USE_DOM && window.matchMedia("(max-width: 1025px)").matches;

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

  function prepopulatedRichText() {
    const root = $getRoot();
    if (root.getFirstChild() === null) {
      const heading = $createHeadingNode("h1");
      heading.append($createTextNode("Welcome to the playground"));
      root.append(heading);
      const quote = $createQuoteNode();
      quote.append(
        $createTextNode(
          `In case you were wondering what the black box at the bottom is – it's the debug view, showing the current state of the editor. ` +
            `You can disable it by pressing on the settings control in the bottom-left of your screen and toggling the debug view setting.`
        )
      );
      root.append(quote);
      const paragraph = $createParagraphNode();
      paragraph.append(
        $createTextNode("The playground is a demo environment built with "),
        $createTextNode("@lexical/react").toggleFormat("code"),
        $createTextNode("."),
        $createTextNode(" Try typing in "),
        $createTextNode("some text").toggleFormat("bold"),
        $createTextNode(" with "),
        $createTextNode("different").toggleFormat("italic"),
        $createTextNode(" formats.")
      );
      root.append(paragraph);
      const paragraph2 = $createParagraphNode();
      paragraph2.append(
        $createTextNode(
          "Make sure to check out the various plugins in the toolbar. You can also use #hashtags or @-mentions too!"
        )
      );
      root.append(paragraph2);
      const paragraph3 = $createParagraphNode();
      paragraph3.append(
        $createTextNode(
          `If you'd like to find out more about Lexical, you can:`
        )
      );
      root.append(paragraph3);
      const list = $createTextNode("bullet");
      root.append(list);
      const paragraph4 = $createParagraphNode();
      paragraph4.append(
        $createTextNode(
          `Lastly, we're constantly adding cool new features to this playground. So make sure you check back here when you next get a chance :).`
        )
      );
      root.append(paragraph4);
    }
  }

  const editorConfig = {
    editorState: isCollab
      ? null
      : emptyEditor
      ? undefined
      : prepopulatedRichText,
    namespace: "Playground",
    nodes: [...PlaygroundNodes],
    onError: (error: Error) => {
      throw error;
    },
    theme: PlaygroundEditorTheme,
  };

  return (
    <LexicalComposer initialConfig={editorConfig}>
      <SharedHistoryContext>
        <TableContext>
          <SharedAutocompleteContext>
            {isRichText && <ToolbarPlugin />}
            <div
              className={`editor-container ${showTreeView ? "tree-view" : ""} ${
                !isRichText ? "plain-text" : ""
              }`}
            >
              {isMaxLength && <MaxLengthPlugin maxLength={30} />}
              <DragDropPaste />
              <AutoFocusPlugin />
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
                  <TablePlugin
                    hasCellMerge={tableCellMerge}
                    hasCellBackgroundColor={tableCellBackgroundColor}
                  />
                  <TableCellResizer />
                  <NewTablePlugin cellEditorConfig={cellEditorConfig}>
                    <AutoFocusPlugin />
                    <RichTextPlugin
                      contentEditable={
                        <ContentEditable className="TableNode__contentEditable" />
                      }
                      placeholder={null}
                      ErrorBoundary={LexicalErrorBoundary}
                    />
                    <MentionsPlugin />
                    <HistoryPlugin />
                    <ImagesPlugin captionsEnabled={false} />
                    <LinkPlugin />
                    {/* <LexicalClickableLinkPlugin /> */}
                    <FloatingTextFormatToolbarPlugin />
                  </NewTablePlugin>
                  <ImagesPlugin />
                  <LinkPlugin />
                  <PollPlugin />
                  {/* {!isEditable && <LexicalClickableLinkPlugin />} */}
                  <HorizontalRulePlugin />
                  <TabFocusPlugin />
                  <TabIndentationPlugin />
                  <CollapsiblePlugin />
                  {floatingAnchorElem && !isSmallWidthViewport && (
                    <>
                      <DraggableBlockPlugin anchorElem={floatingAnchorElem} />
                      <FloatingLinkEditorPlugin
                        anchorElem={floatingAnchorElem}
                      />
                      <TableCellActionMenuPlugin
                        anchorElem={floatingAnchorElem}
                        cellMerge={true}
                      />
                      <FloatingTextFormatToolbarPlugin
                        anchorElem={floatingAnchorElem}
                      />
                    </>
                  )}
                </>
              ) : (
                <>
                  <PlainTextPlugin
                    contentEditable={<ContentEditable />}
                    placeholder={placeholder}
                    ErrorBoundary={LexicalErrorBoundary}
                  />
                  <HistoryPlugin externalHistoryState={historyState} />
                </>
              )}
              {(isCharLimit || isCharLimitUtf8) && (
                <CharacterLimitPlugin
                  charset={isCharLimit ? "UTF-16" : "UTF-8"}
                  maxLength={5}
                />
              )}
              {isAutocomplete && <AutocompletePlugin />}
              <div>{showTableOfContents && <TableOfContentsPlugin />}</div>
            </div>
          </SharedAutocompleteContext>
        </TableContext>
      </SharedHistoryContext>
    </LexicalComposer>
  );
};

export { RichEditor };
