import React, {
  Fragment,
  useContext,
  useState,
  useRef,
  useEffect,
} from "react";
import { useLazyQuery, useMutation } from "@apollo/client";
import { useParams } from "react-router-dom";
import { UrlConstant } from "../../utils/Constants";
import ProductItem from "../../components/organisms/Product/ProductItem";
import authClient from "../../graphql/client/authClient";
import { fileToBase64 } from "../../utils/Helper";
import {
  productMutations,
  mediaMutations,
  farmMutations,
} from "../../graphql/fetching/mutations";
import ProductEditor from "../../components/organisms/Product/ProductEditor";
import { useStore } from "../../store/hook-store";
import { productQueries } from "../../graphql/fetching/queries";
import {
  ErrorBar,
  LoadingBar,
  NoDataBar,
} from "../../components/molecules/NotificationBars";
import { SessionContext } from "../../store/context/session-context";
import InfiniteScroll from "react-infinite-scroll-component";

export default (function (props) {
  const { userId } = useParams();
  const { pageNumber } = props;
  const [state, dispatch] = useStore(false);
  const { currentUser, isLogin } = useContext(SessionContext);
  const [products, setProducts] = useState([]);
  const pageRef = useRef({
    pageNumber: pageNumber ? pageNumber : 1,
    userId: userId,
  });

  const [validateImageUrl] = useMutation(mediaMutations.VALIDATE_IMAGE_URL);
  const [productCategories] = useMutation(
    productMutations.FILTER_PRODUCT_CATEGORIES
  );
  const [productAttributes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTES
  );
  const [productAttributeControlTypes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTE_CONTROL_TYPES
  );
  const [createProduct] = useMutation(productMutations.CREATE_PRODUCT, {
    client: authClient,
  });

  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  const [userFarms] = useMutation(farmMutations.FILTER_FARMS);

  const [
    fetchProducts,
    { loading, data, error, networkStatus, refetch: refetchProducts },
  ] = useLazyQuery(productQueries.GET_USER_PRODUCTS, {
    onCompleted: (data) => {
      setPageInfo(data);
      onFetchCompleted(data);
    },
    fetchPolicy: "cache-and-network",
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
        refetchProducts();
      });
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
      refetchProducts();
    });
  };

  const setPageInfo = (data) => {
    const {
      userProducts: {
        totalPage,
        totalResult,
        filter: { page },
      },
    } = data;
    pageRef.current.totalPage = totalPage;
    pageRef.current.currentPage = page;
    pageRef.current.totalResult = totalResult;
  };

  const onFetchCompleted = (data) => {
    const {
      userProducts: { collections },
    } = data;
    const productCollections = parseCollections(collections);
    setProducts([...products, ...productCollections]);
  };

  useEffect(() => {
    if (state.type === "PRODUCT_UPDATE" || state.type === "PRODUCT_DELETE") {
      refetchProducts();
    }
  }, [state, refetchProducts]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchProducts({
      variables: {
        criterias: {
          userIdentityId: pageRef.current.userId,
          page: page ? parseInt(page) : 1,
        },
      },
    });
  }, [fetchProducts]);

  const renderProductEditor = () => {
    if (currentUser && isLogin) {
      return (
        <ProductEditor
          height={230}
          convertImageCallback={convertImagefile}
          onImageValidate={onImageValidate}
          filterCategories={productCategories}
          onProductPost={onProductPost}
          showValidationError={showValidationError}
          refetchNews={refetchProducts}
          filterFarms={userFarms}
          filterAttributes={productAttributes}
          filterProductAttributeControlTypes={productAttributeControlTypes}
        />
      );
    }
    return null;
  };

  const parseCollections = (collections) => {
    return collections.map((item) => {
      let product = { ...item };
      product.url = `${UrlConstant.Product.url}${product.id}`;
      if (product.pictures && product.pictures.length > 0) {
        const picture = product.pictures[0];
        if (picture.pictureId > 0) {
          product.pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${picture.pictureId}`;
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

      if (product.farms) {
        product.farms = product.farms.map((pf) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.id}`;
          return productFarm;
        });
      }

      return product;
    });
  };

  if ((loading || networkStatus === 1) && products.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProductEditor() : null}
        <LoadingBar>Loading...</LoadingBar>
      </Fragment>
    );
  }
  if ((!data || !pageRef.current.totalResult) && products.length === 0) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProductEditor() : null}
        <NoDataBar>No Data!</NoDataBar>
      </Fragment>
    );
  }
  if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProductEditor() : null}
        <ErrorBar>Error!</ErrorBar>
      </Fragment>
    );
  }

  const fetchMoreData = () => {
    if (pageRef.current.pageNumber === pageRef.current.totalPage) {
      return;
    }
    pageRef.current.pageNumber += 1;
    fetchProducts({
      variables: {
        criterias: {
          userId: pageRef.current.userId,
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  return (
    <Fragment>
      {renderProductEditor()}
      <InfiniteScroll
        style={{ overflowX: "hidden" }}
        dataLength={pageRef.current.totalResult}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<h4>Loading...</h4>}
      >
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
      </InfiniteScroll>
    </Fragment>
  );
});
