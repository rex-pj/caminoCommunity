declare module "draftjs-utils" {
  import {
    BlockMap,
    ContentBlock,
    Editor,
    EditorState,
    Entity,
  } from "draft-js";
  import { OrderedMap, List } from "immutable";
  /**
   * The function will return an Immutable OrderedMap of currently selected Blocks.
   * @param state
   */
  export function getSelectedBlocksMap(state: EditorState): BlockMap;

  /**
   * The function will return an Immutable List of currently selected Blocks.
   * @param state
   */
  export function getSelectedBlocksList(state: EditorState): List<ContentBlock>;

  /**
   * The function will return first of currently selected Blocks, this is more useful when we expect user to select only one Block.
   * @param state
   */
  export function getSelectedBlock(state: EditorState): ContentBlock;

  /**
   * The function will return block just before the selected block(s).
   * @param state
   */
  export function getBlockBeforeSelectedBlock(state: EditorState): ContentBlock;

  /**
   * The function will return all the Blocks of the editor.
   * @param state
   */
  export function getAllBlocks(state: EditorState): List<ContentBlock>;

  /**
   * The function will return the type of currently selected Blocks. The type is a simple string. It will return undefined if not all selected Blocks have same type.
   * @param state
   */
  export function getSelectedBlocksType(state: EditorState): string | undefined;

  /**
   * The function will reset the type of selected Blocks to unstyled.
   * @param state
   */
  export function removeSelectedBlocksStyle(state: EditorState): EditorState;

  /**
   * The function will return plain text of current selection.
   * @param state
   */
  export function getSelectionText(state: EditorState): string;

  /**
   * The function will replace currently selected text with a \n.
   * @param state
   */
  export function addLineBreakRemovingSelection(
    state: EditorState
  ): EditorState;

  /**
   * The function will add a new unstyled Block and copy current selection to it.
   * @param state
   */
  export function insertNewUnstyledBlock(state: EditorState): EditorState;

  /**
   * The function will clear all content from the Editor.
   * @param state
   */
  export function clearEditorContent(state: EditorState): EditorState;

  /**
   * The function will return inline style applicable to current selection. The function will return only those styles that are applicable to whole selection.
   * @param state
   */
  export function getSelectionInlineStyle(
    state: EditorState
  ): Record<string, any>;

  /**
   * The function will add block level meta-data.
   * @param state
   * @param obj
   */
  export function setBlockData(state: EditorState, obj: any): EditorState;

  /**
   * The function will return map of block data of current block.
   * @param state
   */
  export function getSelectedBlocksMetadata(
    state: EditorState
  ): Map<string, any>;

  /**
   * The function will return map of block types Block Type -> HTML Tag.
   */
  export function blockRenderMap(): Map<string, string>;

  /**
   * The function will return the Entity of current selection. Entity can not span multiple Blocks, method will check only first selected Block.
   * @param state
   */
  export function getSelectionEntity(state: EditorState): Entity;

  /**
   * The function will return the range of given Entity in currently selected Block. Entity can not span multiple Blocks, method will check only first selected Block.
   * @param state
   */
  export function getEntityRange(state: EditorState): Entity;

  /**
   * The function will handle newline event in editor gracefully, it will insert \n for soft-new lines and remove selected text if any.
   * @param state
   */
  export function handleNewLine(state: EditorState): [EditorState, Event];

  /**
   * The function will return true is type of block is 'unordered-list-item' or 'ordered-list-item'.
   * @param contentBlock
   */
  export function isListBlock(contentBlock: ContentBlock): boolean;

  /**
   * Change the depth of selected Blocks by adjustment if its less than maxdepth.
   * @param state
   * @param adjustment
   * @param maxDepth
   */
  export function changeDepth(
    state: EditorState,
    adjustment: number,
    maxDepth: number
  ): EditorState;

  /**
   * Function will return Map of custom inline styles applicable to current selection.
   * @param state
   * @param styles
   */
  export function getSelectionCustomInlineStyle(
    state: EditorState,
    styles: string[]
  ): Record<string, any>;

  /**
   * Toggle application of custom inline style to current selection.
   * @param state
   * @param styleType
   * @param styleValue
   */
  export function toggleCustomInlineStyle(
    state: EditorState,
    styleType: string,
    styleValue: string
  ): EditorState;

  /**
   * The function will remove all inline styles of current selection.
   * @param state
   */
  export function removeAllInlineStyles(state: EditorState): EditorState;
}
