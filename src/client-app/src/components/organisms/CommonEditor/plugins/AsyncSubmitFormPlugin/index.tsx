/**
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */

import * as React from "react";
import { FormHTMLAttributes } from "react";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { CLEAR_EDITOR_COMMAND } from "lexical";

interface Props extends FormHTMLAttributes<HTMLFormElement> {
  clearAfterSubmit?: boolean;
  onSubmitAsync: (e: any) => Promise<any>;
}

export default function AsyncSubmitFormPlugin(props: Props): JSX.Element {
  const [editor] = useLexicalComposerContext();
  const { children, method, clearAfterSubmit } = props;

  const onSubmitAsync: (e: any) => Promise<any> = async (e: any) => {
    return props.onSubmitAsync(e).then(() => {
      if (clearAfterSubmit) {
        editor.dispatchCommand(CLEAR_EDITOR_COMMAND, undefined);
        editor.focus();
      }
    });
  };

  return (
    <form onSubmit={(e) => onSubmitAsync(e)} method={method}>
      {children}
    </form>
  );
}
