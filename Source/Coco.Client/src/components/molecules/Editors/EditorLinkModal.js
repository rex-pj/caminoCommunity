import React, { Fragment, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styled from "styled-components";
import { ButtonPrimary, ButtonSecondary } from "../../atoms/Buttons/Buttons";
import { Textbox } from "../../atoms/Textboxes";
import { LabelNormal } from "../../atoms/Labels";
import { checkValidity } from "../../../utils/Validity";
import { EditorState, Modifier } from "draft-js";
import { getEntityRange, getSelectionEntity } from "draftjs-utils";

const Root = styled.div`
  position: absolute;
  display: block;
  z-index: 2;
  left: 0;
  top: 0;
  bottom: 0;
  right: 0;
  margin: auto;
  min-width: 100px;
`;

const Container = styled.div`
  max-width: 80%;
  min-width: 400px;
  background: ${p => p.theme.color.white};
  margin: auto;
  border-radius: ${p => p.theme.borderRadius.normal};
  box-shadow: ${p => p.theme.shadow.BoxShadow};
`;

const Body = styled.div`
  height: auto;
  padding: ${p => p.theme.size.tiny} ${p => p.theme.size.distance};
`;

const Footer = styled.div`
  min-height: 20px;
  text-align: right;
  border-top: 1px solid ${p => p.theme.color.light};
  padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.distance};

  button {
    margin-left: ${p => p.theme.size.exTiny};
    font-weight: normal;
    padding: ${p => p.theme.size.exTiny} ${p => p.theme.size.tiny};

    svg {
      margin-right: ${p => p.theme.size.exTiny};
    }
  }
`;

const FormRow = styled.div`
  margin-bottom: ${p => p.theme.size.tiny};

  ${LabelNormal} {
    display: block;
  }

  ${Textbox} {
    width: 100%;
  }
`;

export default props => {
  const { className, isOpen, editorState, currentValue } = props;
  const currentRef = React.createRef();
  const { link, selectionText } = currentValue;

  let formData = {
    url: {
      value: link && link.target ? link.target : "",
      validation: {
        isRequired: true,
        isLink: true
      },
      isValid: false
    },
    title: {
      value: selectionText,
      validation: {
        isRequired: false
      },
      isValid: true
    }
  };

  const [linkData, setLinkData] = useState(formData);

  const handleInputChange = evt => {
    let data = linkData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setLinkData({
      ...data
    });
  };

  const onClose = () => {
    if (props.onClose) {
      clear();
      props.onClose(false);
    }
  };

  const handleKeyUp = e => {
    if (e.key === "Enter") {
      onAccept();
    }
  };

  const addLink = (linkTitle, linkTarget, linkTargetOption) => {
    const currentEntity = editorState
      ? getSelectionEntity(editorState)
      : undefined;
    let selection = editorState.getSelection();

    if (currentEntity) {
      const entityRange = getEntityRange(editorState, currentEntity);
      const isBackward = selection.getIsBackward();
      if (isBackward) {
        selection = selection.merge({
          anchorOffset: entityRange.end,
          focusOffset: entityRange.start
        });
      } else {
        selection = selection.merge({
          anchorOffset: entityRange.start,
          focusOffset: entityRange.end
        });
      }
    }

    const entityKey = editorState
      .getCurrentContent()
      .createEntity("LINK", "MUTABLE", {
        url: linkTarget,
        targetOption: linkTargetOption
      })
      .getLastCreatedEntityKey();

    let contentState = Modifier.replaceText(
      editorState.getCurrentContent(),
      selection,
      `${linkTitle}`,
      editorState.getCurrentInlineStyle(),
      entityKey
    );

    let newEditorState = EditorState.push(
      editorState,
      contentState,
      "insert-characters"
    );

    // insert a blank space after link
    selection = newEditorState.getSelection().merge({
      anchorOffset: selection.get("anchorOffset") + linkTitle.length,
      focusOffset: selection.get("anchorOffset") + linkTitle.length
    });

    newEditorState = EditorState.acceptSelection(newEditorState, selection);
    contentState = Modifier.insertText(
      newEditorState.getCurrentContent(),
      selection,
      " ",
      newEditorState.getCurrentInlineStyle(),
      undefined
    );

    props.onAddLink({
      newEditorState,
      entityKey: "insert-characters"
    });
  };

  const clear = () => {
    let data = formData || {};

    setLinkData({
      ...data
    });
  };

  const onAccept = () => {
    const { url, title } = linkData;
    const isFormValid = url.isValid && url.value;
    if (props.onAddLink && isFormValid) {
      var linkTitle = title.value ? title.value : url.value;
      addLink(linkTitle, url.value, {});
      clear();
      onClose();
    }
  };

  const { title, url } = linkData;
  const isValid = url.isValid;
  return (
    <Fragment>
      {isOpen ? (
        <Root className={className} ref={currentRef}>
          <Container>
            <Body>
              <FormRow>
                <LabelNormal>Tiêu đề</LabelNormal>
                <Textbox
                  name="title"
                  onKeyUp={handleKeyUp}
                  value={title.value}
                  autoComplete="off"
                  onChange={e => handleInputChange(e)}
                />
              </FormRow>
              <FormRow>
                <LabelNormal>Đường dẫn</LabelNormal>
                <Textbox
                  name="url"
                  onKeyUp={handleKeyUp}
                  value={url.value}
                  autoComplete="off"
                  onChange={e => handleInputChange(e)}
                />
              </FormRow>
            </Body>
            <Footer>
              <ButtonSecondary size="sm" onClick={onClose}>
                Đóng
              </ButtonSecondary>
              <ButtonPrimary size="sm" onClick={onAccept} disabled={!isValid}>
                <FontAwesomeIcon icon="check" />
                Lưu
              </ButtonPrimary>
            </Footer>
          </Container>
        </Root>
      ) : null}
    </Fragment>
  );
};
