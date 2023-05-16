import * as React from "react";
import { Fragment } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PagerBullet from "./PagerBullet";
import { HorizontalList } from "../../molecules/List";

const Root = styled.div`
  text-align: center;
  margin: ${(p) => p.theme.size.distance} 0;
`;

const PageItem = styled.li`
  margin: 0 1px;

  :hover a {
    text-decoration: none;
    color: ${(p) => p.theme.color.neutralText};
  }

  svg > path {
    color: inherit;
  }
`;

type Props = {
  baseUrl?: string;
  currentPage?: number;
  pageQuery?: string;
  totalPage?: number;
  text?: string;
  bulletSize?: string;
};

const Pager = (props: Props) => {
  const { totalPage, baseUrl, pageQuery, text, bulletSize } = props;
  if (!totalPage || totalPage <= 1) {
    return <Fragment></Fragment>;
  }
  let { currentPage } = props;
  currentPage = Number(currentPage);
  currentPage = currentPage <= 0 ? 1 : currentPage;
  currentPage = currentPage > totalPage ? totalPage : currentPage;

  return (
    <Root className="pager">
      <HorizontalList className="pager">
        <PageItem>
          {totalPage > 1 && totalPage > currentPage ? (
            <PagerBullet
              size={bulletSize}
              baseUrl={baseUrl}
              currentPage={currentPage}
              pageQuery={pageQuery}
            >
              {text ? <span className="me-1">{text}</span> : null}
              <FontAwesomeIcon icon="angle-down" />
            </PagerBullet>
          ) : null}
        </PageItem>
      </HorizontalList>
    </Root>
  );
};

export default Pager;
