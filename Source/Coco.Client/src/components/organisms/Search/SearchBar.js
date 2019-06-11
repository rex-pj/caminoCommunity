import React, { Component } from "react";
import styled from "styled-components";
import { Textbox } from "../../atoms/Textboxes";
import { Button } from "../../atoms/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";

const SearchForm = styled.div`
  position: relative;
  border-radius: ${p => p.theme.size.normal};
  border: 1px solid ${p => p.theme.color.secondary};
  background-color: ${p =>
    p.isOnFocus ? p.theme.color.moreDark : p.theme.rgbaColor.dark};
  height: ${p => p.theme.size.normal};
  margin: 1px 0;
`;

const SearchInput = styled(Textbox)`
  border-top-right-radius: 100%;
  border-bottom-right-radius: 100%;
  background: transparent;
  border: 0;
  width: 300px;
  height: calc(${p => p.theme.size.normal} - 2px);
  float: left;
  color: ${p => p.theme.color.exLight};
  max-width: calc(100% - ${p => p.theme.size.normal});

  ::placeholder {
    color: ${p => p.theme.color.light};
  }
`;

const SearchButton = styled(Button)`
  border-radius: 100%;
  height: calc(${p => p.theme.size.normal} - 6px);
  width: calc(${p => p.theme.size.normal} - 6px);
  padding: 0;
  background-color: transparent;
  border: 0;
  float: left;
  margin: 2px 0 2px 2px;

  svg,
  path {
    color: ${p => p.theme.color.light};
  }
`;

export default class SearchBar extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isOnFocus: false
    };
  }

  onFocus = () => {
    this.setState({
      isOnFocus: true
    });
  };

  onBlur = () => {
    this.setState({
      isOnFocus: false
    });
  };

  render() {
    const { isOnFocus } = this.state;
    return (
      <SearchForm isOnFocus={isOnFocus}>
        <SearchButton type="submit">
          <FontAwesomeIcon icon={faSearch} />
        </SearchButton>
        <SearchInput
          onFocus={this.onFocus}
          onBlur={this.onBlur}
          type="search"
          placeholder="Tìm Kiếm"
          aria-label="Search"
        />
      </SearchForm>
    );
  }
}
