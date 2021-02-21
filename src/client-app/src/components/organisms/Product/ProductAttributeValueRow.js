import React from "react";
import {
  ButtonOutlineDanger,
  ButtonOutlineLight,
} from "../../atoms/Buttons/OutlineButtons";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { LabelNormal, LabelDark } from "../../atoms/Labels";

const SecondaryLabel = styled(LabelNormal)`
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

export default (props) => {
  const {
    attributeValue,
    className,
    onEditAttributeValue,
    onRemoveAttributeValue,
  } = props;

  return (
    <div className={className}>
      <div className="col-12 col-md-12 col-lg-3">
        <LabelNormal className="me-1">Value:</LabelNormal>
        <LabelDark>{attributeValue.name}</LabelDark>
      </div>
      <div className="col-12 col-md-6 col-lg-5">
        <div className="mb-1">
          <LabelNormal className="me-1">Adjustment price:</LabelNormal>
          <LabelDark>${attributeValue.priceAdjustment}</LabelDark>
        </div>
        <div className="mb-1">
          <SecondaryLabel className="me-1">In percentage:</SecondaryLabel>
          <LabelDark>{attributeValue.pricePercentageAdjustment}%</LabelDark>
        </div>
      </div>
      <div className="col-12 col-md-3 col-lg-2">
        <LabelNormal className="me-1">Order:</LabelNormal>
        <LabelDark>{attributeValue.displayOrder}</LabelDark>
      </div>
      <div className="col-auto offset-auto">
        <ButtonOutlineLight
          type="button"
          size="xs"
          className="me-1"
          title="Edit value"
          onClick={() => onEditAttributeValue(attributeValue)}
        >
          <FontAwesomeIcon icon="pencil-alt" />
        </ButtonOutlineLight>
        <ButtonOutlineDanger
          type="button"
          size="xs"
          title="Remove value"
          onClick={() => onRemoveAttributeValue(attributeValue)}
        >
          <FontAwesomeIcon icon="times"></FontAwesomeIcon>
        </ButtonOutlineDanger>
      </div>
    </div>
  );
};
