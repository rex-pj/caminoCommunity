import * as React from "react";
import { Fragment } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Bullet from "./Bullet";
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
};

const Pagination = (props: Props) => {
  const { totalPage, baseUrl, pageQuery } = props;
  if (!totalPage || totalPage <= 1) {
    return <Fragment></Fragment>;
  }
  let { currentPage } = props;
  currentPage = Number(currentPage);
  currentPage = currentPage <= 0 ? 1 : currentPage;
  currentPage = currentPage > totalPage ? totalPage : currentPage;
  let first = 0;
  if (currentPage < 2) {
    first = currentPage;
  } else if (currentPage === 2) {
    first = currentPage - 1;
  } else {
    first = currentPage - 2;
  }

  let last = 0;
  if (currentPage <= totalPage - 2) {
    last = currentPage + 2;
  } else if (currentPage === totalPage - 1) {
    last = currentPage + 1;
  } else {
    last = currentPage;
  }

  let pageItems: any[] = [];
  for (let i = first; i <= last; i++) {
    pageItems.push(
      <PageItem key={i}>
        <Bullet
          currentPage={currentPage}
          pageNumber={i}
          baseUrl={baseUrl}
          pageQuery={pageQuery}
        >
          {i}
        </Bullet>
      </PageItem>
    );
  }

  return (
    <Root className="pagination">
      <HorizontalList className="pagination">
        <PageItem>
          <Bullet baseUrl={baseUrl} currentPage={currentPage} pageNumber={1}>
            <FontAwesomeIcon icon="angle-double-left" />
          </Bullet>
        </PageItem>
        <PageItem>
          <Bullet
            baseUrl={baseUrl}
            currentPage={currentPage}
            pageNumber={currentPage > 1 ? currentPage - 1 : 1}
          >
            <FontAwesomeIcon icon="angle-left" />
          </Bullet>
        </PageItem>
        {currentPage > 2 + 1 ? (
          <PageItem>
            <Bullet>...</Bullet>
          </PageItem>
        ) : null}
        {pageItems}
        {currentPage < totalPage - 2 ? (
          <PageItem>
            <Bullet>...</Bullet>
          </PageItem>
        ) : null}
        <PageItem>
          <Bullet
            baseUrl={baseUrl}
            currentPage={currentPage}
            pageNumber={currentPage < totalPage ? currentPage + 1 : totalPage}
          >
            <FontAwesomeIcon icon="angle-right" />
          </Bullet>
        </PageItem>
        <PageItem>
          <Bullet
            baseUrl={baseUrl}
            currentPage={currentPage}
            pageNumber={totalPage}
          >
            <FontAwesomeIcon icon="angle-double-right" />
          </Bullet>
        </PageItem>
      </HorizontalList>
    </Root>
  );
};

export default Pagination;
