import React, { Fragment } from "react";
import { withRouter } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
import FarmItem from "../../components/organisms/Farm/FarmItem";
import { GET_USER_FARMS } from "../../utils/GraphQLQueries/queries";
import {
  VALIDATE_IMAGE_URL,
  FILTER_FARM_TYPES,
  CREATE_FARM,
} from "../../utils/GraphQLQueries/mutations";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import FarmEditor from "../../components/organisms/ProfileEditors/FarmEditor";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";

export default withRouter(function (props) {
  const { location, match, pageNumber } = props;
  const { params } = match;
  const { userId } = params;
  const dispatch = useStore(false)[1];

  const { loading, data, error, refetch: fetchFarms, networkStatus } = useQuery(
    GET_USER_FARMS,
    {
      variables: {
        criterias: {
          userIdentityId: userId,
          page: pageNumber,
        },
      },
    }
  );

  const [createFarm] = useMutation(CREATE_FARM, {
    client: graphqlClient,
  });

  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [farmTypes] = useMutation(FILTER_FARM_TYPES);

  const searchFarmTypes = async (inputValue) => {
    return await farmTypes({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { categories } = data;
        if (!categories) {
          return [];
        }
        return categories.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const convertImagefile = async (file) => {
    const url = await fileToBase64(file);
    return {
      url,
      fileName: file.name,
    };
  };

  const onImageValidate = async (value) => {
    return await validateImageUrl({
      variables: {
        criterias: {
          url: value,
        },
      },
    });
  };

  const onFarmPost = async (data) => {
    return await createFarm({
      variables: {
        criterias: data,
      },
    });
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const refetchNewFarms = () => {
    fetchFarms();
  };

  const farmEditor = (
    <FarmEditor
      height={230}
      convertImageCallback={convertImagefile}
      onImageValidate={onImageValidate}
      filterCategories={searchFarmTypes}
      onFarmPost={onFarmPost}
      refetchNews={refetchNewFarms}
      showValidationError={showValidationError}
    />
  );

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {farmEditor}
        <Loading>Loading...</Loading>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {farmEditor}
        <ErrorBlock>Error!</ErrorBlock>
      </Fragment>
    );
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
  const baseUrl = props.userUrl + "/articles";
  const { totalPage, filter } = userFarms;
  const { page } = filter;

  return (
    <Fragment>
      {farmEditor}
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
});
