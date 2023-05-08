import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { ContentBlock, ContentState } from "draft-js";

export const styleMap = {
  CODE: {
    backgroundColor: "rgba(0, 0, 0, 0.5)",
    fontFamily: '"Inconsolata", "Menlo", "Consolas", monospace',
    fontSize: 16,
    padding: 2,
  },
  HIGHLIGHT: {
    background: "#fffe0d",
  },
};

export interface StyleProps {
  icon?: IconProp;
  style?: string;
  type?:
    | "inline"
    | "block"
    | "image"
    | "link"
    | "unlink"
    | "divide"
    | "headingStyle"
    | "eraser";
}

export const STYLES: StyleProps[] = [
  { icon: "bold", style: "BOLD", type: "inline" },
  { icon: "italic", style: "ITALIC", type: "inline" },
  { icon: "underline", style: "UNDERLINE", type: "inline" },
  { icon: "strikethrough", style: "STRIKETHROUGH", type: "inline" },
  { icon: "highlighter", style: "HIGHLIGHT", type: "inline" },
  { icon: "quote-left", style: "blockquote", type: "block" },
  { icon: "list-ul", style: "unordered-list-item", type: "block" },
  { icon: "list-ol", style: "ordered-list-item", type: "block" },
  { type: "divide" },
  { icon: "image", type: "image" },
  { icon: "link", type: "link" },
  { icon: "unlink", type: "unlink" },
  { type: "headingStyle" },
  { icon: "eraser", type: "eraser" },
];

export interface HeadingStyleProps {
  label?: string;
  style?:
    | "unstyled"
    | "header-one"
    | "header-two"
    | "header-three"
    | "header-four"
    | "header-five"
    | "header-six";
}

export const HEADING_TYPES: HeadingStyleProps[] = [
  { label: "Normal Heading", style: "unstyled" },
  { label: "Heading 1", style: "header-one" },
  { label: "Heading 2", style: "header-two" },
  { label: "Heading 3", style: "header-three" },
  { label: "Heading 4", style: "header-four" },
  { label: "Heading 5", style: "header-five" },
  { label: "Heading 6", style: "header-six" },
];

export const findLinkEntities = (
  contentBlock: ContentBlock,
  callback: (start: number, end: number) => void,
  contentState: ContentState
) => {
  contentBlock.findEntityRanges((character) => {
    const entityKey = character.getEntity();
    return (
      entityKey !== null &&
      contentState.getEntity(entityKey).getType() === "LINK"
    );
  }, callback);
};

export const findImageEntities = (
  contentBlock: ContentBlock,
  callback: (start: number, end: number) => void,
  contentState: ContentState
) => {
  contentBlock.findEntityRanges((character) => {
    const entityKey = character.getEntity();
    return (
      entityKey !== null &&
      contentState.getEntity(entityKey).getType() === "IMAGE"
    );
  }, callback);
};
