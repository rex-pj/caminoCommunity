import React, { Fragment, useContext, useEffect } from "react";
import { withRouter } from "react-router-dom";
import { useQuery, useMutation } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
import FarmItem from "../../components/organisms/Farm/FarmItem";
import { farmQueries } from "../../graphql/fetching/queries";
import {
  farmMutations,
  mediaMutations,
} from "../../graphql/fetching/mutations";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import authClient from "../../graphql/client/authClient";
import FarmEditor from "../../components/organisms/Farm/FarmEditor";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import { SessionContext } from "../../store/context/session-context";

export default withRouter(function (props) {
  const { location, match, pageNumber } = props;
  const { params } = match;
  const { userId } = params;
  const [state, dispatch] = useStore(false);
  const { currentUser, isLogin } = useContext(SessionContext);

  const {
    loading,
    data,
    error,
    refetch: fetchFarms,
    networkStatus,
  } = useQuery(farmQueries.GET_USER_FARMS, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber ? parseInt(pageNumber) : 1,
      },
    },
  });

  const [createFarm] = useMutation(farmMutations.CREATE_FARM, {
    client: authClient,
  });

  const [deleteFarm] = useMutation(farmMutations.DELETE_FARM, {
    client: authClient,
  });

  const [validateImageUrl] = useMutation(mediaMutations.VALIDATE_IMAGE_URL);
  const [farmTypes] = useMutation(farmMutations.FILTER_FARM_TYPES);

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
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { createFarm: farm } = data;
        resolve(farm);
        fetchFarms();
      });
    });
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onOpenDeleteConfirmation = (e) => {
    const { title, innerModal, message, id } = e;
    dispatch("OPEN_MODAL", {
      data: {
        title: title,
        children: message,
        id: id,
      },
      execution: { onDelete: onDelete },
      options: {
        isOpen: true,
        innerModal: innerModal,
        position: "fixed",
      },
    });
  };

  const onDelete = (id) => {
    deleteFarm({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      fetchFarms();
    });
  };

  const renderFarmEditor = () => {
    if (currentUser && isLogin) {
      return (
        <FarmEditor
          height={230}
          convertImageCallback={convertImagefile}
          onImageValidate={onImageValidate}
          filterCategories={farmTypes}
          onFarmPost={onFarmPost}
          refetchNews={fetchFarms}
          showValidationError={showValidationError}
        />
      );
    }

    return null;
  };

  useEffect(() => {
    if (state.type === "FARM_UPDATE" || state.type === "FARM_DELETE") {
      fetchFarms();
    }
  }, [state, fetchFarms]);

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {renderFarmEditor()}
        <LoadingBar>Loading...</LoadingBar>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {renderFarmEditor()}
        <ErrorBar>Error!</ErrorBar>
      </Fragment>
    );
  }

  const { userFarms } = data;
  const { collections } = userFarms;
  const farms = collections.map((item) => {
    let farm = { ...item };
    farm.url = `${UrlConstant.Farm.url}${farm.id}`;
    if (farm.pictures && farm.pictures.length > 0) {
      const picture = farm.pictures[0];
      if (picture.pictureId > 0) {
        farm.pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${picture.pictureId}`;
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
      {renderFarmEditor()}
      <div className="row">
        {farms
          ? farms.map((item, index) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <FarmItem
                  key={item.id}
                  farm={item}
                  onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
                />
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
