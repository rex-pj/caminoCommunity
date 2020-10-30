import React from "react";
import styled from "styled-components";
import { HorizontalList } from "../../atoms/List";
import DropdownButton from "../../molecules/DropdownButton";
import ProfileNavLink from "../../molecules/Links/ProfileNavLink";

const Root = styled.div`
  position: relative;
`;

const ListItem = styled.li`
  display: inline-block;
  margin: 0 ${(p) => p.theme.size.distance};

  a.actived {
    color: ${(p) => p.theme.color.primaryDark};
    text-decoration: none;
    font-weight: 600;
    border-bottom: 3px solid ${(p) => p.theme.color.primary};
  }

  :hover a {
    color: ${(p) => p.theme.color.primaryDark};
    text-decoration: none;
  }

  a {
    color: ${(p) => p.theme.color.neutral};
    font-weight: 500;
    font-size: ${(p) => p.theme.fontSize.small};
    border: 0;
    height: ${(p) => p.theme.size.medium};
    line-height: ${(p) => p.theme.size.medium};
    display: inline-block;
    height: 100%;
  }
`;

const UserDropdown = styled(DropdownButton)`
  padding: 7px;
`;

export default (function (props) {
  const { className, userId, baseUrl } = props;
  const dropdown = [
    {
      url: `${baseUrl}/${userId}/update`,
      name: "Update My Information",
      isNav: true,
    },
    {
      url: `${baseUrl}/${userId}/security`,
      name: "Security",
      isNav: true,
    },
  ];

  const navs = [
    {
      pageNav: "",
      title: "Feeds",
    },
    {
      pageNav: "articles",
      title: "Articles",
    },
    {
      pageNav: "farms",
      title: "Farms",
    },
    {
      pageNav: "products",
      title: "Products",
    },
    {
      pageNav: "about",
      title: "About Me",
    },
    {
      pageNav: "followings",
      title: "Followings",
    },
  ];

  return (
    <Root>
      <div className="row">
        <div className="col-auto mr-auto">
          <HorizontalList className={className}>
            {navs.map((nav) => {
              return (
                <ListItem key={nav.pageNav}>
                  <ProfileNavLink
                    pageNav={nav.pageNav}
                    {...props}
                    userId={userId}
                    baseUrl={baseUrl}
                  >
                    {nav.title}
                  </ProfileNavLink>
                </ListItem>
              );
            })}
          </HorizontalList>
        </div>
        <div className="col-auto">
          <UserDropdown icon="ellipsis-v" dropdown={dropdown} />
        </div>
      </div>
    </Root>
  );
});
