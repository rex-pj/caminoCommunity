import React, { useState } from "react";
import styled from "styled-components";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";

const SearchForm = styled.div`
  position: relative;
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.rgbaColor.light};
  background-color: ${(p) =>
    p.isOnFocus ? p.theme.rgbaColor.dark : p.theme.rgbaColor.darkLight};
  height: ${(p) => p.theme.size.normal};
  margin: 1px 0;
`;

const SearchInput = styled(PrimaryTextbox)`
  border-top-right-radius: 100%;
  border-bottom-right-radius: 100%;
  background: transparent;
  border: 0;
  width: 300px;
  height: calc(${(p) => p.theme.size.normal} - 2px);
  float: left;
  color: ${(p) => p.theme.color.lightText};
  max-width: calc(100% - ${(p) => p.theme.size.normal});

  ::placeholder {
    color: ${(p) => p.theme.color.neutralText};
    font-size: ${(p) => p.theme.fontSize.small};
  }
`;

const SearchButton = styled(ButtonTransparent)`
  border-radius: 100%;
  height: calc(${(p) => p.theme.size.normal} - 6px);
  width: calc(${(p) => p.theme.size.normal} - 6px);
  padding: 0;
  background-color: transparent;
  border: 0;
  float: left;
  margin: 2px 0 2px 2px;

  :active,
  :hover,
  :focus-within {
    background-color: ${(p) => p.theme.rgbaColor.dark};
  }

  :disabled {
    background-color: ${(p) => p.theme.rgbaColor.light};
  }

  svg,
  path {
    color: ${(p) => p.theme.color.neutralText};
  }
`;

export default function (props) {
  const [isOnFocus, setOnFocus] = useState(false);

  const onFocus = () => {
    setOnFocus(true);
  };

  const onBlur = () => {
    setOnFocus(false);
  };

  return (
    <SearchForm isOnFocus={isOnFocus}>
      <SearchButton type="submit">
        <FontAwesomeIcon icon={faSearch} />
      </SearchButton>
      <SearchInput
        onFocus={onFocus}
        onBlur={onBlur}
        type="search"
        placeholder="Search"
        aria-label="Search"
      />
    </SearchForm>
  );
}
