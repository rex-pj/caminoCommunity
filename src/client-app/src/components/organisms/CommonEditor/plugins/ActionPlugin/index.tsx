/**
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */

import * as React from "react";
import {
  $getSelection,
  $isRangeSelection,
  $isTextNode,
  LexicalEditor,
  $getRoot,
  $isParagraphNode,
  $createParagraphNode,
  CLEAR_EDITOR_COMMAND,
} from "lexical";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { $getNearestBlockElementAncestorOrThrow } from "@lexical/utils";
import { $isHeadingNode, $isQuoteNode } from "@lexical/rich-text";
import { useCallback, useEffect, useState } from "react";
import useModal from "../../hooks/useModal";
import Button from "../../ui/Button";
import { $isDecoratorBlockNode } from "@lexical/react/LexicalDecoratorBlockNode";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function ActionsPlugin(): JSX.Element {
  const [editor] = useLexicalComposerContext();
  const [activeEditor] = useState(editor);
  const [isEditorEmpty, setIsEditorEmpty] = useState(true);
  const [modal, showModal] = useModal();

  useEffect(() => {
    return editor.registerUpdateListener(() => {
      // If we are in read only mode, send the editor state
      // to server and ask for validation if possible.
      editor.getEditorState().read(() => {
        const root = $getRoot();
        const children = root.getChildren();

        if (children.length > 1) {
          setIsEditorEmpty(false);
        } else if ($isParagraphNode(children[0])) {
          const paragraphChildren = children[0].getChildren();
          setIsEditorEmpty(paragraphChildren.length === 0);
        } else {
          setIsEditorEmpty(false);
        }
      });
    });
  }, [editor]);

  const clearFormatting = useCallback(() => {
    activeEditor.update(() => {
      const selection = $getSelection();
      if (!$isRangeSelection(selection)) {
        return;
      }

      const anchor = selection.anchor;
      const focus = selection.focus;
      const nodes = selection.getNodes();

      if (anchor.key === focus.key && anchor.offset === focus.offset) {
        return;
      }

      nodes.forEach((node, idx) => {
        // We split the first and last node by the selection
        // So that we don't format unselected text inside those nodes
        if ($isTextNode(node)) {
          if (idx === 0 && anchor.offset !== 0) {
            node = node.splitText(anchor.offset)[1] || node;
          }
          if (idx === nodes.length - 1) {
            node = node.splitText(focus.offset)[0] || node;
          }

          if (node.__style !== "") {
            node.setStyle("");
          }
          if (node.__format !== 0) {
            node.setFormat(0);
            $getNearestBlockElementAncestorOrThrow(node).setFormat("");
          }
        } else if ($isHeadingNode(node) || $isQuoteNode(node)) {
          node.replace($createParagraphNode(), true);
        } else if ($isDecoratorBlockNode(node)) {
          node.setFormat("");
        }
      });
    });
  }, [activeEditor]);

  return (
    <div className="actions">
      <button
        type="button"
        className="action-button clear"
        disabled={isEditorEmpty}
        onClick={clearFormatting}
        title="Clear text formatting"
        aria-label="Clear all text formatting"
      >
        <FontAwesomeIcon icon="eraser"></FontAwesomeIcon>
      </button>
      <button
        type="button"
        className="action-button clear"
        disabled={isEditorEmpty}
        onClick={() => {
          showModal("Clear editor", (onClose) => (
            <ShowClearDialog editor={editor} onClose={onClose} />
          ));
        }}
        title="Clear"
        aria-label="Clear editor contents"
      >
        <FontAwesomeIcon icon="trash-alt"></FontAwesomeIcon>
      </button>
      {modal}
    </div>
  );
}

function ShowClearDialog({
  editor,
  onClose,
}: {
  editor: LexicalEditor;
  onClose: () => void;
}): JSX.Element {
  return (
    <>
      Are you sure you want to clear the editor?
      <div className="Modal__content">
        <Button
          onClick={() => {
            editor.dispatchCommand(CLEAR_EDITOR_COMMAND, undefined);
            editor.focus();
            onClose();
          }}
        >
          Clear
        </Button>{" "}
        <Button
          onClick={() => {
            editor.focus();
            onClose();
          }}
        >
          Cancel
        </Button>
      </div>
    </>
  );
}
