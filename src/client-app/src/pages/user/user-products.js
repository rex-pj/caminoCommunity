import React, {
  Fragment,
  useContext,
  useState,
  useRef,
  useEffect,
  useMemo,
} from "react";
import { useLazyQuery, useMutation } from "@apollo/client";
import { useParams } from "react-router-dom";
import { UrlConstant } from "../../utils/Constants";
import ProductItem from "../../components/organisms/Product/ProductItem";
import {
  productMutations,
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
import { apiConfig } from "../../config/api-config";
import MediaService from "../../services/mediaService";
import ProductService from "../../services/productService";

const UserProducts = (props) => {
  const { userId } = useParams();
  const { pageNumber } = props;
  const [state, dispatch] = useStore(false);
  const { currentUser, isLogin } = useContext(SessionContext);
  const [products, setProducts] = useState([]);
  const pageRef = useRef({
    pageNumber: pageNumber ? pageNumber : 1,
    userId: userId,
  });
  const mediaService = new MediaService();
  const productService = new ProductService();

  const [fetchProductCategories] = useLazyQuery(
    productQueries.FILTER_PRODUCT_CATEGORIES,
    {
      variables: {},
    }
  );
  const [productAttributes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTES
  );
  const [productAttributeControlTypes] = useMutation(
    productMutations.FILTER_PRODUCT_ATTRIBUTE_CONTROL_TYPES
  );

  const [userFarms] = useMutation(farmMutations.FILTER_FARMS);

  const getProductCategories = useMemo(
    () => fetchProductCategories,
    [fetchProductCategories]
  );
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
    return {
      file: file,
      fileName: file.name,
    };
  };

  const onImageValidate = async (formData) => {
    return await mediaService.validatePicture(formData);
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onProductPost = async (data) => {
    return await productService.create(data).then((response) => {
      const { data: id } = response;
      resetProducts();

      return Promise.resolve(id);
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

  const onDelete = async (id) => {
    await productService.delete(id).then(() => {
      resetProducts();
    });
  };

  const resetProducts = () => {
    setProducts([]);
    fetchProducts({
      variables: {
        criterias: {
          userIdentityId: pageRef.current.userId,
          page: 1,
        },
      },
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
          filterCategories={getProductCategories}
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
        <LoadingBar />
      </Fragment>
    );
  }
  if (!(data && pageRef.current.totalResult && products.length > 0)) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProductEditor() : null}
        <NoDataBar />
      </Fragment>
    );
  }
  if (error) {
    return (
      <Fragment>
        {currentUser && isLogin ? renderProductEditor() : null}
        <ErrorBar />
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
        dataLength={pageRef.current.totalResult ?? 0}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<LoadingBar />}
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
};

export default UserProducts;
