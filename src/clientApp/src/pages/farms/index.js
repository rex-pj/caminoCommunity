import React, { Fragment } from "react";
import Farm from "../../components/templates/Farm";
import { UrlConstant } from "../../utils/Constants";
import { useQuery } from "@apollo/client";
import { GET_FARMS } from "../../utils/GraphQLQueries/queries";
import { withRouter } from "react-router-dom";

export default withRouter(function (props) {
  const { pageNumber } = props;
  const { loading, data } = useQuery(GET_FARMS, {
    variables: {
      criterias: {
        page: pageNumber,
      },
    },
  });

  if (loading || !data) {
    return <Fragment></Fragment>;
  }

  const { farms: farmsResponse } = data;
  const { collections } = farmsResponse;
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

  const baseUrl = "/farms";
  const { totalPage, filter } = farmsResponse;
  const { page } = filter;

  const breadcrumbs = [
    {
      isActived: true,
      title: "Farm",
    },
  ];

  return (
    <Farm
      farms={farms}
      breadcrumbs={breadcrumbs}
      totalPage={totalPage}
      baseUrl={baseUrl}
      currentPage={page}
    />
  );
});
