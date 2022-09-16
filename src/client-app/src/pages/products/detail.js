import React, { useEffect } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Product/Detail";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useQuery, useMutation } from "@apollo/client";
import styled from "styled-components";
import {
  productQueries,
  userQueries,
  farmQueries,
} from "../../graphql/fetching/queries";
import { productMutations } from "../../graphql/fetching/mutations";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import ProductItem from "../../components/organisms/Product/ProductItem";
import { TertiaryHeading } from "../../components/atoms/Heading";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import { authClient } from "../../graphql/client";
import { apiConfig } from "../../config/api-config";

const RelationBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

export default (function (props) {
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();
  const [state, dispatch] = useStore(false);
  const { loading, data, error, refetch } = useQuery(
    productQueries.GET_PRODUCT,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
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

  const { data: userFarmData } = useQuery(farmQueries.SELECT_USER_FARMS, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userIdentityId: userIdentityId,
        page: 1,
        pageSize: 3,
      },
    },
  });

  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  const {
    relevantLoading,
    data: relevantData,
    error: relevantError,
    refetch: refetchRelevants,
  } = useQuery(productQueries.GET_RELEVANT_PRODUCTS, {
    variables: {
      criterias: {
        id: parseFloat(id),
        page: 1,
        pageSize: 8,
      },
    },
  });

  const onOpenDeleteConfirmation = (e, onDeleteData) => {
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

  const onOpenDeleteMainConfirmation = (e) => {
    onOpenDeleteConfirmation(e, onDeleteMain);
  };

  const onOpenDeleteRelevantConfirmation = (e) => {
    onOpenDeleteConfirmation(e, onDeleteRelevant);
  };

  const onDeleteMain = (id) => {
    deleteProduct({
      variables: {
        criterias: { id },
      },
    }).then(() => {
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

  const onDeleteRelevant = (id) => {
    deleteProduct({
      variables: {
        criterias: { id },
      },
    }).then(() => {
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

  const { product: productData } = data;
  let product = { ...productData };

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
    product.images = product.pictures.map((item) => {
      let image = { ...item };

      if (image.pictureId > 0) {
        image.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${image.pictureId}`;
        image.url = `${apiConfig.paths.pictures.get.getPicture}/${image.pictureId}`;
      }
      return image;
    });
  }

  if (product.farms) {
    product.farms = product.farms.map((pf) => {
      let productFarm = { ...pf };
      productFarm.url = `/farms/${pf.id}`;
      return productFarm;
    });
  }

  const renderRelevants = (relevantLoading, relevantData, relevantError) => {
    if (relevantLoading || !relevantData) {
      return <LoadingBar className="mt-3">Loading...</LoadingBar>;
    } else if (relevantError) {
      return <ErrorBar>Error!</ErrorBar>;
    }
    const { relevantProducts } = relevantData;
    const relevants = relevantProducts.map((item) => {
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

      if (item.createdByPhotoCode) {
        productItem.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoCode}`;
      }

      if (productItem.farms) {
        productItem.farms = productItem.farms.map((pf) => {
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
            ? relevants.map((item, index) => {
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
      <Detail
        product={product}
        breadcrumbs={breadcrumbs}
        onOpenDeleteConfirmationModal={onOpenDeleteMainConfirmation}
      />
      {renderRelevants(relevantLoading, relevantData, relevantError)}
    </DetailLayout>
  );
});
