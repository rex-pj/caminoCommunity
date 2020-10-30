import React, { Fragment } from "react";
import { useQuery, useMutation } from "@apollo/client";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
import ProductItem from "../../components/organisms/Product/ProductItem";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import { fileToBase64 } from "../../utils/Helper";
import {
  VALIDATE_IMAGE_URL,
  FILTER_PRODUCT_CATEGORIES,
  CREATE_PRODUCT,
  FILTER_FARMS,
} from "../../utils/GraphQLQueries/mutations";
import ProductEditor from "../../components/organisms/ProfileEditors/ProductEditor";
import { useStore } from "../../store/hook-store";
import { GET_USER_PRODUCTS } from "../../utils/GraphQLQueries/queries";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";

export default withRouter(function (props) {
  const { location, match, pageNumber } = props;
  const { params } = match;
  const { userId } = params;
  const dispatch = useStore(false)[1];

  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [productCategories] = useMutation(FILTER_PRODUCT_CATEGORIES);

  const [createProduct] = useMutation(CREATE_PRODUCT, {
    client: graphqlClient,
  });

  const [farms] = useMutation(FILTER_FARMS);

  const {
    loading,
    data,
    error,
    networkStatus,
    refetch: fetchNewProducts,
  } = useQuery(GET_USER_PRODUCTS, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber,
      },
    },
  });

  const searchProductCategories = async (inputValue) => {
    return await productCategories({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { categories } = data;
        if (!categories) {
          return [];
        }
        return categories.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

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

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onProductPost = async (data) => {
    return await createProduct({
      variables: {
        criterias: data,
      },
    });
  };

  const searchFarms = async (inputValue) => {
    return await farms({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { farms } = data;
        if (!farms) {
          return [];
        }
        return farms.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const refetchNewProducts = () => {
    fetchNewProducts();
  };

  const productEditor = (
    <ProductEditor
      height={230}
      convertImageCallback={convertImagefile}
      onImageValidate={onImageValidate}
      filterCategories={searchProductCategories}
      onProductPost={onProductPost}
      showValidationError={showValidationError}
      refetchNews={refetchNewProducts}
      filterFarms={searchFarms}
    />
  );

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {productEditor}
        <Loading>Loading...</Loading>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {productEditor}
        <ErrorBlock>Error!</ErrorBlock>
      </Fragment>
    );
  }

  const { userProducts } = data;
  const { collections } = userProducts;
  const products = collections.map((item) => {
    let product = { ...item };
    product.url = `${UrlConstant.Product.url}${product.id}`;
    if (product.thumbnails && product.thumbnails.length > 0) {
      const thumbnail = product.thumbnails[0];
      if (thumbnail.id > 0) {
        product.thumbnailUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${thumbnail.id}`;
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

  const pageQuery = location.search;
  const baseUrl = props.userUrl + "/articles";
  const { totalPage, filter } = userProducts;
  const { page } = filter;

  return (
    <Fragment>
      {productEditor}
      <div className="row">
        {products
          ? products.map((item) => (
              <div
                key={item.id}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <ProductItem product={item} />
              </div>
            ))
          : null}
      </div>
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        pageQuery={pageQuery}
        currentPage={page}
      />
    </Fragment>
  );
});
