import React, { Fragment, useState } from "react";
import { withRouter } from "react-router-dom";
import CommonEditor from "../../molecules/CommonEditor";
import { Textbox } from "../../atoms/Textboxes";
import { checkValidity } from "../../../utils/Validity";
import styled from "styled-components";
import { stateToHTML } from "draft-js-export-html";
import ArticleEditorBar from "./ArticleEditorBar";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${Textbox} {
    max-width: 100%;
    width: 500px;
  }
`;

export default withRouter((props) => {
  const { convertImageCallback, onImageValidate, height } = props;
  const initialFormData = {
    title: {
      value: "",
      validation: {
        isRequired: false,
      },
      isValid: false,
    },
    content: {
      value: "",
      validation: {
        isRequired: true,
      },
      isValid: false,
    },
    categoryId: {
      value: 0,
      validation: {
        isRequired: true,
      },
      isValid: false,
    },
  };

  const [formData, setFormData] = useState(initialFormData);

  const handleInputChange = (evt) => {
    let data = formData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({
      ...data,
    });
  };

  const onContentChanged = (editorState) => {
    const contentState = editorState.getCurrentContent();
    const html = stateToHTML(contentState);

    let data = formData || {};

    data["content"].isValid = checkValidity(data, html, "content");
    data["content"].value = html;

    setFormData({
      ...data,
    });
  };

  const { title, categoryId } = formData;
  return (
    <Fragment>
      <FormRow>
        <Textbox
          name="title"
          value={title.value}
          autoComplete="off"
          onChange={(e) => handleInputChange(e)}
          placeholder="Tiêu đề bài viết"
        />
      </FormRow>
      <FormRow>
        <ArticleEditorBar
          category={{
            categoryId: categoryId.value,
          }}
          categories={[
            { id: 1, name: "Trồng trọt" },
            { id: 3, name: "Chăn nuôi" },
            { id: 4, name: "Chế biến" },
            { id: 4, name: "Phương thức canh tác" },
          ]}
        />
      </FormRow>
      <CommonEditor
        height={height}
        convertImageCallback={convertImageCallback}
        onImageValidate={onImageValidate}
        placeholder="Nội dung bài viết..."
        onChanged={onContentChanged}
      />
    </Fragment>
  );
});
