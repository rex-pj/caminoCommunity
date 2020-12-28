import React, { useEffect } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Farm/Detail";
import {
  farmQueries,
  productQueries,
  userQueries,
} from "../../graphql/fetching/queries";
import {
  farmMutations,
  productMutations,
} from "../../graphql/fetching/mutations";
import { useQuery, useMutation } from "@apollo/client";
import styled from "styled-components";
import { withRouter } from "react-router-dom";
import Loading from "../../components/atoms/Loading";
import ProductItem from "../../components/organisms/Product/ProductItem";
import { TertiaryHeading } from "../../components/atoms/Heading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { useStore } from "../../store/hook-store";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import { authClient } from "../../graphql/client";

const FarmProductsBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

export default withRouter(function (props) {
  const { match, location } = props;
  const { params } = match;
  const { id } = params;
  const [state, dispatch] = useStore(false);
  const { loading, data, error, refetch } = useQuery(farmQueries.GET_FARM, {
    variables: {
      criterias: {
        id: parseFloat(id),
      },
    },
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

  const { data: userFarmData } = useQuery(farmQueries.GET_USER_FARMS_TITLE, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userIdentityId: userIdentityId,
        page: 1,
        pageSize: 4,
      },
    },
  });

  const [deleteFarm] = useMutation(farmMutations.DELETE_FARM, {
    client: authClient,
  });

  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  const {
    productLoading,
    data: productData,
    error: productError,
    refetch: refetchProducts,
  } = useQuery(productQueries.GET_PRODUCTS, {
    variables: {
      criterias: {
        farmId: parseFloat(id),
        page: 1,
        pageSize: 3,
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

  const onOpenDeleteProductConfirmation = (e) => {
    onOpenDeleteConfirmation(e, onDeleteProduct);
  };

  const onDeleteMain = (id) => {
    deleteFarm({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      if (location.state?.from) {
        dispatch("FARM_DELETE", {
          id: id,
        });
        props.history.push({
          pathname: location.state.from,
        });
        return;
      }
      props.history.push({
        pathname: `/`,
      });
    });
  };

  const onDeleteProduct = (id) => {
    deleteProduct({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      dispatch("PRODUCT_DELETE", {
        id: id,
      });
      refetchProducts();
    });
  };

  useEffect(() => {
    if (state.type === "FARM_UPDATE" && state.id) {
      refetch();
    }

    if (state.type === "PRODUCT_UPDATE" || state.type === "PRODUCT_DELETE") {
      refetchProducts();
    }
  }, [state, refetch, refetchProducts]);

  if (loading || !data) {
    return <Loading>Loading...</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { farm: farmData } = data;
  let farm = { ...farmData };

  const breadcrumbs = [
    {
      title: "Farms",
      url: "/farms/",
    },
    {
      isActived: true,
      title: farm.name,
    },
  ];

  if (farm.thumbnails && farm.thumbnails.length > 0) {
    farm.images = farm.thumbnails.map((item) => {
      let image = { ...item };

      if (image.pictureId > 0) {
        image.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.pictureId}`;
        image.url = `${process.env.REACT_APP_CDN_PHOTO_URL}${image.pictureId}`;
      }
      return image;
    });
  }

  const renderProducts = (productLoading, productData, productError) => {
    if (productLoading || !productData) {
      return (
        <div className="col-12">
          <Loading>Loading...</Loading>
        </div>
      );
    } else if (productError) {
      return (
        <div className="col-12">
          <ErrorBlock>Error!</ErrorBlock>
        </div>
      );
    }

    const { products: ProductCollections } = productData;
    const { collections } = ProductCollections;

    const products = collections.map((item) => {
      let product = { ...item };
      product.url = `${UrlConstant.Product.url}${product.id}`;
      if (product.thumbnails && product.thumbnails.length > 0) {
        const thumbnail = product.thumbnails[0];
        if (thumbnail.pictureId > 0) {
          product.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.pictureId}`;
        }
      }

      product.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoCode) {
        product.creator.photoUrl = `${process.env.REACT_APP_CDN_AVATAR_API_URL}${item.createdByPhotoCode}`;
      }

      if (product.productFarms) {
        product.productFarms = product.productFarms.map((pf) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.farmId}`;
          return productFarm;
        });
      }

      return product;
    });

    return (
      <div className="row">
        {products
          ? products.map((item, index) => {
              return (
                <div
                  key={index}
                  className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                >
                  <ProductItem
                    product={item}
                    onOpenDeleteConfirmationModal={
                      onOpenDeleteProductConfirmation
                    }
                  />
                </div>
              );
            })
          : null}
      </div>
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
    <DetailLayout author={getAuthorInfo()}>
      <Detail
        farm={farm}
        breadcrumbs={breadcrumbs}
        onOpenDeleteConfirmationModal={onOpenDeleteMainConfirmation}
      />
      <FarmProductsBox>
        <TertiaryHeading>{farm.name}'s products</TertiaryHeading>
        {renderProducts(productLoading, productData, productError)}
      </FarmProductsBox>
    </DetailLayout>
  );
});
