import React, { Fragment, useState } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { TextArea } from "../../atoms/TextAreas";
import {
  ButtonOutlinePrimary,
  ButtonOutlineNormal
} from "../../atoms/Buttons/OutlineButtons";

const Wrap = styled.div`
  ${TextArea} {
    margin-bottom: 0;
    vertical-align: middle;
  }

  button {
    vertical-align: bottom;
    margin-left: ${p => p.theme.size.tiny};
  }
`;

const TextLabel = styled.div`
  display: inline-block;

  &.can-edit {
    border-bottom: 1px dashed ${p => p.theme.color.neutral};
    line-height: ${p => p.theme.size.normal};
  }

  &.empty {
    color: ${p => p.theme.color.danger};
    font-weight: 400;
  }
`;

const TextEditing = styled(TextArea)`
  border: 0;
  border-bottom: 1px dashed ${p => p.theme.color.neutral};
  border-radius: 0;
  max-width: 50%;
`;

export default function(props) {
  const [isOpen, setIsOpen] = useState(false);
  const [value, setValue] = useState(props.value ? props.value : "");

  function openTextBox() {
    setIsOpen(true);
  }

  function closeTextBox() {
    setIsOpen(false);
  }

  async function onClickUpdate() {
    await pushData();
  }

  async function onEnterUpdate(e) {
    if (e.keyCode === 13 && props.onUpdated) {
      await pushData();
    } else if (e.keyCode === 13) {
      closeTextBox();
      setValue(value);
    } else if (e.keyCode === 27) {
      closeTextBox();
      const currentValue = props.value ? props.value : "";
      setValue(currentValue);
    }
  }

  async function pushData() {
    const currentValue = props.value ? props.value : "";

    let newValue = value;

    if (!!props.primaryKey && !!props.name && currentValue !== newValue) {
      await props
        .onUpdated({
          primaryKey: props.primaryKey,
          value: newValue,
          propertyName: props.name
        })
        .then(function(response) {
          const { data } = response;
          const { updateUserInfoItem } = data;
          const { result } = updateUserInfoItem;
          setValue(result.value);
        })
        .catch(function(errors) {
          setValue(currentValue);
          console.log(errors);
        });
    }

    closeTextBox();
  }

  function onBlur() {
    cancelEdit();
  }

  function cancelEdit() {
    closeTextBox();
    setValue(value);
  }

  function onChange(e) {
    setValue(e.target.value);
  }

  let emptyText = "Empty";
  if (props.emptyText) {
    emptyText = props.emptyText;
  }

  if (!props.disabled && !!isOpen) {
    return (
      <Wrap>
        {props.enterByKey ? (
          <TextEditing
            rows={props.rows}
            cols={props.cols}
            name={props.name}
            value={value ? value : ""}
            autoFocus={true}
            onKeyUp={onEnterUpdate}
            onChange={onChange}
            onBlur={onBlur}
          />
        ) : (
          <TextEditing
            rows={props.rows}
            cols={props.cols}
            name={props.name}
            value={value ? value : ""}
            autoFocus={true}
            onChange={onChange}
          />
        )}
        {!props.enterByKey ? (
          <Fragment>
            <ButtonOutlinePrimary size="xs" onClick={onClickUpdate}>
              <FontAwesomeIcon icon="check" />
            </ButtonOutlinePrimary>
            <ButtonOutlineNormal size="xs" onClick={cancelEdit}>
              <FontAwesomeIcon icon="times" />
            </ButtonOutlineNormal>
          </Fragment>
        ) : null}
      </Wrap>
    );
  }

  let data = value;
  data = data.replace(/\n/g, "<br />");

  if (!data && !props.disabled) {
    return (
      <TextLabel className="can-edit empty" onClick={openTextBox}>
        {emptyText}
      </TextLabel>
    );
  }

  return !props.disabled ? (
    <TextLabel
      className="can-edit"
      onClick={openTextBox}
      dangerouslySetInnerHTML={{ __html: data }}
    />
  ) : (
    <TextLabel
      className="disabled"
      dangerouslySetInnerHTML={{ __html: data }}
    />
  );
}
