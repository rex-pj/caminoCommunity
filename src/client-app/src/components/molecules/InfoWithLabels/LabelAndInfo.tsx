import * as React from "react";
import { HTMLAttributes } from "react";
import styled from "styled-components";

const ChildItem = styled.div`
  font-size: ${(p) => p.theme.fontSize.small};
  color: ${(p) => p.theme.color.darkText};
  margin-bottom: ${(p) => p.theme.size.exSmall};
  padding-bottom: ${(p) => p.theme.size.exSmall};
  min-height: ${(p) => p.theme.size.normal};

  label {
    color: ${(p) => p.theme.color.primaryText};
    font-weight: 600;
  }

  div {
    font-size: ${(p) => p.theme.fontSize.small};
  }

  p {
    font-size: ${(p) => p.theme.fontSize.small};
    display: block;
    margin-bottom: 0;
    color: ${(p) => p.theme.color.primaryLink};
    text-decoration: underline;
  }

  :last-child {
    border-bottom: 0;
    margin-bottom: 0;
    padding-bottom: 0;
  }
`;

interface LabelAndInfoProps extends HTMLAttributes<HTMLDivElement> {
  label?: string;
  isEmail?: boolean;
}

function renderChildren(isEmaill: boolean, children: any) {
  if (isEmaill) {
    return <p>{children}</p>;
  }

  if (children) {
    return <div>{children}</div>;
  }

  return <div>...</div>;
}

const LabelAndInfo: React.FC<LabelAndInfoProps> = (
  props: LabelAndInfoProps
) => {
  const { className, children, label, isEmail } = props;
  return (
    <ChildItem className={className}>
      {label ? <label>{label}</label> : null}
      {renderChildren(isEmail, children)}
    </ChildItem>
  );
};

export default LabelAndInfo;
