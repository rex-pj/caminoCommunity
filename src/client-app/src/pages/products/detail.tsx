import * as React from "react";
import { useEffect } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Product/Detail";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useQuery } from "@apollo/client";
import styled from "styled-components";
import {
  productQueries,
  userQueries,
  farmQueries,
} from "../../graphql/fetching/queries";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import ProductItem from "../../components/organisms/Product/ProductItem";
import { TertiaryHeading } from "../../components/atoms/Heading";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import { apiConfig } from "../../config/api-config";
import ProductService from "../../services/productService";

const RelationBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

interface Props {}

const DetailPage = (props: Props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const productService = new ProductService();
  const { id } = useParams();
  const [state, dispatch] = useStore(false);
  const { loading, data, error, refetch } = useQuery(
    productQueries.GET_PRODUCT,
    {
      variables: {
        criterias: {
          id: id,
        },
      },
      fetchPolicy: "cache-and-network",
    }
  );

  const userIdentityId = data?.product?.createdByIdentityId;
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

  const {
    loading: relevantLoading,
    data: relevantData,
    error: relevantError,
    refetch: refetchRelevants,
  } = useQuery(productQueries.GET_RELEVANT_PRODUCTS, {
    variables: {
      criterias: {
        id: id,
        page: 1,
        pageSize: 8,
      },
    },
  });

  const onOpenDeleteConfirmation = (
    e: any,
    onDeleteData: (id: number) => Promise<any>
  ) => {
    const { title, innerModal, message, id } = e;
    dispatch("OPEN_MODAL", {
      data: {
        title: title,
        children: message,
        id: id,
      },
      execution: { onDelete: onDeleteData },
      options: {
        isOpen: true,
        innerModal: innerModal,
        position: "fixed",
      },
    });
  };

  const onOpenDeleteMainConfirmation = (e: any) => {
    onOpenDeleteConfirmation(e, onDeleteMain);
  };

  const onOpenDeleteRelevantConfirmation = (e: any) => {
    onOpenDeleteConfirmation(e, onDeleteRelevant);
  };

  const onDeleteMain = (id: number) => {
    return productService.delete(id).then(() => {
      if (location.state?.from) {
        dispatch("PRODUCT_DELETE", {
          id: id,
        });
        navigate({
          pathname: location.state.from,
        });
        return;
      }
      navigate({
        pathname: `/`,
      });
    });
  };

  const onDeleteRelevant = (id: number) => {
    return productService.delete(id).then(() => {
      dispatch("PRODUCT_DELETE", {
        id: id,
      });
      refetchRelevants();
    });
  };

  useEffect(() => {
    if (state.type === "PRODUCT_UPDATE" && state.id) {
      refetch();
    }
  }, [state, refetch]);

  const product = data ? { ...data.product } : {};

  const breadcrumbs = [
    {
      title: "Products",
      url: "/products/",
    },
    {
      isActived: true,
      title: product.name,
    },
  ];

  if (product.pictures && product.pictures.length > 0) {
    product.images = product.pictures.map((item: any) => {
      let image = { ...item };

      if (image.pictureId > 0) {
        image.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${image.pictureId}`;
        image.url = `${apiConfig.paths.pictures.get.getPicture}/${image.pictureId}`;
      }
      return image;
    });
  }

  if (product.farms) {
    product.farms = product.farms.map((pf: any) => {
      let productFarm = { ...pf };
      productFarm.url = `/farms/${pf.id}`;
      return productFarm;
    });
  }

  const renderRelevants = (
    relevantLoading: boolean,
    relevantData: any,
    relevantError: any
  ) => {
    if (relevantLoading || !relevantData) {
      return <LoadingBar className="mt-3" />;
    } else if (relevantError) {
      return <ErrorBar />;
    }
    const { relevantProducts } = relevantData;
    const relevants = relevantProducts.map((item: any) => {
      let productItem = { ...item };
      productItem.url = `${UrlConstant.Product.url}${productItem.id}`;
      if (productItem.pictures) {
        const picture = productItem.pictures[0];
        if (picture && picture.pictureId > 0) {
          productItem.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${picture.pictureId}`;
        }
      }

      productItem.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoId) {
        productItem.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      if (productItem.farms) {
        productItem.farms = productItem.farms.map((pf: any) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.id}`;
          return productFarm;
        });
      }

      return productItem;
    });

    return (
      <RelationBox className="mt-3">
        <TertiaryHeading>Other Products</TertiaryHeading>
        <div className="row">
          {relevants
            ? relevants.map((item: any, index: number) => {
                return (
                  <div
                    key={index}
                    className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                  >
                    <ProductItem
                      product={item}
                      onOpenDeleteConfirmationModal={
                        onOpenDeleteRelevantConfirmation
                      }
                    />
                  </div>
                );
              })
            : null}
        </div>
      </RelationBox>
    );
  };

  const getAuthorInfo = () => {
    if (!authorData) {
      return {};
    }
    const { userInfo } = authorData;
    const authorInfo = { ...userInfo };
    if (authorData) {
      const { userPhotos } = authorData;
      const avatar = userPhotos.find(
        (item: any) => item.photoType === "AVATAR"
      );
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

  return (
    <DetailLayout
      author={getAuthorInfo()}
      isLoading={!!loading}
      hasData={true}
      hasError={!!error}
    >
      <Breadcrumb list={breadcrumbs} />
      <Detail
        product={product}
        breadcrumbs={breadcrumbs}
        onOpenDeleteConfirmationModal={onOpenDeleteMainConfirmation}
      />
      {renderRelevants(relevantLoading, relevantData, relevantError)}
    </DetailLayout>
  );
};

export default DetailPage;
