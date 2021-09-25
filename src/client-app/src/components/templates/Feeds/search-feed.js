import React from "react";
import { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { SecondaryDarkHeading } from "../../atoms/Heading";
import SearchBlock from "./search-blocks";
import { FeedType } from "../../../utils/Enums";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../../utils/Constants";
import { getParameters, generateQueryParameters } from "../../../utils/Helper";

export default withRouter((props) => {
  const { advancedSearchResult, baseUrl, location, keyword } = props;
  const mapSearchResults = (collections) => {
    return collections.map((item) => {
      let feed = { ...item };
      if (feed.feedType === FeedType.Farm) {
        feed.url = `${UrlConstant.Farm.url}${feed.id}`;
      } else if (feed.feedType === FeedType.Article) {
        feed.url = `${UrlConstant.Article.url}${feed.id}`;
      } else if (feed.feedType === FeedType.Product) {
        feed.url = `${UrlConstant.Product.url}${feed.id}`;
      } else if (feed.feedType === FeedType.User) {
        feed.url = `${UrlConstant.Profile.url}${feed.id}`;
      }

      if (feed.pictureId && feed.feedType === FeedType.User) {
        feed.pictureUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${feed.pictureId}`;
      } else if (feed.pictureId) {
        feed.pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${feed.pictureId}`;
      }

      feed.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdByName,
      };

      if (item.createdByPhotoCode) {
        feed.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
      }

      return feed;
    });
  };

  const buildPageQuery = (filterType) => {
    const { userIdentityId, hoursCreatedFrom, hoursCreatedTo } = getParameters(
      location.search
    );

    const query = generateQueryParameters({
      userIdentityId,
      hoursCreatedFrom,
      hoursCreatedTo,
      feedFilterType: filterType,
    });

    return keyword ? `${keyword}?${query}` : `?${query}`;
  };

  return (
    <Fragment>
      {advancedSearchResult.farms && advancedSearchResult.farms.length > 0 ? (
        <Fragment>
          <SecondaryDarkHeading>
            <FontAwesomeIcon icon="tractor" className="me-1"></FontAwesomeIcon>
            Nông trại
          </SecondaryDarkHeading>
          <SearchBlock
            feeds={mapSearchResults(advancedSearchResult.farms)}
            totalPage={advancedSearchResult.totalFarmPage}
            baseUrl={`${baseUrl}`}
            currentPage={advancedSearchResult.page}
            pageQuery={buildPageQuery(FeedType.Farm)}
          />
        </Fragment>
      ) : null}
      {advancedSearchResult.products &&
      advancedSearchResult.products.length > 0 ? (
        <Fragment>
          <SecondaryDarkHeading>
            <FontAwesomeIcon icon="carrot" className="me-1"></FontAwesomeIcon>
            Sản phẩm
          </SecondaryDarkHeading>
          <SearchBlock
            feeds={mapSearchResults(advancedSearchResult.products)}
            totalPage={advancedSearchResult.totalProductPage}
            baseUrl={`${baseUrl}`}
            currentPage={advancedSearchResult.page}
            pageQuery={buildPageQuery(FeedType.Product)}
          />
        </Fragment>
      ) : null}

      {advancedSearchResult.articles &&
      advancedSearchResult.articles.length > 0 ? (
        <Fragment>
          <SecondaryDarkHeading>
            <FontAwesomeIcon icon="book" className="me-1"></FontAwesomeIcon>
            Bài viết
          </SecondaryDarkHeading>
          <SearchBlock
            feeds={mapSearchResults(advancedSearchResult.articles)}
            totalPage={advancedSearchResult.totalArticlePage}
            baseUrl={`${baseUrl}`}
            currentPage={advancedSearchResult.page}
            pageQuery={buildPageQuery(FeedType.Article)}
          />
        </Fragment>
      ) : null}
      {advancedSearchResult.users && advancedSearchResult.users.length > 0 ? (
        <Fragment>
          <SecondaryDarkHeading>
            <FontAwesomeIcon icon="users" className="me-1"></FontAwesomeIcon>
            Thành viên
          </SecondaryDarkHeading>
          <SearchBlock
            feeds={mapSearchResults(advancedSearchResult.users)}
            totalPage={advancedSearchResult.totalUserPage}
            baseUrl={`${baseUrl}`}
            currentPage={advancedSearchResult.page}
            pageQuery={buildPageQuery(FeedType.User)}
          />
        </Fragment>
      ) : null}
    </Fragment>
  );
});
