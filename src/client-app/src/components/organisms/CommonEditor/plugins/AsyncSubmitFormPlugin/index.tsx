/**
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */

import * as React from "react";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { CLEAR_EDITOR_COMMAND } from "lexical";

interface Props extends React.FormHTMLAttributes<HTMLFormElement> {
  clearAfterSubmit?: boolean;
  onSubmitAsync: (e: React.FormEvent<HTMLFormElement>) => Promise<any>;
}

export default function AsyncSubmitFormPlugin(props: Props): JSX.Element {
  const [editor] = useLexicalComposerContext();
  const { children, method, clearAfterSubmit } = props;

  const onSubmitAsync: (
    e: React.FormEvent<HTMLFormElement>
  ) => Promise<any> = async (e: React.FormEvent<HTMLFormElement>) => {
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
