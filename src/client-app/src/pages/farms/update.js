import React, { useEffect } from "react";
import { farmQueries, userQueries } from "../../graphql/fetching/queries";
import {
  mediaMutations,
  farmMutations,
} from "../../graphql/fetching/mutations";
import { useQuery, useMutation } from "@apollo/client";
import authClient from "../../graphql/client/authClient";
import { withRouter } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import FarmEditor from "../../components/organisms/Farm/FarmEditor";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import farmCreationModel from "../../models/farmCreationModel";
import DetailLayout from "../../components/templates/Layout/DetailLayout";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;
  const dispatch = useStore(false)[1];
  const [validateImageUrl] = useMutation(mediaMutations.VALIDATE_IMAGE_URL);
  const [updateFarm] = useMutation(farmMutations.UPDATE_FARM, {
    client: authClient,
  });
  const [farmTypes] = useMutation(farmMutations.FILTER_FARM_TYPES);

  const { loading, data, error, refetch, called } = useQuery(
    farmQueries.GET_FARM_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
    }
  );

  const userIdentityId = data?.farm?.createdByIdentityId;
  const { data: authorData } = useQuery(userQueries.GET_USER_INFO, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userId: userIdentityId,
      },
    },
  });

  const { data: userFarmData } = useQuery(farmQueries.SELECT_USER_FARMS, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userIdentityId: userIdentityId,
        page: 1,
        pageSize: 4,
      },
    },
  });

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
    return await updateFarm({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { updateFarm: farm } = data;
        if (props.location.state && props.location.state.from) {
          const referrefUri = props.location.state.from;
          const farmUpdateUrl = `/farms/update/${farm.id}`;
          if (referrefUri !== farmUpdateUrl) {
            raiseFarmUpdatedNotify(farm);
            props.history.push(referrefUri);
            resolve({ farm });
            return;
          }
        }

        raiseFarmUpdatedNotify(farm);
        props.history.push(`/farms/${farm.id}`);
        resolve({ farm });
      });
    });
  };

  const raiseFarmUpdatedNotify = (farm) => {
    dispatch("FARM_UPDATE", {
      id: farm.id,
    });
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  useEffect(() => {
    if (!loading && called) {
      window.scrollTo(0, 0);
      refetch();
    }
  }, [refetch, called, loading]);

  if (loading || !data) {
    return <LoadingBar>Loading...</LoadingBar>;
  } else if (error) {
    return <ErrorBar>Error!</ErrorBar>;
  }

  const { farm } = data;

  const currentFarm = JSON.parse(JSON.stringify(farmCreationModel));
  for (const formIdentifier in currentFarm) {
    currentFarm[formIdentifier].value = farm[formIdentifier];
    if (farm[formIdentifier]) {
      currentFarm[formIdentifier].isValid = true;
    }
  }

  const breadcrumbs = [
    {
      title: "Farms",
      url: "/farms/",
    },
    {
      title: farm.name,
      url: `/Farms/${farm.id}`,
    },
    {
      isActived: true,
      title: "Update",
    },
  ];

  const getAuthorInfo = () => {
    if (!authorData) {
      return {};
    }
    const { userInfo } = authorData;
    const authorInfo = { ...userInfo };
    if (authorData) {
      const { userPhotos } = authorData;
      const avatar = userPhotos.find((item) => item.photoType === "AVATAR");
      if (avatar) {
        authorInfo.userAvatar = avatar;
      }
      const cover = userPhotos.find((item) => item.photoType === "COVER");
      if (cover) {
        authorInfo.userCover = cover;
      }
    }

    if (userFarmData) {
      const { userFarms } = userFarmData;
      const { collections } = userFarms;
      authorInfo.farms = collections;
    }
    return authorInfo;
  };

  return (
    <DetailLayout author={getAuthorInfo()}>
      <Breadcrumb list={breadcrumbs} />
      <FarmEditor
        height={350}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={farmTypes}
        onFarmPost={onFarmPost}
        showValidationError={showValidationError}
        currentFarm={currentFarm}
      />
    </DetailLayout>
  );
});
