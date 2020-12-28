import React, { Fragment, useContext, useEffect } from "react";
import { useQuery, useMutation } from "@apollo/client";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
import ProductItem from "../../components/organisms/Product/ProductItem";
import authClient from "../../graphql/client/authClient";
import { fileToBase64 } from "../../utils/Helper";
import {
  productMutations,
  mediaMutations,
  farmMutations,
} from "../../graphql/fetching/mutations";
import ProductEditor from "../../components/organisms/ProfileEditors/ProductEditor";
import { useStore } from "../../store/hook-store";
import { productQueries } from "../../graphql/fetching/queries";
import Loading from "../../components/atoms/Loading";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import { SessionContext } from "../../store/context/session-context";

export default withRouter(function (props) {
  const { location, match, pageNumber, pageSize } = props;
  const { params } = match;
  const { userId } = params;
  const [state, dispatch] = useStore(false);
  const { currentUser, isLogin } = useContext(SessionContext);

  const [validateImageUrl] = useMutation(mediaMutations.VALIDATE_IMAGE_URL);
  const [productCategories] = useMutation(
    productMutations.FILTER_PRODUCT_CATEGORIES
  );

  const [createProduct] = useMutation(productMutations.CREATE_PRODUCT, {
    client: authClient,
  });

  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  const [farms] = useMutation(farmMutations.FILTER_FARMS);

  const {
    loading,
    data,
    error,
    networkStatus,
    refetch: fetchNewProducts,
  } = useQuery(productQueries.GET_USER_PRODUCTS, {
    variables: {
      criterias: {
        userIdentityId: userId,
        page: pageNumber ? parseInt(pageNumber) : 1,
        pageSize: pageSize ? parseInt(pageSize) : 10,
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
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { createProduct: product } = data;
        resolve(product);
        fetchNewProducts();
      });
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
        return [];
      });
  };

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
    deleteProduct({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      fetchNewProducts();
    });
  };

  useEffect(() => {
    if (state.type === "PRODUCT_UPDATE" || state.type === "PRODUCT_DELETE") {
      fetchNewProducts();
    }
  }, [state, fetchNewProducts]);

  const renderProductEditor = () => {
    if (currentUser && isLogin) {
      return (
        <ProductEditor
          height={230}
          convertImageCallback={convertImagefile}
          onImageValidate={onImageValidate}
          filterCategories={searchProductCategories}
          onProductPost={onProductPost}
          showValidationError={showValidationError}
          refetchNews={fetchNewProducts}
          filterFarms={searchFarms}
        />
      );
    }
    return null;
  };

  if (loading || !data || networkStatus === 1) {
    return (
      <Fragment>
        {renderProductEditor()}
        <Loading>Loading...</Loading>
      </Fragment>
    );
  } else if (error) {
    return (
      <Fragment>
        {renderProductEditor()}
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

  const pageQuery = location.search;
  const baseUrl = props.userUrl + "/articles";
  const { totalPage, filter } = userProducts;
  const { page } = filter;

  return (
    <Fragment>
      {renderProductEditor()}
      <div className="row">
        {products
          ? products.map((item) => (
              <div
                key={item.id}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <ProductItem
                  product={item}
                  onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
                />
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
