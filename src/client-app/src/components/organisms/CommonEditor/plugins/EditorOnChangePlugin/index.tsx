import { OnChangePlugin } from "@lexical/react/LexicalOnChangePlugin";
import { LexicalEditor } from "lexical";

interface Props {
  onChange: (editor: LexicalEditor) => void;
}

const EditorOnChangePlugin = (props: Props) => {
  const onChange = (editor: LexicalEditor) => {
    props.onChange(editor);
  };

  return (
    <OnChangePlugin
      onChange={(editorState, editor) => {
        editorState.read(() => {
          onChange(editor);
        });
      }}
    />
  );
};

export default EditorOnChangePlugin;
