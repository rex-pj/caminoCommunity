import * as React from "react";
import { useEffect } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Farm/Detail";
import {
  farmQueries,
  productQueries,
  userQueries,
} from "../../graphql/fetching/queries";
import { useQuery } from "@apollo/client";
import styled from "styled-components";
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
import FarmService from "../../services/farmService";
import ProductService from "../../services/productService";

const FarmProductsBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

interface Props {}

const DetailPage = (props: Props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const farmService = new FarmService();
  const productService = new ProductService();
  const { id } = useParams();
  const [state, dispatch] = useStore(false);
  const { loading, data, error, refetch } = useQuery(farmQueries.GET_FARM, {
    variables: {
      criterias: {
        id: Number(id),
      },
      fetchPolicy: "cache-and-network",
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
    loading: productLoading,
    data: productData,
    error: productError,
    refetch: refetchProducts,
  } = useQuery(productQueries.GET_PRODUCTS, {
    variables: {
      criterias: {
        farmId: Number(id),
        page: 1,
        pageSize: 3,
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

  function onOpenDeleteProductConfirmation(e: any) {
    onOpenDeleteConfirmation(e, onDeleteProduct);
  }

  const onDeleteMain = (id: number) => {
    return farmService.delete(id).then(() => {
      if (location.state?.from) {
        dispatch("FARM_DELETE", {
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

  const onDeleteProduct = (id: number) => {
    return productService.delete(id).then(() => {
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

  const farm = data ? { ...data.farm } : {};

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

  if (farm.pictures && farm.pictures.length > 0) {
    farm.images = farm.pictures.map((item: any) => {
      let image = { ...item };

      if (image.pictureId > 0) {
        image.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${image.pictureId}`;
        image.url = `${apiConfig.paths.pictures.get.getPicture}/${image.pictureId}`;
      }
      return image;
    });
  }

  const renderProducts = (
    productLoading: boolean,
    productData: any,
    productError: any
  ) => {
    if (productLoading || !productData) {
      return (
        <div className="col-12">
          <LoadingBar />
        </div>
      );
    } else if (productError) {
      return (
        <div className="col-12">
          <ErrorBar />
        </div>
      );
    }

    const { products: ProductCollections } = productData;
    const { collections } = ProductCollections;

    const products = collections.map((item: any) => {
      let product = { ...item };
      product.url = `${UrlConstant.Product.url}${product.id}`;
      if (product.pictures && product.pictures.length > 0) {
        const picture = product.pictures[0];
        if (picture.pictureId > 0) {
          product.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${picture.pictureId}`;
        }
      }

      product.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoId) {
        product.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      if (product.farms) {
        product.farms = product.farms.map((pf: any) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.id}`;
          return productFarm;
        });
      }

      return product;
    });

    return (
      <div className="row">
        {products
          ? products.map((item: any, index: number) => {
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
};

export default DetailPage;
