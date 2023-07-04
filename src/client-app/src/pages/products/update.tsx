import * as React from "react";
import { useEffect, useMemo } from "react";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import { useQuery, useMutation, useLazyQuery } from "@apollo/client";
import {
  productQueries,
  userQueries,
  farmQueries,
} from "../../graphql/fetching/queries";
import { productMutations } from "../../graphql/fetching/mutations";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import { ProductCreationModel } from "../../models/productCreationModel";
import ProductEditor from "../../components/organisms/Product/ProductEditor";
import DetailLayout from "../../components/templates/Layout/DetailLayout";
import ProductService from "../../services/productService";

interface Props {}

const UpdatePage = (props: Props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const { id } = useParams();
  const dispatch = useStore(false)[1];
  const { loading, data, error, refetch, called } = useQuery(
    productQueries.GET_PRODUCT_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: Number(id),
        },
      },
      fetchPolicy: "cache-and-network",
    }
  );

  const userIdentityId = data?.product?.createdByIdentityId;
  const [fetchUserFarms] = useLazyQuery(farmQueries.SELECT_USER_FARMS, {
    variables: {},
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

  const [fetchProductCategories] = useLazyQuery(
    productQueries.FILTER_PRODUCT_CATEGORIES,
    {
      variables: {},
    }
  );
  const productService = new ProductService();

  const getProductCategories = useMemo(
    () => fetchProductCategories,
    [fetchProductCategories]
  );
  const getUserFarms = useMemo(() => fetchUserFarms, [fetchUserFarms]);

  const [productAttributeControlTypes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTE_CONTROL_TYPES
  );
  const [productAttributes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTES
  );

  const { data: authorData } = useQuery(userQueries.GET_USER_INFO, {
    skip: !userIdentityId,
    variables: {
      criterias: {
        userId: userIdentityId,
      },
    },
  });

  async function onProductPost(data: any) {
    if (!id) {
      return;
    }
    return await productService
      .update(data, Number.parseInt(id))
      .then((response) => {
        refetch().then(() => {
          return new Promise((resolve) => {
            if (location.state && location.state.from) {
              const referrefUri = location.state.from;
              const productUpdateUrl = `/products/update/${id}`;
              if (referrefUri !== productUpdateUrl) {
                navigate(referrefUri);
                resolve(response);
                return;
              }
            }

            navigate(`/products/${id}`);
            resolve(response);
          });
        });
      });
  }

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

  const product = data ? { ...data.product } : {};
  const currentProduct = { ...product };

  const breadcrumbs = [
    {
      title: "Products",
      url: "/products/",
    },
    {
      url: `/products/${product.id}`,
      title: product.name,
    },
    {
      isActived: true,
      title: "Update",
    },
  ];

  return (
    <DetailLayout
      author={getAuthorInfo()}
      isLoading={!!loading}
      hasData={true}
      hasError={!!error}
    >
      <Breadcrumb list={breadcrumbs} />
      <ProductEditor
        currentProduct={currentProduct}
        height={350}
        filterCategories={getProductCategories}
        onProductPost={onProductPost}
        showValidationError={showValidationError}
        filterFarms={getUserFarms}
        filterAttributes={productAttributes}
        filterProductAttributeControlTypes={productAttributeControlTypes}
      />
    </DetailLayout>
  );
};

export default UpdatePage;
