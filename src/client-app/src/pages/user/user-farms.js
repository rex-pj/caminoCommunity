import React, {
  Fragment,
  useContext,
  useState,
  useRef,
  useEffect,
} from "react";
import { useParams } from "react-router-dom";
import { useLazyQuery, useMutation } from "@apollo/client";
import { UrlConstant } from "../../utils/Constants";
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
  NoDataBar,
} from "../../components/molecules/NotificationBars";
import { SessionContext } from "../../store/context/session-context";
import InfiniteScroll from "react-infinite-scroll-component";

export default (function (props) {
  const { userId } = useParams();
  const { pageNumber } = props;
  const [state, dispatch] = useStore(false);
  const { currentUser, isLogin } = useContext(SessionContext);
  const [farms, setFarms] = useState([]);
  const pageRef = useRef({
    pageNumber: pageNumber ? pageNumber : 1,
    userId: userId,
  });

  const [
    fetchFarms,
    { loading, data, error, refetch: refetchFarms, networkStatus },
  ] = useLazyQuery(farmQueries.GET_USER_FARMS, {
    onCompleted: (data) => {
      setPageInfo(data);
      onFetchCompleted(data);
    },
    fetchPolicy: "cache-and-network",
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
        refetchFarms();
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
      refetchFarms();
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
          refetchNews={refetchFarms}
          showValidationError={showValidationError}
        />
      );
    }

    return null;
  };

  const setPageInfo = (data) => {
    const {
      userFarms: {
        totalPage,
        totalResult,
        filter: { page },
      },
    } = data;
    pageRef.current.totalPage = totalPage;
    pageRef.current.currentPage = page;
    pageRef.current.totalResult = totalResult;
  };

  const onFetchCompleted = (data) => {
    const {
      userFarms: { collections },
    } = data;
    const farmCollections = parseCollections(collections);
    setFarms([...farms, ...farmCollections]);
  };

  useEffect(() => {
    if (state.type === "FARM_UPDATE" || state.type === "FARM_DELETE") {
      refetchFarms();
    }
  }, [state, refetchFarms]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchFarms({
      variables: {
        criterias: {
          userIdentityId: pageRef.current.userId,
          page: page ? parseInt(page) : 1,
        },
      },
    });
  }, [fetchFarms]);

  const parseCollections = (collections) => {
    return collections.map((item) => {
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
  };

  if ((loading || networkStatus === 1) && farms.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderFarmEditor() : null}
        <LoadingBar>Loading...</LoadingBar>
      </Fragment>
    );
  }
  if (!(data && pageRef.current.totalResult && farms.length >= 0)) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderFarmEditor() : null}
        <NoDataBar>No Data!</NoDataBar>
      </Fragment>
    );
  }
  if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderFarmEditor() : null}
        <ErrorBar>Error!</ErrorBar>
      </Fragment>
    );
  }

  const fetchMoreData = () => {
    if (pageRef.current.pageNumber === pageRef.current.totalPage) {
      return;
    }
    pageRef.current.pageNumber += 1;
    fetchFarms({
      variables: {
        criterias: {
          userId: pageRef.current.userId,
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  return (
    <Fragment>
      {renderFarmEditor()}
      <InfiniteScroll
        style={{ overflowX: "hidden" }}
        dataLength={pageRef.current.totalResult ?? 0}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<h4>Loading...</h4>}
      >
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
      </InfiniteScroll>
    </Fragment>
  );
});
