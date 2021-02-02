import React, { useEffect } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Farm from "../../components/templates/Farm";
import { UrlConstant } from "../../utils/Constants";
import { useQuery, useMutation } from "@apollo/client";
import { farmQueries } from "../../graphql/fetching/queries";
import { farmMutations } from "../../graphql/fetching/mutations";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { pageNumber, pageSize } = params;
  const [state, dispatch] = useStore(false);
  const { loading, data, error, refetch } = useQuery(farmQueries.GET_FARMS, {
    variables: {
      criterias: {
        page: pageNumber ? parseInt(pageNumber) : 1,
        pageSize: pageSize ? parseInt(pageSize) : 10,
      },
    },
  });

  const [deleteFarm] = useMutation(farmMutations.DELETE_FARM, {
    client: authClient,
  });

  useEffect(() => {
    if (state.type === "FARM_UPDATE" || state.type === "FARM_DELETE") {
      refetch();
    }
  }, [state, refetch]);

  if (loading || !data) {
    return <Loading>Loading</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { farms: farmsData } = data;
  const { collections } = farmsData;
  const farms = collections.map((item) => {
    let farm = { ...item };
    farm.url = `${UrlConstant.Farm.url}${farm.id}`;
    if (farm.thumbnails && farm.thumbnails.length > 0) {
      const thumbnail = farm.thumbnails[0];
      if (thumbnail.pictureId > 0) {
        farm.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.pictureId}`;
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

  const { totalPage, filter } = farmsData;
  const { page } = filter;

  const breadcrumbs = [
    {
      isActived: true,
      title: "Farm",
    },
  ];

  return (
    <DefaultLayout>
      <Farm
        onOpenDeleteConfirmation={onOpenDeleteConfirmation}
        farms={farms}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl="/farms"
        currentPage={page}
      />
    </DefaultLayout>
  );
});
