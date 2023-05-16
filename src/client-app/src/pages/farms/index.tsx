import * as React from "react";
import { useEffect, useState, useRef } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Farm from "../../components/templates/Farm";
import { UrlConstant } from "../../utils/Constants";
import { useLazyQuery } from "@apollo/client";
import { farmQueries } from "../../graphql/fetching/queries";
import { useParams } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import InfiniteScroll from "react-infinite-scroll-component";
import { apiConfig } from "../../config/api-config";
import FarmService from "../../services/farmService";
import { LoadingBar } from "../../components/molecules/NotificationBars";

type Props = {};

const Index = (props: Props) => {
  const { pageNumber } = useParams();
  const [state, dispatch] = useStore(false);
  const [farms, setFarms] = useState<any[]>([]);
  const farmService = new FarmService();
  const pageRef = useRef<any>({ pageNumber: pageNumber ? pageNumber : 1 });
  const [fetchFarms, { loading, data, error, refetch }] = useLazyQuery(
    farmQueries.GET_FARMS,
    {
      onCompleted: (data) => {
        setPageInfo(data);
        onFetchCompleted(data);
      },
      fetchPolicy: "cache-and-network",
    }
  );

  const setPageInfo = (data: any) => {
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

  const onFetchCompleted = (data: any) => {
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

  const parseCollections = (collections: any[]): any[] => {
    return collections.map((item: any) => {
      let farm = { ...item };
      farm.url = `${UrlConstant.Farm.url}${farm.id}`;
      if (farm.pictures && farm.pictures.length > 0) {
        const picture = farm.pictures[0];
        if (picture.pictureId > 0) {
          farm.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${picture.pictureId}`;
        }
      }

      farm.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoId) {
        farm.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      return farm;
    });
  };

  const onOpenDeleteConfirmation = (e: any) => {
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

  const onDelete = (id: number) => {
    farmService.delete(id).then(() => {
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

  const checkHasData = () => {
    return data && pageRef.current.totalResult && farms.length >= 0;
  };

  return (
    <DefaultLayout
      isLoading={!!loading}
      hasData={checkHasData()}
      hasError={!!error}
    >
      <Breadcrumb list={breadcrumbs} className="px-2" />
      <InfiniteScroll
        style={{ overflowX: "hidden" }}
        dataLength={pageRef.current.totalResult ?? 0}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<LoadingBar />}
      >
        <Farm
          onOpenDeleteConfirmation={onOpenDeleteConfirmation}
          farms={farms}
          baseUrl="/farms"
        />
      </InfiniteScroll>
    </DefaultLayout>
  );
};

export default Index;
