import React, { Fragment } from "react";
import { withRouter } from "react-router-dom";
import { useQuery } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
import FarmItem from "../../components/organisms/Farm/FarmItem";
import { GET_USER_FARMS } from "../../utils/GraphQLQueries/queries";

export default withRouter(function (props){
  const { location, match, pageNumber } = props;
  const { params } = match;
  const { userId } = params;

  const { loading, data } = useQuery(GET_USER_FARMS, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber,
      },
    },
  });

  if (loading || !data) {
    return <Fragment></Fragment>;
  }

  const { userFarms } = data;
  const { collections } = userFarms;
  const farms = collections.map((item) => {
    let farm = { ...item };
    farm.url = `${UrlConstant.Farm.url}${farm.id}`;
    if (farm.thumbnails) {
      const thumbnail = farm.thumbnails[0];
      if (thumbnail.id > 0) {
        farm.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.id}`;
      }
    }

    farm.creator = {
      createdDate: item.createdDate,
      profileUrl: `/profile/${item.createdByIdentityId}`,
      name: item.createdBy,
    };

    if (item.createdByPhotoCode) {
      farm.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
    }

    return farm;
  });

  const pageQuery = location.search;
  const baseUrl = props.userUrl + "/posts";
  const { totalPage, filter } = userFarms;
  const { page } = filter;

  return (
    <Fragment>
      <div className="row">
        {farms
          ? farms.map((item, index) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <FarmItem key={item.id} farm={item} />
              </div>
            ))
          : null}
      </div>
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={page}
      />
    </Fragment>
  );
  }
);
