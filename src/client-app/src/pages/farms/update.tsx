import * as React from "react";
import { useContext, useEffect } from "react";
import { farmQueries, userQueries } from "../../graphql/fetching/queries";
import { farmMutations } from "../../graphql/fetching/mutations";
import { useQuery, useMutation } from "@apollo/client";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import FarmEditor from "../../components/organisms/Farm/FarmEditor";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import FarmService from "../../services/farmService";
import { Helmet } from "react-helmet-async";
import { SessionContext } from "../../store/context/session-context";

type Props = {};

const UpdatePage = (props: Props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();
  const { currentUser, isLogin } = useContext(SessionContext);
  const dispatch = useStore(false)[1];
  const [farmTypes] = useMutation(farmMutations.FILTER_FARM_TYPES);
  const farmService = new FarmService();

  const { loading, data, error, refetch, called } = useQuery(farmQueries.GET_FARM_FOR_UPDATE, {
    variables: {
      criterias: {
        id: Number(id),
      },
    },
    fetchPolicy: "cache-and-network",
  });

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

  const onFarmPost = async (data: any) => {
    if (!id) {
      return;
    }
    return await farmService.update(data, Number.parseInt(id)).then((response) => {
      return new Promise((resolve) => {
        if (location.state?.from) {
          const referrefUri = location.state.from;
          const farmUpdateUrl = `/farms/update/${id}`;
          if (referrefUri !== farmUpdateUrl) {
            navigate(referrefUri);
            resolve({});
            return;
          }
        }
        navigate(`/farms/${id}`);
        resolve({});
      });
    });
  };

  const showValidationError = (title: string, message: string) => {
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
  const currentFarm = { ...farm };
  const { createdByIdentityId } = currentFarm;
  const isAuthor = currentUser && createdByIdentityId === currentUser.userIdentityId;
  if (!isLogin || (createdByIdentityId && !isAuthor)) {
    navigate({
      pathname: `/not-found`,
    });
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
      const avatar = userPhotos.find((item: any) => item.photoType === "AVATAR");
      if (avatar) {
        authorInfo.userAvatar = avatar;
      }
      const cover = userPhotos.find((item: any) => item.photoType === "COVER");
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

  const metaTitle = `Cập nhật thông tin nông trại ${currentFarm?.name} ${"| Nông Trại LỒ Ồ"}`;
  return (
    <>
      <Helmet>
        {metaTitle ? <title>{metaTitle}</title> : null}
        <meta name="robots" content="noindex,nofollow" />
      </Helmet>
      <DetailLayout author={getAuthorInfo()} isLoading={!!loading} hasData={true} hasError={!!error}>
        <Breadcrumb list={breadcrumbs} />
        <FarmEditor height={350} filterCategories={farmTypes} onFarmPost={onFarmPost} showValidationError={showValidationError} currentFarm={currentFarm} />
      </DetailLayout>
    </>
  );
};

export default UpdatePage;
