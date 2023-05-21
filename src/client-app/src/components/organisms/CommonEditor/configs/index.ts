import { InitialConfigType } from "@lexical/react/LexicalComposer";
import PlaygroundNodes from "../nodes/PlaygroundNodes";
import PlaygroundEditorTheme from "../themes/PlaygroundEditorTheme";

export const defaultEditorConfigs: InitialConfigType = {
  editorState: null,
  namespace: "Playground",
  nodes: [...PlaygroundNodes],
  onError: (error: Error) => {
    throw error;
  },
  theme: PlaygroundEditorTheme,
};
