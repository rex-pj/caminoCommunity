export const styleMap = {
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

export const STYLES = [
  { icon: "bold", style: "BOLD", type: "inline" },
  { icon: "italic", style: "ITALIC", type: "inline" },
  { icon: "underline", style: "UNDERLINE", type: "inline" },
  { icon: "strikethrough", style: "STRIKETHROUGH", type: "inline" },
  { icon: "highlighter", style: "HIGHLIGHT", type: "inline" },
  { icon: "quote-left", style: "blockquote", type: "block" },
  { icon: "list-ul", style: "unordered-list-item", type: "block" },
  { icon: "list-ol", style: "ordered-list-item", type: "block" }
];

export const HEADING_TYPES = [
  { label: "Normal Heading", style: "unstyled" },
  { label: "Heading 1", style: "header-one" },
  { label: "Heading 2", style: "header-two" },
  { label: "Heading 3", style: "header-three" },
  { label: "Heading 4", style: "header-four" },
  { label: "Heading 5", style: "header-five" },
  { label: "Heading 6", style: "header-six" }
];

export const findLinkEntities = (contentBlock, callback, contentState) => {
  contentBlock.findEntityRanges(character => {
    const entityKey = character.getEntity();
    return (
      entityKey !== null &&
      contentState.getEntity(entityKey).getType() === "LINK"
    );
  }, callback);
};

export const findImageEntities = (contentBlock, callback, contentState) => {
  contentBlock.findEntityRanges(character => {
    const entityKey = character.getEntity();
    return (
      entityKey !== null &&
      contentState.getEntity(entityKey).getType() === "IMAGE"
    );
  }, callback);
};
