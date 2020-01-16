import React from "react";
import styled from "styled-components";
import { DefaultButton } from "./EditorButtons";
import EditorDropdown from "./EditorDropdown";

const Toolbar = styled.div`
  padding: ${p => p.theme.size.tiny};
  border-bottom: 1px solid ${p => p.theme.color.light};
  background-color: ${p => p.theme.color.lighter};
`;

const EditorButton = styled(DefaultButton)`
  margin-right: 3px;
  :hover {
    background-color: ${p => p.theme.color.light};
  }
`;

const Divide = styled.span`
  display: inline-block;
  border-right: 1px solid ${p => p.theme.color.light};
  height: 20px;
  width: 0px;
  margin: 0 6px 0 3px;
  vertical-align: middle;
`;

const SelectHeading = styled(EditorDropdown)`
  background-color: ${p => p.theme.rgbaColor.light};
  color: ${p => p.theme.color.primaryLight};
  font-weight: 600;

  :hover {
    background-color: ${p => p.theme.color.light};
  }
`;

export default props => {
  const { editorState, inlineTyles, blockTyles, headingTypes } = props;

  const toggleBlockType = e => {
    const value = e.target.value;
    props.toggleBlockType(value);
  };

  const toggleInlineStyle = e => {
    const value = e.target.value;
    props.toggleInlineStyle(value);
  };

  const currentStyle = editorState.getCurrentInlineStyle();
  const selection = editorState.getSelection();
  const blockType = editorState
    .getCurrentContent()
    .getBlockForKey(selection.getStartKey())
    .getType();

  return (
    <Toolbar>
      {blockTyles.map(type => (
        <EditorButton
          key={type.style}
          actived={type.style === blockType}
          label={type.label}
          icon={type.icon}
          onToggle={toggleBlockType}
          style={type.style}
        />
      ))}
      <Divide />
      {inlineTyles.map(type => (
        <EditorButton
          key={type.style}
          actived={currentStyle.has(type.style)}
          label={type.label}
          icon={type.icon}
          onToggle={toggleInlineStyle}
          style={type.style}
        />
      ))}
      <Divide />
      <SelectHeading
        options={headingTypes}
        actived={blockType}
        onToggle={toggleBlockType}
        placeholder="Heading Styles"
      />
    </Toolbar>
  );
};
