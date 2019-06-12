import React, { Component } from "react";
import styled from "styled-components";
import ConnectionSuggestionItem from "./ConnectionSuggestionItem";
import { VerticalList } from "../../atoms/List";
import { QuaternaryHeading } from "../../atoms/Heading";

const Root = styled.div`
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  border-radius: ${p => p.theme.borderRadius.normal};
  background-color: ${p => p.theme.color.white};
`;

export default class ConnectionSuggestions extends Component {
  constructor(props) {
    super(props);

    let connections = [];
    for (let i = 0; i < 3; i++) {
      connections.push({
        name: "Ông Năm Cự",
        description:
          "Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.",
        url: "/profile?id=SXaSDRHRfds3zUDFQzC6jg==",

        id: "1212234r5423",
        ImageUrl: `${process.env.PUBLIC_URL}/photos/conn-farmer.png`
      });
    }

    this.state = {
      connections: connections
    };
  }

  render() {
    const { connections } = this.state;
    return (
      <div>
        <QuaternaryHeading>Kết Nối Nhà Nông</QuaternaryHeading>
        <Root>
          <VerticalList>
            {connections &&
              connections.map((connection, index) => (
                <ConnectionSuggestionItem
                  key={index}
                  connection={connection}
                  index={index}
                />
              ))}
          </VerticalList>
        </Root>
      </div>
    );
  }
}
