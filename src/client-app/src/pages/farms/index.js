import React, { useEffect, useState, useRef } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Farm from "../../components/templates/Farm";
import { UrlConstant } from "../../utils/Constants";
import { useLazyQuery, useMutation } from "@apollo/client";
import { farmQueries } from "../../graphql/fetching/queries";
import { farmMutations } from "../../graphql/fetching/mutations";
import { withRouter } from "react-router-dom";
import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import InfiniteScroll from "react-infinite-scroll-component";

export default withRouter(function (props) {
  const {
    match: {
      params: { pageNumber },
    },
  } = props;
  const [state, dispatch] = useStore(false);
  const [farms, setFarms] = useState([]);
  const pageRef = useRef({ pageNumber: pageNumber ? pageNumber : 1 });
  const [fetchFarms, { loading, data, error, refetch }] = useLazyQuery(
    farmQueries.GET_FARMS,
    {
      onCompleted: (data) => {
        setPageInfo(data);
        onFetchCompleted(data);
      },
    }
  );

  const [deleteFarm] = useMutation(farmMutations.DELETE_FARM, {
    client: authClient,
  });

  const setPageInfo = (data) => {
    const {
      farms: {
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
      farms: { collections },
    } = data;
    const farmCollections = parseCollections(collections);
    setFarms([...farms, ...farmCollections]);
  };

  useEffect(() => {
    if (state.type === "FARM_UPDATE" || state.type === "FARM_DELETE") {
      refetch();
    }
  }, [state, refetch]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchFarms({
      variables: {
        criterias: {
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

  if (loading && farms.length === 0) {
    return <LoadingBar>Loading</LoadingBar>;
  }
  if ((!data || !pageRef.current.totalResult) && farms.length === 0) {
    return <NoDataBar>No data</NoDataBar>;
  }
  if (error) {
    return <ErrorBar>Error!</ErrorBar>;
  }

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
      refetch();
    });
  };

  const breadcrumbs = [
    {
      isActived: true,
      title: "Farm",
    },
  ];

  const fetchMoreData = () => {
    if (pageRef.current.pageNumber === pageRef.current.totalPage) {
      return;
    }
    pageRef.current.pageNumber += 1;
    fetchFarms({
      variables: {
        criterias: {
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  return (
    <DefaultLayout>
      <Breadcrumb list={breadcrumbs} className="px-2" />
      <InfiniteScroll
        style={{ overflowX: "hidden" }}
        dataLength={pageRef.current.totalResult}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<h4>Loading...</h4>}
      >
        <Farm
          onOpenDeleteConfirmation={onOpenDeleteConfirmation}
          farms={farms}
          baseUrl="/farms"
        />
      </InfiniteScroll>
    </DefaultLayout>
  );
});
