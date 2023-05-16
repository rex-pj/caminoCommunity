import * as React from "react";
import { Fragment } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { SecondaryDarkHeading } from "../../atoms/Heading";
import SearchBlock from "./search-blocks";
import { FeedType } from "../../../utils/Enums";
import { useLocation } from "react-router-dom";
import { UrlConstant } from "../../../utils/Constants";
import { getParameters, generateQueryParameters } from "../../../utils/Helper";
import { apiConfig } from "../../../config/api-config";
import { IAdvancedSearchResult } from ".";

type Props = {
  advancedSearchResult: IAdvancedSearchResult;
  baseUrl: string;
  keyword?: string;
};

const SearchFeed = (props: Props) => {
  const { advancedSearchResult, baseUrl, keyword } = props;
  const location = useLocation();
  const mapSearchResults = (collections: any) => {
    return collections.map((item: any) => {
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
        feed.pictureUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${feed.pictureId}`;
      } else if (feed.pictureId) {
        feed.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${feed.pictureId}`;
      }

      feed.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdByName,
      };

      if (item.createdByPhotoId) {
        feed.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      return feed;
    });
  };

  const buildPageQuery = (filterType: FeedType) => {
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
};

export default SearchFeed;
