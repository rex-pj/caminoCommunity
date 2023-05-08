import React, { useEffect } from "react";
import { farmQueries, userQueries } from "../../graphql/fetching/queries";
import { farmMutations } from "../../graphql/fetching/mutations";
import { useQuery, useMutation } from "@apollo/client";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import FarmEditor from "../../components/organisms/Farm/FarmEditor";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import farmCreationModel from "../../models/farmCreationModel";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import MediaService from "../../services/mediaService";
import FarmService from "../../services/farmService";

const UpdatePage = (props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();
  const dispatch = useStore(false)[1];
  const [farmTypes] = useMutation(farmMutations.FILTER_FARM_TYPES);
  const mediaService = new MediaService();
  const farmService = new FarmService();

  const { loading, data, error, refetch, called } = useQuery(
    farmQueries.GET_FARM_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
      fetchPolicy: "cache-and-network",
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

  const { data: userFarmData } = useQuery(farmQueries.GET_USER_FARMS, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userIdentityId: userIdentityId,
        page: 1,
        pageSize: 5,
      },
    },
  });

  const onImageValidate = async (formData) => {
    return await mediaService.validatePicture(formData);
  };

  const convertImagefile = async (file) => {
    return {
      file: file,
      fileName: file.name,
    };
  };

  const onFarmPost = async (data) => {
    return await farmService.update(data).then((response) => {
      return new Promise((resolve) => {
        if (location.state && location.state.from) {
          const referrefUri = location.state.from;
          const farmUpdateUrl = `/farms/update/${data.id}`;
          if (referrefUri !== farmUpdateUrl) {
            navigate(referrefUri);
            resolve();
            return;
          }
        }
        navigate(`/farms/${farm.id}`);
        resolve();
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

  useEffect(() => {
    if (!loading && called) {
      window.scrollTo(0, 0);
      refetch();
    }
  }, [refetch, called, loading]);

  const farm = data ? { ...data.farm } : {};

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
    <DetailLayout
      author={getAuthorInfo()}
      isLoading={!!loading}
      hasData={true}
      hasError={!!error}
    >
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
};

export default UpdatePage;
