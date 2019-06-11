import React, { useState, useEffect } from "react";
import styled from "styled-components";
import GroupSuggestionItem from "./GroupSuggestionItem";
import { VerticalList } from "../../atoms/List";
import { QuaternaryHeading } from "../../atoms/Heading";
import { UrlConstant } from "../../../utils/Constant";

const Root = styled.div`
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  border-radius: ${p => p.theme.borderRadius.normal};
  background-color: ${p => p.theme.color.white};
`;

const List = styled(VerticalList)`
  li {
    margin-bottom: ${p => p.theme.size.exSmall};
  }
`;

export default function (props) {
  const [groups, setGroups] = useState([]);

  useEffect(function () {
    let groups = [];
    for (let i = 0; i < 3; i++) {
      groups.push({
        info: "125 người tham gia",
        name: "Hội trái cây Mini",
        description:
          "Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.",
        url: `${UrlConstant.FarmGroup.url}1`,
        id: "1212234r5423",
        photoUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`
      });
    }

    setGroups(groups);
  })

  return (
    <div>
      <QuaternaryHeading>
        Tham Gia Nông Hội Để Kết Nối Tốt Hơn!
      </QuaternaryHeading>
      <Root>
        <List>
          {groups &&
            groups.map((group, index) => (
              <GroupSuggestionItem key={index} group={group} index={index} />
            ))}
        </List>
      </Root>
    </div>
  );
}
