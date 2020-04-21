import React, { Fragment } from "react";
import { withRouter } from "react-router-dom";
import { Selection } from "../../atoms/Selections";
import styled from "styled-components";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${Selection} {
    max-width: 100%;
  }
`;

const CategorySelection = styled(Selection)`
  margin-right: ${(p) => p.theme.size.tiny};
  vertical-align: middle;
`;

const ThumbnailUpload = styled(ImageUpload)`
  text-align: center;
  margin: auto;
  display: inline-block;
  vertical-align: middle;

  > span {
    color: ${(p) => p.theme.color.neutral};
    height: ${(p) => p.theme.size.normal};
    padding: 0 ${(p) => p.theme.size.tiny};
    font-size: ${(p) => p.theme.fontSize.tiny};
    background-color: ${(p) => p.theme.color.lighter};
    border-radius: ${(p) => p.theme.borderRadius.normal};
    border: 1px solid ${(p) => p.theme.color.neutral};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${(p) => p.theme.color.light};
    }

    svg {
      display: inline-block;
      margin: 10px auto 0 auto;
    }
  }
`;

export default withRouter((props) => {
  const { category, categories } = props;
  const { categoryId } = category;
  const handleInputChange = (event) => {};

  return (
    <Fragment>
      <FormRow>
        <CategorySelection
          name="categoryId"
          value={categoryId}
          onChange={(e) => handleInputChange(e)}
          placeholder="Danh mục bài viết"
        >
          <option value={0} key={0} selected={false}>
            Danh mục bài viết
          </option>
          {categories
            ? categories.map((cate) => (
                <option value={cate.id} key={cate.id}>
                  {cate.name}
                </option>
              ))
            : null}
        </CategorySelection>
        <ThumbnailUpload>Chọn hình đại diện bài viết</ThumbnailUpload>
      </FormRow>
    </Fragment>
  );
});
